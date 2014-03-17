using UnityEngine;
using System.Threading;

public class GrayscaleGaussianBlur : IImagePostProcessor {
	
	protected float[] coefficients;
	protected int activeThreadCount;
	protected ManualResetEvent allThreadsDone;
	
	/**
	 *	Applies gaussian blur effect to a grayscale bitmap.
	 * 	@param colorBitmap the image to be processed.
	 */
	public void ProcessBitmap(ref ColorBitmap colorBitmap)
	{
		OnionLogger.globalLog.PushInfoLayer("Performing GrayscaleGaussianBlur on bitmap.");
		
		RGBBitmap rgbBitmap = new RGBBitmap();
		ColorUtils.colorBitmapToRGBBitmap(ref colorBitmap, ref rgbBitmap);

		/* Since we are assuming the image is grayscale, we can apply the blur to just one
		 * channel. We arbitrarily choose the red channel.
		 */
		MonochromeBitmap blurR = ColorUtils.redBitmapFromRGBBitmap(ref rgbBitmap);
		
		// temporarily hard-coded
		int numberOfCoefficients = 9;
		this.coefficients = ApproximateGaussianCoefficients(numberOfCoefficients);

		OnionLogger.globalLog.PushInfoLayer("Blurring rows");
		ExecuteHorizontalBlur(ref blurR);
		ColorUtils.Transpose(ref blurR); /* transpose to set up for blurring columns */
		OnionLogger.globalLog.PopInfoLayer();

		// Since we transposed the image, we can re-use the horizontal blur operation to blur the columns.
		OnionLogger.globalLog.PushInfoLayer("Blurring columns");
		ExecuteHorizontalBlur(ref blurR);
		ColorUtils.Transpose(ref blurR); /* reset the image to original orientation */
		OnionLogger.globalLog.PopInfoLayer();

		// Since we assumed that we are using a grayscale image, the Blue and Green channels can be copied from
		// the red channel.
		rgbBitmap.rgb.r = blurR.channel;
		rgbBitmap.rgb.b = blurR.channel;
		rgbBitmap.rgb.g = blurR.channel;

		ColorUtils.RGBBitmapToColorBitmap(ref rgbBitmap, ref colorBitmap);
		
		OnionLogger.globalLog.PopInfoLayer();
	}

	private void ExecuteHorizontalBlur(ref MonochromeBitmap channelToBlur)
	{
		// We will blur each row in parallel.
		int numberOfThreads = channelToBlur.height;
		OnionLogger.globalLog.PushInfoLayer(string.Format("Setting up {0} GrayscaleGaussianBlur threads",
		                                                  numberOfThreads));
		
		// We will keep an active thread count that is atomically decremented when each thread finishes.
		// When this count reaches 0, then we know that all threads have finished and can set "allThreadsDone".
		activeThreadCount = numberOfThreads;
		allThreadsDone = new ManualResetEvent(false);
		
		// Initialize thread objects
		GrayscaleGaussianBlurThread[] threads = new GrayscaleGaussianBlurThread[numberOfThreads];
		for (int i = 0; i < numberOfThreads; ++i) {
			threads[i] = new GrayscaleGaussianBlurThread(this, i);
		}
		OnionLogger.globalLog.PopInfoLayer();
		
		OnionLogger.globalLog.PushInfoLayer("Running Gaussian blur");
		for (int i = 0; i < numberOfThreads; ++i) {
			ThreadPool.QueueUserWorkItem(new WaitCallback(threads[i].BlurRow), channelToBlur);
		}
		
		if (!allThreadsDone.WaitOne(500)) {
			string logStr = string.Format("Timed out after 500ms - {0} threads unfinished.",
			                              activeThreadCount);
			OnionLogger.globalLog.LogError(logStr);
		}
		OnionLogger.globalLog.PopInfoLayer(); /* Done with parallel section. */
	}
	
	/**
	 *	Uses an approximation method for calculating a normal distribution
	 *	based on this paper (tweaked a bit so that it is centered at 0 and gives values < 1)
	 *
	 *	“A cosine approximation to the normal distribution” D. H. Raab, E. H. Green,
	 *	Psychometrika, Volume 26, pages 447-450.
	 * 
	 * 	@param count How many Gaussian terms to calculate (in addition to the 0th one, which is always 1.0).
	 */
	public float[] ApproximateGaussianCoefficients(int count)
	{
		OnionLogger.globalLog.PushDebugLayer(
			string.Format("Approximating {0} Gaussian coefficients", count + 1));
		
		float[] coefficients = new float[count + 1];
		float stepSize = Mathf.PI / coefficients.Length;
		float x = 0;
		for (int index = 0; index < coefficients.Length; ++index) {
			coefficients[index] = (1f + Mathf.Cos(x)) / (2f * Mathf.PI);
			
			string logStr = string.Format("g({0}) = {1}", x, coefficients[index]);
			OnionLogger.globalLog.LogDebug(logStr);
			x += stepSize;
		}
		OnionLogger.globalLog.PopDebugLayer();
		return coefficients;
	}

	/**
	 * 	A single thread corresponds to a single row in the image.
	 */
	public class GrayscaleGaussianBlurThread : IImagePostProcessorThread {
		
		private GrayscaleGaussianBlur threadManager;
		private readonly float[] coefficients;
		private readonly int rowNumber;
		private MonochromeBitmap original;

		/**
		 * 	A new instance of GrayscaleGaussianBlurThread.
		 * 	@param imageProcessor a pointer to the parent GaussianBlur image processor.
		 * 	@param rowNumber the row that this thread will blur.
		 */
		public GrayscaleGaussianBlurThread(GrayscaleGaussianBlur imageProcessor,
		                                   int rowNumber) 
		{
			this.threadManager = imageProcessor;
			this.coefficients = imageProcessor.coefficients;
			this.rowNumber = rowNumber;
		}
		
		/**
		 * 	Blurs a single row of a bitmap.
		 * 	@param state 	An object containing a MonochromeBitmap.
		 * 					Must be 'object' because of how .NET handles threads.
		 */
		public void BlurRow(object state)
		{
			MonochromeBitmap blurred = (MonochromeBitmap)state;
			this.original = ColorUtils.Copy(ref blurred);
			
			// Zero the row we are blurring.
			int startingPixel = rowNumber * blurred.width;
			int finishPixel = startingPixel + blurred.width;
			for (int i = startingPixel; i < finishPixel; ++i) {
				blurred.channel[i] = 0f;
			}
			
			int numberOfCoefficients = this.coefficients.Length - 1;

			for (int j = 0; j < blurred.width; ++j) {
				int targetPixel = rowNumber * blurred.width + j;
				
				for (int k = 0; k <= numberOfCoefficients; ++k) {
					if (j + k < 0 || j + k >= blurred.width) {
						continue;
					}
					int sourcePoint = targetPixel + k;
					float factor = coefficients[Mathf.Abs(k)];
					float weightedValue = factor * original.channel[sourcePoint];
					blurred.channel[targetPixel] += weightedValue;
				}
			}
			
			// This thread is considered finished. We atomically decrement, and if the active thread count hits 0 then
			// this thread is the last to finish.
			if (Interlocked.Decrement(ref threadManager.activeThreadCount) <= 0) {
				threadManager.allThreadsDone.Set();
			}
		}
	}
}
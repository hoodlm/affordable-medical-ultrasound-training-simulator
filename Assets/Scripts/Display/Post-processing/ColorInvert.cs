using UnityEngine;
using System.Threading;

/**
 *	An example IImagePostProcessor that inverts the colors of a bitmap.
 *	This is utilized by the BlackOnWhiteBModeOutputImageDecoder class.
 */
public class ColorInvert : IImagePostProcessor {

	/**
	 *	Invert the colors of a bitmap.
	 *	The channels are run in parallel using the ColorInvertThread class.
	 * 	@param bitmap 	An array of UnityEngine Color structs,
	 * 					to which the effect will be applied.
	 * 	@param width 	The width of the bitmap in pixels.
	 * 	@param height 	The height of the bitmap in pixels.
	 */
	public void ProcessBitmap(ref ColorBitmap colorBitmap)
	{
		OnionLogger.globalLog.PushInfoLayer("Inverting bitmap");
		RGBBitmap rgbBitmap = new RGBBitmap();
		ColorUtils.colorBitmapToRGBBitmap(ref colorBitmap, ref rgbBitmap);


		MonochromeBitmap invertedr = ColorUtils.redBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap invertedg = ColorUtils.greenBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap invertedb = ColorUtils.blueBitmapFromRGBBitmap(ref rgbBitmap);

		// We can run each channel in parallel.
		int numberOfThreads = 3;
		ManualResetEvent[] threadsDone = new ManualResetEvent[numberOfThreads];
		ColorInvertThread[] threads = new ColorInvertThread[numberOfThreads];
		for (int i = 0; i < numberOfThreads; ++i) {
			threadsDone[i] = new ManualResetEvent(false);
			threads[i] = new ColorInvertThread(threadsDone[i]);
		}

		ThreadPool.QueueUserWorkItem(new WaitCallback(threads[0].ThreadedProcessChannel), invertedr);
		ThreadPool.QueueUserWorkItem(new WaitCallback(threads[1].ThreadedProcessChannel), invertedg);
		ThreadPool.QueueUserWorkItem(new WaitCallback(threads[2].ThreadedProcessChannel), invertedb);

		WaitHandle.WaitAll(threadsDone);
		// Done with parallel section.

		rgbBitmap.rgb.r = invertedr.channel;
		rgbBitmap.rgb.g = invertedg.channel;
		rgbBitmap.rgb.b = invertedb.channel;

		ColorUtils.RGBBitmapToColorBitmap(ref rgbBitmap, ref colorBitmap);
		OnionLogger.globalLog.PopInfoLayer();
	}

	/**
	 * 	Inverts the values in a single channel, represented as an array of floats.
	 * 	This is a serial implementation and isn't really used right now.
	 * 	@param channel 	An array of floats representing the value of each pixel,
	 * 					to which the effect will be applied.
	 * 	@param width 	The width of the bitmap in pixels.
	 * 	@param height 	The height of the bitmap in pixels.
	 */
	public void ProcessChannel(ref MonochromeBitmap monochromeBitmap)
	{
		for (int i = 0; i < monochromeBitmap.channel.Length; ++i) {
			monochromeBitmap.channel[i] = 1f - monochromeBitmap.channel[i];
		}
	}
}

/**
 *	A thread that can invert a single channel in parallel.
 */
public class ColorInvertThread : IImagePostProcessorThread {

	private ManualResetEvent done;

	public ColorInvertThread(ManualResetEvent handle) {
		this.done = handle;
	}

	/**
	 *	No parallel implementation yet for ProcessBitmap of ColorInvert.
	 * 	@param state
	 */
	public void ThreadedProcessBitmap(object state)
	{
		// not implemented
	}

	/**
	 * 	Inverts the values in a single channel, represented as an array of floats
	 * 	@param state 	An object containing a MonochromeBitmap.
	 * 					Must be 'object' because of how .NET handles threads.
	 */
	public void ThreadedProcessChannel(object state)
	{
		MonochromeBitmap monochromeBitmap = (MonochromeBitmap)state;
		for (int i = 0; i < monochromeBitmap.channel.Length; ++i) {
			monochromeBitmap.channel[i] = 1f - monochromeBitmap.channel[i];
		}
		done.Set();
	}
}

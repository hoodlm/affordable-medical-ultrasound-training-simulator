using UnityEngine;
using System.Threading;

/**
 *	An example IImagePostProcessor that inverts the colors of a bitmap.
 */
public class ColorInvert : IImagePostProcessor {

	/**
	 *	Invert the colors of a bitmap.
	 *
	 * 	@param colorBitmap The ColorBitmap to invert.
	 */
	public void ProcessBitmap(ref ColorBitmap colorBitmap)
	{
		OnionLogger.globalLog.PushInfoLayer("Inverting bitmap");
		RGBBitmap rgbBitmap = new RGBBitmap();
		ColorUtils.colorBitmapToRGBBitmap(ref colorBitmap, ref rgbBitmap);

		MonochromeBitmap invertedr = ColorUtils.redBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap invertedg = ColorUtils.greenBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap invertedb = ColorUtils.blueBitmapFromRGBBitmap(ref rgbBitmap);

		// We'll run a thread for each color channel.
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
}

/**
 *	A thread that can invert a single channel in parallel.
 */
public class ColorInvertThread : IImagePostProcessorThread {

	/// A flag for whether this thread has finished working.
	private ManualResetEvent done;

	/// Initializes a new instance of the ColorInvertThread class.
	/// @param handle A flag for the thread to set when the thread is finished working.
	public ColorInvertThread(ManualResetEvent handle) {
		this.done = handle;
	}

	/**
	 *	No parallel implementation for ProcessBitmap of ColorInvert - use the
	 *  ThreadedProcessChannel method.
	 * 	@param state
	 */
	public void ThreadedProcessBitmap(object state)
	{
		// not implemented
		done.Set();
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

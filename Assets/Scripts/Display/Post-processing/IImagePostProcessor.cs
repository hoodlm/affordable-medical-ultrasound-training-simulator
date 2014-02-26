using UnityEngine;
using System.Threading;

/**
 *	Interface for a class that applies a post-processing effect to a 2D image.
 */
public interface IImagePostProcessor {

	/**
	 *	Applies the effect to a bitmap.
	 * 	@param colorBitmap the image to be processed.
	 */
	void ProcessBitmap(ref ColorBitmap colorBitmap);

	/**
	 * 	Applies the effect to a single channel, represented as an array of floats.
	 * 	@param channel A single channel (or monochrome image) to be processed.
	 */
	void ProcessChannel(ref MonochromeBitmap channel);
}

/**
 *	Interface for a class run in a separate thread to help apply a post-processing effect
 *	to a 2D image. The constructor for such a class should take a waitHandle - but constructors
 *	cannot be defined in interfaces.
 */
public interface IImagePostProcessorThread {

	/// public IImagePostProcessorThread(WaitHandle waitHandle);

	/**
	 *	Applies the effect to a bitmap, perhaps representing a portion of an image.
	 * 	@param state	An object containing a ColorBitmap with the image to be processed.
	 * 					Must be 'object' because of how .NET handles threads.
	 */
	void ThreadedProcessBitmap(object state);

	/**
	 * 	Applies the effect - for use in threads.
	 * 	@param state 	An object containing a MonochromeBitmap.
	 * 					Must be 'object' because of how .NET handles threads.
	 */
	void ThreadedProcessChannel(object state);
}

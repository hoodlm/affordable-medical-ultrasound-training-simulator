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
}

/**
 *	Interface for a class run in a separate thread to help apply a post-processing effect
 *	to a 2D image. The constructor for such a class should at minimum take a waitHandle - but constructors
 *	cannot be defined in interfaces. Other than this, the implementation may vary depending on the particular
 *	effect. (Some effects may be applied one-thread-per-channel, some one-thread-per-row, etc.)
 */
public interface IImagePostProcessorThread {

	/// public IImagePostProcessorThread(WaitHandle waitHandle);
	
}

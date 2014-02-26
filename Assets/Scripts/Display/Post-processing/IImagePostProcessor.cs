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

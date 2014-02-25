using UnityEngine;
using System.Collections;

/**
 *	Interface for a class that applies a post-processing effect to a 2D image.
 */
public interface IImagePostProcessor {

	/**
	 *	Applies the effect to a bitmap.
	 * 	@param bitmap An array of UnityEngine Color structs 
	 * 	@param width The width of the bitmap in pixels.
	 * 	@param height The height of the bitmap in pixels.
	 * 	@param The bitmap with the effect applied.
	 */
	void ProcessBitmap(ref Color[] bitmap, int width, int height);

	/**
	 * 	Applies the effect to a single channel, represented as an array of floats.
	 * 	@param channel An array of floats representing the value of each pixel.
	 * 	@param width The width of the bitmap in pixels.
	 * 	@param height The height of the bitmap in pixels.
	 * 	@return The input channel with the effect applied.
	 */
	void ProcessChannel(ref float[] channel, int width, int height);

}

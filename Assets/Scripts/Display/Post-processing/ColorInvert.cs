using UnityEngine;
using System.Collections;

public class ColorInvert : IImagePostProcessor {
	
	/**
	 *	Invert the colors of a bitmap.
	 * 	@param bitmap 	An array of UnityEngine Color structs,
	 * 					to which the effect will be applied.
	 * 	@param width 	The width of the bitmap in pixels.
	 * 	@param height 	The height of the bitmap in pixels.
	 */
	public void ProcessBitmap(ref Color[] bitmap, int width, int height)
	{
		OnionLogger.globalLog.PushInfoLayer("Inverting bitmap");
		RGBChannels rgb = ColorUtils.bitmapToRGBChannels(ref bitmap);

		ProcessChannel(ref rgb.r, width, height);
		ProcessChannel(ref rgb.g, width, height);
		ProcessChannel(ref rgb.b, width, height);

		bitmap = ColorUtils.RGBChannelsToBitmap(ref rgb);
		OnionLogger.globalLog.PopInfoLayer();
	}

	/**
	 * 	Inverts the values in a single channel, represented as an array of floats.
	 * 	@param channel 	An array of floats representing the value of each pixel,
	 * 					to which the effect will be applied.
	 * 	@param width 	The width of the bitmap in pixels.
	 * 	@param height 	The height of the bitmap in pixels.
	 */
	public void ProcessChannel(ref float[] channel, int width, int height)
	{
		for (int i = 0; i < channel.Length; ++i) {
			channel[i] = 1f - channel[i];
		}
	}

}

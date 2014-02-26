using UnityEngine;
using System.Threading;

/**
 *	An example IImagePostProcessor that inverts the colors of a bitmap.
 *	This is utilized by the BlackOnWhiteBModeOutputImageDecoder class.
 */
public class ColorInvert : IImagePostProcessor {
	
	/**
	 *	Invert the colors of a bitmap.
	 * 	@param bitmap 	An array of UnityEngine Color structs,
	 * 					to which the effect will be applied.
	 * 	@param width 	The width of the bitmap in pixels.
	 * 	@param height 	The height of the bitmap in pixels.
	 */
	public void ProcessBitmap(ref ColorBitmap colorBitmap)
	{
		OnionLogger.globalLog.PushInfoLayer("Inverting bitmap");
		RGBBitmap rgbBitmap = ColorUtils.colorBitmapToRGBBitmap(ref colorBitmap);

		OnionLogger.globalLog.PushInfoLayer("Inverting channels");
		MonochromeBitmap r = ColorUtils.redBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap g = ColorUtils.greenBitmapFromRGBBitmap(ref rgbBitmap);
		MonochromeBitmap b = ColorUtils.blueBitmapFromRGBBitmap(ref rgbBitmap);
		ProcessChannel(ref r);
		ProcessChannel(ref g);
		ProcessChannel(ref b);
		OnionLogger.globalLog.PopInfoLayer();

		colorBitmap = ColorUtils.RGBBitmapToColorBitmap(ref rgbBitmap);
		OnionLogger.globalLog.PopInfoLayer();
	}

	/**
	 * 	Inverts the values in a single channel, represented as an array of floats.
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

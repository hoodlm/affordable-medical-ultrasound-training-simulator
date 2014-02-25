using UnityEngine;
using System.Collections;


/// Struct to hold parallel arrays of RGB components.
public struct RGBChannels {
	public float[] r;
	public float[] g;
	public float[] b;
}

/// Utility class containing some static methods for processing color objects.
public class ColorUtils {

	/**
	 *	Joins an RGBChannel struct containing individual color components into a single
	 *	bitmap (array of UnityEngine.Color)
	 *
	 *	@param rgb The RGB data to join.
	 *	@return An array of UnityEngine.Color containing the rgb data.
	 */
	public static Color[] RGBChannelsToBitmap(ref RGBChannels rgb)
	{
		int channelLength = rgb.r.Length;
		string logString = string.Format("Converting RGB to bitmap (length {0})", channelLength);
		OnionLogger.globalLog.PushInfoLayer(logString);

		Color[] bitmap = new Color[channelLength];

		for (int i = 0; i < channelLength; ++i) {
			bitmap[i] = new Color(rgb.r[i], rgb.g[i], rgb.b[i]);
		}

		OnionLogger.globalLog.PopInfoLayer();
		return bitmap;
	}

	/**
	 *	Splits a bitmap (an array of UnityEngine.Color) into an RGBChannel struct containing the
	 *	individual color componenets. The bitmap is left unmodified.
	 *
	 *	@param bitmap The array of UnityEngine.color to split.
	 *	@return An RGBChannels struct containing the channels for the bitmap.
	 */
	public static RGBChannels bitmapToRGBChannels(ref Color[] bitmap)
	{
		int channelLength = bitmap.Length;
		string logString = string.Format("Converting bitmap to RGB (length {0})", channelLength);
		OnionLogger.globalLog.PushInfoLayer(logString);

		float[] r = new float[channelLength];
		float[] g = new float[channelLength];
		float[] b = new float[channelLength];

		for (int i = 0; i < channelLength; ++i) {
			Color colorTuple = bitmap[i];
			r[i] = colorTuple.r;
			g[i] = colorTuple.g;
			b[i] = colorTuple.b;
		}

		RGBChannels rgb = new RGBChannels();
		rgb.r = r;
		rgb.g = g;
		rgb.b = b;

		OnionLogger.globalLog.PopInfoLayer();
		return rgb;
	}

}

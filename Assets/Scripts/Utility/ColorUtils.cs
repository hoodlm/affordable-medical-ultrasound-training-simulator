using UnityEngine;
using System.Collections;

/*
 * Unity's color representation (an RGB tuple) is not great for parallelization.
 * This file contains some structs and methods that allow for better parallel 2D image processing.
 */


/// Struct to hold UnityEngine.Color array of image, with a width and height in pixels.
public struct ColorBitmap {
	public Color[] colors;
	public int width;
	public int height;
}

/// Struct to hold a single color channel as a float array, with a width and height in pixels.
public struct MonochromeBitmap {
	public float[] channel;
	public int width;
	public int height;
}

/// Struct to hold parallel arrays of RGB components. (Rather than individual tuples, as in UnityEngine.Color)
public struct RGBChannels {
	public float[] r;
	public float[] g;
	public float[] b;
}

/// Struct to hold RGB channels of image, with a width and height in pixels.
public struct RGBBitmap {
	public RGBChannels rgb;
	public int width;
	public int height;
}

/// Utility class containing some static methods for processing color objects.
public class ColorUtils {

	/**
	 *	Joins an RGBBitmap struct containing individual color components into a single
	 *	bitmap (array of UnityEngine.Color). The RGBBitmap is left unchanged.
	 *
	 *	@param fromRGB The RGB data to join into a ColorBitmap.
	 *	@param toColors A ColorBitmap equivalent to the rgb data.
	 */
	public static void RGBBitmapToColorBitmap(ref RGBBitmap fromRGB, ref ColorBitmap toColors)
	{
		int channelLength = fromRGB.rgb.r.Length;
		string logString = string.Format("Converting RGBBitmap to ColorBitmap (length {0})", channelLength);
		OnionLogger.globalLog.PushInfoLayer(logString);

		toColors.colors = new Color[channelLength];
		toColors.width = fromRGB.width;
		toColors.height = fromRGB.height;

		for (int i = 0; i < channelLength; ++i) {
			toColors.colors[i] = new Color(fromRGB.rgb.r[i], fromRGB.rgb.g[i], fromRGB.rgb.b[i]);
		}

		OnionLogger.globalLog.PopInfoLayer();
	}

	/**
	 *	Splits a ColorBitmap into an RGBBitmap struct containing the
	 *	individual color componenets. The original ColorBitmap is left unmodified.
	 *
	 *	@param fromColors The bitmap to split into channels.
	 *	@param toRGB RGBBitmap struct containing the channels for the ColorBitmap.
	 */
	public static void colorBitmapToRGBBitmap(ref ColorBitmap fromColors, ref RGBBitmap toRGB)
	{
		int channelLength = fromColors.colors.Length;
		string logString = string.Format("Converting bitmap to RGB (length {0})", channelLength);
		OnionLogger.globalLog.PushInfoLayer(logString);

		float[] r = new float[channelLength];
		float[] g = new float[channelLength];
		float[] b = new float[channelLength];

		for (int i = 0; i < channelLength; ++i) {
			Color colorTuple = fromColors.colors[i];
			r[i] = colorTuple.r;
			g[i] = colorTuple.g;
			b[i] = colorTuple.b;
		}

		RGBChannels rgb = new RGBChannels();
		rgb.r = r;
		rgb.g = g;
		rgb.b = b;

		toRGB.rgb = rgb;
		toRGB.width = fromColors.width;
		toRGB.height = fromColors.height;

		OnionLogger.globalLog.PopInfoLayer();
	}

	/**
	 * 	Extract a MonochromeBitmap from the red channel of an RGBBitmap.
	 * 	@param RGBBitmap The RGBBitmap to extract R from.
	 * 	@return A MonochromeBitmap containing the red channel.
	 */
	public static MonochromeBitmap redBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap) {
		MonochromeBitmap red = new MonochromeBitmap();
		red.channel = rgbBitmap.rgb.r;
		red.height = rgbBitmap.height;
		red.width = rgbBitmap.width;
		return red;
	}

	/**
	 * 	Extract a MonochromeBitmap from the green channel of an RGBBitmap.
	 * 	@param RGBBitmap The RGBBitmap to extract G from.
	 * 	@return A MonochromeBitmap containing the green channel.
	 */
	public static MonochromeBitmap greenBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap) {
		MonochromeBitmap green = new MonochromeBitmap();
		green.channel = rgbBitmap.rgb.g;
		green.height = rgbBitmap.height;
		green.width = rgbBitmap.width;
		return green;
	}

	/**
	 * 	Extract a MonochromeBitmap from the blue channel of an RGBBitmap.
	 * 	@param RGBBitmap The RGBBitmap to extract B from.
	 * 	@return A MonochromeBitmap containing the blue channel.
	 */
	public static MonochromeBitmap blueBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap) {
		MonochromeBitmap blue = new MonochromeBitmap();
		blue.channel = rgbBitmap.rgb.b;
		blue.height = rgbBitmap.height;
		blue.width = rgbBitmap.width;
		return blue;
	}

}

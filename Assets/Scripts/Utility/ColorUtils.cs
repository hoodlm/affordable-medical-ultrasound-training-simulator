﻿using UnityEngine;
using System.Collections;

/*
 * Unity's color representation (an RGB tuple) is not great for parallelization.
 * This file contains some structs and methods that allow for better parallel 2D image processing.
 */

/// Struct to hold UnityEngine.Color array of image, with a width and height in pixels.
public struct ColorBitmap {
	/// An array of RGB-tuples representing the pixels of an image.
	public Color[] colors;
	/// Width of the bitmap, in pixels.
	public int width;
	/// Height of the bitmap, in pixels.
	public int height;
}

/// Struct to hold a single color channel as a float array, with a width and height in pixels.
public struct MonochromeBitmap {
	/// An array of float values, representing the brightness of each pixel in this channel.
	public float[] channel;
	/// Width of the bitmap, in pixels.
	public int width;
	/// Height of the bitmap, in pixels
	public int height;
}

/// Struct to hold parallel arrays of RGB components. (Rather than individual tuples, as in UnityEngine.Color)
public struct RGBChannels {
	/// A float array containing the intensity of the red channel of the image.
	public float[] r;
	/// A float array containing the intensity of the green channel of the image.
	public float[] g;
	/// A float array containing the intensity of the blue channel of the image.
	public float[] b;
}

/// Struct to hold RGB channels of image, with a width and height in pixels.
public struct RGBBitmap {
	/// An RGBChannels struct containing the separate RGB channels of the image as float arrays.
	public RGBChannels rgb;
	/// Width of the bitmap, in pixels.
	public int width;
	/// Height of the bitmap, in pixels.
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
	 * 	@param rgbBitmap The RGBBitmap to extract R from.
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
	 * 	@param rgbBitmap The RGBBitmap to extract G from.
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
	 * 	@param rgbBitmap The RGBBitmap to extract B from.
	 * 	@return A MonochromeBitmap containing the blue channel.
	 */
	public static MonochromeBitmap blueBitmapFromRGBBitmap(ref RGBBitmap rgbBitmap) {
		MonochromeBitmap blue = new MonochromeBitmap();
		blue.channel = rgbBitmap.rgb.b;
		blue.height = rgbBitmap.height;
		blue.width = rgbBitmap.width;
		return blue;
	}

	/**
	 *	Create a deep copy of this monochrome bitmap.
	 *	@param original The bitmap to copy.
	 *	@return A deep copy of the original.
	 */
	public static MonochromeBitmap Copy(ref MonochromeBitmap original) {

		MonochromeBitmap copy = new MonochromeBitmap();
		copy.height = original.height;
		copy.width = original.width;
		copy.channel = (float[])original.channel.Clone();

		return copy;
	}

	/**
	 *	Transposes the data in a bitmap.
	 *	@param bitmap The bitmap that will be transposed.
	 */
	public static void Transpose(ref MonochromeBitmap bitmap) {
		OnionLogger.globalLog.PushInfoLayer("Transposing bitmap of length "+(bitmap.height * bitmap.width));
		float[] transposedData = new float[bitmap.channel.Length];
		int insertionIndex = 0;
		for (int x = 0; x < bitmap.width; ++x) {
			for (int y = 0; y < bitmap.height; ++y) {
				int pixelIndex = y * bitmap.width + x;
				//OnionLogger.globalLog.LogTrace(string.Format("Moving pixel {0} to {1}", pixelIndex, insertionIndex));
				transposedData[insertionIndex++] = bitmap.channel[pixelIndex];
			}
		}
		// set the new data
		bitmap.channel = transposedData;

		// swap width and height
		int temp = bitmap.height;
		bitmap.height = bitmap.width;
		bitmap.width = temp;

		OnionLogger.globalLog.PopInfoLayer();
	}

}

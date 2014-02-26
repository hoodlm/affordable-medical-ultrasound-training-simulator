using UnityEngine;
using System.Collections;

/**
 *	Generates an image that has a test Gaussian Blur applied to it.
 */
public class TestGaussianImageSource : IImageSource {
	
	/// The colors used to generate this test image.
	private Color[] bars = {Color.white, Color.black, Color.black};

	/**
     *  Generate the next frame, and populate a provided bitmap.
     *  @param bitmap A ColorBitmap struct (Color[], width, height) to be populated for this frame.
     */
	public void RenderColorImageInBitmap (ref ColorBitmap bitmap)
	{
		OnionLogger.globalLog.PushInfoLayer("TestGaussianBlurImageSource");
		long totalPixels = bitmap.colors.Length;
		for (int index = 0; index < totalPixels; ++index) {
			Color c = bars[++index % bars.Length];
			bitmap.colors[index] = c;
		}
		GaussianBlur blur = new GaussianBlur();
		blur.ProcessBitmap(ref bitmap);

		OnionLogger.globalLog.PopInfoLayer();
	}

}

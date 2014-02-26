using UnityEngine;
using System.Collections;

/**
 * A fake IImageSource class to generate a test bitmap.
 */
public class TestImageSource : IImageSource {

	/// The colors used to generate this test image.
    private Color[] bars = {Color.green, Color.white, Color.red};

	public void RenderColorImageInBitmap (ref ColorBitmap bitmap) {
		OnionLogger.globalLog.PushInfoLayer("TestImageSource");
		long totalPixels = bitmap.colors.Length;
        for (int index = 0; index < totalPixels; ++index) {
            Color c = bars[(index / (totalPixels / bars.Length)) % bars.Length];
            bitmap.colors[index] = c;
        }
		OnionLogger.globalLog.PopInfoLayer();
    }

	public void AddPostProcessingEffect(IImagePostProcessor effect) {
		// do nothing
	}
}

using UnityEngine;
using System.Collections.Generic;

/**
 * A fake IImageSource class to generate a test bitmap.
 */
public class TestImageSource : IImageSource {

	/// The colors used to generate this test image.
    private Color[] bars = {Color.green, Color.white, Color.red};

	private IList<IImagePostProcessor> imageEffects;

	public TestImageSource() {
		imageEffects = new List<IImagePostProcessor>();
	}

	public void RenderColorImageInBitmap (ref ColorBitmap bitmap) {
		OnionLogger.globalLog.PushInfoLayer("TestImageSource");
		long totalPixels = bitmap.colors.Length;
        for (int index = 0; index < totalPixels; ++index) {
            Color c = bars[(index / 5) % bars.Length];
            bitmap.colors[index] = c;
        }

		OnionLogger.globalLog.PushInfoLayer("Post-processing");
		foreach(IImagePostProcessor effect in imageEffects) {
			effect.ProcessBitmap(ref bitmap);
		}
		OnionLogger.globalLog.PopInfoLayer();
		OnionLogger.globalLog.PopInfoLayer();
    }

	public void AddPostProcessingEffect(IImagePostProcessor effect) {
		UltrasoundDebug.Assert(null != effect, "Null effect added to ImageSource", this);
		imageEffects.Add(effect);
	}
}

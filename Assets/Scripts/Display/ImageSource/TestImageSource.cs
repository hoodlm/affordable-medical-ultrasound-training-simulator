using UnityEngine;
using System.Collections;

/**
 * A fake IImageSource class to generate a test bitmap.
 */
public class TestImageSource : IImageSource {

	/// The colors used to generate this test image.
    private Color[] bars = {Color.green, Color.white, Color.red};

    public Color[] BitmapWithDimensions (int width, int height) {
        Color[] pixels = new Color[width * height];
        for (int index = 0; index < pixels.Length; ++index) {
            Color c = bars[(index / (width * height / bars.Length)) % bars.Length];
            pixels[index] = c;
        }
        return pixels;
    }

}

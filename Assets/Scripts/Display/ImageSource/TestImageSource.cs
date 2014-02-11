using UnityEngine;
using System.Collections;

/**
 * A fake IImageSource class to generate a test bitmap.
 */
public class TestImageSource : IImageSource {

    private Color[] bars = {Color.white, Color.yellow,
        Color.cyan, Color.green, 
        Color.magenta, Color.red, Color.blue};

    public Color[] BitmapWithDimensions (int width, int height) {
        Color[] pixels = new Color[width * height];
        for (int index = 0; index < pixels.Length; ++index) {
            Color c = bars[(index / (width * height / bars.Length)) % bars.Length];
            pixels[index] = c;
        }
        return pixels;
    }

}

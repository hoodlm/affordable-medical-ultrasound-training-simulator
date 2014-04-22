using UnityEngine;

/**
 * A fake ITextureSource class to generate a test texture.
 */
public class TestTextureSource : ITextureSource {

	/// The set of colors used to generate the test texture.
	private Color[] bars = {Color.white, Color.yellow,
							Color.cyan, Color.green, 
							Color.magenta, Color.red, Color.blue};

    public void RenderNextFrameToTexture(ref Texture2D texture){
		// Set up a Color array
		int width = texture.width;
		int height = texture.height;
		Color[] pixels = new Color[width * height];

		// Populate the color array with the colors defined in bars above.
		for (int index = 0; index < pixels.Length; ++index) {
			int pixelRow = index % width;
			Color c = bars[pixelRow % bars.Length];
			pixels[index] = c;
		}

		// Render the color array to the texture.
		texture.SetPixels(pixels);
		texture.Apply();
	}
}

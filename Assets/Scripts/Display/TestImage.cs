﻿using UnityEngine;

/**
 * Generates a test image for checking displayBehavior.
 */
public class TestImage : ITextureSource {

	private Color[] bars = {Color.white, Color.yellow,
							Color.cyan, Color.green, 
							Color.magenta, Color.red, Color.blue};

    public void RenderNextFrameToTexture(ref Texture2D texture){
		int width = texture.width;
		int height = texture.height;
		Color[] pixels = new Color[width * height];
		for (int index = 0; index < pixels.Length; ++index) {
			Color c = bars[(index / (width * height / bars.Length)) % bars.Length];
			pixels[index] = c;
		}
		texture.SetPixels(pixels);
		texture.Apply();
	}
}

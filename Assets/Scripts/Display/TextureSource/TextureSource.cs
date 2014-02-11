using UnityEngine;
using System.Collections;

/**
 *  A basic ITextureSource implimentation that renders a texture from a bitmap (UnityEngine.Color array)
 *  with no post-processing effects.
 */
public class TextureSource : ITextureSource {

    private IImageSource imageSource;

    /**
     * Instantiate a new TextureSource.
     * @param imageSource The class providing a bitmap every frame.
     */
    public TextureSource(IImageSource imageSource) {
        this.imageSource = imageSource;
    }

    public void RenderNextFrameToTexture(ref Texture2D texture){
        int width = texture.width;
        int height = texture.height;
        Color[] pixels = imageSource.BitmapWithDimensions (width, height);
        texture.SetPixels(pixels);
        texture.Apply();
    }

}

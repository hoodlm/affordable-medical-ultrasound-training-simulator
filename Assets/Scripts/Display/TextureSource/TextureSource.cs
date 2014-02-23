using UnityEngine;
using System.Collections;

/**
 *  A basic ITextureSource implementation that renders a texture from a bitmap (UnityEngine.Color array)
 *  with no post-processing effects.
 */
public class TextureSource : ITextureSource {

	/// The class providing a bitmap to render.
    private IImageSource imageSource;

    /**
     * Instantiate a new TextureSource.
     * @param imageSource The IImageSource providing a bitmap every frame.
     */
    public TextureSource(IImageSource imageSource) {
		UltrasoundDebug.Assert(null != imageSource, "Null ImageSource used in constructor", this, true);
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

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
		OnionLogger.globalLog.PushInfoLayer("TextureSource");
		
		OnionLogger.globalLog.PushInfoLayer("Allocating ColorBitmap");
		ColorBitmap colorBitmap = new ColorBitmap();
		colorBitmap.width = texture.width;
		colorBitmap.height = texture.height;
		colorBitmap.colors = new Color[colorBitmap.width * colorBitmap.height];
		OnionLogger.globalLog.PopInfoLayer();

        imageSource.RenderColorImageInBitmap(ref colorBitmap);

		OnionLogger.globalLog.PushInfoLayer("Applying Bitmap to texture");
        texture.SetPixels(colorBitmap.colors);
        texture.Apply();
		OnionLogger.globalLog.PopInfoLayer();

		OnionLogger.globalLog.PopInfoLayer();
    }

}

using UnityEngine;

/**
 *  An interface describing a class that renders to a display Texture2D.
 */
public interface ITextureSource {

    /**
     *  Draw the next frame to a texture.
     *  @param texture The texture onto which the frame will be drawn.
     */
    void RenderNextFrameToTexture(ref Texture2D texture);

}

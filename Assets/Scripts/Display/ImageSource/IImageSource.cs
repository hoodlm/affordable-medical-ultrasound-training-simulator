using UnityEngine;
using System.Collections;

/**
 *  Interface describing a class that can provide a bitmap every frame.
 */
public interface IImageSource {

    /**
     *  Generate the next frame, and populate a provided bitmap.
     *  @param bitmap A ColorBitmap struct (Color[], width, height) to be populated for this frame.
     */
    void RenderColorImageInBitmap (ref ColorBitmap bitmap);
}

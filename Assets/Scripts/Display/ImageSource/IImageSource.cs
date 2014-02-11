using UnityEngine;
using System.Collections;

/**
 *  Interface describing a class that can provide a bitmap every frame.
 */
public interface IImageSource {

    /**
     *  Generate the next frame.
     *  @param width The requested width in pixels of the image.
     *  @param height The requested height in pixels of the image.
     *  @return A color array containing the pixels of the image.
     */
    Color[] BitmapWithDimensions (int width, int height);

}

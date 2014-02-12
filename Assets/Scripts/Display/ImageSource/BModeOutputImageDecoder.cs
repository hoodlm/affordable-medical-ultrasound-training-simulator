using UnityEngine;
using System.Collections;

/**
 *  An IImageSource that generates image data from an UltrasoundProbe%'s IProbeOutput.
 */
public class BModeOutputImageDecoder : IImageSource {

    private IProbeOutput probeOutput;
    private readonly static Color drawColor = Color.white;

    /**
     *  Instantiate a new ProbeOutputImageDecoder
     *  @param output The source of probedata from which to generate image data.
     */
    public BModeOutputImageDecoder(IProbeOutput output) {
        probeOutput = output;
    }

    public Color[] BitmapWithDimensions (int width, int height) {
        Color[] buffer = new Color[width * height];
        UltrasoundScanData data = probeOutput.SendScanData ();
        foreach (UltrasoundScanline scanline in data) {
            foreach (UltrasoundPoint point in scanline) {
                int index = MapScanningPlaneToPixelCoordinate (height, 
                                                                width, 
                                                                point.GetProjectedLocation (),
				                                                data.GetProbeConfig());
                DrawPoint (point, index, ref buffer, width * height);
            }
        }


        return buffer;
    }

    /**
     *  Draw a single UltrasoundPoint into a color buffer.
     *  @param point The UltrasoundPoint data to draw.
     *  @param index The index in the buffer at which to place the pixel.
     *  @param buffer An array of UnityEngine.Color representing the result image. 
     */
    private void DrawPoint(UltrasoundPoint point, int index, ref Color[] buffer, int bufferSize) {
        if (index >= bufferSize || index < 0) {
            return;
        }

        Color pointColor = drawColor;
        pointColor *= point.GetBrightness();
        buffer[index] = pointColor;
    }

    /**
     *  Maps a point in the probe's scanning plane to a pixel index in the image.
     *  @param imageHeight The height of the image being rendered.
     *  @param imageWidth The width of the image being rendered.
     *  @param vector2 The point in the scanning plane to be mapped.
     *  @return The corresponding index in the image at which to render the point.
     */
    private int MapScanningPlaneToPixelCoordinate(int imageHeight, 
	                                              int imageWidth, 
	                                              Vector2 vector2,
	                                              UltrasoundProbeConfiguration probeConfig) {

        float pixelsPerWorldUnit = Mathf.Min(imageWidth, imageHeight) / probeConfig.GetMaxScanDistance();
        //float pixelsPerWorldUnit = Mathf.Min (imageWidth, imageHeight) / 10;
        int xCenter = imageWidth / 2;

        int newX = (int)(xCenter - pixelsPerWorldUnit * vector2.x);
        int newY = imageHeight - (int)(pixelsPerWorldUnit * vector2.y);

        int pixelMapping = newY * imageWidth + newX;
        return pixelMapping;
    }
}

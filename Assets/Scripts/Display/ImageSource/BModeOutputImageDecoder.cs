using UnityEngine;
using System.Collections;

/**
 *  An IImageSource that generates image data from an UltrasoundProbe%'s IProbeOutput.
 */
public class BModeOutputImageDecoder : IImageSource {

	///	The IProbeOutput from which this BModeOutputImageDecoder receives data.
    protected IProbeOutput probeOutput;

	/// The color used for drawing the ultrasound image.
    protected Color drawColor;

    /**
     *  Instantiate a new ProbeOutputImageDecoder
     *  @param output The source of probedata from which to generate image data.
     */
    public BModeOutputImageDecoder(IProbeOutput output) {
		UltrasoundDebug.Assert(null != output, "Null probe output used in constructor", this);
		drawColor = Color.white;
        probeOutput = output;
    }

    public virtual Color[] BitmapWithDimensions (int width, int height) {
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
     *  @param bufferSize The number of pixels in the buffer.
     */
    protected virtual void DrawPoint(UltrasoundPoint point, int index, ref Color[] buffer, int bufferSize) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(index < bufferSize && index >= 0,
		                       string.Format("{0} should be in the interval[0, {1})", index, bufferSize),
		                       this);
		UltrasoundDebug.Assert(point.GetBrightness() >= 0f && point.GetBrightness() <= 1f,
		                       string.Format("Pixel brightness {0} should be in the interval [0,1]",
		              						 point.GetBrightness()),
								this);
		#endif
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
     * 	@param probeConfig The UltrasoundProbeConfiguration of the probe object.
     *  @return The corresponding index in the image at which to render the point.
     */
    protected int MapScanningPlaneToPixelCoordinate(int imageHeight, 
	                                              int imageWidth, 
	                                              Vector2 vector2,
	                                              UltrasoundProbeConfiguration probeConfig) {

        float pixelsPerWorldUnit = Mathf.Min(imageWidth, imageHeight) / probeConfig.GetMaxScanDistance();
        int xCenter = imageWidth / 2;

        int newX = (int)(xCenter - pixelsPerWorldUnit * vector2.x);
        int newY = imageHeight - (int)(pixelsPerWorldUnit * vector2.y);

        int pixelMapping = newY * imageWidth + newX;
        return pixelMapping;
    }
}

using UnityEngine;
using System.Collections;

/**
 *  An IImageSource that generates image data from an UltrasoundProbe%'s IProbeOutput.
 * 	This renders the ultrasound image inverted (black on white).
 */
public class BlackOnWhiteBModeOutputImageDecoder : BModeOutputImageDecoder {

	/**
     *  Instantiate a new BlackOnWhiteBModeOutputImageDecoder
     *  @param output The source of probedata from which to generate image data.
     */
	public BlackOnWhiteBModeOutputImageDecoder(IProbeOutput output) : base(output)
	{
		drawColor = Color.black;
	}

	public override Color[] BitmapWithDimensions (int width, int height) {
		Color[] buffer = new Color[width * height];

		for (int i = 0; i < buffer.Length; ++i) {
			buffer[i] = Color.white;
		}

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

	protected override void DrawPoint(UltrasoundPoint point, int index, ref Color[] buffer, int bufferSize) {
		#if UNITY_EDITOR
		UltrasoundDebug.Assert(index < bufferSize && index >= 0,
		                       string.Format("Pixel index {0} should be in the interval[0, {1})", 
		              						 index, bufferSize),
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
		pointColor += Color.white * (1 - point.GetBrightness());
		buffer[index] = pointColor;
	}
	
}

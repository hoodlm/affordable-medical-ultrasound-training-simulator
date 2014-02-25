using UnityEngine;
using System.Collections;

/**
 *  An IImageSource that generates image data from an UltrasoundProbe%'s IProbeOutput.
 * 	This renders the ultrasound image inverted (black on white).
 */
public class BlackOnWhiteBModeOutputImageDecoder : BModeOutputImageDecoder {

	private IImagePostProcessor inverter;

	public BlackOnWhiteBModeOutputImageDecoder(IProbeOutput output) : base(output) {
		inverter = new ColorInvert();
	}

	public override Color[] BitmapWithDimensions (int width, int height) {
		Color[] buffer = base.BitmapWithDimensions(width, height);
		inverter.ProcessBitmap(ref buffer, width, height);
		return buffer;
	}
}

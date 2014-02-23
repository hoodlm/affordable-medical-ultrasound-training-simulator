using UnityEngine;
using System.Collections;

/**
 * A fake IProbeOutput class to generate test probe data.
 */
public class TestProbeOutput : IProbeOutput {

	/// The left edge of the simulated scanning plane
    private static float MIN_X = -4f;
	/// The left edge of the simulated scanning plane
	private static float MAX_X = 4f;
	/// The near edge of the simulated scanning plane
    private static float MIN_Y = 1f;
	/// The far edge of the simulated scanning plane
    private static float MAX_Y = 9f;
	/// The "sampling" frequency (distance between points along scanlines)
    private static float STEPSIZE = 0.1f;

    public UltrasoundScanData SendScanData () {
		UltrasoundProbeConfiguration config = new UltrasoundProbeConfiguration();
		config.SetMaxScanDistance(MAX_Y);
		config.SetMinScanDistance(MIN_Y);
        UltrasoundScanData data = new UltrasoundScanData (config);
        for (float i = MIN_X; i <= MAX_X; i += STEPSIZE) {

            UltrasoundScanline scanline = new UltrasoundScanline (config.GetPosition());

            for (float j = MIN_Y; j <= MAX_Y; j += STEPSIZE) {
                UltrasoundPoint p = new UltrasoundPoint (Vector3.zero, new Vector2(i * (j / MAX_Y), j));
                p.SetBrightness (Random.Range(0f, 1f)); // Generate noise.
                scanline.AddUltrasoundPoint (p);
            }

            data.AddScanline (scanline);
        }

        return data;
    }
}

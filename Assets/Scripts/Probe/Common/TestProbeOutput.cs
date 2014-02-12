using UnityEngine;
using System.Collections;

/**
 * A fake IProbeOutput class to generate test probe data.
 */
public class TestProbeOutput : IProbeOutput {

    private static float MIN_X = -4f;
    private static float MAX_X = 4f;
    private static float MIN_Y = 1f;
    private static float MAX_Y = 9f;
    private static float STEPSIZE = 0.1f;

    public UltrasoundScanData Scan () {
		UltrasoundProbeConfiguration config = new UltrasoundProbeConfiguration();
		config.SetMinScanDistance(MIN_Y);
		config.SetMaxScanDistance(MAX_Y);
        UltrasoundScanData data = new UltrasoundScanData (config);
        for (float i = MIN_X; i <= MAX_X; i += STEPSIZE) {

            UltrasoundScanline scanline = new UltrasoundScanline ();

            for (float j = MIN_Y; j <= MAX_Y; j += STEPSIZE) {
                UltrasoundPoint p = new UltrasoundPoint (Vector3.zero, new Vector2(i * (j / MAX_Y), j));
                p.SetBrightness (Random.Range(0f, 1f));
                scanline.AddUltrasoundPoint (p);
            }

            data.AddScanline (scanline);
        }

        return data;
    }
}

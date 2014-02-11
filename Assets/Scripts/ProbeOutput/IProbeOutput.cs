/**
 *  An interface to simulate the output of an UltrasoundProbe.
 *  The output yields a single UltrasoundScanData object.
 */
public interface IProbeOutput {

    /**
     *  Instruct the probe to scan this frame.
     *  @return An UltrasoundScanData object, containing data about the scan.
     */
    UltrasoundScanData Scan ();

}
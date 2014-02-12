/**
 *  An interface to simulate the output of an UltrasoundProbe.
 *  The output yields a single UltrasoundScanData object.
 */
public interface IProbeOutput {

    /**
     *  Send the results of the probe scanning this frame.
     *  @return An UltrasoundScanData object, containing data about the scan.
     */
    UltrasoundScanData SendScanData ();

}
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/** 
 *  All the information collected by an UltrasoundProbe in a single frame.
 */
public class UltrasoundScanData : IEnumerable<UltrasoundScanline> {

    private IList<UltrasoundScanline> scanlines;
	private readonly UltrasoundProbeConfiguration probeConfig;
    
    /** 
     *  Instantiate a new instance of UltrasoundScanData.
     * 	@param config The configuration settings associated with the probe object.
     */
    public UltrasoundScanData(UltrasoundProbeConfiguration config) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(null != config, "Null config used in constructor", this);
#endif
		this.probeConfig = config;
        scanlines = new List<UltrasoundScanline>();
    }
    
    /** 
     *  Add a collection of UltrasoundScanline%s to this scanline.
     *
     *  @param scanlines The UltrasoundScanline%s to add.
     *  @throw ArgumentNullException
     */
    public void AddScanlines (ICollection<UltrasoundScanline> scanlines) {
#if UNITY_EDITOR
        UltrasoundDebug.Assert(null != scanlines, 
		                       "A null collection of scanlines was added to this UltrasoundScanData",
		                       this);
#endif
        foreach (UltrasoundScanline s in scanlines) {
            AddScanline(s);
        }
    }
    
    /** 
     *  Add an UltrasoundPoint to this scanline.
     *
     *  @param s The UltrasoundPoint to add.
     *  @throw ArgumentNullException
     */
    public void AddScanline (UltrasoundScanline s) {
#if UNITY_EDITOR
        UltrasoundDebug.Assert(null != s, 
		                       "A null scanline was added to this UltrasoundScanData",
		                       this);
#endif
        scanlines.Add(s);
    }
    
    /** 
     *  Get read-only collection of the scanlines.
     */
    public ReadOnlyCollection<UltrasoundScanline> GetScanlines() {
        return new ReadOnlyCollection<UltrasoundScanline>(scanlines);
    }

	/**
	 * 	Returns a (deep) copy of the configuration of the probe that generated this UltrasoundScanData.
	 * 	@return The UltrasoundProbeConfiguration object describing the configuration of the probe.
	 */
	public UltrasoundProbeConfiguration GetProbeConfig() {
		return new UltrasoundProbeConfiguration(probeConfig);
	}

    /**
     *  An iterator over all the points in this scanline.
     */
    public IEnumerator<UltrasoundScanline> GetEnumerator() {
        foreach (UltrasoundScanline scanline in scanlines) {
            yield return scanline;
        }
    }

    /*    *
     *  An iterator over all the points in this scanline.
     */
    IEnumerator IEnumerable.GetEnumerator() {
        foreach (UltrasoundScanline scanline in scanlines) {
            yield return scanline;
        }
    }
}

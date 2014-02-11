using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/** 
 *  All the information collected by an UltrasoundProbe in a single frame.
 */
public class UltrasoundScanData : IEnumerable<UltrasoundScanline> {

    private IList<UltrasoundScanline> scanlines;
    
    /** 
     *  Instantiate a new instance of UltrasoundScanData.
     */
    public UltrasoundScanData() {
        scanlines = new List<UltrasoundScanline>();
    }
    
    /** 
     *  Add a collection of UltrasoundScanline%s to this scanline.
     *
     *  @param scanlines The UltrasoundScanline%s to add.
     *  @throw ArgumentNullException
     */
    public void AddScanlines (ICollection<UltrasoundScanline> scanlines) {
        UltrasoundInputValidator.CheckNotNull(scanlines);
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
        UltrasoundInputValidator.CheckNotNull(s);
        scanlines.Add(s);
    }
    
    /** 
     *  Get read-only collection of the scanlines.
     */
    public ReadOnlyCollection<UltrasoundScanline> GetScanlines() {
        return new ReadOnlyCollection<UltrasoundScanline>(scanlines);
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

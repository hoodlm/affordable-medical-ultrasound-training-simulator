using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

/** 
 *  A scanline wraps a list of UltrasoundPoint%s along the same trajectory in space.
 */
public class UltrasoundScanline : IEnumerable<UltrasoundPoint>{

	private IList<UltrasoundPoint> points;
	public readonly Vector3 origin;
    
    /** 
     *  Instantiate a new instance of UltrasoundScanline.
     */
    public UltrasoundScanline(Vector3 origin) {
		this.origin = origin;
        points = new List<UltrasoundPoint>();
    }
    
    /** 
     *  Add a collection of UltrasoundPoint%s to this scanline.
     *
     *  @param points The UltrasoundPoint%s to add.
     *  @throw ArgumentNullException
     */
    public void AddUltrasoundPoints (ICollection<UltrasoundPoint> points) {
#if UNITY_EDITOR
        UltrasoundDebug.Assert(null != points, 
		                       "A null collection of points was added to a Scanline",
		                       this);
#endif
        foreach (UltrasoundPoint p in points) {
            AddUltrasoundPoint(p);
        }
    }
    
    /** 
     *  Add an UltrasoundPoint to this scanline.
     *
     *  @param p The UltrasoundPoint to add.
     *  @throw ArgumentNullException
     */
    public void AddUltrasoundPoint (UltrasoundPoint p) {
#if UNITY_EDITOR
        UltrasoundDebug.Assert(null != p, 
		                       "A null UltrasoundPoint was added to a Scanline",
		                       this);
#endif
        points.Add(p);
    }
    
    /** 
     *  Return a read-only collection of the points in this scanline.
     */
    public ReadOnlyCollection<UltrasoundPoint> GetPoints() {
        return new ReadOnlyCollection<UltrasoundPoint>(points);
    }

    /**
     *  An iterator over all the points in this scanline.
     */
    public IEnumerator<UltrasoundPoint> GetEnumerator() {
        foreach (UltrasoundPoint point in points) {
            yield return point;
        }
    }

    /**
     *  An iterator over all the points in this scanline.
     */
    IEnumerator IEnumerable.GetEnumerator() {
        foreach (UltrasoundPoint point in points) {
            yield return point;
        }
    }

	/// Returns a string that represents the current UltrasoundScanline.
	public override string ToString ()
	{
		string str = "SCANLINE";
		foreach (UltrasoundPoint point in points) {
			str = str + string.Format("\n{0}", point);
		}
		return str;
	}
}

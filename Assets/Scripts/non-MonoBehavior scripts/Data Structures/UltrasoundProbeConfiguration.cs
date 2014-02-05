using UnityEngine;
using System.Collections;

/** 
 *  Holds the configuration of the current probe: that is, all the "settings" (master brightness, scanning
 *  depth, etc.) as well as the current position and orientation of the probe GameObject.
 */
public class UltrasoundProbeConfiguration {

    private Transform probeTransform;
    private float maxDistance;
    private float minDistance;

    /** 
     *  Copy constructor to instantiate a new UltrasoundProbeConfiguration from another.
     *
     *  @param config The other UltrasoundProbeConfiguration object.
     *  @throw ArgumentNullException
     */
    public UltrasoundProbeConfiguration (UltrasoundProbeConfiguration config) {
        UltrasoundInputValidator.CheckNotNull(config);
        this.SetTransform(config.GetTransform());
        this.SetMaxScanDistance(config.GetMaxScanDistance());
        this.SetMinScanDistance(config.GetMinScanDistance());
    }
    
    /** 
     *  Instantiate a new UltrasoundProbeConfiguration with a UnityEngine.Transform.
     *  Initial min scanning distance is float.Epsilon (1.40E-45), max scanning distance is 10.
     *  @param probeTransform The UnityEngine.Transform of the probe GameObject.
     *  @throw ArgumentNullException
     */
    public UltrasoundProbeConfiguration (Transform probeTransform) {
        this.SetTransform(probeTransform);
        this.SetMinScanDistance(float.Epsilon);
        this.SetMaxScanDistance(10f);
    }
    
    /** 
     *  Sets the probe transform (position and orientation).
     *  @param t The UnityEngine.Transform of the probe GameObject.
     *  @throw ArgumentNullException
     */
    public void SetTransform (Transform t) {
        UltrasoundInputValidator.CheckNotNull(t);
        this.probeTransform = t;
    }

    /** 
     *  Get the probe transform (position and orientation).
     */
    public Transform GetTransform() {
        return probeTransform;
    }
    
    /** 
     *  Sets the minimum scanning distance of the probe.
     *  @param min Clamped within the (exclusive) interval 0 to Positive Infinity.
     */
    public void SetMinScanDistance(float min) {
        maxDistance = Mathf.Clamp(min, float.Epsilon, float.MaxValue);
    }
    
    /** 
     *  Get the minimum scanning distance of the probe.
     *  @return a float value in the (exclusive) interval 0 to Positive Infinity.
     */
    public float GetMinScanDistance() {
        return maxDistance;
    }
    
    /** 
     *  Sets the maximum scanning distance of the probe.
     *  @param max Clamped within the (exclusive) interval 0 to Positive Infinity.
     */
    public void SetMaxScanDistance(float max) {
        minDistance = Mathf.Clamp(max, float.Epsilon, float.MaxValue);
    }
    
    /** 
     *  Get the maximum scanning distance of the probe.
     *  @return a float value in the (exclusive) interval 0 to Positive Infinity.
     */
    public float GetMaxScanDistance() {
        return minDistance;
    }
}

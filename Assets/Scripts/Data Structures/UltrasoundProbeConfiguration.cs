using UnityEngine;

/**
 *	Struct for convenience to store position and rotation info from a GameObject's transform.
 */
public struct PositionAndRotation {
	/// The position in world space of a probe.
	public Vector3 position;

	/// The Quaternion representation of the rotation in world space of the probe.
	public Quaternion rotation;
}

/** 
 *  Holds the configuration of the current probe: that is, all the "settings" (master brightness, scanning
 *  depth, etc.) as well as the current position and orientation of the probe GameObject.
 */
public class UltrasoundProbeConfiguration {

	private PositionAndRotation positionAndRotation;
    private float maxDistance;
    private float minDistance;

	/** 
     *  Instantiate a new UltrasoundProbeConfiguration with default values.
     *  Initial min scanning distance is float.Epsilon (1.40E-45), max scanning distance is 10.\
     */
	public UltrasoundProbeConfiguration () {
		this.SetPosition(Vector3.zero);
		this.SetRotation(Quaternion.AngleAxis(0f, Vector3.forward));
		this.SetMinScanDistance(float.Epsilon);
		this.SetMaxScanDistance(10f);
	}

    /** 
     *  Copy constructor to instantiate a new UltrasoundProbeConfiguration from another.
     *
     *  @param config The other UltrasoundProbeConfiguration object.
     *  @throw ArgumentNullException
     */
    public UltrasoundProbeConfiguration (UltrasoundProbeConfiguration config) {
        UltrasoundInputValidator.CheckNotNull(config);
		this.SetPosition(config.GetPosition());
		this.SetRotation(config.GetRotation());
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
		UltrasoundInputValidator.CheckNotNull(probeTransform);
		this.SetRotation(probeTransform.rotation);
		this.SetPosition(probeTransform.position);
        this.SetMinScanDistance(float.Epsilon);
        this.SetMaxScanDistance(10f);
    }

	/** 
     *  Sets the probe rotation.
     *  @param rotation The rotation of the probe GameObject in world space.
     */
	public void SetRotation (Quaternion rotation) {
		this.positionAndRotation.rotation = rotation;
	}

	/** 
     *  Gets the probe rotation.
     *  @return The rotation of the probe GameObject in world space.
     */
	public Quaternion GetRotation () {
		return this.positionAndRotation.rotation;
	}
	
	/** 
     *  Sets the probe position.
     *  @param position The position of the probe GameObject in world space.
     */
	public void SetPosition (Vector3 position) {
		this.positionAndRotation.position = position;
	}

	/** 
     *  Gets the probe position.
     *  @return The position of the probe GameObject in world space.
     */
	public Vector3 GetPosition () {
		return this.positionAndRotation.position;
	}
    
    /** 
     *  Sets the minimum scanning distance of the probe.
     *  @param min Clamped within the (exclusive) interval 0 to Positive Infinity.
     */
    public void SetMinScanDistance(float min) {
        minDistance = Mathf.Clamp(min, float.Epsilon, float.MaxValue);
    }
    
    /** 
     *  Get the minimum scanning distance of the probe.
     *  @return a float value in the (exclusive) interval 0 to Positive Infinity.
     */
    public float GetMinScanDistance() {
        return minDistance;
    }
    
    /** 
     *  Sets the maximum scanning distance of the probe.
     *  @param max Clamped within the (exclusive) interval 0 to Positive Infinity.
     */
    public void SetMaxScanDistance(float max) {
        maxDistance = Mathf.Clamp(max, float.Epsilon, float.MaxValue);
    }
    
    /** 
     *  Get the maximum scanning distance of the probe.
     *  @return a float value in the (exclusive) interval 0 to Positive Infinity.
     */
    public float GetMaxScanDistance() {
        return maxDistance;
    }
}

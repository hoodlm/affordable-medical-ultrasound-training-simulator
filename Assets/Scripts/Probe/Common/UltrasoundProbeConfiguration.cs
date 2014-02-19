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
	private float arcSizeInDegrees;
	private float gain;
	private int numberOfScanlines;
	private int pointsPerScanline;

	/** 
     *  Instantiate a new UltrasoundProbeConfiguration with default values.
     *  	minDistance: float.Epsilon (1.40E-45)
     * 		maxDistance: 10.0
     * 		arcSizeInDegrees: 75
     * 		numberOfScanlines: 40
     * 		pointsPerScanline: 40
     * 		gain: 1.0
     */
	public UltrasoundProbeConfiguration () {
		this.SetPosition(Vector3.zero);
		this.SetRotation(Quaternion.AngleAxis(0f, Vector3.forward));
		this.minDistance = float.Epsilon;
		this.maxDistance = 10f;
		this.arcSizeInDegrees = 75;
		this.numberOfScanlines = 40;
		this.pointsPerScanline = 40;
		this.gain = 1.0f;
	}

    /** 
     *  Copy constructor to instantiate a new UltrasoundProbeConfiguration from another.
     *
     *  @param config The other UltrasoundProbeConfiguration object.
     *  @throw ArgumentNullException
     */
    public UltrasoundProbeConfiguration (UltrasoundProbeConfiguration config) {
        UltrasoundDebug.Assert(null != config, 
							   "Null UltrasoundProbeConfiguration used for copy constructor.",
		                       this);
		this.SetPosition(config.GetPosition());
		this.SetRotation(config.GetRotation());

        this.maxDistance 			= config.GetMaxScanDistance();
        this.minDistance 			= config.GetMinScanDistance();
		this.arcSizeInDegrees 		= config.GetArcSizeInDegrees();
		this.pointsPerScanline 		= config.GetPointsPerScanline();
		this.numberOfScanlines 		= config.GetNumberOfScanlines();
		this.gain					= config.GetGain();
    }
    
    /** 
     *  Instantiate a new UltrasoundProbeConfiguration with a UnityEngine.Transform.
     * 	Other configuration values use the defaults:
     * 
     *  - minDistance: float.Epsilon (1.40E-45)
     * 	- maxDistance: 10.0
     * 	- arcSizeInDegrees: 75
     * 	- numberOfScanlines: 40
     * 	- pointsPerScanline: 40
     * 	- gain: 1.0f
     * 
     *  @param probeTransform The UnityEngine.Transform of the probe GameObject.
     */
    public UltrasoundProbeConfiguration (Transform probeTransform) {
		UltrasoundDebug.Assert(null != probeTransform,
		                       "Null transform passed to UltrasoundProbeConfiguration constructor.",
		                       this);
		this.SetRotation(probeTransform.rotation);
		this.SetPosition(probeTransform.position);

		this.minDistance 		= float.Epsilon;
		this.maxDistance 		= 10f;
		this.arcSizeInDegrees 	= 75;
		this.numberOfScanlines 	= 40;
		this.pointsPerScanline 	= 40;
		this.gain				= 1.0f;
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
#if UNITY_EDITOR
		UltrasoundDebug.Assert(maxDistance > minDistance, 
		                       string.Format("SetMinScanDistance:" +
		                       				 "Max distance ({0}) should be greater than min distance ({1})!", 
		              						  maxDistance, minDistance),
		                       this);
#endif
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
#if UNITY_EDITOR
		UltrasoundDebug.Assert(maxDistance > minDistance, 
		                       string.Format("SetMaxScanDistance:" +
		                       				 "Max distance ({0}) should be greater than min distance ({1})!", 
		              						 maxDistance, minDistance),
		                       this);
#endif
    }
    
    /** 
     *  Get the maximum scanning distance of the probe.
     *  @return a float value in the (exclusive) interval 0 to Positive Infinity.
     */
    public float GetMaxScanDistance() {
        return maxDistance;
    }

	/**
	 * 	Sets the size of the scanning arc, measured in degrees.
	 * 	@param degrees A float value between 0 and 180 (inclusive).
	 */
	public void SetArcSizeInDegrees(float degrees) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(degrees >= 0f && degrees <= 180f,
		                       string.Format("Tried to set degrees to {0}", degrees),
		                       this, false);
#endif
		arcSizeInDegrees = Mathf.Clamp(degrees, 0f, 180f);
	}

	/**
	 *	Get the size of the scanning arc (measured in degrees).
	 *	@return a float value in the interval 0 to 180.
	 */
	public float GetArcSizeInDegrees() {
		return arcSizeInDegrees;
	}

	/**
	 * 	Set the number of scanlines to scan.
	 * 	@param count A positive integer value.
	 */
	public void SetNumberOfScanlines(int count) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(count > 0,
		                       string.Format ("Tried to set scanline count to {0}", count),
		                       this, false);
#endif
		numberOfScanlines = Mathf.Clamp(count, 0, int.MaxValue);
	}

	/**
	 * 	Get the number of scanlines to scan.
	 * 	@return A positive integer value.
	 */
	public int GetNumberOfScanlines() {
		return numberOfScanlines;
	}

	/**
	 * 	Sets the number of points to check per scanline.
	 * 	@param count A positive integer value.
	 */
	public void SetPointsPerScanline(int count) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(count > 0,
		                       string.Format("Tried to set points per scanline to {0}", count),
		                       this, false);
#endif
		pointsPerScanline = count;
	}

	/**
	 * 	Get the number of points per scanline.
	 * 	@return A positive integer value.
	 */
	public int GetPointsPerScanline() {
		return pointsPerScanline;
	}

	/**
	 * 	Set the gain of this probe.
	 * 
	 * 	@param gain A non-negative float value.
	 */
	public void SetGain(float gain) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert (gain > 0f,
		                        string.Format("Tried to set gain to {0}", gain),
		                        this);
#endif
		this.gain = gain;
	}

	/**
	 * 	Get the gain for this probe.
	 * 
	 * 	@return A non-negative float value.
	 */
	public float GetGain() {
		return this.gain;
	}
}

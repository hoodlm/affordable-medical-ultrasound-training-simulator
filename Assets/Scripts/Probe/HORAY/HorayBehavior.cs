using UnityEngine;
using System.Collections;

/**
 *	The MonoBehaviour script to attach to the GameObject representing an Ultrasound Probe.
 *	Used to set various configuration properties through the inspector.
 */
public class HorayBehavior : MonoBehaviour {

	/// The maximum scanning distance of this probe.
	public float MaxDistance = 10;

	/// The minimum scanning distance of this probe.
	public float MinDistance = 1;

	/// The "brightness" of the pulses sent by the probe.
	public float Intensity = 1.0f;

	/// The size of the scanning arc for the probe.
	public float ArcSizeInDegrees = 75;

	/// The total number of scanlines to scan.
	public int NumberOfScanlines = 40;

	/// The total number of points to check per scanline.
	public int PointsPerScanline = 40;

	/// The IProbeOutput responsible for returning this probe's data to an IImageSource.
	protected IProbeOutput output;

	/// The instance of HorayProbe that houses all of the scanning logic and produces UltrasoundScanData.
	protected HorayProbe dataSource;

	// Use this for initialization
	void Start () {
		// Some input validation
#if UNITY_EDITOR
		UltrasoundDebug.Assert(NumberOfScanlines > 0, 
		                       "Number of scanlines should be a positive integer",
		                       this, true);
		UltrasoundDebug.Assert(PointsPerScanline > 0, 
		                       "Points per scanline should be a positive integer",
		                       this, true);
		UltrasoundDebug.Assert(ArcSizeInDegrees >= 0 && ArcSizeInDegrees <= 180f, 
		                       "Arc size should be between 0 and 180 degrees.",
		                       this, true);
		UltrasoundDebug.Assert(MinDistance > 0, 
		                       "Min distance should be greater than 0",
		                       this, true);
		UltrasoundDebug.Assert(MaxDistance > 0, 
		                       "Max distance should be greater than 0",
		                       this, true);
		UltrasoundDebug.Assert(MinDistance < MaxDistance, 
		                       "Min distance should be smaller than max distance",
		                       this, true);
#endif

		output = new HorayProbeOutput(this.gameObject);
		dataSource = new HorayProbe(this.gameObject, output);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 *	@return The IProbeOutput associated with this ultrasound probe.
	 */
	public IProbeOutput GetOutput() {
		return this.output;
	}

	/**
	 * 	Retrieve the current probe configuration.
	 *	@return The UltrasoundProbeConfiguration of the probe GameObject.
	 */
	public UltrasoundProbeConfiguration GetProbeConfig() {
		UltrasoundProbeConfiguration config = new UltrasoundProbeConfiguration(this.transform);
		config.SetMaxScanDistance(this.MaxDistance);
		config.SetMinScanDistance(this.MinDistance);
		config.SetArcSizeInDegrees(this.ArcSizeInDegrees);
		config.SetNumberOfScanlines(this.NumberOfScanlines);
		config.SetPointsPerScanline(this.PointsPerScanline);
		return config;
	}
}

using UnityEngine;
using System.Collections;

/**
 *	The MonoBehaviour script to attach to the actual GameObject representing an Ultrasound Probe.
 */
public class HorayBehavior : MonoBehaviour {

	/// The maximum scanning distance of this probe.
	public float MaxDistance;

	/// The minimum scanning distance of this probe.
	public float MinDistance;

	/// The IProbeOutput responsible for returning this probe's data to an IImageSource.
	protected IProbeOutput output;

	/// The instance of HorayProbe that houses all of the scanning logic and produces UltrasoundScanData.
	protected HorayProbe dataSource;

	// Use this for initialization
	void Start () {
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
		return config;
	}
}

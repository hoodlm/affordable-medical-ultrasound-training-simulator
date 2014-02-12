using UnityEngine;
using System.Collections;

/**
 *	The MonoBehaviour script to attach to the actual GameObject representing an Ultrasound Probe.
 */
public class HorayBehavior : MonoBehaviour {

	public float MaxDistance;
	public float MinDistance;

	protected IProbeOutput output;
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

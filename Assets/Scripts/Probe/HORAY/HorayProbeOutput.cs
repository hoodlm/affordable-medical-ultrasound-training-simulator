using UnityEngine;
using System.Collections;

/**
 *	HOmogeneous RAYcasting ultrasound model.
 *	This is a very basic ultrasound model to simulate B-Mode ultrasound imaging.
 *	Organs are modelled as a set of GameObjects, and each individual GameObject represents
 *	a homogeneous tissue type (that is, every point inside the tissue has uniform echogenicity).
 */
public class HorayProbeOutput : IProbeOutput {

	/// The GameObject representing the physical location of the probe.
	protected readonly GameObject probeGameObject;

	/// The instance of HorayProbe that houses all of the scanning logic and produces UltrasoundScanData.
	protected readonly HorayProbe probe;

	/**
	 * 	Instantiate a new HorayProbeOutput.
	 *	@param gameObject The probe from which data will be transmitted.
	 *	@throw ArgumentException If the probe object does not have the correct components.
	 */
	public HorayProbeOutput(GameObject gameObject) {
		UltrasoundDebug.Assert(null != gameObject, 
		                       "Null GameObject passed to HorayProbeOutput constructor.",
		                       this);

		if (null == gameObject.GetComponent<HorayBehavior>()) {
			string str = 
				"The probe object used to instantiate this output " +
				"does not have the required HorayBehavior script.";
			Debug.LogError(str);
		}
		this.probeGameObject = gameObject;
		probe = new HorayProbe(probeGameObject, this);
	}

	public UltrasoundScanData SendScanData () {
		UltrasoundProbeConfiguration currentConfig =
			probeGameObject.GetComponent<HorayBehavior>().GetProbeConfig();
		UltrasoundScanData data = new UltrasoundScanData(currentConfig);
		probe.PopulateData(ref data);
		return data;
	}


}

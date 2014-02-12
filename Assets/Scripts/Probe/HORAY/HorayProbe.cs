using UnityEngine;
using System.Collections;

/**
 *	Simulate the functionality of an Ultrasound probe.
 */
public class HorayProbe {

	protected readonly GameObject probeGameObject;
	protected readonly IProbeOutput output;

	public HorayProbe(GameObject probe, IProbeOutput output) {
		UltrasoundInputValidator.CheckNotNull(probe);
		UltrasoundInputValidator.CheckNotNull(output);
		if (null == probe.GetComponent<HorayBehavior>()) {
			string str = 
				"The probe object used to instantiate this class " +
				"does not have the required HorayBehavior script.";
			throw new System.ArgumentException(str);
		}
		this.probeGameObject = probe;
		this.output = output;
	}
}

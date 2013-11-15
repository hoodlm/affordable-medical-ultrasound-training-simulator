using UnityEngine;
using System.Collections;

/// <summary>
/// Various helper methods for simulating ultrasound pulses.
/// </summary>
public static class UltrasoundPhysics {

	// The process of finding organs in a radius is expensive, so we cache this and only check it periodically.
	private static Collider[] cachedPossibleOrgans;
	private static UltrasoundPulse cachedPulse;
	
	/// <summary>
	/// Sends a pulse to a single point and returns information about that point.
	/// Note that the point's projected position is not set. This is the responsibility
	/// of the probe, since information about the probe's orientation is necessary for projection.
	/// </summary>
	public static UltrasoundPoint SendPulse(UltrasoundPulse pulse) {
		Collider[] organs = FindPossibleOrgans(pulse);
		foreach (Collider collider in organs) {
			if (UltrasoundCollisionUtils.IsContained(pulse.GetTarget(), collider)) {
				UltrasoundOrgan organ = GetOrganFromCollider(collider);
				if (null != organ && organ.enabled) {
					UltrasoundPoint result = new UltrasoundPoint();
					result.SetOrgan(organ);
					result.SetIntensity(CalculateIntensityAtPoint(organ, pulse.GetIntensity()));
					result.SetPosition(pulse.GetTarget());
					
					// If the point is too "dark" to see, then for all intents and purposes we should consider it empty.
					if (result.GetIntensity() > 0f) {
						result.SetEmpty(false);
					} else {
						result.SetEmpty(true);
					}
					
					return result;
				}
			}
		}
		
		// No colliders are at this point
		return UltrasoundPoint.EmptyPoint(pulse.GetTarget());
	}
	
	/// <summary>
	/// Finds the possible organs at a point.
	/// </summary>
	private static Collider[] FindPossibleOrgans(UltrasoundPulse pulse) {
		if (null == cachedPossibleOrgans || !pulse.Equals(cachedPulse)) {
			RaycastHit[] hits = 
				Physics.RaycastAll(pulse.GetOrigin(), pulse.GetTarget() - pulse.GetOrigin(), pulse.GetLength());
			cachedPossibleOrgans = new Collider[hits.Length];
			for (int i = 0; i < hits.Length; ++i) {
				cachedPossibleOrgans[i] = hits[i].collider;
			}
			cachedPulse = pulse;
		}
		
		return cachedPossibleOrgans;
	}
	
	/// <summary>
	/// Gets the organ from collider, if it has one.
	/// </summary>
	private static UltrasoundOrgan GetOrganFromCollider(Collider collider) {
		return collider.gameObject.GetComponent<UltrasoundOrgan>();
	}
	
	/// <summary>
	/// Calculates the intensity of sound reflected from a point.
	/// </summary>
	/// <param name='organ'>
	/// The type of organ at the point.
	/// </param>
	/// <param name='pulseIntensity'>
	/// The intensity of the pulse striking the point.
	/// </param>
	private static float CalculateIntensityAtPoint(UltrasoundOrgan organ, float pulseIntensity) {
		float intensity = (pulseIntensity * organ.echogenicity) - 0.02f * organ.attenuation;
		return Mathf.Clamp(intensity, 0f, 1f);
	}
}

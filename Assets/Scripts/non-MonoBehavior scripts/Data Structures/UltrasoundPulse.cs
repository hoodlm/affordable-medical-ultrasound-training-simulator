using UnityEngine;
using System.Collections;

/// <summary>
/// A data structure to represent a single ultrasound pulse returning from a point in space.
/// </summary>
public class UltrasoundPulse {
	
	private Vector3 origin;
	private Vector3 target;
	private float intensity;
	
	public UltrasoundPulse(Vector3 origin, Vector3 target, float intensity) {
		this.SetOrigin(origin);
		this.SetTarget(target);
		this.SetIntensity(intensity);
	}
	
	/// <summary>
	/// Set the origin in world coordinates.
	/// </summary>
	public void SetOrigin(Vector3 origin) {
		this.origin = origin;
	}
	
	/// <summary>
	/// Sets the target in world coordinates.
	/// </summary>
	public void SetTarget(Vector3 target) {
		this.target = target;
	}
	
	/// <summary>
	/// Sets the intensity of the sound pulse.
	/// This value will be clamped to be non-negative.
	/// </summary>
	public void SetIntensity(float intensity) {
		if (intensity < 0f) {
			string warningString = "Tried to set an intensity less than 0.";
			Debug.LogWarning(warningString);
		}
		this.intensity = Mathf.Clamp(intensity, 0f, float.PositiveInfinity);
	}
	
	/// <summary>
	/// Returns the origin in world coordinates.
	/// </summary>
	public Vector3 GetOrigin() {
		return this.origin;
	}
	
	/// <summary>
	/// Returns the target in world coordinates.
	/// </summary>
	public Vector3 GetTarget() {
		return this.target;
	}
	
	/// <summary>
	/// Returns the intensity of the sound pulse.
	/// This value is guaranteed to be non-negative.
	/// </summary>
	public float GetIntensity() {
		return this.intensity;
	}
	
	/// <summary>
	/// Calculate the distance between the target and the origin points of the pulse.
	/// </summary>
	public float GetLength() {
		return Vector3.Distance(this.target, this.origin);
	}
}

using UnityEngine;
using System.Collections;

/// <summary>
/// An object to describe a point in 3D world space.
/// </summary>
public class UltrasoundPoint : System.IComparable {
	
	private Vector3 position;
	private Vector2 projectedPosition;
	private bool empty;
	private UltrasoundOrgan organ;
	private float intensity;
	
	/// <summary>
	/// Factory method to generate an empty UltrasoundPoint at a given position in world space.
	/// </summary>
	public static UltrasoundPoint EmptyPoint(Vector3 position) {
		UltrasoundPoint point = new UltrasoundPoint();
		point.SetEmpty(true);
		point.SetIntensity(0f);
		point.SetPosition(position);
		
		return point;
	}
	
	/// <summary>
	/// Sets the absolute position in world space of this UltrasoundPoint.
	/// </summary>
	public void SetPosition(Vector3 position) {
		this.position = position;
	}
	
	/// <summary>
	/// Sets the location of this point as it is projected into the probe's scanning plane.
	/// </summary>
	public void SetProjectedPosition(Vector2 projectedPosition) {
		this.projectedPosition = projectedPosition;
	}
	
	/// <summary>
	/// Specify whether any GameObject exists at this point.
	/// </summary>
	public void SetEmpty(bool empty) {
		this.empty = empty;
	}
	
	/// <summary>
	/// Sets the type of organ that exists at this point. This field can be null.
	/// </summary>
	public void SetOrgan(UltrasoundOrgan organ) {
		this.organ = organ;
	}
	
	/// <summary>
	/// Sets the intensity of the simulated sound wave reflected from this point.
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
	/// Returns the absolute position in world space of this UltrasoundPoint.
	/// </summary>
	public Vector3 GetPosition() {
		return this.position;
	}
	
	/// <summary>
	/// Returns the location of this point as it is projected into the probe's scanning plane.
	/// </summary>
	public Vector2 GetProjectedPosition() {
		return this.projectedPosition;
	}
	
	/// <summary>
	/// Returns whether any GameObject exists at this point.
	/// </summary>
	public bool IsEmpty() {
		return this.empty;
	}
	
	/// <summary>
	/// Returns the type of organ that exists at this point. This field can be null.
	/// </summary>
	public UltrasoundOrgan GetUltrasoundOrgan() {
		return this.organ;
	}
	
	/// <summary>
	/// Returns the intensity of the simulated sound wave reflected from this point.
	/// This value is guaranteed to be non-negative.
	/// </summary>
	public float GetIntensity() {
		return this.intensity;	
	}
	
	/// <summary>
	/// Gets a seed between 0 and 1, based on the point's 3D position, to be used for deterministic noise generation.
	/// </summary>
	public float GetSeed() {
		int hash = Mathf.Abs(this.position.GetHashCode());
		float seed = (float)(hash % 255) / 255f;
		return seed;
	}
	
	/// <summary>
	/// Modifies the intensity of this point to account for noise.
	/// Note that this is not an idempotent operation!
	/// </summary>
	/// <param name='noiseLevel'>
	/// The maximum noise level.
	/// </param>
	public void ApplyNoise(float noiseLevel) {
		if (noiseLevel > 0f) {
			this.intensity = Mathf.Lerp(intensity, this.GetSeed(), noiseLevel);
		}
	}
	
	/// <summary>
	/// Compares this point to another point, ordered by Y coordinate as most significant, then by X coordinate.
	/// </summary>
	public int CompareTo(object obj) {
        UltrasoundInputValidator.CheckNotNull(obj);

        UltrasoundPoint otherPoint = obj as UltrasoundPoint;
        if (otherPoint == null) {
           throw new System.ArgumentException("Compared object is not an UltrasoundPoint");
		} else {
			// We consider the Y coordinate to be the most significant in comparsion.
			int yComparison = this.GetProjectedPosition().y.CompareTo(otherPoint.GetProjectedPosition().y);
			if (yComparison != 0) {
				return yComparison;
			} else {
				int xComparison = this.GetProjectedPosition().x.CompareTo(otherPoint.GetProjectedPosition().x);
				return xComparison;
			}
		}
    }
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="UltrasoundPoint"/> as tab-separated X,Y coordinates.
	/// </summary>
	public override string ToString () {
		return string.Format ("{0} \t {1}", this.GetProjectedPosition().x, this.GetProjectedPosition().y);
	}
}

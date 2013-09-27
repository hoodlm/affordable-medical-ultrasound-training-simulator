using UnityEngine;
using System.Collections;

/// <summary>
/// POCO encapsulating all the settings associated with an ultrasound probe.
/// </summary>
public class ProbeConfiguration : MonoBehaviour {

	public float minDistance;
	public float maxDistance;
	public int pointsPerScanline;
	public float arcSize;
	public int scanlineCount;
	public float noiseLevel;
	
	/// <summary>
	/// Sets the minimum scanning distance. Clamped to be greater than zero and less than the value of maxDistance.
	/// </summary>
	public void SetMinDistance(float minDistance) {
		this.minDistance = Mathf.Clamp(minDistance, float.Epsilon, this.GetMaxDistance());
	}
	
	/// <summary>
	/// Sets the maximum scanning distance. Clamped to be greater than the value of minDistance.
	/// </summary>
	public void SetMaxDistance(float maxDistance) {
		this.maxDistance = Mathf.Clamp(maxDistance, this.GetMinDistance(), float.MaxValue);
	}
	
	/// <summary>
	/// Sets the number of points per scanline. Clamped to be positive.
	/// </summary>
	public void SetPointsPerScanline(int pointsPerScanline) {
		this.pointsPerScanline = Mathf.Clamp(pointsPerScanline, 1, int.MaxValue);
	}
	
	/// <summary>
	/// Sets the size of the arc in degrees.
	/// </summary>
	public void SetArcSize(float degrees) {
		this.arcSize = Mathf.Clamp(degrees, 0, 359);
	}
	
	/// <summary>
	/// Sets the number of scanlines in the scanning plane. Clamped to be positive.
	/// </summary>
	public void SetScanlineCount(int scanlineCount) {
		this.scanlineCount = Mathf.Clamp(scanlineCount, 1, int.MaxValue);
	}
	
	/// <summary>
	/// Sets the noise level of the probe. Clamped to be between 0 and 1.
	/// </summary>
	public void SetNoiseLevel(float noiseLevel) {
		this.noiseLevel = Mathf.Clamp(0f, noiseLevel, 1f);
	}
	
	/// <summary>
	/// The minimum distance to begin scanning.
	/// </summary>
	public float GetMinDistance() {
		return this.minDistance;
	}
	
	/// <summary>
	/// The maximum distance that the probe can scan.
	/// </summary>
	public float GetMaxDistance() {
		return this.maxDistance;
	}
	
	/// <summary>
	/// The discrete number of points along a single scanline.
	/// </summary>
	public int GetPointCount() {
		return this.pointsPerScanline;
	}
	
	/// <summary>
	/// The width of the scanning arc, in degrees.
	/// </summary>
	public float GetArcSize() {
		return this.arcSize;
	}
	
	/// <summary>
	/// The number of scanlines, or the "angular" resolution of the probe.
	/// </summary>
	public int GetScanlineCount() {
		return this.scanlineCount;
	}
	
	/// <summary>
	/// Returns the noise level of the probe.
	/// </summary>
	/// <returns>
	public float GetNoiseLevel() {
		return noiseLevel;
	}
	
	/// <summary>
	/// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="ProbeConfiguration"/>.
	/// </summary>
	/// <param name='that'>
	/// The <see cref="System.Object"/> to compare with the current <see cref="ProbeConfiguration"/>.
	/// </param>
	/// <returns>
	/// <c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="ProbeConfiguration"/>;
	/// otherwise, <c>false</c>.
	/// </returns>
	public override bool Equals(object that) {
		if (null == that) {
			return false;
		}
		
		ProbeConfiguration thatConfig = that as ProbeConfiguration;
		if (null == thatConfig) {
			return false;
		} else {
			return
				this.GetPointCount() == thatConfig.GetPointCount() &&
				this.GetScanlineCount() == thatConfig.GetScanlineCount() &&
				Mathf.Approximately(this.GetArcSize(), thatConfig.GetArcSize()) &&
				Mathf.Approximately(this.GetMaxDistance(), thatConfig.GetMaxDistance()) &&
				Mathf.Approximately(this.GetMinDistance(), thatConfig.GetMinDistance());
		}
	}
	
	/// <summary>
	/// Serves as a hash function for a <see cref="ProbeConfiguration"/> object.
	/// </summary>
	/// <returns>
	/// A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.
	/// </returns>
	public override int GetHashCode ()
	{
		int hash = 17;
		hash = hash * 11 + this.GetPointCount().GetHashCode();
		hash = hash * 11 + this.GetScanlineCount().GetHashCode();
		hash = hash * 11 + this.GetMaxDistance().GetHashCode();
		hash = hash * 11 + this.GetArcSize().GetHashCode();
		hash = hash * 11 + this.GetMinDistance().GetHashCode();
		return hash;
	}
}

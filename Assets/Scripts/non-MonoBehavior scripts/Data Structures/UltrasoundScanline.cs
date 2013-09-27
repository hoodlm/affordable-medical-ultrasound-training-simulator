using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Text;

/// <summary>
/// Represents an ultrasound scanline. Wraps an array of UltrasoundPoints.
/// </summary>
public class UltrasoundScanline : IEnumerable<UltrasoundPoint> {

	private UltrasoundPoint[] points;
	private Vector2 trajectory;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="UltrasoundScanline"/> class.
	/// </summary>
	/// <param name='pointsPerScanline'>
	/// The number of UltrasoundPoints on this scanline.
	/// </param>
	/// <param name='degrees'>
	/// The angle, in degrees, measured relative to the probe's forward vector.
	/// </param>
	public UltrasoundScanline(int pointsPerScanline, float degrees) {
		points = new UltrasoundPoint[pointsPerScanline];
		trajectory = TrajectoryFromDegrees(degrees);
	}
	
	/// <summary>
	/// Add at an index the specified UltrasoundPoint to this scanline.
	/// </summary>
	public void Set(int index, UltrasoundPoint point) {
		points[index] = point;
	}
	
	/// <summary>
	/// Get the point at the specified index.
	/// </summary>
	public UltrasoundPoint Get(int index) {
		return points[index];
	}
	
	/// <summary>
	/// Gets the number of points in the scanline.
	/// </summary>
	public int GetCount() {
		return points.GetLength(0);
	}
	
	/// <summary>
	/// Gets the 2D trajectory vectory of this scanline in the scanning plane.
	/// </summary>
	public Vector2 GetTrajectory() {
		return trajectory;
	}
	
	/// <summary>
	/// Gets an enumerator for all the points in this scanline.
	/// </summary>
	public IEnumerator<UltrasoundPoint> GetEnumerator() {
        foreach (UltrasoundPoint point in points) {
			yield return point;
		}
    }
	
	/// <summary>
	/// Gets an enumerator for all the points in this scanline.
	/// </summary>
	IEnumerator IEnumerable.GetEnumerator() {
        foreach (UltrasoundPoint point in points) {
			yield return point;
		}
    }
	
	/// <summary>
	/// Calculates the unit vector along an angle, given in degrees.
	/// </summary>
	private Vector2 TrajectoryFromDegrees(float degrees) {
		float theta = Mathf.Deg2Rad * degrees;
		
		float y = 1;
		float x = Mathf.Tan(theta);
		
		return new Vector2(x, y).normalized;
	}
	
	/// <summary>
	/// Calculates the angle of a scanline, given its index and a probe configuration.
	/// </summary>
	/// <returns>
	/// The angle in degrees.
	/// </returns>
	public static float GetAngleForIndex(int index, ProbeConfiguration config) {
		float minAngle = -config.GetArcSize() / 2;
		float stepSize = config.GetArcSize() / config.GetScanlineCount();
		return minAngle + index * stepSize;
	}
	
	/// <summary>
	/// Returns an array of points along a scanline, given a ProbeConfiguration.
	/// </summary>
	public Vector2[] GetPointsAlongScanline(ProbeConfiguration config) {
		Vector2[] pointsInScanline = new Vector2[config.GetPointCount()];
		
		Vector2 firstPoint = this.trajectory * config.GetMinDistance();
		float stepSize = (config.GetMaxDistance() - config.GetMinDistance()) / config.GetPointCount();
		
		for (int index = 0; index < config.GetPointCount(); ++index) {
			pointsInScanline[index] = firstPoint + index * stepSize * trajectory;
		}
		
		return pointsInScanline;
	}
	
	public override string ToString () {
		StringBuilder outString = new StringBuilder();
		foreach (UltrasoundPoint point in points) {
			if (!point.IsEmpty()) {
				outString.AppendLine(point.ToString());
			}
		}
		return outString.ToString();
	}
}

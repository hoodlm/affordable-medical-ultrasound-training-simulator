using UnityEngine;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Behaviors for an ultrasound probe.
/// </summary>
public class Probe : MonoBehaviour {
	
	private ProbeConfiguration config;
	
	// If the configuration has not changed, we do not want to recalculate all of our scanlines.
	private bool configDirty;
	private UltrasoundScanline[] cachedScanlines;
	
	// Use this for initialization
	void Start () {
		configDirty = true;
		RefreshConfig();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// Scan a single frame and return the result.
	/// This method is marked as "virtual" so that it can be overridden for testing.
	/// </summary>
	public virtual UltrasoundScanline[] Scan() {
		UltrasoundScanline[] scanlines = BuildScanlines();
		scanlines = ScanPlane(scanlines);
		return scanlines;
	}
	
	/// <summary>
	/// Scans in a plane defined by the scanlines passed in, and returns an array of scanlines with the results.
	/// </summary>
	private UltrasoundScanline[] ScanPlane(UltrasoundScanline[] scanlines) {
		foreach (UltrasoundScanline scanline in scanlines) {
			ScanAlongScanline(scanline);
		}
		
		return scanlines;
	}
	
	/// <summary>
	/// Scans along a scanline and returns a scanline populated with the results.
	/// </summary>
	private UltrasoundScanline ScanAlongScanline(UltrasoundScanline scanline) {
		
		Vector2[] pointsToScan = scanline.GetPointsAlongScanline(this.config);
		Vector3 origin = transform.position;
		
		// In a single scanline, we always initialize intensity at 1.0.
		float pulseIntensity = 1.0f;
		
		// We will need to know the last organ hit to calculate intensity of next pulse.
		UltrasoundOrgan previousOrgan;
		
		for (int index = 0; index < pointsToScan.GetLength(0); ++index)
		{
			Vector2 localPoint = pointsToScan[index];
			
			Vector3 target = TransformPointFromPlane(localPoint, this.transform);
			UltrasoundPulse pulse = new UltrasoundPulse(origin, target, pulseIntensity);
			UltrasoundPoint resultPoint = UltrasoundPhysics.SendPulse(pulse);
			resultPoint.SetProjectedPosition(localPoint);
			resultPoint.ApplyNoise(config.GetNoiseLevel());
			
			scanline.Set(index, resultPoint);
			
			previousOrgan = resultPoint.GetUltrasoundOrgan();
			pulseIntensity = RecalculatePulseIntensity(pulseIntensity, previousOrgan);
		}
		
		return scanline;
	}
	
	/// <summary>
	/// Refresh the configuration settings for the probe and return a reference.
	/// </summary>
	public ProbeConfiguration RefreshConfig() {
		this.config = gameObject.GetComponent<ProbeConfiguration>();
		configDirty = true;
		return config;
	}
	
	/// <summary>
	/// Based on the ProbeConfiguration, builds a new scanning plane, represented as an array of scanlines.
	/// </summary>
	private UltrasoundScanline[] BuildScanlines() {
		
		if (configDirty || null != cachedScanlines) {
			UltrasoundScanline[] scanlines = new UltrasoundScanline[config.GetScanlineCount()];
			for (int i = 0; i < scanlines.GetLength(0); ++i) {
				float angle = UltrasoundScanline.GetAngleForIndex(i, config);
				scanlines[i] = new UltrasoundScanline(config.GetPointCount(), angle);
			}
			cachedScanlines = scanlines;
			configDirty = false;
		}
		return (UltrasoundScanline[])cachedScanlines.Clone();
	}
	
	/// <summary>
	/// Recalculates the pulse intensity based on the acoustic properties of the organ just hit.
	/// </summary>
	private float RecalculatePulseIntensity(float previousIntensity, UltrasoundOrgan previousOrgan) {
		if (null == previousOrgan) {
			return previousIntensity;
		} else {
			float resolutionScalingCoefficient = config.GetPointCount();
			float newIntensity = previousIntensity - ((previousOrgan.echogenicity + previousOrgan.attenuation) / resolutionScalingCoefficient);
			return Mathf.Clamp(newIntensity, 0f, 1f);
		}
	}
	
	/// <summary>
	/// Projects a point from world space into the scanning plane of the probe.
	/// </summary>
	private static Vector2 ProjectPointIntoPlane(Vector3 point, Transform probeOrientation) {
		return Vector2FromVector3(probeOrientation.InverseTransformPoint(point));
	}
	
	/// <summary>
	/// Transforms the point from the scanning plane of the probe into world space.
	/// </summary>
	private static Vector3 TransformPointFromPlane(Vector2 point, Transform probeOrientation) {
		return probeOrientation.TransformPoint(Vector3FromVector2(point));
	}
	
	/// <summary>
	/// Convert a Vector3 into a Vector2, discarding the y component.
	/// </summary>
	private static Vector2 Vector2FromVector3(Vector3 vector3) {
		return new Vector2(vector3.x, vector3.z);
	}
	
	/// <summary>
	/// Convert a Vector2 into a Vector3, initializing the y component with a zero.
	/// </summary>
	private static Vector3 Vector3FromVector2(Vector2 vector2) {
		return new Vector3(vector2.x, 0f, vector2.y);
	}
	
	/// <summary>
	/// Writes the recorded points to file for debugging purposes.
	/// </summary>
	private static void WriteToFile(UltrasoundScanline[] scanlines) {
		string filename = "./log.txt";
		
		StringBuilder outputString = new StringBuilder();
		
		outputString.AppendLine(System.DateTime.Now.ToString());
		foreach (UltrasoundScanline scanline in scanlines) {
			outputString.Append(scanline.ToString());
		}
		System.IO.File.AppendAllText(filename, outputString.ToString());
		print("logged to "+filename);
	}
}

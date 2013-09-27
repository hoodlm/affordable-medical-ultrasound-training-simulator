using UnityEngine;
using System.Collections;
using System.Text;

public class MockProbe : Probe {

	// Use this for initialization
	void Start () {
		this.RefreshConfig();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Return a test pattern
	/// </summary>
	public override UltrasoundScanline[] Scan() {
		return GetTestScanlines();
	}
	
	private UltrasoundScanline[] GetTestScanlines() {
		UltrasoundPoint[] points = GetTestPoints(50);
		UltrasoundScanline[] scanlines = new UltrasoundScanline[10];
		
		for (int i = 0; i < scanlines.GetLength(0); ++i) {
			UltrasoundScanline scanline = new UltrasoundScanline(5, 0f);
			for (int j = 0; j < scanline.GetCount(); ++j) {
				scanline.Set(j, points[i*5 + j]);
			}
			scanlines.SetValue(scanline, i);
		}
		
		return scanlines;
	}
	
	private UltrasoundPoint[] GetTestPoints(int count) {
		UltrasoundPoint[] points = new UltrasoundPoint[count];
		for (int i = 0; i < count; ++i) {
			UltrasoundPoint point = new UltrasoundPoint();
			point.SetEmpty(false);
			point.SetIntensity(0.5f + (i % 5) / 5f);
			point.SetOrgan(gameObject.GetComponent<UltrasoundOrgan>());
			point.SetPosition(Vector3.zero);
			point.SetProjectedPosition(GetTestPosition(i));
			points.SetValue(point, i);
		}
		return points;
	}
	
	private static Vector2 GetTestPosition(int index) {
		return new Vector2(0.05f*Mathf.Cos(0.4f*index) * index, 0.05f*Mathf.Sin(0.4f*index) * index + 4);
	}
	
	/// <summary>
	/// Writes the recorded points to file for debugging purposes.
	/// </summary>
	private static void WriteToFile(UltrasoundScanline[] scanlines) {
		StringBuilder outputString = new StringBuilder();
		
		//outputString.AppendLine(System.DateTime.Now.ToString());
		foreach (UltrasoundScanline scanline in scanlines) {
			outputString.Append(scanline.ToString());
		}
		System.IO.File.AppendAllText("./log.txt", outputString.ToString());
	}
}

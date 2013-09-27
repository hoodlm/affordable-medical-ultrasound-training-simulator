using UnityEngine;
using System.Collections;
using System.Text;

public class ProbeTests : MonoBehaviour {
	
	public Rect displayRect;
	public GameObject testProbeGameObject;
	private Probe testProbe;
	
	// Use this for initialization
	void Start () {
		testProbe = this.testProbeGameObject.GetComponent<Probe>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		StringBuilder sb = new StringBuilder();
		
		sb.AppendLine("Tests of Probe:");
		sb.AppendLine(TestConfigFor60Points());
		sb.AppendLine(TestSingleScanlineFromWorldOrigin());
		sb.AppendLine(TestSingleScanlineFromAboveWorldOrigin());
		
		GUI.Label(displayRect, sb.ToString());
	}
	
	private string TestConfigFor60Points() {
		string testLabel = "Config point count";
		
		ProbeConfiguration config = SetProbeTestConfig60Pts();
		UltrasoundScanline[] results = testProbe.Scan();
		
		int expectedPointCount = config.GetPointCount() * config.GetScanlineCount();
		int pointCount = 0;
		foreach (UltrasoundScanline scanline in results) {
			pointCount += scanline.GetCount();
		}
		
		string resultString = string.Empty;
		if (pointCount == expectedPointCount) {
			resultString = "passed";
		} else {
			resultString = string.Format("FAILED: {0} points; expected {1}", pointCount, expectedPointCount);
		}
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	private string TestSingleScanlineFromWorldOrigin() {
		string testLabel = "Single scanline from origin";
		testProbe.transform.position = Vector3.zero;
		SetProbeTestConfigSingleScanline();
		UltrasoundScanline[] results = testProbe.Scan();		
		
		string resultString = "passed";
		
		foreach (UltrasoundPoint point in results[0]) {
			if (point.GetProjectedPosition().x != point.GetPosition().x ||
				point.GetProjectedPosition().y != point.GetPosition().z) {
				resultString = "FAILED";
				break;
			}
		}
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	private string TestSingleScanlineFromAboveWorldOrigin() {
		string testLabel = "Single scanline from (0,3,0)";
		testProbe.transform.position = Vector3.zero + new Vector3(0,3f,0);
		SetProbeTestConfigSingleScanline();
		UltrasoundScanline[] results = testProbe.Scan();		
		
		string resultString = "passed";
		
		foreach (UltrasoundPoint point in results[0]) {
			if (point.GetProjectedPosition().x != point.GetPosition().x ||
				point.GetProjectedPosition().y != point.GetPosition().z ||
				point.GetPosition().y != 3f || !point.IsEmpty()) {
				resultString = "FAILED";
				break;
			}
		}
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// Sets the probe test config to have 60 points.
	/// </summary>
	private ProbeConfiguration SetProbeTestConfig60Pts() {
		ProbeConfiguration config = this.testProbeGameObject.GetComponent<ProbeConfiguration>();
		config.SetMinDistance(0f);
		config.SetMaxDistance(10f);
		config.SetArcSize(120f);
		config.SetPointsPerScanline(20);
		config.SetScanlineCount(30);
		this.testProbe.RefreshConfig();
			
		return config;
	}
	
	/// <summary>
	/// Sets the probe test config to have a single scanline, roughly straight ahead.
	/// </summary>
	private ProbeConfiguration SetProbeTestConfigSingleScanline() {
		ProbeConfiguration config = this.testProbeGameObject.GetComponent<ProbeConfiguration>();
		config.SetMinDistance(0f);
		config.SetMaxDistance(10f);
		config.SetArcSize(0f);
		config.SetPointsPerScanline(20);
		config.SetScanlineCount(1);
		this.testProbe.RefreshConfig();
			
		return config;
	}

}

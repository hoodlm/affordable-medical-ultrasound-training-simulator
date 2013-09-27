using UnityEngine;
using System.Collections;
using System.Text;

/// <summary>
/// Makeshift integration tests for UltrasoundPhysics.
/// Verifies that the methods of UltrasoundPhysics interact correctly with the UltrasoundCollision class.
/// </summary>
public class UltrasoundRaycastTests : MonoBehaviour {
	
	public Rect displayRect;
	
	public GameObject testCube;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		StringBuilder sb = new StringBuilder();
		
		sb.AppendLine("Tests of UltrasoundPhysics:");
		sb.AppendLine(OriginOfCube());
		sb.AppendLine(PointAboveCube());
		
		GUI.Label(displayRect, sb.ToString());
	}
	
	private string OriginOfCube() {
		string testLabel = "Pulse to cube origin";
		string resultString = string.Empty;
		
		Vector3 targetPoint = testCube.transform.position;
		UltrasoundPulse testPulse = new UltrasoundPulse(Vector3.zero, targetPoint, 1.0f);
		
		UltrasoundPoint point = UltrasoundPhysics.SendPulse(testPulse);
		if (!point.GetPosition().Equals(targetPoint)) {
			resultString = "FAILED - result doesn't match target point";
		} else if (point.IsEmpty()) {
			resultString = "FAILED - result was empty!";
		} else {
			resultString = "passed";
		}
			
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	private string PointAboveCube() {
		string testLabel = "PointAboveCube";
		string resultString = string.Empty;
		
		Vector3 targetPoint = testCube.transform.position + Vector3.up * 2f;
		UltrasoundPulse testPulse = new UltrasoundPulse(Vector3.zero, targetPoint, 1.0f);
		
		UltrasoundPoint point = UltrasoundPhysics.SendPulse(testPulse);
		if (!point.GetPosition().Equals(targetPoint)) {
			resultString = "FAILED - result doesn't match target point";
		} else if (!point.IsEmpty()) {
			resultString = "FAILED - result should be empty!";
		} else {
			resultString = "passed";
		}
			
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
}

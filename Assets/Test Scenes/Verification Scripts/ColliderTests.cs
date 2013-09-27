using UnityEngine;
using System.Collections;
using System.Text;

/// <summary>
/// Some makeshift unit tests for the UltrasoundCollision methods.
/// </summary>
public class ColliderTests : MonoBehaviour {
	
	public Rect displayRect;
	
	public GameObject testCube;
	public GameObject testSphere;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
		
	void OnGUI() {
		StringBuilder sb = new StringBuilder();
		
		sb.AppendLine("Tests of UltrasoundCollisionUtils:");
		sb.AppendLine(OriginOfCube());
		sb.AppendLine(TestPointInsideCube());
		sb.AppendLine(TestPointBehindCube());
		sb.AppendLine(TestPointLeftOfCube());
		sb.AppendLine(TestPointAboveCube());
		sb.AppendLine(OriginOfSphere());
		sb.AppendLine(CornerOfSphereBoundBox());
		
		GUI.Label(displayRect, sb.ToString());
	}
	
	/// <summary>
	/// The origin of the cube should be recognized as being contained inside it.
	/// </summary>
	private string OriginOfCube() {
		string testLabel = "Origin of cube";
		
		Vector3 testPoint = testCube.transform.position;
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testCube.collider);
		string resultString = ResultString(contained, true);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// Tests a point inside cube is contained inside it.
	/// </summary>
	private string TestPointInsideCube() {
		string testLabel = "Point inside cube";
		
		Vector3 testPoint = testCube.transform.position + new Vector3(0.5f, 0f, 0.4f);
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testCube.collider);
		string resultString = ResultString(contained, true);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// Tests that a point to the rear of the cube is not contained inside it.
	/// </summary>
	private string TestPointBehindCube() {
		string testLabel = "Point behind cube";
		
		Vector3 testPoint = testCube.transform.position + Vector3.back * 3;
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testCube.collider);
		string resultString = ResultString(contained, false);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// Tests that a point to the left of the cube is not contained inside it.
	/// </summary>
	private string TestPointLeftOfCube() {
		string testLabel = "Point left of cube";
		
		Vector3 testPoint = testCube.transform.position + Vector3.left * 3;
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testCube.collider);
		string resultString = ResultString(contained, false);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// Tests that a point above the cube is not contained inside it.
	/// </summary>
	private string TestPointAboveCube() {
		string testLabel = "Point above cube";
		
		Vector3 testPoint = testCube.transform.position + Vector3.up * 3;
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testCube.collider);
		string resultString = ResultString(contained, false);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// The origin of the sphere should be recognized as being contained inside it.
	/// </summary>
	private string OriginOfSphere() {
		string testLabel = "Origin of sphere";
		
		Vector3 testPoint = testSphere.transform.position;
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testSphere.collider);
		string resultString = ResultString(contained, true);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	/// <summary>
	/// A point that is inside the sphere's bounds, but not its collider, should not be contained inside it.
	/// </summary>
	private string CornerOfSphereBoundBox() {
		string testLabel = "Corner Of Sphere Bounds";
		
		Vector3 testPoint = testSphere.transform.position + new Vector3(0.99f, 0.99f, 0.99f);
		if (!testSphere.collider.bounds.Contains(testPoint)) {
			return string.Format("Test \"{0}\" is using the wrong test point", testLabel);
		}
		
		bool contained = UltrasoundCollisionUtils.IsContained(testPoint, testSphere.collider);
		string resultString = ResultString(contained, false);
		
		return string.Format("Test \"{0}\" {1}", testLabel, resultString);
	}
	
	private static string ResultString(bool expected, bool result) {
		if (expected == result) {
			return "passed";
		} else {
			return "FAILED";
		}
	}
}

using UnityEngine;
using System.Collections;

public class FingerCountChecker : MonoBehaviour {

	private LeapManager _leapManager;
	private TextMesh text;
	// Use this for initialization
	void Start () {
		text = gameObject.GetComponent(typeof(TextMesh)) as TextMesh;
		_leapManager = (GameObject.Find("LeapManager") as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Finger Count: " + _leapManager.frontmostHand().Fingers.Count;
	}
}

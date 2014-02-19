using UnityEngine;
using System.Collections;

public class KeyboardBodyMovement : MonoBehaviour {
	
	public float rotationSpeed = 1.0f;
	public GameObject bodyPivotPoint;
	private GameObject skin;
	
	// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");

		UltrasoundDebug.Assert(null != bodyPivotPoint, "Body object not set!", this);
		UltrasoundDebug.Assert(null != skin, 
		                       "Could not find Skin object. Did you forget to tag it?",
		                       this);
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardRotation();
		if (Input.GetKeyDown(KeyCode.Space)) {
			skin.renderer.enabled = !skin.renderer.enabled;
		}
	}
	
	/// <summary>
	/// Rotate the body with the W & S keys
	/// </summary>
	void KeyboardRotation () {
		Vector3 rotationAxis = Vector3.zero;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			rotationAxis += Vector3.up;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			rotationAxis += Vector3.down;
		}
		
		bodyPivotPoint.transform.RotateAround(bodyPivotPoint.transform.position, 
		                                      rotationAxis, 
		                                      rotationSpeed * Time.deltaTime);
	}
}

using UnityEngine;
using System.Collections;

/**
 *	Uses the keyboard to move the virual patient.
 *	Should be used in conjunction with the MouseProbeMovement script.
 *
 *	Allows the user to:
 *	- rotate the patient with the arrow keys
 *	- toggle skin visibility with the space bar.
 */
public class KeyboardBodyMovement : MonoBehaviour {

	/// The rate (degrees per second) that the body is rotated.
	public float rotationSpeed = 20.0f;

	/// A GameObject at the body's pivot point. The body should be a child of this GameObject.
	public GameObject bodyPivotPoint;

	/// The skin GameObject.
	private GameObject skin;
	
	/// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");

		UltrasoundDebug.Assert(null != bodyPivotPoint, "Body object not set!", this);
		UltrasoundDebug.Assert(null != skin, 
		                       "Could not find Skin object. Did you forget to tag it?",
		                       this);
		UltrasoundDebug.Assert(null != skin.collider, 
		                       "The skin object does not seem to have a collider.",
		                       this);
	}
	
	/// Update is called once per frame
	void Update () {
		KeyboardRotation();
		if (Input.GetKeyDown(KeyCode.Space)) {
			skin.renderer.enabled = !skin.renderer.enabled;
		}
	}

	/// Rotate the body with the arrow keys
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

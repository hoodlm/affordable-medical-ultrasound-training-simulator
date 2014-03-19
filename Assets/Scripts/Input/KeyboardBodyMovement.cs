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

	/// The position to reset the probe to if it is lost.
	private Vector3 initialPosition;

	/// The orientation to reset the probe to if it is lost.
	private Quaternion initialRotation;
	
	/// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
		initialRotation = this.transform.rotation;

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

		if (Input.GetKeyDown(KeyCode.LeftShift)) {
			this.transform.position = initialPosition;
			this.transform.rotation = initialRotation;
		}
	}

	/// Rotate the body with the arrow keys
	void KeyboardRotation () {
		Vector3 rotationAxis = Vector3.zero;
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			rotationAxis += bodyPivotPoint.transform.up;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			rotationAxis += -bodyPivotPoint.transform.up;
		}
		
		bodyPivotPoint.transform.RotateAround(bodyPivotPoint.transform.position, 
		                                      rotationAxis, 
		                                      rotationSpeed * Time.deltaTime);
	}
}

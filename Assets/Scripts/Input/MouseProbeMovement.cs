using UnityEngine;
using System.Collections;

/// Move the probe using the mouse. Controls are:
/// - RIGHT CLICK (or alt-click) to position the probe
/// - CLICK AND DRAG to rotate the probe in place
public class MouseProbeMovement : MonoBehaviour {

	/// The speed of rotation of the probe when dragged by the mouse.
	public float rotationSpeed = 20.0f;

	/// The skin GameObject.
	private GameObject skin;

	/// "Drag mode" is turned on when the user is click-dragging to rotate the probe.
	/// When not in drag mode, the user is free to use the mouse to interact with other
	/// parts of the scene (e.g. GUI elements).
	private bool dragMode;
	
	/// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");
		UltrasoundDebug.Assert(null != skin, 
		                       "Could not find Skin object. Did you forget to tag it?",
		                       this);
		UltrasoundDebug.Assert(null != skin.collider, 
		                       "Skin object does not have a collider.",
		                       this);
		dragMode = false;
	}
	
	/// Update is called once per frame
	void Update () {
		MoveOnMouseClick();
		RotateOnMouseMove();
	}

	/// Move the probe to the targeted position. (Right click)
	private void MoveOnMouseClick() {
		 if (Input.GetButtonDown("Fire2")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (skin.collider.Raycast(ray, out hit, float.PositiveInfinity)) {
				this.transform.position = hit.point;
			}
        }
	}

	/// Rotates the probe based on mouse movement. (Left-click + drag)
	private void RotateOnMouseMove() {
		if (!dragMode && Input.GetButtonDown("Fire1")) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (this.collider.Raycast(ray, out hit, float.PositiveInfinity)) {
				dragMode = true;
			}
		}
		
		if (dragMode && Input.GetButton("Fire1")) {
			float mouseX = Input.GetAxis("Mouse X");
			float mouseY = Input.GetAxis("Mouse Y");
			float magnitude = Mathf.Abs (mouseX + mouseY) * rotationSpeed;

			Vector3 mouseVector = new Vector3(mouseY, -mouseX, 0f).normalized;
			this.transform.Rotate(mouseVector, magnitude, Space.World);
		} else if (dragMode && !Input.GetButton("Fire1")) {
			dragMode = false;
		}
	}
}

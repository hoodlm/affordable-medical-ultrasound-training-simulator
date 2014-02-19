using UnityEngine;
using System.Collections;

/// <summary>
/// Move the probe using the mouse. Controls are:
/// - RIGHT CLICK to position the probe
/// - CLICK AND DRAG to rotate the probe in place
/// </summary>
public class MouseProbeMovement : MonoBehaviour {
	
	public float rotationSpeed = 20.0f;
	
	private GameObject skin;
	private bool dragMode;
	
	// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");
		UltrasoundDebug.Assert(null != skin, 
		                       "Could not find Skin object. Did you forget to tag it?",
		                       this);
		dragMode = false;
	}
	
	// Update is called once per frame
	void Update () {
		MoveOnMouseClick();
		RotateOnMouseMove();
	}
	
	/// <summary>
	/// Move the probe to the targeted position. (Right click)
	/// </summary>
	private void MoveOnMouseClick() {
		 if (Input.GetButtonDown("Fire2")) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
            if (skin.collider.Raycast(ray, out hit, float.PositiveInfinity)) {
				this.transform.position = hit.point;
			}
        }
	}
	
	/// <summary>
	/// Rotates the probe based on mouse movement. (Left-click + drag)
	/// </summary>
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

//			Vector3 xRotationAxis = -transform.forward;
//			this.transform.Rotate(xRotationAxis, scaledMouseX, Space.World);
//		
//
//			Vector3 yRotationAxis = transform.right;
//			this.transform.Rotate(yRotationAxis, scaledMouseY, Space.World);
		} else if (dragMode && !Input.GetButton("Fire1")) {
			dragMode = false;
		}
	}
}

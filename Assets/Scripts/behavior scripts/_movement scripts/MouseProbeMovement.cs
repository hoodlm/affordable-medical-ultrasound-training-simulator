using UnityEngine;
using System.Collections;

public class MouseProbeMovement : MonoBehaviour {
	
	public float rotationSpeed = 20.0f;
	
	private GameObject skin;
	
	// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");
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
	/// Rotates the probe based on mouse movement. (CTRL + drag)
	/// </summary>
	private void RotateOnMouseMove() {
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) {
			float scaledMouseX = Input.GetAxis("Mouse X") * rotationSpeed * -1.0f * Time.deltaTime;
			this.transform.Rotate(Vector3.up, scaledMouseX, Space.World);
		
			float scaledMouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
			this.transform.Rotate(transform.right, scaledMouseY, Space.World);
		}
	}
}

using UnityEngine;
using System.Collections;

public class KeyboardBodyMovement : MonoBehaviour {
	
	public float rotationSpeed = 1.0f;
	public float zoomSpeed = 1.0f;
	
	public GameObject bodyCenter;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardRotation();
		//KeyboardZoom();
	}
	
	/// <summary>
	/// Rotate the camera with the A & D keys
	/// </summary>
	void KeyboardRotation () {
		Vector3 rotationAxis = Vector3.zero;
		
		if (Input.GetKey(KeyCode.A)) {
			rotationAxis += Vector3.up;
		} else if (Input.GetKey(KeyCode.D)) {
			rotationAxis += Vector3.down;
		}
		
		this.transform.RotateAround(Vector3.zero, rotationAxis, rotationSpeed * Time.deltaTime);
	}
	
	/// <summary>
	/// Zoom the camera toward the center of the body. (Still buggy.)
	/// </summary>
	void KeyboardZoom() {
		if (Input.GetKey(KeyCode.W)) {
			Vector3 trajectory = bodyCenter.transform.position - this.transform.position;
			transform.Translate(zoomSpeed * trajectory * Time.deltaTime, Space.World);
		} else if (Input.GetKey(KeyCode.S)) {
			Vector3 trajectory = -(bodyCenter.transform.position - this.transform.position);
			transform.Translate(zoomSpeed * trajectory * Time.deltaTime, Space.World);
		}
	}
}

using UnityEngine;
using System.Collections;

public class KeyboardBodyMovement : MonoBehaviour {
	
	public float rotationSpeed = 1.0f;
	public GameObject body;
	
	// Use this for initialization
	void Start () {
		body = GameObject.FindGameObjectWithTag("Body");
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
		
		body.transform.RotateAround(body.transform.position, rotationAxis, rotationSpeed * Time.deltaTime);
		
		//this.transform.RotateAround(Vector3.zero, rotationAxis, rotationSpeed * Time.deltaTime);
	}
}

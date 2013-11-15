using UnityEngine;
using System.Collections;

public class KeyboardBodyMovement : MonoBehaviour {
	
	public float rotationSpeed = 1.0f;
	public GameObject body;
	public Transform probeTransform;
	public CachedTransform probeResetTransform;
	
	// Use this for initialization
	void Start () {
		body = GameObject.FindGameObjectWithTag("Body");
		probeResetTransform = new CachedTransform(probeTransform);
	}
	
	// Update is called once per frame
	void Update () {
		KeyboardRotation();
		if (Input.GetKeyDown(KeyCode.Space)) {
			probeResetTransform.copyIntoTransform(probeTransform);
		}
	}
	
	/// <summary>
	/// Rotate the camera with the W & S keys
	/// </summary>
	void KeyboardRotation () {
		Vector3 rotationAxis = Vector3.zero;
		
		if (Input.GetKey(KeyCode.W)) {
			rotationAxis += Vector3.up;
		} else if (Input.GetKey(KeyCode.S)) {
			rotationAxis += Vector3.down;
		}
		
		body.transform.RotateAround(body.transform.position, rotationAxis, rotationSpeed * Time.deltaTime);
	}
}

using UnityEngine;
using System.Collections;

public class SimpleMovement : MonoBehaviour {
	
	public float translateSpeed = 2.0f;
	public float rotationSpeed = 1.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckMovementControls();
	}
	
	private void CheckMovementControls() {
		Vector3 translation = Vector3.zero;
		Vector3 rotationAxis = Vector3.zero;
		
		if (Input.GetKey(KeyCode.UpArrow)) {
			translation += Vector3.up;
		} else if (Input.GetKey(KeyCode.DownArrow)) {
			translation += Vector3.down;
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			translation += Vector3.left;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			translation += Vector3.right;
		}
		
		if (Input.GetKey(KeyCode.Equals)) {
			translation += Vector3.forward;
		} else if (Input.GetKey(KeyCode.Minus)) {
			translation += Vector3.back;
		}
		
		if (Input.GetKey(KeyCode.Q)) {
			rotationAxis += Vector3.forward;
		} else if (Input.GetKey(KeyCode.E)) {
			rotationAxis += Vector3.back;
		}
		
		if (Input.GetKey(KeyCode.A)) {
			rotationAxis += Vector3.up;
		} else if (Input.GetKey(KeyCode.D)) {
			rotationAxis += Vector3.down;
		}
		
		if (Input.GetKey(KeyCode.W)) {
			rotationAxis += Vector3.left;
		} else if (Input.GetKey(KeyCode.S)) {
			rotationAxis += Vector3.right;
		}
		
		Translate(translation);
		Rotate(rotationAxis);
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			gameObject.renderer.enabled = !gameObject.renderer.enabled;
		}
	}
	
	private void Translate(Vector3 translation) {
		transform.position += translation * translateSpeed * Time.deltaTime;
	}
	
	private void Rotate(Vector3 rotationAxis) {
		transform.RotateAround(rotationAxis, rotationSpeed * Time.deltaTime);
	}
}

using UnityEngine;
using System.Collections;

public class HideGenitals : MonoBehaviour {
	
	public bool censoring = true;
	
	// Use this for initialization
	void Start () {
		gameObject.renderer.enabled = censoring;
		gameObject.renderer.material.color = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.X)) {
			gameObject.renderer.enabled = !censoring;
			censoring = !censoring;
		}
	}
}

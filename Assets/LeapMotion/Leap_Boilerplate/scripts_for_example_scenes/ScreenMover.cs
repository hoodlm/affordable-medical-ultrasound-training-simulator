using UnityEngine;
using System.Collections;

public class ScreenMover : MonoBehaviour {
	public float _projectionDistance = 5;

	private Camera _mainCam;
	private LeapManager _leapManager;
	// Use this for initialization
	void Start () {
		_mainCam = (GameObject.FindGameObjectWithTag("MainCamera")as GameObject).GetComponent(typeof(Camera)) as Camera;
		_leapManager = (GameObject.Find("LeapManager")as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
	}
	
	// Update is called once per frame
	void Update () {
		if(_leapManager != null) { 
			if(_leapManager.pointerAvailible)
			{
				_leapManager.screenToWorldDistance = _projectionDistance;
				this.transform.position = _leapManager.pointerPositionScreenToWorld;
				if(!renderer.enabled) { renderer.enabled = true; }
			}
			else
			{
				renderer.enabled = false;
			}
		}
	}
}

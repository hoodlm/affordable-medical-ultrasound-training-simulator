using UnityEngine;
using System.Collections;
using Leap;

public class RevolvingCamera : MonoBehaviour {
	public GameObject _targetObject;
	public Vector3 _controlMin;
	public Vector3 _controlMax;
	public Vector2 _distanceLimits;
	
	private LeapManager _leapManager;
	private Vector3 _lastHandPosition = Vector3.zero;
	private Vector3 _lastHandPositionOnLoss = Vector3.zero;
	private Vector3 _handPositionOnAquire = Vector3.zero;
	private int _lastHandId = -1;
	
	// Use this for initialization
	void Start () {
		_leapManager = (GameObject.Find("LeapManager") as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
	}
	
	// Update is called once per frame
	void Update () {
		if(_leapManager.frontmostHand().IsValid && LeapManager.isHandOpen(_leapManager.frontmostHand()))
		{
			Hand hand = _leapManager.frontmostHand();
			Vector3 handLocation = hand.PalmPosition.ToUnityTranslated();

			if(hand.Id != _lastHandId)
			{
				_handPositionOnAquire = handLocation;
				_lastHandId = hand.Id;

			}

			handLocation = _lastHandPositionOnLoss + (handLocation - _handPositionOnAquire);

			handLocation.y = Mathf.Clamp(handLocation.y, _controlMin.y, _controlMax.y);

			_lastHandPosition = handLocation;
			
			Quaternion yRotation = Quaternion.Euler(new Vector3(
				0,
				(((handLocation.x - _controlMin.x) / (_controlMax.x - _controlMin.x)) * 180.0f) + 90.0f,
				0
				));
			
			Quaternion xRotation = Quaternion.Euler(new Vector3(
				((1-((handLocation.y - _controlMin.y) / (_controlMax.y - _controlMin.y))) * 180.0f) + 90.0f,
				0,
				0));
			
			Vector3 directionVector = yRotation * xRotation * Vector3.forward;
			
			float dist = ((1.0f - Mathf.Clamp((handLocation.z - _controlMin.z)/(_controlMax.z - _controlMin.x),0.0f,1.0f)) * (_distanceLimits.y - _distanceLimits.x)) + _distanceLimits.x;
			gameObject.transform.position = directionVector * dist;
			gameObject.transform.LookAt(_targetObject.transform);



		}
		else
		{
			_lastHandId = -1;
			_lastHandPositionOnLoss = _lastHandPosition;
		}
	}
}

/* 
 * Project: LeapMotion Controller Unity C# Boilerplate
 * File: LeapManager.cs
 * Author: Daniel Plemmons
 * Twitter: @RandomOutput
 * 
 * Requires the LeapMotion SDK
 * http://developer.leapmotion.com
 * Unity Project Setup: https://developer.leapmotion.com/documentation/Languages/CSharpandUnity/Guides/Setup_Unity.html
 * 
 * [Excerpt from readme.txt]
 * ======================
 * General Useage
 * ======================
 * 
 * This package is meant to act as a set of boilerplate functions and architecture to get your Leap enabled 
 * application up and running rapidly.
 * 
 * It solves the following problems:
 * 
 * - Providing a central point of access to the LeapMotion Controller data via a LeapManager GameObject with
 *   the LeapManager.cs script.
 * 
 * - Converting Leap data into Unity's units and Vector3 objects. (LeapExtensions.cs)
 *  > This is accomplished via three extension methods which return Unity Vector3 objects:
 *   1. myVectorInstance.ToUnity() # For things like direciton vectors which are normalized.
 *   2. myVectorInstance.ToUnityScales() # For things like Acceleration/Velocity where units must be converted, but position is not a factor.
 *   3. myVectorInstance.ToUnityTranslated() # For things like positions where units must be scaled AND we want to offset the coordinates into our game's coordinate space.
 * 
 * - Providing easy access to common data like pointing fingers and hand/finger locations in Unity screen or world coordinates.
 * 
 * - Providing helper methods for common concepts like "is this hand open?", or "how many fingers am I holding up?"
 * 
 * To use the LeapManager in your scene, simply drag the LeapManager prefab from the "prefabs" folder in Leap_GGJ into your scene. It's just an empty game object with the script attached.
 * 
 * A few of the class functions are static and can simply be accessed with a call to:
 * 
 * LeapManager.staticMethodName() 
 * 
 * ...but most require a reference to an instance. As you can see in the example 
 * scenes, grabbing this instance is as "simple" as: 
 * 
 * LeapManager myLeapManagerInstance = (GameObject.Find("LeapManager") as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
 * 
 * Remember that GameObject.Find() is really slow so you only want to do this one in your MonoBehavior's Start() function and store the reference for later. Again, see any of the example scenes for a full implementation.
 */

using UnityEngine;
using System.Collections;
using Leap;

public class LeapManager : MonoBehaviour {
	public Camera _mainCam; //Not required to be set. Defaults to camera tagged "MainCamera".

	public static float _forwardFingerContraint = 0.7f; // Min result of dot product between finger direction and hand direction to determine if finger is facing forward.

	private static Controller _leapController = new Controller();
	private static Frame _currentFrame = Frame.Invalid;
	private static bool _pointerAvailible = false;
	private Vector2 _pointerPositionScreen = new Vector3(0,0);
	private Vector3 _pointerPositionWorld = new Vector3(0,0,0);
	private Vector3 _pointerPositionScreenToWorld = new Vector3(0,0,0);
	private float _screenToWorldDistance = 10.0f;

	//Accessors
	/*
	 * A direct reference to the Controller for accessing the LeapMotion data yourself rather than going through the helper.
	 */
	public Controller leapController{
		get { return _leapController; }
	}
	
	/*
	 * The most recent frame of data from the LeapMotion controller.
	 */
	public Frame currentFrame
	{
		get { return _currentFrame; }
	}
	
	/*
	 * Is there a pointing finger currently tracked in the scene.
	 */
	public bool pointerAvailible {
		get{ return _pointerAvailible; }
	}
	
	/*
	 * The currently tracked (if any) pointing finger in screen space.
	 */
	public Vector2 pointerPositionScreen {
		get { return _pointerAvailible ? _pointerPositionScreen : Vector2.zero; }
	}
	
	/*
	 * The currently tracked (if any) pointing finger in world space.
	 */
	public Vector3 pointerPositionWorld {
		get { return _pointerAvailible ? _pointerPositionWorld : Vector3.zero; }
	}

	/*
	 * The screen position of the currently tracked (if any) pointing finger projected into world space
	 * at a distance of [screenToWorldDistance].
	 */
	public Vector3 pointerPositionScreenToWorld {
		get { return _pointerPositionScreenToWorld; }
	}

	/*
	 * The projection distance for the pointerPositionScreenToWorld calculation. 
	 * Default Value is 10.0f
	 */
	public float screenToWorldDistance {
		get { return _screenToWorldDistance; }
		set { _screenToWorldDistance = value; }
	}

	//Public Static Functions

	/*
	 * Returns the most likely finger to be pointing on the given hand. 
	 * Returns Finger.Invalid if no such finger exists.
	 */
	public static Finger pointingFigner(Hand hand)
	{
		Finger forwardFinger = Finger.Invalid;
		ArrayList forwardFingers = forwardFacingFingers(hand);
		
		if(forwardFingers.Count > 0)
		{
			
			float minZ = float.MaxValue;
			
			foreach(Finger finger in forwardFingers)
			{
				if(finger.TipPosition.z < minZ)
				{
					minZ = finger.TipPosition.z;
					forwardFinger = finger;
				}
			}
		}
		
		return forwardFinger;
	}

	/*
	 * Returns a list of fingers whose position is in front 
	 * of the hand (relative to the hand direction). 
	 * 
	 * This is most useful in trying to lower the chances 
	 * of detecting a thumb (though not a perfect method).
	 */
	public static ArrayList forwardFacingFingers(Hand hand)
	{
		ArrayList forwardFingers = new ArrayList();
		
		foreach(Finger finger in hand.Fingers)
		{
			if(isForwardRelativeToHand(finger, hand)) { forwardFingers.Add(finger); }
		}
		
		return forwardFingers;
	}

	/*
	 * Returns whether or not the given hand is open.
	 */
	public static bool isHandOpen(Hand hand)
	{
		return hand.Fingers.Count > 2;
	}

	public static bool isForwardRelativeToHand(Pointable item, Hand hand)
	{
		return Vector3.Dot((item.TipPosition.ToUnity() - hand.PalmPosition.ToUnity()).normalized, hand.Direction.ToUnity()) > _forwardFingerContraint;
	}
	
	// Unity Monobehavior Defenitions

	/*
	 * If the _mainCam isn't overridden, 
	 * find the camera with the "MainCamera" tag.
	 */
	void Start () {
		if(_mainCam == null)
		{
			_mainCam = (GameObject.FindGameObjectWithTag("MainCamera") as GameObject).GetComponent(typeof(Camera)) as Camera;
		}
		Debug.Log(_mainCam);
	}
	
	/*
	 * Set the pointer world and screen positions each frame.
	 */
	void Update () {
		_currentFrame = _leapController.Frame();

		Hand primeHand = frontmostHand();

		Finger primeFinger = Finger.Invalid;

		if(primeHand.IsValid)
		{
			primeFinger = pointingFigner(primeHand);

			if(primeFinger.IsValid) 
			{ 
				_pointerAvailible = true; 

				_pointerPositionWorld = primeFinger.TipPosition.ToUnityTranslated();
				//TODO: Needs Improvement: Doesn't work if camera is not looking at world origin.
				_pointerPositionScreen = _mainCam.WorldToScreenPoint(_pointerPositionWorld);
				_pointerPositionScreenToWorld = _mainCam.ScreenToWorldPoint(new Vector3(pointerPositionScreen.x,
				                                                                        pointerPositionScreen.y,
				                                                                        _screenToWorldDistance));
			}
			else
			{ 
				_pointerAvailible = false; 
			}
		}
	}

	//Public Instance Methods

	/*
	 * Get the screen coordinates of all the tracked fingers 
	 * on the given hand.
	 */
	public Vector2[] getScreenFingerPositions(Hand hand)
	{
		Vector2[] retArr = new Vector2[hand.Fingers.Count];

		for(int i=0;i<hand.Fingers.Count;i++) { retArr[i] = leapPositionToScreen(hand.Fingers[i].TipPosition); }

		return retArr;
	}

	/*
	 * Get the world coordinates of all the tracked 
	 * fingers on the given hand.
	 */
	public Vector3[] getWorldFingerPositions(Hand hand)
	{
		Vector3[] retArr = new Vector3[hand.Fingers.Count];
		
		for(int i=0;i<hand.Fingers.Count;i++) { retArr[i] = hand.Fingers[i].TipPosition.ToUnityTranslated(); }
		
		return retArr;
	}

	/*
	 * Take a Leap.Vector and convert it to Screen coordinates.
	 * 
	 * Exmaple: Vector3 handScreenPosition = _leapManager.leapPositionToScreen(_leapManager.frontmostHand().palmPosition);
	 */
	public Vector2 leapPositionToScreen(Vector leapVector)
	{
		return _mainCam.WorldToScreenPoint(leapVector.ToUnityTranslated());
	}

	/*
	 * Get the frontmost detected hand in the scene. 
	 * Returns Leap.Hand.Invalid if no hands are being tracked.
	 */
	public Hand frontmostHand()
	{
		float minZ = float.MaxValue;
		Hand forwardHand = Hand.Invalid;

		foreach(Hand hand in _currentFrame.Hands)
		{
			if(hand.PalmPosition.z < minZ)
			{
				minZ = hand.PalmPosition.z;
				forwardHand = hand;
			}
		}

		return forwardHand;
	}
	
}

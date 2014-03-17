==========================================

Project: LeapMotion Controller Unity C# Boilerplate
File: readme.txt
Author: Daniel Plemmons - LeapMotion Developer Experience Engineer
Twitter: @RandomOutput

==========================================

Unity Asset Version: 1.0.001
Leap Motion SDK Version: 1.0.9.8391
Leap Motion Software Version: 1.1.3.9188

==========================================


Hello! If you have trouble with this UnityPackage or if 
you have feedback on how it's built, please feel free to tweet at me. 
I threw this together very quickly as a helper for common tasks 
jammers might run into based on my own jam experience. Would love to 
know how it could be improved so others may benefit.

======================
Contents
======================

 - Dependencies and Installation
 - Package Contents
 - General Useage
 - LeapManager.cs Class Reference
 - Change History

======================
Dependencies and 
Installation
======================

1. Import this package into your Unity project.
2. Drag the LeapManager prefab into your scene.
3. Go make a cool game.
4. When your cool game is done (or before!) show it to us via @LeapMotion on twitter.

======================
Package Contents:
======================

The folders and files you must import for the helper to work have been marked here with "**" before the name.
All other folders are example resources to help get you up to speed quickly.

+ ** Leap_Boilerplate # The root folder for the helper.
 + example_scenes # Scenes showing example implementations of basic concepts.
  
  - Pointer_Movers.unity # Two game objects following a pointing finger in screen and world space.
  - Revolving_Camera.Unity # A camera controlled by your hand position moving around the earth. 
  - Single_Hand.unity # Maps game objects to palm and fingers. Displays some stats about the hand.
  - Finger_Physics.unity # Adds some rigid bodies to your fingers so you can move some cubes.

 + ** prefabs # prefab for the helper object.
  
  - ** LeapManager # The LeapManager GameObject containing resources to get your project (and the examples) going!

 + resources_for_example_scenes # A bunch of resources and assets for the example scenes. Best to ignore this.
  
  [various sordid textures and materials and such]

 + ** scripts # scripts for the helper object.
  
  - ** LeapExtensions.cs # Extensions to the Leap Vector class for Leap to Unity unit conversions.
  - ** LeapManager.cs # Centralizes LeapMotion data access and provides a wide variety of helper functions.

 + scripts_for_example_scenes
  
  - FingerCountChecker.cs # When attached to a text mesh, that mesh will say how many finger's you're holding up.
  - HandController.cs # When attached to a palm and linked to fingers, will model a hand's movement.
  - HandOpenChecker.cs # When attached to a text mesh, that mesh will say if your hand is open or not.
  - RevolvingCamera.cs # When attached to a camera and linked to a target, will rotate the camera around the target based on your hand position.
  - ScreenMover.cs # When attached to a GameObject will move that object in screen space based on your pointing finger.
  - WorldMover.cs # When attached to a GameObject will move that object in world space based on your pointing finger.

======================
General Usage
======================

This package is meant to act as a set of boilerplate functions and architecture to get your Leap enabled 
project up and running rapidly.

It solves the following problems:

- Providing a central point of access to the LeapMotion Controller data.

- Converting Leap data into Unity's units and Vector3 objects. 
 > This is accomplished via three extension methods which return Unity Vector3 objects:
  1. myVectorInstance.ToUnity() # For things like direction vectors which are normalized.
  2. myVectorInstance.ToUnityScales() # For things like Acceleration/Velocity where units must be converted, but position is not a factor.
  3. myVectorInstance.ToUnityTranslated() # For things like positions where units must be scaled AND we want to offset the coordinates into our game's coordinate space.

- Providing easy access to common data like pointing fingers and hand/finger locations in Unity screen or world coordinates.

- Providing helper methods for common concepts like "is this hand open?", or "how many fingers am I holding up?"

To use the LeapManager in your scene, simply drag the LeapManager prefab from the "prefabs" folder in the root into your scene. It's just an empty game object with the script attached.

A few of the class functions are static and can simply be accessed with a call to:

LeapManager.staticMethodName() 

...but most require a reference to an instance. As you can see in the example 
scenes, grabbing this instance is as "simple" as: 

LeapManager myLeapManagerInstance = (GameObject.Find("LeapManager") as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;

Remember that GameObject.Find() is really slow so you probably only want to do this one in your MonoBehavior's Start() function and store the reference for later. Again, see any of the example scenes for a full implementation.

For a listing of all the data and helper functions in the LeapManager use the next section, 'LeapManager.cs Class Reference'.

======================
LeapManger.cs 
Class Reference
======================

--------------------
Public Properties
--------------------
- Leap.Controller | leapController # A direct reference to the Controller for accessing the LeapMotion data yourself rather than going through the helper.
- Leap.Frame | currentFrame # The most recent frame of data from the LeapMotion controller.
- bool | pointerAvailible # Is there a pointing finger currently tracked in the scene.
- Vector2 | pointerPositionScreen # The currently tracked (if any) pointing finger in screen space.
- Vector3 | pointerPositionWorld # The currently tracked (if any) pointing finger in world space.
- Vector3 | pointerPositionScreenToWorld # The screen position of the currently tracked (if any) pointing finger projected into world space at a distance of [screenToWorldDistance].
- Vector3 | screenToWorldDistance # The projection distance for the pointerPositionScreenToWorld calculation. Default Value is 10.0f.
- Camera | _mainCam # Can be set to any camera you want. Used for screen position calculations. Defaults to "MainCamera"
--------------------
Public Methods
--------------------
- static  Leap.Finger | pointingFinger(Hand hand) # Returns the most likely finger to be pointing on the given hand. Returns Finger.Invalid if no such finger exists.
- static  ArrayList  | forwardFacingFingers(Hand hand) # Returns a list of fingers whose position is in front of the hand (relative to the hand direction). This is most useful in trying to lower the chances of detecting a thumb (though not a perfect method).
- static  bool  | isHandOpen(Hand hand) # Returns whether or not the given hand is open.
- void  | Start() # If the _mainCam isn't overridden, find the camera with the "MainCamera" tag.
- void  | Update() # Set the pointer world and screen positions each frame.
- Vector2[] | getScreenFingerPositions(Hand hand) # Get the screen coordinates of all the tracked fingers on the given hand.
- Vector3[] | getWorldFingerPositions(Hand hand) # Get the world coordinates of all the tracked fingers on the given hand.
- Vector2 | leapPositionToScreen(Vector leapVector) # Take a Leap.Vector and convert it to Screen coordinates. Exmaple: Vector3 handScreenPosition = _leapManager.leapPositionToScreen(frame.Hands[0].palmPosition);
- Leap.Hand  | frontmostHand() # Get the frontmost detected hand in the scene. Returns Leap.Hand.Invalid if no hands are being tracked.


======================
Change History
======================

v1.0.001
Fixed issue where projection distance was being ignored in the example. Added colliders to act as a bounding box in the physics example.

v1.0.000
First Release



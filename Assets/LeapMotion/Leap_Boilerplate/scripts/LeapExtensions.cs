/* 
 * Requires the LeapMotion SDK
 * http://developer.leapmotion.com
 * Unity Project Setup: https://developer.leapmotion.com/documentation/Languages/CSharpandUnity/Guides/Setup_Unity.html
 */

using UnityEngine;
using System.Collections;
using Leap;

namespace Leap
{
	//Extension to the unity vector class. Provides automatic scaling into unity scene space.
	//Leap coordinates are in cm, so the .02f scaling factor means 1cm of hand motion = .02m scene motion
	public static class LeapExtensions
	{
		public static Vector3 InputScale = new Vector3(0.04f, 0.04f, 0.04f);
		public static Vector3 InputOffset = new Vector3(0,-8,0);
		
		//For Directions
		public static Vector3 ToUnity(this Vector lv)
		{
			return FlippedZ(lv);
		}
		//For Acceleration/Velocity
		public static Vector3 ToUnityScaled(this Vector lv)
		{
			return Scaled(FlippedZ( lv ));
		}
		//For Positions
		public static Vector3 ToUnityTranslated(this Vector lv)
		{
			return Offset(Scaled(FlippedZ( lv )));
		}
		
		private static Vector3 FlippedZ( Vector v ) { return new Vector3( v.x, v.y, -v.z ); }
		private static Vector3 Scaled( Vector3 v ) { return new Vector3( v.x * InputScale.x,
																		 v.y * InputScale.y,
																		 v.z * InputScale.z ); }
		private static Vector3 Offset( Vector3 v ) { return v + InputOffset; }
	}
}
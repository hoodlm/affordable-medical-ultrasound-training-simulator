using UnityEngine;

public static class UltrasoundCollisionUtils {
	
	private static readonly Vector3[] raycastDirections;
	
	static UltrasoundCollisionUtils() {
		raycastDirections = new Vector3[5];
		raycastDirections[0] = new Vector3(0,1,0);
		raycastDirections[1] = new Vector3(0,-1,-0);
		raycastDirections[2] = new Vector3(0,0,1);
		raycastDirections[3] = new Vector3(-1.41f, 0, -0.5f);
		raycastDirections[4] = new Vector3(1.41f, 0, -0.5f);
	}
	
	/// <summary>
	/// Uses a heuristic raycasting approach to decide if a point in world space is contained in a collider.
	/// </summary>
	/// <returns>
	/// Is this point contained in the collider?
	/// </returns>
	/// <param name='targetPoint'>
	/// The point in world space being tested.
	/// </param>
	/// <param name='collider'>
	/// The collider to use as a bounds.
	/// </param>
	public static bool IsContained (Vector3 targetPoint, Collider collider) {
		// Simple low-hanging fruit test - if the targetPoint isn't in the bounding box, it can't be in the collider.
		if (!collider.bounds.Contains(targetPoint)) {
			return false;
		}
		
		// We can use a heuristic of checking several directions from our test point.
		
		foreach (Vector3 direction in raycastDirections) {
			Ray ray = new Ray(targetPoint - 100f * direction, direction);
			
		/* There doesn't seem to be a good way in the unity libraries to check if a point is contained in an object.
		 * 
		 * This is a "bed of nails" approach by checking the point from several directions.
		 * 
		 * If from any direction, we don't hit the collider, that point probably cannot be contained inside
		 * the object.
		 */
			RaycastHit dummyHit = new RaycastHit();
			if (!collider.Raycast(ray, out dummyHit, float.PositiveInfinity)) {
				return false;
			}
		}
		
		return true;
	}
}
using UnityEngine;
using System.Collections;

public class CachedTransform {

	private readonly Transform transform;
	private readonly Quaternion cachedRotation;
	private readonly Vector3 cachedPosition;
	
	public CachedTransform() {
		transform = null;
	}
	
	public CachedTransform(Transform t) {
		if (null != t) {
			this.transform = t; // Deep copy
			cachedRotation = new Quaternion(t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w);
			cachedPosition = new Vector3(t.position.x, t.position.y, t.position.z);
		}
	}
	
	public bool isDirty() {
		if (transform == null) {
			return true;
		} else {
			return (!transform.rotation.Equals(cachedRotation)
				 || !transform.position.Equals(cachedPosition));
		}
	}
}

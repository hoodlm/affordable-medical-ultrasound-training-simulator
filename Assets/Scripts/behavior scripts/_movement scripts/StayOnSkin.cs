using UnityEngine;
using System.Collections;

public class StayOnSkin : MonoBehaviour {
	
	private GameObject skin;
	
	// Use this for initialization
	void Start () {
		skin = GameObject.FindGameObjectWithTag("Skin");
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = ClosestPointOnSkin();
	}
	
	private Vector3 ClosestPointOnSkin() {
		// 10f is a magic number so we start far enough back
		Vector3 origin = transform.position - transform.forward * 10f;
		Ray ray = new Ray(origin, transform.forward);
		
		RaycastHit hit;
		skin.collider.Raycast(ray, out hit, 20f);
		
		return hit.point;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 *	Class responsible for detecting which organs can possibly be hit by raycasts.
 */
public class HorayOrganCuller {
	
	/// List of every GameObjects in the scene with a HorayMaterialProperties component.
	private readonly IList<GameObject> allOrgansInScene;

	/**	When organs are detected as visible, they are added to this queue.
	 * 	The Key for the organ should be the time that it was added (as measured by Time.time).
	 * 	Organs are removed from the dictionary after EXPIRATION_TIME.
	 */
	private SortedDictionary<float, GameObject> currentVisibleOrgans;

	/**
	 *	If a GameObject is already in currentVisibleOrgans, but we need to update its key,
	 *	we need to be able to get the key for the object. .NET does not have a two-way 
	 *	SortedDictionary, so we	must create another dictionary structure to hold the inverse relationship.
	 */
	private IDictionary<GameObject, float> reverseMapLookup;

	/// The number of scanlines to check per frame
	private int SCANLINES_PER_FRAME = 5;

	/// How long to wait to remove an organ.
	private float EXPIRATION_TIME_IN_SECONDS = 5f;

	/// The last scanline checked during the last frame.
	private int scanlineIndex;

	/**
	 *	Instantiate a new HorayOrganCuller.
	 */
	public HorayOrganCuller()
	{
		currentVisibleOrgans 	= new SortedDictionary<float, GameObject>();
		reverseMapLookup 		= new Dictionary<GameObject, float>();

		allOrgansInScene 		= new List<GameObject>();
		object[] allGameObjectsInScene = GameObject.FindObjectsOfType<GameObject>();

		foreach(Object anObject in allGameObjectsInScene) {
			GameObject gameObject = (GameObject)anObject;
			if (null != gameObject.GetComponent<HorayMaterialProperties>()) {
				allOrgansInScene.Add(gameObject);
			}
		}
		UltrasoundDebug.Assert(allOrgansInScene.Count > 0, 
		                       "No valid HORAY organs were found.",
		                       this);
	}

	/**
	 *	Based on a probe configuration, determine which organs can possibly appear in the image.
	 *	@param config The current UltrasoundProbeConfiguration
	 *	@return A culled list of GameObjects to test for collisions.
	 */
	public IList<GameObject> HitableOrgansOnScanlines(IList<UltrasoundScanline> scanlines,
	                                                  UltrasoundProbeConfiguration config)
	{
		CheckScanlines(scanlines);
		RemoveExpiredObjects();
		return new List<GameObject>(currentVisibleOrgans.Values);
	}

	private void CheckScanlines(IList<UltrasoundScanline> scanlines) {
		int scanlineCount = scanlines.Count;

		// Rather than checking scanlines sequentially, we try to check them somewhat uniformly so
		// that new objects are more likely to be noticed within a few frames. Therefore, we generate
		// a number smaller than scanlineCount that is NOT a divisor of scanlineCount.
		int scanlineStepSize = (scanlineCount / (SCANLINES_PER_FRAME + 1));
		while (scanlineCount % scanlineStepSize == 0) {
			scanlineStepSize++;
		}

#if UNITY_EDITOR
		UltrasoundDebug.Assert(scanlineCount > 0, "Zero scanlines passed to culler!", this);
#endif
		for (int i = 0; i < SCANLINES_PER_FRAME; ++i) {
			scanlineIndex = (scanlineIndex + scanlineStepSize) % scanlineCount;
//			Debug.Log (scanlineIndex);

			IList<GameObject> organsToAdd = (ValidOrgansOnScanline(scanlines[scanlineIndex]));
			foreach (GameObject organ in organsToAdd) {
#if UNITY_EDITOR
				UltrasoundDebug.Assert(null != organ.GetComponent<HorayMaterialProperties>(), 
				                       "organ without HORAY properties was added",
				                       this);
#endif
				if (!reverseMapLookup.ContainsKey(organ)) {
					// Store in a temp variable since Time.time may change between the two assignments below.
					float keyTime = Time.time;

					/* On faster machines, two raycasts may be completed before Time.time has been measureably
					 * incremented. In this situation, we can nudge the time forward a little bit to avoid a
					 * key collision.
					 */
					while (currentVisibleOrgans.ContainsKey(keyTime)) {
						keyTime += 0.01f;
					}

					currentVisibleOrgans.Add(keyTime, organ);
					reverseMapLookup.Add(organ, keyTime);
//					Debug.Log(string.Format("Added organ {0}", organ.name));
				} else {
					float oldTime = float.NegativeInfinity;
					reverseMapLookup.TryGetValue(organ, out oldTime);
#if UNITY_EDITOR
					UltrasoundDebug.Assert(oldTime > float.NegativeInfinity, 
					                       string.Format("Organ {0} wasn't in reverse map", organ.name),
					                       this);
#endif
					currentVisibleOrgans.Remove(oldTime);
					reverseMapLookup.Remove(organ);
					float newTime = Time.time;

					/* On faster machines, two raycasts may be completed before Time.time has been measureably
					 * incremented. In this situation, we can nudge the time forward a little bit to avoid a
					 * key collision.
					 */
					while (currentVisibleOrgans.ContainsKey(newTime)) {
						newTime += 0.01f;
					}

					currentVisibleOrgans.Add(newTime, organ);
					reverseMapLookup.Add(organ, newTime);
//					Debug.Log(string.Format("Changed organ {0} time from {1} to {2}", 
//					                        organ.name, oldTime, newTime));
				}
			}
		}
	}

	private IList<GameObject> ValidOrgansOnScanline(UltrasoundScanline scanline) {
		IList<UltrasoundPoint> points = scanline.GetPoints();
#if UNITY_EDITOR
		UltrasoundDebug.Assert(points.Count > 0, "Zero points on scanline", this);
#endif
		Vector3 terminalPoint = points[points.Count - 1].GetWorldSpaceLocation();
		Ray ray = new Ray(scanline.origin, terminalPoint - scanline.origin);
		float raycastDistance = (terminalPoint - scanline.origin).magnitude;
		RaycastHit[] hits = Physics.RaycastAll(ray, raycastDistance);
		IList<GameObject> validOrgans = new List<GameObject>();
		foreach(RaycastHit hit in hits) {
			if (allOrgansInScene.Contains(hit.collider.gameObject)) {
				validOrgans.Add (hit.collider.gameObject);
			}
		}

		return validOrgans;
	}

	/**
	 * 	Removes organs that are visible, but haven't been seen recently enough.
	 * 	(i.e. the time that the organ was last hit by a raycast is more than EXPIRATION_TIME_IN_SECONDS ago) 
	 */
	private void RemoveExpiredObjects()
	{
//		Debug.Log(string.Format("{0} organs visible before removal.", currentVisibleOrgans.Count));

		/* We store the keys and organs to remove in temporary lists. This is to avoid bugs associated
		 * with editing the items in a Dictionary while iterating over them in a foreach loop.
		 */
		IList<float> keysToRemove = new List<float>();
		IList<GameObject> organsToRemove = new List<GameObject>();

		foreach (float timeLastSeen in currentVisibleOrgans.Keys) {
			float timeElapsed = Time.time - timeLastSeen;
			if (timeElapsed >= EXPIRATION_TIME_IN_SECONDS) {
				GameObject organToRemove = null;
				currentVisibleOrgans.TryGetValue(timeLastSeen, out organToRemove);
#if UNITY_EDITOR
				UltrasoundDebug.Assert(null != organToRemove, 
				                       string.Format("Couldn't find organ added at time {0}", timeLastSeen),
				                       this);
#endif
				keysToRemove.Add(timeLastSeen);
				organsToRemove.Add(organToRemove);
//				Debug.Log(string.Format("{0} marked for removal; last seen {1} seconds ago.", 
//				                        organToRemove.name, timeElapsed));
			} else {
				// The SortedDictionary implementation will store the oldest objects first.
				// As soon as we reach an object that isn't expired, we're done.
				break;
			}
		}
#if UNITY_EDITOR
		UltrasoundDebug.Assert(organsToRemove.Count == keysToRemove.Count,
		                        string.Format("{0} organs marked for removal, {1} keys marked for removal",
		              							organsToRemove.Count, keysToRemove.Count),
		                        this);
#endif
		for (int i = 0; i < organsToRemove.Count; ++i) {
			currentVisibleOrgans.Remove(keysToRemove[i]);
			reverseMapLookup.Remove(organsToRemove[i]);
		}

//		Debug.Log(string.Format("{0} organs visible after removal.", currentVisibleOrgans.Count));
#if UNITY_EDITOR
		UltrasoundDebug.Assert (currentVisibleOrgans.Count == reverseMapLookup.Count,
		                        string.Format("currentVisibleOrgans.Count = {0}, reverseMapLookup.Count = {1}",
		              						   currentVisibleOrgans.Count, reverseMapLookup.Count),
								this);
#endif
	}
}
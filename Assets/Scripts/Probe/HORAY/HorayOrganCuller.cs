using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 *	Class responsible for detecting which organs can possibly be hit by raycasts.
 *	This function is roughly analagous to "culling" in a traditional 3D renderings pipeline.
 *	
 *	There may be a short (2-5 frame) delay before new objects are recognized as potentially visible.
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

	/// The number of scanlines to check per frame. This may be increased on faster machines so that newly
	/// visible organs are recognized more quickly.
	private int SCANLINES_PER_FRAME = 5;

	/// How long to wait to remove an organ. If the probe is configured for a large number of scanlines
	/// then this time may need to be increased.
	private float EXPIRATION_TIME_IN_SECONDS = 5f;

	/// The last scanline checked during the previous frame.
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
	 *	@param scanlines The list of scanlines to be checked.
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

	/**
	 *	Checks a list of scanlines for visible objects. If an object on the scanlines is hit by
	 *	a raycast, it is marked as visible (added to the currentVisibleOrgans and reverseMapLookup dictionaries).
	 *
	 *	Visible objects are given an expiration time (EXPIRATION_TIME_IN_SECONDS after the present).
	 *	If an object that is already known to be visible is detected, its expiration time is reset.
	 *
	 *	This method only checks a few scanlines each frame (as defined in the SCANLINES_PER_FRAME constant).
	 *	We attempt to check scanlines that are spread apart, rather than checking sequentially, so that new 
	 *	objects are detected more quickly. For example, if there are 10 scanlines in total, it is more helpful
	 *	to check scanlines (0, 3, 6, 9) in a single frame than to check (0, 1, 2, 3).
	 *
	 *	@param scanlines The set of all scanlines sent out by this probe.
	 */
	private void CheckScanlines(IList<UltrasoundScanline> scanlines) {
		int scanlineCount = scanlines.Count;
#if UNITY_EDITOR
		UltrasoundDebug.Assert(scanlineCount > 0, "Zero scanlines passed to culler!", this);
#endif

		// Rather than checking scanlines sequentially, we try to check them somewhat uniformly so
		// that new objects are more likely to be noticed within a few frames. Therefore, we generate
		// a number smaller than scanlineCount, but one that is NOT a divisor of scanlineCount.
		int scanlineStepSize = (scanlineCount / (SCANLINES_PER_FRAME + 1));
		while (scanlineCount % scanlineStepSize == 0) {
			scanlineStepSize++;
		}

		for (int i = 0; i < SCANLINES_PER_FRAME; ++i) {
			scanlineIndex = (scanlineIndex + scanlineStepSize) % scanlineCount;

			IList<GameObject> organsToAdd = (ValidOrgansOnScanline(scanlines[scanlineIndex]));
			foreach (GameObject organ in organsToAdd) {
#if UNITY_EDITOR
				UltrasoundDebug.Assert(null != organ.GetComponent<HorayMaterialProperties>(), 
				                       "organ without HORAY properties was added",
				                       this);
#endif
				if (!reverseMapLookup.ContainsKey(organ)) {
					// Store in a temp variable since Time.time may change between assignments below.
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
				}

				else {
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
				}
			}
		}
	}

	/// Checks an individual scanline for valid organs.
	/// @param scanline The UltrasoundScanline being tested
	/// @return A list of organs that are on the scanline.
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
		/* We store the keys and organs to remove in temporary lists. This is to avoid bugs associated
		 * with editing the items in a Dictionary while iterating over it in a foreach loop.
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
			} else {
				// Remember that the SortedDictionary implementation will store the oldest objects first.
				// As soon as we reach an object that isn't expired, we know that all the rest won't be expired either.
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

#if UNITY_EDITOR
		UltrasoundDebug.Assert (currentVisibleOrgans.Count == reverseMapLookup.Count,
		                        string.Format("currentVisibleOrgans.Count = {0}, reverseMapLookup.Count = {1}",
		              						   currentVisibleOrgans.Count, reverseMapLookup.Count),
								this);
#endif
	}
}
using UnityEngine;
using System.Collections.Generic;

/**
 *	Class responsible for detecting which organs can possibly be hit by raycasts.
 */
public class HorayOrganCuller {
	
	/// List of every GameObjects in the scene with a HorayMaterialProperties component.
	private IList<GameObject> allOrgansInScene;

	/**
	 *	Instantiate a new HorayOrganCuller.
	 */
	public HorayOrganCuller()
	{
		allOrgansInScene = new List<GameObject>();
		object[] allGameObjectsInScene = GameObject.FindObjectsOfType<GameObject>();

		foreach(Object anObject in allGameObjectsInScene) {
			GameObject gameObject = (GameObject)anObject;
			if (null != gameObject.GetComponent<HorayMaterialProperties>()) {
				allOrgansInScene.Add(gameObject);
			}
		}
	}

	/**
	 *	Based on a probe configuration, determine which organs can possibly appear in the image.
	 *	@param config The current UltrasoundProbeConfiguration
	 *	@return A culled list of GameObjects to test for collisions.
	 */
	public IList<GameObject> HitableOrgans(UltrasoundProbeConfiguration config)
	{
		// TODO
		return this.allOrgansInScene;
	}
}

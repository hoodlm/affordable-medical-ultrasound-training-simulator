using UnityEngine;
using System.Collections.Generic;

/**
 *	Simulate the functionality of an Ultrasound probe using HORAY:
 *	HOmogenious RAYcasting.
 */
public class HorayProbe {

	/// Used to determine which GameObjects can possibly appear in the image.
	protected readonly HorayOrganCuller culler;

	/// The GameObject representing the physical location of the probe.
	protected readonly GameObject probeGameObject;

	/// The IProbeOutput providing data to an IImageSource.
	protected readonly IProbeOutput output;

	/**
	 *	Instantiate a new HorayProbe.
	 *	@param probe The GameObject that represents the scanning array.
	 *	@param output An IProbeOutput to pass information to.
	 */
	public HorayProbe(GameObject probe, IProbeOutput output) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(null != probe, 
		                       "Null probe GameObject passed to HorayProbe constructor.",
		                       this);
		UltrasoundDebug.Assert(null != output, 
		                       "Null IProbeOutput passed to HorayProbe constructor.",
		                       this);
		if (null == probe.GetComponent<HorayBehavior>()) {
			string str = 
				"The probe object used to instantiate this class " +
				"does not have the required HorayBehavior script.";
			Debug.LogError(str);
		}
#endif
		this.probeGameObject = probe;
		this.output = output;
		this.culler = new HorayOrganCuller();
	}

	/**
	 * 	Populates an UltrasoundScanData with scan data for this frame.
	 * 	@param data The object into which to put the scan data.
	 */
	public void PopulateData(ref UltrasoundScanData data) {
#if UNITY_EDITOR
		UltrasoundDebug.Assert(null != data, 
		                       "Null data object passed to HorayProbe's PopulateData method.",
		                       this);
#endif
		EstablishScanningPlane(ref data);
		IList<GameObject> culledOrganList = culler.HitableOrgans(data.GetProbeConfig());
		ScanPointsForOrgans(ref data, culledOrganList);
	}

	/**
	 *	"Shades" all points in an UltrasoundScanData by checking their position against a
	 *	list of GameObjects that could conceiveably contain those points.
	 */
	private void ScanPointsForOrgans(ref UltrasoundScanData data, IList<GameObject> organList)
	{
		foreach (UltrasoundScanline scanline in data) {
			foreach (UltrasoundPoint point in scanline) {
				foreach (GameObject gameObject in organList) {
					Vector3 target = point.GetWorldSpaceLocation();
					Collider collider = gameObject.collider;
					bool hit = UltrasoundCollisionUtils.IsContained(target, collider);

					if (hit) {
						point.SetBrightness(1f);
					}
				}
			}
		}
	}

	/**
	 * 	Set up the data object with empty UltrasoundPoint%s representing the points
	 * 	that need to be scanned.
	 * 
	 * 	This is analagous to setting up the view frustum in traditional 3D graphics.
	 */
	private void EstablishScanningPlane(ref UltrasoundScanData data) {
		UltrasoundProbeConfiguration config = data.GetProbeConfig();

		// nearZ and farZ represent near and far clipping "planes" (they're really arcs)
		float nearZ = config.GetMinScanDistance();
		float farZ	= config.GetMaxScanDistance();

#if UNITY_EDITOR
		UltrasoundDebug.Assert(farZ > nearZ, 
		                       "Max distance should be greater than min distance!",
		                       this);
#endif

		// Currently hard-coded -- these should be in the probe config eventually.
		const int POINTS_PER_SCANLINE	= 50;
		const int SCANLINES				= 50;
		const float ARC_SIZE_IN_DEGREES	= 75;

		for (int i = 0; i < SCANLINES; ++i) {
			UltrasoundScanline scanline = new UltrasoundScanline();
			float angleInDegrees = -(ARC_SIZE_IN_DEGREES / 2) + i * ARC_SIZE_IN_DEGREES / (SCANLINES - 1);
			float angleInRadians = Mathf.Deg2Rad * angleInDegrees;
			Vector2 trajectory = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

			for (int j = 0; j < POINTS_PER_SCANLINE; ++j) {
				float d = nearZ + j * (farZ - nearZ) / (POINTS_PER_SCANLINE - 1);
				Vector2 positionOnPlane = d * trajectory;
				Vector3 positionInWorldSpace = WorldSpaceFromProjectedPosition(positionOnPlane, config);
				UltrasoundPoint point = new UltrasoundPoint(positionInWorldSpace, positionOnPlane);
				scanline.AddUltrasoundPoint(point);
			}

			data.AddScanline(scanline);
		}
	}

	/**
	 *	Helper method to calculate a Vector3 (WorldSpace) from a Projected Position in local space.
	 *	@param positionInPlace A Vector2 representing a point in the scanning plane.
	 *	@param config The UltrasoundProbeConfiguration object, used for rotation and translation.
	 *	@return A 3D point in world space.
	 */
	private Vector3 WorldSpaceFromProjectedPosition(Vector2 positionInPlane, 
	                                                UltrasoundProbeConfiguration config)
	{
		Vector3 positionInWorldSpace = new Vector3(positionInPlane.x, 0, positionInPlane.y);

		// Apply rotation, using Quaternion's overloaded * operator.
		positionInWorldSpace = config.GetRotation() * positionInWorldSpace;

		positionInWorldSpace += config.GetPosition();
		return positionInWorldSpace;
	}
}

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
	 *	Checks all points in an UltrasoundScanData point array against a list of possible organs
	 *	that can be contained in those points. The points are then appropriately shaded based
	 *	on the material properties of those organs.
	 *
	 *	@param data An UltrasoundScanData populated with empty UltrasoundPoint objects.
	 *	@param organList A list of GameObjects to check against.
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
		float nearZ 			= config.GetMinScanDistance();
		float farZ				= config.GetMaxScanDistance();

		float arcSizeDegrees 	= config.GetArcSizeInDegrees();
		int scanlines 			= config.GetNumberOfScanlines();
		int pointsPerScanline 	= config.GetPointsPerScanline();

		for (int i = 0; i < scanlines; ++i) {
			UltrasoundScanline scanline = new UltrasoundScanline();
			float angleInDegrees = -(arcSizeDegrees / 2) + i * arcSizeDegrees / (scanlines - 1);
			float angleInRadians = Mathf.Deg2Rad * angleInDegrees;
			Vector2 trajectory = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

			for (int j = 0; j < pointsPerScanline; ++j) {
				float d = nearZ + j * (farZ - nearZ) / (pointsPerScanline - 1);
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

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
		UltrasoundInputValidator.CheckNotNull(probe);
		UltrasoundInputValidator.CheckNotNull(output);
		if (null == probe.GetComponent<HorayBehavior>()) {
			string str = 
				"The probe object used to instantiate this class " +
				"does not have the required HorayBehavior script.";
			throw new System.ArgumentException(str);
		}
		this.probeGameObject = probe;
		this.output = output;
		this.culler = new HorayOrganCuller();
	}

	/**
	 * 	Populates an UltrasoundScanData with scan data for this frame.
	 * 	@param data The object into which to put the scan data.
	 */
	public void PopulateData(ref UltrasoundScanData data) {
		UltrasoundInputValidator.CheckNotNull(data);
		EstablishScanningPlane(ref data);
		IList<GameObject> culledOrganList = culler.HitableOrgans(data.GetProbeConfig());
	}

	/**
	 * 	Populate the data object with empty UltrasoundPoint%s representing the points
	 * 	that need to be scanned.
	 * 
	 * 	This is analagous to setting up the view frustum in traditional 3D graphics.
	 */
	private void EstablishScanningPlane(ref UltrasoundScanData data) {
		UltrasoundProbeConfiguration config = data.GetProbeConfig();

		// nearZ and farZ represent near and far clipping "planes" (they're really arcs)
		float nearZ = config.GetMinScanDistance();
		float farZ	= config.GetMaxScanDistance();

		// Currently hard-coded -- these should be in the probe config eventually.
		const int POINTS_PER_SCANLINE	= 40;
		const int SCANLINES				= 40;
		const float ARC_SIZE_IN_DEGREES	= 75;

		for (int i = 0; i < SCANLINES; ++i) {
			UltrasoundScanline scanline = new UltrasoundScanline();
			float angleInDegrees = -(ARC_SIZE_IN_DEGREES / 2) + i * ARC_SIZE_IN_DEGREES / (SCANLINES - 1);
			float angleInRadians = Mathf.Deg2Rad * angleInDegrees;
			Vector2 trajectory = new Vector2(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians));

			for (int j = 0; j < POINTS_PER_SCANLINE; ++j) {
				float d = nearZ + j * (farZ - nearZ) / (POINTS_PER_SCANLINE - 1);
				Vector2 positionOnPlane = d * trajectory;
				UltrasoundPoint point = new UltrasoundPoint(Vector3.zero, positionOnPlane);
				point.SetBrightness(1f);
				scanline.AddUltrasoundPoint(point);
			}

			data.AddScanline(scanline);
		}
	}
}

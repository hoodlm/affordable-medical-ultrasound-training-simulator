using UnityEditor;

[CustomEditor (typeof(ProbeConfiguration))]
public class ProbeConfigurationEditor : Editor {

	private float minDistanceValue = 1;
	private float maxDistanceValue = 10;
	private int pointsPerScanline = 20;
	private float arcSize = 90;
	private int scanlineCount = 90;
	private float noiseLevel = 0.1f;

	public override void OnInspectorGUI () {
		
		ProbeConfiguration config = (ProbeConfiguration) target;
		RetrieveValuesFrom(config);
		
		if (minDistanceValue.Equals(maxDistanceValue)) {
			EditorGUILayout.LabelField("Warning: Min should be less than Max!");
		} else {
			EditorGUILayout.LabelField(string.Empty);
		}
		
		config.SetMinDistance(EditorGUILayout.Slider("Min Distance", minDistanceValue, 0, 20));
		config.SetMaxDistance(EditorGUILayout.Slider("Max Distance", maxDistanceValue, 0, 20));
		config.SetArcSize(EditorGUILayout.Slider("Arc size (deg)", arcSize, 1, 359));
		config.SetPointsPerScanline(EditorGUILayout.IntSlider("Points per scanline", pointsPerScanline, 1, 200));
		config.SetScanlineCount(EditorGUILayout.IntSlider("Number of scanlines", scanlineCount, 1, 400));
		config.SetNoiseLevel(EditorGUILayout.Slider("Noise level", noiseLevel, 0f, 1f));
    }
	
	private void RetrieveValuesFrom(ProbeConfiguration config) {
		this.minDistanceValue = config.GetMinDistance();
		this.maxDistanceValue = config.GetMaxDistance();
		this.pointsPerScanline = config.GetPointCount();
		this.arcSize = config.GetArcSize();
		this.scanlineCount = config.GetScanlineCount();
		this.noiseLevel = config.GetNoiseLevel();
	}
}

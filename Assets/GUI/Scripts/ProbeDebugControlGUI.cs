using UnityEngine;
using System.Collections;

public class ProbeDebugControlGUI : MonoBehaviour {
	
	public Rect windowRect = new Rect(250, 20, 180, 50);
	public string windowTitle = "Probe Settings";
	
	private GameObject probeGameObject;
	private ProbeConfiguration config;
	private Probe probeScript;
	
	private CachedProbeConfig cachedConfig;
	
	private float minDistanceValue;
	private float maxDistanceValue;
	private int pointsPerScanline;
	private float arcSize;
	private int scanlineCount;
	private float noiseLevel;
	
	private float leftMargin = 10f;
	private float topMargin = 20f;
	private float labelHeight = 22f;
	private float labelWidth;
	private float GUISpacing = 25f;
	
	// Use this for initialization
	void Start () {
		probeGameObject = GameObject.FindGameObjectWithTag("probe");
		probeScript = probeGameObject.GetComponent<Probe>();
		config = probeGameObject.GetComponent<ProbeConfiguration>();
		cachedConfig = new CachedProbeConfig(config);
		
		labelWidth = windowRect.width - 2 * leftMargin;
		
		// position window
		windowRect.x = Screen.width - windowRect.width - leftMargin;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {		
		RetrieveValuesFromConfig();
        windowRect = GUI.Window(1, windowRect, ListOptions, windowTitle);
		if (cachedConfig.isDirty()) {
			cachedConfig = new CachedProbeConfig(config);
			probeScript.RefreshConfig();
			GameObject.FindGameObjectWithTag("display").SendMessage("DrawThisFrame");
		}
    }
	
	void ListOptions(int windowID) {
		
		Rect MinMaxWarningRect = new Rect(leftMargin, topMargin, labelWidth, labelHeight*2);
		if (minDistanceValue.Equals(maxDistanceValue)) {
			GUI.Label(MinMaxWarningRect, "Warning: Min should be less than Max!");
		} else {
			GUI.Label(MinMaxWarningRect, string.Empty);
		}
		
		Rect MinDistanceLabelRect = new Rect(leftMargin, topMargin + 2*GUISpacing, labelWidth, labelHeight);
		GUI.Label(MinDistanceLabelRect, "Minimum Distance: "+minDistanceValue);
		Rect MinDistanceSliderRect = new Rect(leftMargin, topMargin + 3*GUISpacing, labelWidth, labelHeight);
		config.SetMinDistance(GUI.HorizontalSlider(MinDistanceSliderRect, minDistanceValue, float.Epsilon, 4f));

		Rect MaxDistanceLabelRect = new Rect(leftMargin, topMargin + 4*GUISpacing, labelWidth, labelHeight);
		GUI.Label(MaxDistanceLabelRect, "Maximum Distance: "+maxDistanceValue);
		Rect MaxDistanceSliderRect = new Rect(leftMargin, topMargin + 5*GUISpacing, labelWidth, labelHeight);
		config.SetMaxDistance(GUI.HorizontalSlider(MaxDistanceSliderRect, maxDistanceValue, float.Epsilon, 5f));
		
		Rect ArcSizeLabelRect = new Rect(leftMargin, topMargin + 6*GUISpacing, labelWidth, labelHeight);
		GUI.Label(ArcSizeLabelRect, "Arc Size: "+arcSize+" degrees");
		Rect ArcSizeSliderRect = new Rect(leftMargin, topMargin + 7*GUISpacing, labelWidth, labelHeight);
		config.SetArcSize(GUI.HorizontalSlider(ArcSizeSliderRect, arcSize, 1, 359));
		
		Rect PointsPerScanlineLabelRect = new Rect(leftMargin, topMargin + 8*GUISpacing, labelWidth, labelHeight);
		GUI.Label(PointsPerScanlineLabelRect, "Points per scanline: "+pointsPerScanline);
		Rect PointsPerScanlineSliderRect = new Rect(leftMargin, topMargin + 9*GUISpacing, labelWidth, labelHeight);
		config.SetPointsPerScanline((int)GUI.HorizontalSlider(PointsPerScanlineSliderRect, pointsPerScanline, 1, 200));
		
		Rect ScanlineCountLabelRect = new Rect(leftMargin, topMargin + 10*GUISpacing, labelWidth, labelHeight);
		GUI.Label(ScanlineCountLabelRect, "Number of Scanlines: "+scanlineCount);
		Rect ScanlineCountSliderRect = new Rect(leftMargin, topMargin + 11*GUISpacing, labelWidth, labelHeight);
		config.SetScanlineCount((int)GUI.HorizontalSlider(ScanlineCountSliderRect, scanlineCount, 1, 400));
		
		Rect NoiseLevelLabelRect = new Rect(leftMargin, topMargin + 12*GUISpacing, labelWidth, labelHeight);
		GUI.Label(NoiseLevelLabelRect, "Noise level: "+noiseLevel);
		Rect NoiseLevelSliderRect = new Rect(leftMargin, topMargin + 13*GUISpacing, labelWidth, labelHeight);
		config.SetNoiseLevel(GUI.HorizontalSlider(NoiseLevelSliderRect, noiseLevel, 0f, 1f));
		
		GUI.DragWindow();
    }
	
	private void RetrieveValuesFromConfig() {
		this.minDistanceValue = config.GetMinDistance();
		this.maxDistanceValue = config.GetMaxDistance();
		this.pointsPerScanline = config.GetPointCount();
		this.arcSize = config.GetArcSize();
		this.scanlineCount = config.GetScanlineCount();
		this.noiseLevel = config.GetNoiseLevel();
	}
}

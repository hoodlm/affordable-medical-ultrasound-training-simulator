using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Display : MonoBehaviour {
	
	public int displayWidth = 640;
	public int displayHeight = 480;
	
	public Color bgrdColor = Color.black;
	public Color defaultColor = Color.white;
	
	public bool colorMode = true;
	
	public GameObject probeGameObject;
	private Probe probe;
	private ProbeConfiguration probeConfig;
	
	// We cache the probe's position and rotation. If the probe hasn't moved, we won't need to redraw the image.
	private CachedTransform cachedProbeTransform;
	
	// An override for cacheing behavior.
	private bool forceDrawNextFrame = false;
	
	// lots of SetPixel calls are very expensive, so colors are stored in buffer before drawn to texture.
	private Color[] colorBuffer;
	
	private Texture2D texture;
	private int pixelCount;
	
	// Use this for initialization
	void Start () {
		texture = new Texture2D(displayWidth, displayHeight, TextureFormat.RGB24, false);
		renderer.material.mainTexture = texture;
		
		pixelCount 	= displayWidth * displayHeight;
		colorBuffer = new Color[pixelCount];
		
		if (null == probeGameObject) {
			probeGameObject = GameObject.FindGameObjectWithTag("probe");
		}
		
		probe = probeGameObject.GetComponent<Probe>();
		probeConfig = probe.RefreshConfig();
		cachedProbeTransform = new CachedTransform();
	}
	
	// Update is called once per frame
	void Update () {
		if (cachedProbeTransform.isDirty() || forceDrawNextFrame) {
			cachedProbeTransform = new CachedTransform(probe.transform);
			DrawToDisplay(probe.Scan());
		}
		forceDrawNextFrame = false;
		
		if (Input.GetKeyDown(KeyCode.G)) {
			colorMode = !colorMode;
			forceDrawNextFrame = true;
		}
	}
	
	/// <summary>
	/// Force the display to draw this frame, overriding any cacheing behavior.
	/// </summary>
	public void DrawThisFrame() {
		forceDrawNextFrame = true;
	}
	
	public void ReadProbeConfig() {
		probeConfig = probe.GetComponent<ProbeConfiguration>();
	}
	
	private void DrawToDisplay(UltrasoundScanline[] scanlines) {
		ClearDisplay();
		
		foreach (UltrasoundScanline scanline in scanlines) {
			foreach (UltrasoundPoint point in scanline) {
				int index = MapVector2ToPixelCoordinate(point.GetProjectedPosition());
				DrawPoint(point, index);
			}
		}
		
		texture.SetPixels(colorBuffer);
		texture.Apply();
	}
	
	/// <summary>
	/// Draws the base texture to the color buffer.
	/// </summary>
	private void ClearDisplay()	{
		for (int i = 0; i < pixelCount; ++i) {
			colorBuffer[i] = bgrdColor;
		}
	}
	
	/// <summary>
	/// Draws the given point at a specified index in the color buffer.
	/// </summary>
	private void DrawPoint(UltrasoundPoint point, int index) {
		Color drawColor = defaultColor;
		if (colorMode && !point.IsEmpty() && point.GetIntensity() != 0f) {
			UltrasoundOrgan organ = point.GetUltrasoundOrgan();
			drawColor = organ.color;
		}
		
		drawColor *= point.GetIntensity();
		colorBuffer[index] = drawColor;
	}
	
	/// <summary>
	/// Maps a projected location on the scanning plane to a pixel coordinate.
	/// </summary>
	public int MapVector2ToPixelCoordinate(Vector2 vector2) {
		
		float pixelsPerWorldUnit = Mathf.Min(displayWidth, displayHeight) / probeConfig.GetMaxDistance();
		int xCenter = displayWidth / 2;
		
		int newX = (int)(xCenter - pixelsPerWorldUnit * vector2.x);
		int newY = displayHeight - (int)(pixelsPerWorldUnit * vector2.y);
		
		int pixelMapping = newY * displayWidth + newX;
		return pixelMapping;
	}
}

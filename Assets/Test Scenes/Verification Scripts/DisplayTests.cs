using UnityEngine;
using System.Collections;
using System.Text;

public class DisplayTests : MonoBehaviour {
	
	public Rect displayRect;
	public Rect controlRect1;
	public Rect controlRect2;
	public float maxXYvalue;
	
	public GameObject testDisplay;
	private Display display;
	
	private Vector2 testPoint;
	
	// Use this for initialization
	void Start () {
		display = testDisplay.GetComponent<Display>();
		testPoint = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		StringBuilder sb = new StringBuilder();
		
		sb.AppendLine("x = "+testPoint.x.ToString());
		sb.AppendLine("y = "+testPoint.y.ToString());
		
		int index = MapTest();
		sb.AppendLine(index.ToString());
		
		sb.AppendLine("hashcode: "+testPoint.GetHashCode());
		
		GUI.Label(displayRect, sb.ToString());
		testPoint.x = GUI.HorizontalSlider(controlRect1, testPoint.x, -maxXYvalue / 2, maxXYvalue / 2);
		testPoint.y = GUI.HorizontalSlider(controlRect2, testPoint.y, Mathf.Epsilon, maxXYvalue);
	}
	
	private int MapTest() {
		return display.MapVector2ToPixelCoordinate(testPoint);
	}
}

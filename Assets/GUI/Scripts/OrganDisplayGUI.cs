using UnityEngine;
using System.Collections;

public class OrganDisplayGUI : MonoBehaviour {
	
	public Rect windowRect = new Rect(20, 20, 180, 50);
	public string windowTitle;
	
	private string[] organTypes = 
	{ "Skin", "Skeletal Muscles", "Bones", "Lungs", "Heart", "Blood Vessels", "Brain", "Genitals", "Liver",
	  "Intestine", "Stomach", "Digestive Organs", "Kidneys", "Urinary System", "Sensory Organs", "Endocrine System",
	  "Lymphoid System"};
	private bool[] visible;
	
	// Use this for initialization
	void Start () {
		visible = new bool[organTypes.GetLength(0)];
		for (int index = 0; index < visible.GetLength(0); ++index) {
			string organName = organTypes[index];
			GameObject organ = GameObject.FindGameObjectWithTag(organName);
			visible[index] = organ.renderer.enabled;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    void OnGUI() {
        windowRect = GUI.Window(0, windowRect, ListOptions, windowTitle);
    }
	
	void ListOptions(int windowID) {
		for (int index = 0; index < organTypes.GetLength(0); ++index) {
			string organName = organTypes[index];
			bool toggle = GUI.Toggle(new Rect(10, 15 + 22 * index, windowRect.width - 10, 20), visible[index], organName);
			if (toggle!= visible[index]) {
				visible[index] = toggle;
				ToggleVisibility(organName);
			}
		}
		GUI.DragWindow();
    }
	
	void ToggleVisibility(string organType) {
		foreach (GameObject organ in GameObject.FindGameObjectsWithTag(organType)) {
			organ.renderer.enabled = !organ.renderer.enabled;
		}
	}
}

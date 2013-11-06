using UnityEngine;
using System.Collections;

public class SimpleOrganDisplayGUI : MonoBehaviour {
	
	public Rect windowRect = new Rect(20, 20, 180, 50);
	public string windowTitle;
	
	private float detailLevel = 10.0f;
	
	// Skin
	// Bones
	// Intestine, Lungs, Liver
	
//	private float[] organRevealLevels = 
//	{ "Skin", "Bones", "Lungs", "Heart", "Blood Vessels", "Brain", "Genitals", "Liver",
//	  "Intestine", "Stomach", "Digestive Organs", "Kidneys", "Urinary System", "Sensory Organs", "Endocrine System",
//	  "Lymphoid System"};
//	private bool[] visible;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	 void OnGUI() {
        windowRect = GUI.Window(0, windowRect, DetailSlider, windowTitle);
    }
	
	void DetailSlider(int windowID) {
		detailLevel = GUI.HorizontalSlider(new Rect(10, 37, windowRect.width - 10, 20), detailLevel, 0f, 10f);
		
		if (detailLevel >= 9.95f) {
			SetVisibility("Skin", true);
			SetVisibility("Bones", false);
			SetVisibility("Intestine", false);
			SetVisibility("Lungs", false);
			SetVisibility("Liver", false);
		} else if (detailLevel >= 6.66f) {
			SetVisibility("Skin", false);
			SetVisibility("Bones", true);
			SetVisibility("Intestine", true);
			SetVisibility("Lungs", true);
			SetVisibility("Liver", true);
		} else if (detailLevel >= 3.33f) {
			SetVisibility("Skin", false);
			SetVisibility("Bones", false);
			SetVisibility("Intestine", true);
			SetVisibility("Lungs", true);
			SetVisibility("Liver", true);
		} else {
			SetVisibility("Skin", false);
			SetVisibility("Bones", false);
			SetVisibility("Intestine", false);
			SetVisibility("Lungs", false);
			SetVisibility("Liver", false);
		}
		
		GUI.DragWindow();
    }
	
	/// <summary>
	/// Sets the visibility of a group of organs with the same tag.
	/// </summary>
	/// <param name='organTag'>
	/// Organ tag.
	/// </param>
	/// <param name='visible'>
	/// Visible.
	/// </param>
	void SetVisibility(string organTag, bool visible) {
		if (GameObject.FindGameObjectWithTag(organTag).renderer.enabled != visible) {
			foreach (GameObject organ in GameObject.FindGameObjectsWithTag(organTag)) {
				organ.renderer.enabled = visible;
			}
		}
	}
}

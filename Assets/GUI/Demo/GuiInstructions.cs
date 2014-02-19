using UnityEngine;
using System.Collections;

public class GuiInstructions : MonoBehaviour {

	private const string INSTRUCTIONS =
			"Click with the right mouse button (or alt-LMB) to place the probe.\n" +
			"Click and drag the probe with the left mouse button to rotate it.\n" +
			"Press SPACE to toggle the skin visibility to look inside the model.\n" +
			"Use the LEFT and RIGHT arrow keys to rotate the model.";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		Rect instructionArea = new Rect(0f, 0f, Screen.width, Screen.height * 0.20f);
		GUI.Box (instructionArea, INSTRUCTIONS);
	}
}

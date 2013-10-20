using UnityEngine;
using System.Collections;
using System.Text;

public class InstructionsGUI : MonoBehaviour {
	
	public string[] instructions;
	
	private int width;
	private int height;
	
	private Vector2 position;
	
	// Use this for initialization
	void Start () {
		width = Screen.width / 4;
		height = Screen.height / 4;
		position = new Vector2(Screen.width - width, 0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < instructions.Length; ++i) {
			sb.AppendLine(string.Format("{0}. {1}", i+1, instructions[i]));
		}
		GUI.Label(new Rect(position.x , position.y, width, height), sb.ToString());
    }
}

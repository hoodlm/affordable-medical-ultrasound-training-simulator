using UnityEngine;
using System.Collections;

/// <summary>
/// Information about the organ represented by a GameObject.
/// </summary>
public class UltrasoundOrgan : MonoBehaviour {
	
	/// <summary>
	/// In color mode, the color of this organ.
	/// </summary>
	public Color color = Color.white;
	
	/// <summary>
	/// The degree to which this organ reflects sound.
	/// </summary>
	public float echogenicity = 1.0f;
	
	/// <summary>
	/// The degree to which this organ absorbs sound.
	/// </summary>
	public float attenuation = 0.01f;
	
	void Start() {
		
	}
}

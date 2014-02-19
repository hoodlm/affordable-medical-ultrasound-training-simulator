using UnityEngine;
using System.Collections;

/**
 *	Description of the physic properties of an Organ, for use with a HorayProbe.
 *	The HorayOrganCuller will not detect a GameObject as an organ unless this script is attached.
 */
public class HorayMaterialProperties : MonoBehaviour {

	/// <summary>
	/// A display color for this organ.
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
		this.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

using UnityEngine;
using System.Collections;

/**
 *	Description of the physic properties of an Organ, for use with a HorayProbe.
 *	The HorayOrganCuller will not detect a GameObject as an organ unless this script is attached.
 */
public class HorayMaterialProperties : MonoBehaviour {

	/// Sets of tissues with pre-defined parameters.
	public enum OrganPresets {Custom, Bone}

	/// Set the type of this organ. If it is set to custom, the parameters should be specified in the inspector.
	public OrganPresets organType;

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
		switch (organType) {
		case OrganPresets.Bone:
			echogenicity = 0.6f;
			attenuation = 2.0f;
			color = new Color(0.80f, 0.68f, 0.40f);
			break;
		
		default:
			// Use values from inspector.
			break;
		}

		this.renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

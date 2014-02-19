using UnityEngine;
using System.Collections;

/**
 *	Description of the physic properties of an Organ, for use with a HorayProbe.
 *	The HorayOrganCuller will not detect a GameObject as an organ unless this script is attached.
 */
public class HorayMaterialProperties : MonoBehaviour {

	/// Sets of tissues with pre-defined parameters.
	public enum OrganPresets {Custom, Bone, Kidneys, Intestines, Liver, Stomach, Lung}

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

		if (gameObject.collider == null) {
			Debug.LogWarning(string.Format("{0} has HORAY mat properties, but no collider", name));
		}

		switch (organType) {
		case OrganPresets.Bone:
			echogenicity = 0.6f;
			attenuation = 10.0f;
			color = new Color(0.80f, 0.68f, 0.40f);
			break;
		
		case OrganPresets.Kidneys:
			echogenicity = 1.0f;
			attenuation = 0.1f;
			color = Color.yellow;
			break;
		
		case OrganPresets.Intestines:
			echogenicity = 2.0f;
			attenuation = 5f;
			color = new Color(0.3f, 0.5f, 0.1f);
			break;

		case OrganPresets.Liver:
			echogenicity = 2.0f;
			attenuation = 1f;
			color = new Color(0.3f, 0.2f, 0.1f);
			break;

		case OrganPresets.Stomach:
			echogenicity = 2.0f;
			attenuation = 5f;
			color = new Color(0.3f, 0.5f, 0.5f);
			break;

		case OrganPresets.Lung:
			echogenicity = 5.0f;
			attenuation = 7f;
			color = new Color(0.6f, 0.3f, 0.2f);
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

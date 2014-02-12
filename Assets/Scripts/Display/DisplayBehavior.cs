using UnityEngine;
using System.Collections;

/**
 *  Handles the behavior of a display GameObject.
 */
public class DisplayBehavior : MonoBehaviour {

    private ITextureSource textureSource;

	/**
	 * A set of different modes for the display.
	 * - FakeTexture: Uses TestTextureSource to render a ColorBar graphic to the display.
	 * - FakeImage: A standard TextureSource is initialized with TestImageSource, which renders a tri-color flag graphic to the display.
	 * - FakeProbeOutput: A standard TextureSource and BModeOutputImageDecoder are fed output from a TestProbeOutput. The result should be a noisy white-on-black triangle graphic.
	 */
	public enum DisplayModes {FakeTexture, FakeImage, FakeProbeOutput};
	
	/// The selected mode from DisplayModes for this display.
	public DisplayModes displayMode;

    /// The width of the display's texture.
    public int textureWidth = 640;

    /// The height of the display's texture.
    public int textureHeight = 480;
    
    private Texture2D texture;

    void Start () {
		switch (displayMode) {
		case (DisplayModes.FakeTexture):
			textureSource = new TestTextureSource();
			break;
		case (DisplayModes.FakeImage):
			textureSource = new TextureSource(new TestImageSource());
			break;
		case (DisplayModes.FakeProbeOutput):
			textureSource = new TextureSource(new BModeOutputImageDecoder(new TestProbeOutput()));
			break;
		}
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        this.renderer.material.mainTexture = texture;
        
    }

    void Update () {
        textureSource.RenderNextFrameToTexture(ref texture);
    }
}

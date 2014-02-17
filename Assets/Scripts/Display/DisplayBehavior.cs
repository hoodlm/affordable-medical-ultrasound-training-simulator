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
	 * - HORAY: a HOmogeneous tissue RAYcasting algorithm to generate an ultrasound image.
	 */
	public enum DisplayModes {HORAY, FakeTexture, FakeImage, FakeProbeOutput};
	
	/// The selected mode from DisplayModes for this display.
	public DisplayModes displayMode;

    /// The width of the display's texture.
    public int textureWidth = 640;

    /// The height of the display's texture.
    public int textureHeight = 480;
    
    private Texture2D texture;

    void Start () {
		switch (displayMode) {

		case (DisplayModes.HORAY):
			GameObject probe = GameObject.FindGameObjectWithTag("Probe");
			UltrasoundDebug.Assert(null != probe, "No object with Probe tag in scene.", this);

			IProbeOutput horayOutput = new HorayProbeOutput(probe);
			IImageSource bmodeImageDecoder = new BModeOutputImageDecoder(horayOutput);
			textureSource = new TextureSource(bmodeImageDecoder);
			break;

		case (DisplayModes.FakeTexture):
			textureSource = new TestTextureSource();
			break;

		case (DisplayModes.FakeImage):
			IImageSource fakeImageSource = new TestImageSource();
			textureSource = new TextureSource(fakeImageSource);
			break;

		case (DisplayModes.FakeProbeOutput):
			IProbeOutput fakeProbeOutput = new TestProbeOutput();
			IImageSource aBmodeImageDecoder = new BModeOutputImageDecoder(fakeProbeOutput);
			textureSource = new TextureSource(aBmodeImageDecoder);
			break;
		
		default:
			UltrasoundDebug.Assert(false, "Unknown display mode!", this);
			break;
		}
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        this.renderer.material.mainTexture = texture;
        
    }

    void Update () {
        textureSource.RenderNextFrameToTexture(ref texture);
    }
}

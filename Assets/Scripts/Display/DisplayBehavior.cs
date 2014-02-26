using UnityEngine;
using System.Collections;

/**
 *  Handles the behavior of a display GameObject. The behavior is primarily configured by setting the displayMode
 * 	in the inspector. See documentation below on DisplayBehavior.DisplayModes for specific information on what
 *  each mode does.
 */
public class DisplayBehavior : MonoBehaviour {

	/// The class responsible for dynamically generating the texture that is drawn to this display object.
    private ITextureSource textureSource;

	/**
	 * A set of different modes for the display.
	 * - FakeTexture: Uses TestTextureSource to render a ColorBar graphic to the display.
	 * - FakeImage: A standard TextureSource is initialized with TestImageSource, which renders a tri-color flag graphic to the display.
	 * - FakeProbeOutput: A standard TextureSource and BModeOutputImageDecoder are fed output from a TestProbeOutput. The result should be a noisy white-on-black triangle graphic.
	 * - HORAY: a HOmogeneous tissue RAYcasting algorithm to generate an ultrasound image.
	 * - InvHORAY: HORAY with inverted colors (black on white).
	 */
	public enum DisplayModes {InvHORAY, HORAY, FakeTexture, FakeImage, FakeProbeOutput, GaussianBlurTest};
	
	/// The selected mode from DisplayModes for this display.
	public DisplayModes displayMode;

    /// The width of the display's texture.
    public int textureWidth = 640;

    /// The height of the display's texture.
    public int textureHeight = 480;
    
	/// The actual texture applied to the display's model. This texture is dynamically generated, not static.
    private Texture2D texture;

	/// Initialization when the scene is started. This sets up the source of dynamic texture generation for the display.
	/// Based on the current choice of DisplayModes, the DisplayTexturePipelineFactory will provide the appropriate
	/// ITextureSource.
    void Start () {
		OnionLogger.globalLog.PushInfoLayer("Initializing Display");
		switch (displayMode) {

		case (DisplayModes.HORAY):
			textureSource = DisplayTexturePipelineFactory.BuildStandardHORAYConfig();
			break;

		case (DisplayModes.InvHORAY):
			textureSource = DisplayTexturePipelineFactory.BuildBlackOnWhiteHORAYConfig();
			break;
			
		case (DisplayModes.FakeTexture):
			textureSource = DisplayTexturePipelineFactory.BuildFakeTextureSource();
			break;

		case (DisplayModes.FakeImage):
			textureSource = DisplayTexturePipelineFactory.BuildWithFakeImageSource();
			break;

		case (DisplayModes.FakeProbeOutput):
			textureSource = DisplayTexturePipelineFactory.BuildWithFakeBModeProbeOutput();
			break;

		case (DisplayModes.GaussianBlurTest):
			textureSource = DisplayTexturePipelineFactory.BuildWithGaussianBlurImageSource();
			break;
		
		default:
			UltrasoundDebug.Assert(false, "Unknown display mode!", this);
			break;
		}
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        this.renderer.material.mainTexture = texture;
		OnionLogger.globalLog.PopInfoLayer();
    }

	/// Called every frame. This is the "entry point" into the ultrasound rendering procedure.
    void Update () {
		string frameCountStr = string.Format("Frame {0}", Time.frameCount);
		OnionLogger.globalLog.PushInfoLayer(frameCountStr);
        textureSource.RenderNextFrameToTexture(ref texture);
		OnionLogger.globalLog.PopInfoLayer();
    }
}

/**
 * 	Factory class for setting up the Display rendering pipeline.
 * 	Generally, the rendering pipeline is something like this:
 * 
 * 		probe -> IProbeOutput -> IImageSource -> ITextureSource -> Display
 */
public sealed class DisplayTexturePipelineFactory
{
	/**
	 *	Sets up a standard HORAY configuration:
	 *
	 *		probe -> HorayProbeOutput -> BModeOutputImageDecoder (+blur) -> TextureSource
	 */
	public static ITextureSource BuildStandardHORAYConfig() {
		GameObject probe = GameObject.FindGameObjectWithTag("Probe");
		UltrasoundDebug.Assert(null != probe, "No object with Probe tag in scene.", new DisplayTexturePipelineFactory());
		
		IProbeOutput horayOutput = new HorayProbeOutput(probe);
		IImageSource bmodeImageDecoder = new BModeOutputImageDecoder(horayOutput);
		bmodeImageDecoder.AddPostProcessingEffect(new GaussianBlur());
		return new TextureSource(bmodeImageDecoder);
	}

	/**
	 *	Sets up a HORAY configuration with inverted colors:
	 *
	 *		probe -> HorayProbeOutput -> BlackOnWhiteBModeOutputImageDecoder -> TextureSource
	 */
	public static ITextureSource BuildBlackOnWhiteHORAYConfig() {
		GameObject probe = GameObject.FindGameObjectWithTag("Probe");
		UltrasoundDebug.Assert(null != probe, "No object with Probe tag in scene.", new DisplayTexturePipelineFactory());
		
		IProbeOutput horayOutput = new HorayProbeOutput(probe);
		IImageSource bmodeImageDecoder = new BlackOnWhiteBModeOutputImageDecoder(horayOutput);
		return new TextureSource(bmodeImageDecoder);
	}

	/**
	 *	Set up a fake ITextureSource that renders a static Color Bar texture.
	 */
	public static ITextureSource BuildFakeTextureSource() {
		return new TestTextureSource();
	}

	/**
	 *	Set up a test IImageSource that provides a tri-color flag bitmap:
	 *
	 *		TestImageSource -> TextureSource
	 */
	public static ITextureSource BuildWithFakeImageSource() {
		IImageSource fakeImageSource = new TestImageSource();
		return new TextureSource(fakeImageSource);
	}

	/**
	 *	Simulate the output of a BMode probe.
	 *
	 *		TestProbeOutput -> BModeOutputImageDecoder -> TextureSource
	 */
	public static ITextureSource BuildWithFakeBModeProbeOutput() {
		IProbeOutput fakeProbeOutput = new TestProbeOutput();
		IImageSource imageSource = new BModeOutputImageDecoder(fakeProbeOutput);
		return new TextureSource(imageSource);
	} 

	/**
	 *	Test image for gaussian blur.
	 */
	public static ITextureSource BuildWithGaussianBlurImageSource() {
		IImageSource gaussianBlurredTestImage = new TestGaussianImageSource();
		return new TextureSource(gaussianBlurredTestImage);
	}
}
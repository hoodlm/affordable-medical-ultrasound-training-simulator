using UnityEngine;
using System.Collections;

/**
 *  Handles the behavior of a display GameObject.
 */
public class DisplayBehavior : MonoBehaviour {

    private ITextureSource textureSource;

    /// The width of the display's texture.
    public int textureWidth = 640;

    /// The height of the display's texture.
    public int textureHeight = 480;
    
    private Texture2D texture;

    void Start () {
        texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGB24, false);
        this.renderer.material.mainTexture = texture;
        textureSource = new TextureSource(new ProbeOutputImageDecoder(new TestProbeOutput()));
    }

    void Update () {
        textureSource.RenderNextFrameToTexture(ref texture);
    }
}

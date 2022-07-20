using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReSize : MonoBehaviour
{
    public Texture2D testImage;
    public RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        rawImage.texture=ResizeTexture(testImage, 64, 64);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Texture2D ResizeTexture(Texture2D tex, int w, int h)
{
	Texture2D result = new Texture2D(100, 100, TextureFormat.RGBA32, true);
	Color[] rpixels = result.GetPixels(0);
	float incX = (1.0f / (float)100);
	float incY = (1.0f / (float)100);
	for (int px = 0; px < rpixels.Length; px++)
	{
		rpixels[px] = tex.GetPixelBilinear(incX * ((float)px % 100), incY * ((float)Mathf.Floor(px / 100)));
	}
	result.SetPixels(rpixels, 0);
	result.Apply();
	return result;
}
}

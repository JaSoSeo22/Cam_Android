using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadImg : MonoBehaviour
{
    public RawImage img;

    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file)=>
        {
            FileInfo selected = new FileInfo(file);

            // 용량 제한
            if(selected.Length > 50000000) // 10,000,000byte == 1mb
            {
                return;
            }
            // 불러오기
            if(string.IsNullOrEmpty(file))
            {
                // 불러와라
                StartCoroutine(LoadImage(file));
                
            }

        });
    }
    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split(',')[0];
        string savePath = Application.persistentDataPath + "/Test";

        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllBytes(savePath + filename + ".png", fileData);
        
        var temp = File.ReadAllBytes(savePath + filename + ".png");

        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(temp);

        img.texture = tex;
        img.SetNativeSize();
        ImageSizeSetting(img, 300, 300);
        
    }

    private void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if( x / y > imgX / imgY) // 세로길이가 더 길다
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, imgX * (y / imgY));
        }
        else // 가로길이가 더 길다
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, imgY * (x / imgX));
        }

    }

}

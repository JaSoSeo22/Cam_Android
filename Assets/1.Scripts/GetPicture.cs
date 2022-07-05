using System.IO;
using UnityEngine;

public class GetPicture : MonoBehaviour {

// 사진이 저장된 경로를 가져올 때 필요
#if !UNITY_EDITOR && UNITY_ANDROID
    private static AndroidJavaClass m_ajc = null;
    private static AndroidJavaClass AJC
    {
        get
        {
            if (m_ajc == null)
                m_ajc = new AndroidJavaClass("com.yasirkula.unity.NativeGallery");
            return m_ajc;
        }
    }
#endif
    
    // 다른 스크립트에서 GetPicture.GetLastPicturePath()로 호출
    // 마지막으로 저장된 사진의 경로 가져오기
    public static string GetLastPicturePath()
    {
        // 디바이스마다 다른 저장경로, 안드로이드일 경우
        string saveDir;
        #if !UNITY_EDITOR && UNITY_ANDROID
        saveDir = AJC.CallStatic<string>("/storage/emulated/0/DCIM/{Application.productName}/");

        #elif UNITY_EDITOR
        Debug.Log("UnityEditor");
        string url = "/1.Scripts/img";
        saveDir = Application.dataPath+url;
        
        // #else
        // // saveDir = Application.persistentDataPath; // 눈에 안보이는 경로임 비추, 위에서 경로 연결이 되어야 한다.
        // saveDir = "E:/Unity/GItHub/Cam_Android/Assets/1.Scripts/img/";
        #endif
        // 저장경로에서 PNG파일 모두 검색
        string[] files = Directory.GetFiles(saveDir, "*.png");
        // 만약 PNG파일이 있다면, 마지막 파일을 반환
        if (files.Length > 0)
        {
            return files[files.Length - 1];
        }
        // 없다면 null을 
        return null;
    }
 
 
}
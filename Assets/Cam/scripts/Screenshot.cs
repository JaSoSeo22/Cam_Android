using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Screenshot : MonoBehaviour
{
    bool isCoroutinePlaying;             // 코루틴 중복방지

    private int num = 1;

    // 지정한 경로( _savepath)에 PNG 파일형식으로 저장합니다.
    private string _SavePath = "/img"; //경로 바꾸세요!
    int _CaptureCounter = 0; // 파일명을 위한 숫자 변수

    static WebCamTexture cam; // PC 확인용 캠 
    
    public TextMeshProUGUI debugUI;

    // 캡쳐 버튼을 누르면 호출
    public void Capture_Button()
    {
        // 중복방지 bool, true 일 때 실행
        if (!isCoroutinePlaying)
        {
            StartCoroutine("captureScreenshot");
        }
    }

    IEnumerator captureScreenshot()
    {
        isCoroutinePlaying = true;

        // 스크린샷 + 갤러리갱신
        ScreenshotAndGallery();

        debugUI.text = "Phase 1";
        yield return new WaitForEndOfFrame();

        // 셔터 사운드 넣기...
        yield return new WaitForSeconds(1f);

        debugUI.text = "Phase 2";
        yield return new WaitForSeconds(1f);
        // yield return new WaitForEndOfFrame();

        isCoroutinePlaying = false;
        yield return new WaitForEndOfFrame();
        debugUI.text = "capture" + num;
        
        num++;
    }


    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery()
    {
#if UNITY_EDITOR // PC로 할 경우 
        //스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        screenShot.ReadPixels(area, 0, 0); 
        screenShot.Apply();

        Debug.Log("Screenshot!!!");

        System.IO.File.WriteAllBytes("E:/Unity/GItHub/Cam_Android/Assets/Resources/AIMG" + "foto" + _CaptureCounter.ToString() + ".png", screenShot.EncodeToPNG());
        Debug.Log(++_CaptureCounter);

#elif !UNITY_EDITOR && UNITY_ANDROID // 안드로이드 경우 
        // 스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        screenShot.ReadPixels(area, 0, 0); 
        screenShot.Apply();

        // 갤러리갱신
        Debug.Log("" + NativeGallery.SaveImageToGallery( screenShot, AIMG, "foto" + _CaptureCounter.ToString() + ".png", 
                ( success, path ) => Debug.Log( "Media save result: " + success + " " + path )));
        System.IO.File.WriteAllBytes("E:/Unity/GItHub/Cam_Android/Assets/Resources/AIMG" + "foto" + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());

        Debug.Log(++_CaptureCounter);

        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(screenShot);
#endif
    }

}

using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Screenshot : MonoBehaviour
{
    bool isCoroutinePlaying;             // 코루틴 중복방지

    int _CaptureCounter = 0; // 파일명을 위한 숫자 변수

    static WebCamTexture cam; // PC 확인용 캠 
    
    //<debug>
    public TextMeshProUGUI debugUI;
    private int debugnum = 1;

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
        //ScreenshotAndGallery();
        //yield return new WaitForEndOfFrame();
        StartCoroutine(ScreenshotAndGallery2());
        debugUI.text = "Phase 1";
        

        // 셔터 사운드 넣기...
        yield return new WaitForSeconds(1f);

        debugUI.text = "Phase 2";
        isCoroutinePlaying = false;
        //yield return new WaitForEndOfFrame();
        debugUI.text = "capture" + debugnum;
        
        debugnum++;
    }

    IEnumerator ScreenshotAndGallery2() 
    {
        yield return new WaitForEndOfFrame();
         //스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        screenShot.ReadPixels(area, 0, 0); 
        screenShot.Apply();

        GetInferenceFromModel.texture = screenShot; // 찍은 사진을 따로 저장하지 않고 texture에 넘겨줌 

        // Debug.Log("Screenshot!!!");

        // var workdir = Directory.GetCurrentDirectory(); // 현재 프로젝트 경로 찾기
        // System.IO.File.WriteAllBytes(workdir+"/Assets/TextMesh Pro/Resources/SavedImg/" + "foto" + _CaptureCounter.ToString() + ".png", screenShot.EncodeToPNG());
        // Debug.Log(++_CaptureCounter);
        // Debug.Log(workdir); 경로 확인

        // 복사 완료됐기 때문에 원본 메모리 삭제
        // Destroy(screenShot);    // MissingReferenceException: The object of type 'Texture2D' has been destroyed but you are still trying to access it.
    }


    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery() // readofpixel error 
    {
//#if UNITY_EDITOR // PC로 할 경우 
        //스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        screenShot.ReadPixels(area, 0, 0); 
        screenShot.Apply();

        Debug.Log("Screenshot!!!");

        var workdir = Directory.GetCurrentDirectory(); // 현재 경로 찾기
        System.IO.File.WriteAllBytes(workdir+"/Assets/TextMesh Pro/Resources/SavedImg/" + "foto" + _CaptureCounter.ToString() + ".png", screenShot.EncodeToPNG());
        Debug.Log(++_CaptureCounter);

        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(screenShot);    
    }

}

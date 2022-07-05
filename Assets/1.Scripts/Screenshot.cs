using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class Screenshot : MonoBehaviour
{
   public GameObject blink;             // 사진 찍을 때 깜빡일 것

    bool isCoroutinePlaying;             // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "Test";           // 생성될 앨범의 이름
    string fileName = "Shot.png";           // 생성될 파일의 이름

    [SerializeField]
    GameObject showSavedImg;                    // 찍은 사진이 뜰 패널

    public TextMeshProUGUI debugUI;

    private int num = 1;

    // 지정한 경로( _savepath)에 PNG 파일형식으로 저장합니다.
    private string _SavePath = "/1.Scripts/img"; //경로 바꾸세요!
    int _CaptureCounter = 0; // 파일명을 위한

    static WebCamTexture cam;
    

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

        // 블링크
        BlinkUI();

        // 셔터 사운드 넣기...
        yield return new WaitForSeconds(1f);

        debugUI.text = "Phase 2";
        yield return new WaitForSeconds(1f);
        // yield return new WaitForEndOfFrame();

        // 찍은 사진이 등장
        GetPirctureAndShowIt();
        debugUI.text = "Phase 3";
        yield return new WaitForSeconds(1f);

        isCoroutinePlaying = false;
        yield return new WaitForEndOfFrame();
        debugUI.text = "capture" + num;
        
        num++;
        Debug.Log(isCoroutinePlaying);
    }

    //흰색 블링크 생성
    void BlinkUI()
    {
        GameObject b = Instantiate(blink);
        b.transform.SetParent(transform);
        b.transform.localPosition = new Vector3(0, 0, 0);
        b.transform.localScale = new Vector3(1, 1, 1);
    }

    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery()
    {


#if UNITY_EDITOR
        // // 스크린샷할 이미지 담을 공간 생성
        // Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
      
        
        // // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        // Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        // screenShot.ReadPixels(area, 0, 0); 
        // screenShot.Apply();

        Texture2D snap = new Texture2D(cam.width, cam.height);
        snap.SetPixels(cam.GetPixels());
        snap.Apply();

        Debug.Log("Screenshot!!!");
        //System.IO.File.WriteAllBytes(Application.dataPath+_SavePath + _CaptureCounter.ToString() + ".png", screenShot.EncodeToPNG());
        System.IO.File.WriteAllBytes("E:/Unity/GItHub/Cam_Android/Assets/1.Scripts/img/" + _CaptureCounter.ToString() + ".png", snap.EncodeToPNG());
        Debug.Log(++_CaptureCounter);

#elif !UNITY_EDITOR && UNITY_ANDROID
        // 스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
      
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        Rect area = new Rect(300, 40, 1000, 1000); // (cameraview UI Pivot 좌하단 기준) Rect(좌표 x,y 입력, 가로 길이, 세로 길이)
        screenShot.ReadPixels(area, 0, 0); 
        screenShot.Apply();

        // 갤러리갱신
        // Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName, "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
        Debug.Log("" + NativeGallery.SaveImageToGallery( screenShot, albumName, fileName, 
                ( success, path ) => Debug.Log( "Media save result: " + success + " " + path )));

      //  if(NativeGallery.SaveImageToGallery(screenShot,"GalleryTest", "Image.png", ( success, path ) => Debug.Log( "Media save result: " + success + " " + path ))
        // To avoid memory leaks.
        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(screenShot);
#endif
    }

    // 찍은 사진을 showsavedimg에 보여준다.
    void GetPirctureAndShowIt()
    {
        string pathToFile = GetPicture.GetLastPicturePath();
        Debug.Log("Test : "+pathToFile);
        if (pathToFile == null)
        {
            return;
        }
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        showSavedImg.SetActive(true);
        showSavedImg.GetComponent<Image>().sprite = sp;
    }
    
    // 찍은 사진을 불러온다.
    Texture2D GetScreenshotImage(string filePath)
    {
        Texture2D texture = null;
        byte[] fileBytes;
        if (File.Exists(filePath))
        {
            fileBytes = File.ReadAllBytes(filePath);
            // texture = new Texture2D(2, 2, TextureFormat.RGB24, false);
            texture = new Texture2D(1, 1, TextureFormat.RGB24, false);
            texture.LoadImage(fileBytes);
        }
        return texture;
    }
}

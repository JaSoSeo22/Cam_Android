using System.Collections;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class Screenshot : MonoBehaviour
{
   // public GameObject blink;             // 사진 찍을 때 깜빡일 것
    public GameObject shareButtons;      // 공유 버튼

    bool isCoroutinePlaying;             // 코루틴 중복방지

    // 파일 불러올 때 필요
    string albumName = "Test";           // 생성될 앨범의 이름

    [SerializeField]
    GameObject fotoPanel;                    // 찍은 사진이 뜰 패널
    

    // 캡쳐 버튼을 누르면 호출
    public void Capture_Button()
    {
        // 중복방지 bool
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

        yield return new WaitForEndOfFrame();

        // 블링크
       // BlinkUI();

        // 셔터 사운드 넣기...
        yield return new WaitForSeconds(2f);

        yield return new WaitForEndOfFrame();

        // 찍은 사진이 등장
        GetPirctureAndShowIt();

        isCoroutinePlaying = false;
    }

    // // 흰색 블링크 생성
    // void BlinkUI()
    // {
    //     GameObject b = Instantiate(blink);
    //     b.transform.SetParent(transform);
    //     b.transform.localPosition = new Vector3(0, 0, 0);
    //     b.transform.localScale = new Vector3(1, 1, 1);
    // }

    // 스크린샷 찍고 갤러리에 갱신
    void ScreenshotAndGallery()
    {
        // 스크린샷할 이미지 담을 공간 생성
        Texture2D screenShot = new Texture2D(1000, 1000, TextureFormat.RGB24, false); //카메라가 인식할 영역의 크기
        
        // 현재 이미지로부터 지정 영역의 픽셀들을 텍스쳐에 저장
        screenShot.ReadPixels(new Rect(300, 40, 1000, 1000), 0, 0); // (cameraview UI Pivot 좌하단 기준) 좌표 x,y 입력, 가로 길이, 세로 길이
        screenShot.Apply();

        // 갤러리갱신
        // Debug.Log("" + NativeGallery.SaveImageToGallery(ss, albumName, "Screenshot_" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + "{0}.png"));
        Debug.Log("" + NativeGallery.SaveImageToGallery(screenShot, albumName, "Screenshot_" + "{0}.png"));

        // To avoid memory leaks.
        // 복사 완료됐기 때문에 원본 메모리 삭제
        Destroy(screenShot);
    }

    // 찍은 사진을 show img에 보여준다.
    void GetPirctureAndShowIt()
    {
        string pathToFile = GetPicture.GetLastPicturePath();
        if (pathToFile == null)
        {
            return;
        }
        Texture2D texture = GetScreenshotImage(pathToFile);
        Sprite sp = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        fotoPanel.SetActive(true);
        shareButtons.SetActive(true);
        fotoPanel.GetComponent<Image>().sprite = sp;
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

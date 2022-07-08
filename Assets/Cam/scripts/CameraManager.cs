using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;


public class CameraManager : MonoBehaviour
{
    private WebCamTexture camTexture; // 모바일 카메라의 영상을 받아오는 api

    public RawImage cameraView; // 카메라가 보일 화면

    public void CameraOn()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        // 카메라 권한 확인
        if(!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }

        // 카메라가 없다면...
        if(WebCamTexture.devices.Length == 0)
        {
            Debug.Log("no Camera!");
        }

        // 스마트폰의 카메라 정보를 모두 가져옴
        WebCamDevice[] devices = WebCamTexture.devices;
        int selectedCameraIndex = -1;

        // 후면 카메라 찾기 isFrontFacing true면 전면캠, false면 후면캠
        for( int i= 0; i < devices.Length; i++)
        {
            if(devices[i].isFrontFacing == false)
            {
                selectedCameraIndex = i;
                break;
            }
        }

        // 후면 카메라 찾으면 켜기
        if(selectedCameraIndex >= 0)
        {
            // 선택된 후면 카메라를 가져옴
            camTexture = new WebCamTexture(devices[selectedCameraIndex].name);
            // 카메라 프레임 설정
            camTexture.requestedFPS = 30;
            // 영상을 raw image에 할당
            cameraView.texture = camTexture;
            // 카메라 시작하기
            camTexture.Play();
        }

#elif UNITY_EDITOR
        if (camTexture == null)
            camTexture = new WebCamTexture();

            //GetComponent<Renderer>().material.mainTexture = camTexture;
            cameraView.texture = camTexture;

        if (!camTexture.isPlaying)
            camTexture.Play();

#endif
    }

    // 카메라 끄기
    public void CameraOff() 
    {
        if(camTexture != null)
        {
            // 카메라 정지
            camTexture.Stop();
            // 카메라 객체 반납
            WebCamTexture.Destroy(camTexture);
            // 변수 초기화
            camTexture = null;
        }

    }
 
}

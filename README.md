# Cam_Android
 mobile camera with Unity (S20)

## Native Gallery for Android & iOS
- Asset Store 에서 다운받아 패키지 매니저로 적용  
Version 1.6.7 - January 18, 2022 사용

- Game view Ratio : S20 기준 20 : 9 커스텀 설정

> **UI 구성**
- Panel(Foto Panel)
    - Raw Image(Camera View)
    - Image(blink)
    - Button(Shot)
    - Image(Show Saved Img)
    - Button (Share) -> 사용 안함
- Button (Cam)
- Bitton (Exit)
</n>

> **Button Onclick Methode 연결**
- Cam 
    - Foto Panel : GameObject.SetActive V
    - Exit : GameObject.SetActive V
    - Cam : GameObject.SetActive 
    - Camera Manager : CameraManager.CameraOn

- Exit 
    - Foto Panel : GameObject.SetActive 
    - Exit : GameObject.SetActive 
    - Cam : GameObject.SetActive V 
    - Camera Manager : CameraManager.CameraOff

- Shot
    - CameraManager.Screenshot.Capture_Button

> GameObject 구성
- Canvas (AndroidCamCanvas)
    - Screen Space - Overlay
    - Scale With Screen Size 2400 * 1080
- GameObject (Camera Manager)
- Camera (Main Camera)
- EventSystem (Default)
- Directional Light (Default)

using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using TMPro;

// 모델의 추론 과정을 모방한 클래스 
public class GetInferenceFromModel : MonoBehaviour
{

    public Texture2D texture; // 모델이 예측할 이미지 텍스처
    
    public NNModel modelAsset; // 학습된 모델

    private Model _runtimeModel; // 실행할 모델

    private IWorker _engine; // 모델을 돌릴 엔진

    public TextMeshProUGUI result; // 결과값을 확인할 UI Text


    /// <summary>
    /// 인스펙터에서 쉽게 볼 수 있는 방식으로 예측 결과를 유지하는 데 사용되는 구조체.
    /// </summary>
    [Serializable]
    public struct Prediction
    {
        // 모델의 예측가능성 중 가장 높은 값
        public int predictedValue;
        // 라벨에 대한 예측값 배열
        public float[] predicted;

        // 텐서를 매개변수로 받아 예측값을 가져오는 메서드
        public void SetPrediction(Tensor t)
        {
            // 부동 소수점 값 출력을 예측 배열로 추출
            predicted = t.AsFloats();
            // 가장 가능성이 높은 것은 예측 값
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            Debug.Log($"Predicted {predictedValue}");
        }

    }

    // 예측값 구조체를 통해 필요한 기능 받아오기 
    public Prediction prediction;
    
    private void Start()
    {
        // 런타임 모델 및 작업자를 설정
        _runtimeModel = ModelLoader.Load(modelAsset);
        // 실행 엔진 설정(worker 생성 (실행 모델, 실행할 엔진 : CPU, GPU, Auto))
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.CPU);
        // 예측 구조체 인스턴스화.
        prediction = new Prediction();
        
        // 마지막으로 사진이 저장된 경로 가져오기
        // 경로가 비어있다면, 즉 저장된 사진이 없다면 실행 안함
        // 마지막으로 저장된 사진 texture에 가져오기
        
        // string pathToFile = GetPicture.GetLastPicturePath();
        // if (pathToFile == null) return;
        // texture = Screenshot.GetScreenshotImage(pathToFile);
    }

    void Update()
    {
        
    }

    public void PreModel()
    {
        // 색상 텍스처에서 텐서 만들기
        var channelCount = 3; //회색조, 3 = 색상, 4 = 색상 알파
        // 텍스처에서 입력을 위한 텐서 생성.
        var inputX = new Tensor(texture, channelCount);

        // 실행해서(Execute) 결과값 내보내기(PeekOutput)
        Tensor outputY = _engine.Execute(inputX).PeekOutput();
        // 출력 텐서를 사용하여 예측 구조체의 값을 설정
        prediction.SetPrediction(outputY);   

        // 예측값중 가장 높은 값 문자열로 변환해서 UI Text에 보여주기
        result.text = "result : " + prediction.predictedValue.ToString();

        // 입력 텐서를 수동으로 폐기(가비지 컬렉터 아님).
        inputX.Dispose();
    }

    private void OnDestroy()
    {
        // 엔진을 수동으로 폐기합니다(가비지 컬렉터 아님).
        _engine?.Dispose();
    }
}
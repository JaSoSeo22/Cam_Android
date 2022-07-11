using System;
using System.Linq;
using Unity.Barracuda;
using UnityEngine;
using TMPro;

public class GetInferenceFromModel : MonoBehaviour
{

    public Texture2D texture;
    
    public NNModel modelAsset;

    private Model _runtimeModel;

    private IWorker _engine;

    public TextMeshProUGUI result;



    /// <summary>
    /// 인스펙터에서 쉽게 볼 수 있는 방식으로 예측 결과를 유지하는 데 사용되는 구조체입니다.
    /// </summary>
    [Serializable]
    public struct Prediction
    {
        // 이 예측의 가능성이 가장 높은 값
        public int predictedValue;
        // 가능한 모든 클래스에 대한 가능성 목록
        public float[] predicted;

        public void SetPrediction(Tensor t)
        {
            // 부동 소수점 값 출력을 예측 배열로 추출합니다.
            predicted = t.AsFloats();
            // 가장 가능성이 높은 것은 예측 값입니다.
            predictedValue = Array.IndexOf(predicted, predicted.Max());
            Debug.Log($"Predicted {predictedValue}");
            
        }

    }

    public Prediction prediction;
    
    void Start()
    {
        // 런타임 모델 및 작업자를 설정합니다.
        _runtimeModel = ModelLoader.Load(modelAsset);
        // 실행 엔진 설정
        _engine = WorkerFactory.CreateWorker(_runtimeModel, WorkerFactory.Device.CPU);
        // 예측 구조체 인스턴스화.
        prediction = new Prediction();
    }

    void Update()
    {
        PreModel();
    }

    public void PreModel()
    {
        // 마지막으로 저장된 사진 texture에 가져오기
        string pathToFile = GetPicture.GetLastPicturePath();
        if (pathToFile == null) return;
        texture = Screenshot.GetScreenshotImage(pathToFile);
        
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        // 색상 텍스처에서 텐서 만들기
        var channelCount = 3; //회색조, 3 = 색상, 4 = 색상 알파
        // 텍스처에서 입력을 위한 텐서를 생성합니다.
        var inputX = new Tensor(texture, channelCount);

        // 복사하지 않고 출력 텐서를 엿보세요.
        // 실행해서(Execute) 결과값 내보내기(PeekOutput)
        Tensor outputY = _engine.Execute(inputX).PeekOutput();
        // 출력 텐서를 사용하여 예측 구조체의 값을 설정합니다.
        prediction.SetPrediction(outputY);   
        // }  
        // 입력 텐서를 수동으로 폐기(가비지 수집 아님).
        inputX.Dispose();
        result.text = prediction.predictedValue.ToString();
        
        
    }

    private void OnDestroy()
    {
        // 엔진을 수동으로 폐기합니다(가비지 수집 아님).
        _engine?.Dispose();
    }
}
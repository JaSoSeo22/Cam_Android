using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Unity.Barracuda;

public class ModelPredict : MonoBehaviour
{

    [SerializeField]
    private TMP_InputField inputValue;

    [SerializeField]
    private TMP_Text outputPrediction;

    [SerializeField]
    private NNModel CnnModel;

    private Model runtimeModel;
    private IWorker worker;
    private string outputLayerName;

    private void Start() {
        runtimeModel = ModelLoader.Load(CnnModel);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        outputLayerName = runtimeModel.outputs[runtimeModel.outputs.Count -1];
    }

    // 예측값 구하기 + UI로 눈에 보이도록 작성
    public void Predict()
    {
        int numberInput;

        if( int.TryParse(inputValue.text, out numberInput))
        {
            using Tensor inputTensor = new Tensor(1,1);

            inputTensor[0] = numberInput;
            worker.Execute(inputTensor);

            Tensor outputTensor = worker.PeekOutput(outputLayerName);

            outputPrediction.text = outputTensor[0].ToString();
        }
    }

    // 사용이 끝난 엔진은 수동으로 삭제처리 
    public void OnDestroy() 
    {
      worker?.Dispose();   
    }
}

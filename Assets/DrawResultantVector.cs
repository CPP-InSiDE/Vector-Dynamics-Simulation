using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DrawResultantVector : MonoBehaviour
{
    // PRIVATE VARIABLES
    private DrawArrow _arrow;
    private Vector3 _vector1EndPoint;
    private Vector3 _vector2EndPoint;
    private float _vectorMagnitudeAddition;
    private float _vectorMagnitudeMultiplication;
    private float _angleBetweenVectorsInDegrees;
    private float _cosineOfAngleBetweenVectors;
    private Vector3 _vectorAddition;
    private Vector3 _vectorSubtraction;
    private Vector3 _vectorCrossProduct;
    private float _vectorDotProduct;
    private enum Operation { Addition, Subtraction, CrossProduct, DotProduct };
    private Operation _previousSelectedOperation;
    private Vector3 _previousVector1EndPoint;
    private Vector3 _previousVector2EndPoint;
    private bool _previousReverseOperands;
    private bool _is3DMode = true;
    private bool _previousIs3DMode = false;

    // SERIALIZABLE VARIABLES
    [SerializeField] private DrawArrow _vector1Line;
    [SerializeField] private DrawArrow _vector2Line;
    [SerializeField] private DrawArrow _vectorProjectionLine;
    [SerializeField] private Operation _selectedOperation = Operation.Addition;
    [SerializeField] private bool _reverseOperands = false;
    
    [SerializeField] private TMPro.TextMeshProUGUI _vectorMagnitudeAdditionText;
    [SerializeField] private TMPro.TextMeshProUGUI _vectorMagnitudeMultiplicationText;
    [SerializeField] private TMPro.TextMeshProUGUI _angleBetweenVectorsText;
    [SerializeField] private TMPro.TextMeshProUGUI _cosineOfAngleBetweenVectorsText;
    [SerializeField] private TMPro.TextMeshProUGUI _vectorDotProductText;
    [SerializeField] private TMPro.TextMeshProUGUI _vectorAdditionText;
    [SerializeField] private TMPro.TextMeshProUGUI _vectorSubtractionText;
    [SerializeField] private TMPro.TextMeshProUGUI _vectorCrossProductText;

    [SerializeField] private GameObject _dotProductPanel;

    // LIFECYCLE METHODS
    void Start() {
        _arrow = GetComponent<DrawArrow>();
    }

    void Update() {
        _is3DMode = CameraModeToggle._Instance.IsCamera3D;
        if(_selectedOperation != Operation.DotProduct) _dotProductPanel.SetActive(false);
        ReverseVectorEndPoints();
        if (IsStateUnChanged()) return;
        PerformVectorArithmetic();
        // ClearLog();
        // PrintVectorArithmetic();
        _arrow._EndPoint = CalculateResultantVectorEndPoint();
        UpdateProjection();        
        HandleDotProductCalculationsUIText();
        HandleUIText();
        SaveState();
    }

    // CLASS METHODS
    void ReverseVectorEndPoints() {
        _vector1EndPoint = !_reverseOperands ? _vector1Line._EndPoint : _vector2Line._EndPoint;
        _vector2EndPoint = !_reverseOperands ? _vector2Line._EndPoint : _vector1Line._EndPoint;
    }

    bool IsStateUnChanged() {
        return _vector1EndPoint == _previousVector1EndPoint
        && _vector2EndPoint == _previousVector2EndPoint
        && _selectedOperation == _previousSelectedOperation
        && _reverseOperands == _previousReverseOperands
        && _is3DMode == _previousIs3DMode;
    }

    void PerformVectorArithmetic() {
        _vectorMagnitudeAddition = _vector1EndPoint.magnitude + _vector2EndPoint.magnitude;
        _vectorMagnitudeMultiplication = _vector1EndPoint.magnitude * _vector2EndPoint.magnitude;
        _angleBetweenVectorsInDegrees = Vector3.Angle(_vector1EndPoint, _vector2EndPoint);
        float angleBetweenVectorsInRadians = _angleBetweenVectorsInDegrees * (Mathf.PI / 180f);
        _cosineOfAngleBetweenVectors = Mathf.Cos(angleBetweenVectorsInRadians);
        _vectorAddition = _vector1EndPoint + _vector2EndPoint;
        _vectorSubtraction = _vector1EndPoint - _vector2EndPoint;
        _vectorCrossProduct = Vector3.Cross(_vector1EndPoint, _vector2EndPoint);
        _vectorDotProduct = Vector3.Dot(_vector1EndPoint, _vector2EndPoint);
    }

    // public void ClearLog() {
    //     Assembly assembly = Assembly.GetAssembly(typeof(ActiveEditorTracker));
    //     System.Type type = assembly.GetType("UnityEditor.LogEntries");
    //     MethodInfo method = type.GetMethod("Clear");
    //     method.Invoke(new object(), null);
    // }

    // void PrintVectorArithmetic() {
    //     print((!_reverseOperands ? "|V1| + |V2|" : "|V2| + |V1|") + ": " + _vectorMagnitudeAddition);
    //     print((!_reverseOperands ? "|V1| * |V2|" : "|V2| * |V1|") + ": " + _vectorMagnitudeMultiplication);
    //     print((!_reverseOperands ? "Θ (V1, V2)" : "Θ (V2, V1)") + ": " + _angleBetweenVectorsInDegrees);
    //     print("Cos (" + _angleBetweenVectorsInDegrees + "): " + _cosineOfAngleBetweenVectors);
    //     print((!_reverseOperands ? "V1 . V2" : "V2 . V1") + ": " + _vectorDotProduct);
    //     print((!_reverseOperands ? "V1 + V2" : "V2 + V1") + ": " + (_is3DMode ? _vectorAddition : (Vector2) _vectorAddition));
    //     print((!_reverseOperands ? "V1 - V2" : "V2 - V1") + ": " + (_is3DMode ? _vectorSubtraction : (Vector2) _vectorSubtraction));
    //     print((!_reverseOperands ? "V1 x V2" : "V2 x V1") + ": " + (_is3DMode ? _vectorCrossProduct : (Vector2) _vectorCrossProduct));
    // }

    Vector3 CalculateResultantVectorEndPoint() {
        Vector3 resultantVectorEndPoint = Vector3.zero;
        switch(_selectedOperation) {
            case Operation.Addition:
                resultantVectorEndPoint = _vectorAddition;
                break;
            case Operation.Subtraction:
                resultantVectorEndPoint = _vectorSubtraction;
                break;
            case Operation.CrossProduct:
                resultantVectorEndPoint = _vectorCrossProduct;
                break;
            case Operation.DotProduct:
                break;
        }
        return resultantVectorEndPoint;
    }

    void UpdateProjection() {
        _vectorProjectionLine._EndPoint = _arrow._EndPoint;
        switch(_selectedOperation) {
            case Operation.Addition:
                _vectorProjectionLine._Color = Color.magenta;
                _vectorProjectionLine._StartPoint = _vector1Line._EndPoint;
                break;
            case Operation.Subtraction:
                _vectorProjectionLine._StartPoint = !_reverseOperands ? _vector1Line._EndPoint : _vector2Line._EndPoint;
                _vectorProjectionLine._Color = !_reverseOperands ? Color.magenta : Color.yellow;
                break;
            case Operation.CrossProduct:
                _vectorProjectionLine._StartPoint = Vector3.zero;
                _vectorProjectionLine._EndPoint = Vector3.zero;
                break;
            case Operation.DotProduct:
                _vectorProjectionLine._StartPoint = Vector3.zero;
                _vectorProjectionLine._EndPoint = Vector3.zero;
                break;
        }
    }

    void SaveState() {
        _previousVector1EndPoint = _vector1EndPoint;
        _previousVector2EndPoint = _vector2EndPoint;
        _previousSelectedOperation = _selectedOperation;
        _previousReverseOperands = _reverseOperands;
        _previousIs3DMode = _is3DMode;
    }

    // UI Methods
    public void HandleDropdownInputData(int value) {
        CameraModeToggle._Instance.EnableToggle();
        _dotProductPanel.SetActive(false);
        switch (value) {
            case 0: 
                _selectedOperation = Operation.Addition;
                _arrow._Label = !_reverseOperands ? "V1 + V2" : "V2 + V1";
                break;
            case 1:
                _selectedOperation = Operation.Subtraction;
                _arrow._Label = !_reverseOperands ? "V1 - V2" : "V2 - V1";
                break;
            case 2:
                _selectedOperation = Operation.CrossProduct;
                _arrow._Label = !_reverseOperands ? "V1 x V2" : "V2 x V1";
                CameraModeToggle._Instance.Force3DMode();
                break;
            case 3:
                _selectedOperation = Operation.DotProduct;
                _dotProductPanel.SetActive(true);
                break;
        }
    }

    public void HandleToggle(bool toggle) {
        _reverseOperands = toggle;
    }

    public void HandleDotProductCalculationsUIText() {
        _vectorMagnitudeMultiplicationText.text = (!_reverseOperands ? "|V1| * |V2|" : "|V2| * |V1|") + ": " + _vectorMagnitudeMultiplication;
        _angleBetweenVectorsText.text = (!_reverseOperands ? "Θ (V1, V2)" : "Θ (V2, V1)") + ": " + _angleBetweenVectorsInDegrees;
        _cosineOfAngleBetweenVectorsText.text = "Cos (" + _angleBetweenVectorsInDegrees + "): " + _cosineOfAngleBetweenVectors;
        _vectorDotProductText.text = (!_reverseOperands ? "V1 . V2" : "V2 . V1") + ": " + _vectorDotProduct;
    }

    public void HandleUIText() {
        _vectorMagnitudeAdditionText.text = (!_reverseOperands ? "|V1| + |V2|" : "|V2| + |V1|") + ": " + _vectorMagnitudeAddition;
        _vectorAdditionText.text = (!_reverseOperands ? "V1 + V2" : "V2 + V1") + ": " + (_is3DMode ? _vectorAddition : (Vector2) _vectorAddition);
        _vectorSubtractionText.text = (!_reverseOperands ? "V1 - V2" : "V2 - V1") + ": " + (_is3DMode ? _vectorSubtraction : (Vector2) _vectorSubtraction);
        _vectorCrossProductText.text = (!_reverseOperands ? "V1 x V2" : "V2 x V1") + ": " + (_is3DMode ? _vectorCrossProduct : (Vector2) _vectorCrossProduct);
    }
}

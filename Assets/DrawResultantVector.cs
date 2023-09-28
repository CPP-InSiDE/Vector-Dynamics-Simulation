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
    private Vector3 _vectorAddition;
    private Vector3 _vectorSubtraction;
    private Vector3 _vectorCrossProduct;
    private float _vectorDotProduct;
    private enum Operation { Addition, Subtraction, CrossProduct };
    private Operation _previousSelectedOperation;
    private Vector3 _previousVector1EndPoint;
    private Vector3 _previousVector2EndPoint;

    // SERIALIZABLE VARIABLES
    [SerializeField] private DrawArrow _vector1Line;
    [SerializeField] private DrawArrow _vector2Line;
    [SerializeField] private Operation _selectedOperation = Operation.Addition;
    [SerializeField] private bool _reverseOperands = false;

    // LIFECYCLE METHODS
    void Start() {
        _arrow = GetComponent<DrawArrow>();
    }

    void Update() {
        ReverseVectorEndPoints();
        if (IsStateUnChanged()) return;
        PerformVectorArithmetic();
        ClearLog();
        PrintVectorArithmetic();
        _arrow._EndPoint = CalculateResultantVectorEndPoint();
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
        && _selectedOperation == _previousSelectedOperation;
    }

    void PerformVectorArithmetic() {
        _vectorMagnitudeAddition = _vector1EndPoint.magnitude + _vector2EndPoint.magnitude;
        _vectorAddition = _vector1EndPoint + _vector2EndPoint;
        _vectorSubtraction = _vector1EndPoint - _vector2EndPoint;
        _vectorCrossProduct = Vector3.Cross(_vector1EndPoint, _vector2EndPoint);
        _vectorDotProduct = Vector3.Dot(_vector1EndPoint, _vector2EndPoint);
    }

    public void ClearLog() {
        Assembly assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        System.Type type = assembly.GetType("UnityEditor.LogEntries");
        MethodInfo method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }

    void PrintVectorArithmetic() {
        print((!_reverseOperands ? "|V1| + |V2|" : "|V2| + |V1|") + ": " + _vectorMagnitudeAddition);
        print((!_reverseOperands ? "V1 + V2" : "V2 + V1") + ": " + _vectorAddition);
        print((!_reverseOperands ? "V1 - V2" : "V2 - V1") + ": " + _vectorSubtraction);
        print((!_reverseOperands ? "V1 x V2" : "V2 x V1") + ": " + _vectorCrossProduct);
        print((!_reverseOperands ? "V1 . V2" : "V2 . V1") + ": " + _vectorDotProduct);
    }

    Vector3 CalculateResultantVectorEndPoint() {
        Vector3 resultantVectorEndPoint = new Vector3();
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
        }
        return resultantVectorEndPoint;
    }

    void SaveState() {
        _previousVector1EndPoint = _vector1EndPoint;
        _previousVector2EndPoint = _vector2EndPoint;
        _previousSelectedOperation = _selectedOperation;
    }
}

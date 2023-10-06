using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager _Instance {
        get {
            if (_instance == null) Debug.LogError("Game Manager instance is null!");
            return _instance;
        }
    }
    
    void Awake() {
        _instance = this;
    }

    // public TextMeshProUGUI 


    private float _largestXValue;
    private float _largestYValue;
    private float _largestZValue;
    
    [SerializeField] private DrawArrow _xAxis;
    [SerializeField] private DrawArrow _yAxis;
    [SerializeField] private DrawArrow _zAxis;
    [SerializeField] private DrawArrow _vector1;
    [SerializeField] private DrawArrow _vector2;
    [SerializeField] private DrawArrow _resultantVector;
    [SerializeField] private float _scaleUnit = 5f;

    void Start() {
        _xAxis._StartPoint = new Vector3(-_scaleUnit, 0, 0);
        _yAxis._StartPoint = new Vector3(0, -_scaleUnit, 0);
        _zAxis._StartPoint = new Vector3(0, 0, -_scaleUnit);

        _xAxis._EndPoint = new Vector3(_scaleUnit, 0, 0);
        _yAxis._EndPoint = new Vector3(0, _scaleUnit, 0);
        _zAxis._EndPoint = new Vector3(0, 0, _scaleUnit);
    }

    void Update() {
        Vector3 vector1EndPoint = _vector1._EndPoint;
        Vector3 vector2EndPoint = _vector2._EndPoint;
        Vector3 resultantVectorEndPoint = _resultantVector._EndPoint;
        _largestXValue = Mathf.Max(new float[] { Math.Abs(vector1EndPoint.x), Math.Abs(vector2EndPoint.x), Math.Abs(resultantVectorEndPoint.x) });
        _largestYValue = Mathf.Max(new float[] { Math.Abs(vector1EndPoint.y), Math.Abs(vector2EndPoint.y), Math.Abs(resultantVectorEndPoint.y) });
        _largestZValue = Mathf.Max(new float[] { Math.Abs(vector1EndPoint.z), Math.Abs(vector2EndPoint.z), Math.Abs(resultantVectorEndPoint.z) });
    }

    public Vector3 ScaleVector(Vector3 vectorToScale) {
        return new Vector3 {
            x = _largestXValue == 0 ? 0 : (vectorToScale.x / _largestXValue * _scaleUnit),
            y = _largestYValue == 0 ? 0 : (vectorToScale.y / _largestYValue * _scaleUnit),
            z = _largestZValue == 0 ? 0 : (vectorToScale.z / _largestZValue * _scaleUnit),
        };
    }
}

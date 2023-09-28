using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawVector : MonoBehaviour
{
    // PRIVATE VARIABLES
    private DrawArrow _arrow;
    private float _previousAngleInDegrees;
    private float _previousMagnitude;
    private enum Direction { Positive, Negative };
    private Direction _previousXDirection;
    private Direction _previousYDirection;
    
    // SERIALIZABLE VARIABLES
    [SerializeField] private float _angleInDegrees;
    [SerializeField] private float _magnitude;
    [SerializeField] private Direction _xDirection = Direction.Positive;
    [SerializeField] private Direction _yDirection = Direction.Positive;

    // LIFECYCLE METHODS
    void Start() {
        _arrow = GetComponent<DrawArrow>();
    }

    void Update() {
        if (IsStateUnChanged()) return;
        _arrow._EndPoint = CalculateVectorEndPoint();
        SaveState();
    }

    // CLASS METHODS
    bool IsStateUnChanged() {
        return _angleInDegrees == _previousAngleInDegrees
        && _magnitude == _previousMagnitude
        && _xDirection == _previousXDirection
        && _yDirection == _previousYDirection;
    }

    Vector3 CalculateVectorEndPoint() {
        float angleInRadians = _angleInDegrees * (Mathf.PI / 180f);
        Vector3 vectorEndPoint = new Vector3 {
            x = (_xDirection == Direction.Positive ? 1 : -1) * _magnitude * Mathf.Cos(angleInRadians),
            y = (_yDirection == Direction.Positive ? 1 : -1) * _magnitude * Mathf.Sin(angleInRadians)
        };
        return vectorEndPoint;
    }

    void SaveState() {
        _previousAngleInDegrees = _angleInDegrees;
        _previousMagnitude = _magnitude;
        _previousXDirection = _xDirection;
        _previousYDirection = _yDirection;
    }
}

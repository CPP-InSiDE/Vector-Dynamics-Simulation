using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawVector : MonoBehaviour
{
    // PRIVATE VARIABLES
    private DrawArrow _arrow;
    private float _previousAngleInDegrees;
    private float _previousMagnitude;
    private Direction _previousXDirection;
    private Direction _previousYDirection;

    public enum Direction { Positive, Negative };
    
    // SERIALIZABLE VARIABLES
    [SerializeField] private float _angleInDegrees;
    [SerializeField] private float _magnitude;
    [SerializeField] private Direction _xDirection = Direction.Positive;
    [SerializeField] private Direction _yDirection = Direction.Positive;

    [SerializeField] private TMPro.TMP_InputField _AngleInDegreesInput;
    [SerializeField] private TMPro.TMP_InputField _MagnitudeInput;

    // LIFECYCLE METHODS
    void Start() {
        _arrow = GetComponent<DrawArrow>();
    }

    void Update() {
        if (IsStateUnChanged()) return;
        _arrow._EndPoint = CalculateVectorEndPoint();
        // _arrow._ScaledEndPoint = GameManager._Instance.ScaleVector(_arrow._EndPoint);
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

    public void GrabFromAngleInputField(string angle) {
        _angleInDegrees = float.Parse(angle);
    }

    public void GrabFromMagnitudeInputField(string magnitude) {
        _magnitude = float.Parse(magnitude);
    }

    public void HandleXDirectionDropdownInputData(int value) {
        switch (value) {
            case 0:
                _xDirection = Direction.Positive;
                break;
            case 1:
                _xDirection = Direction.Negative;
                break;
        }
    }

    public void HandleYDirectionDropdownInputData(int value) {
        switch (value) {
            case 0:
                _yDirection = Direction.Positive;
                break;
            case 1:
                _yDirection = Direction.Negative;
                break;
        }
    }
}

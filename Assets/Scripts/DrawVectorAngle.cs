using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawVectorAngle : MonoBehaviour
{
    // PRIVATE VARIABLES
    private DrawArrow _arrow;
    private float _previousAngleInDegrees;
    private float _previousMagnitude;

    public enum Direction { Positive, Negative };
    
    // SERIALIZABLE VARIABLES
    [SerializeField] private float _angleInDegrees;
    [SerializeField] private float _magnitude;

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
        && _magnitude == _previousMagnitude;
    }

    Vector3 CalculateVectorEndPoint() {
        float angleInRadians = _angleInDegrees * (Mathf.PI / 180f);
        Vector3 vectorEndPoint = new Vector3 {
            x = _magnitude * Mathf.Cos(angleInRadians),
            y = _magnitude * Mathf.Sin(angleInRadians)
        };
        return vectorEndPoint;
    }

    void SaveState() {
        _previousAngleInDegrees = _angleInDegrees;
        _previousMagnitude = _magnitude;
    }

    public void GrabFromAngleInputField(string angle) {
        _angleInDegrees = float.Parse(angle);
    }

    public void GrabFromMagnitudeInputField(string magnitude) {
        _magnitude = Mathf.Clamp(float.Parse(magnitude), -6f, 6f);
    }
}

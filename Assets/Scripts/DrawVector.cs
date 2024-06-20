using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DrawVector : MonoBehaviour
{
    // PRIVATE VARIABLES
    private DrawArrow _arrow;
    private Vector3 _previousVector;

    public enum Direction { Positive, Negative };

    // SERIALIZABLE VARIABLES
    [SerializeField] private Vector3 _vector;

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
        return _vector == _previousVector;
    }

    Vector3 CalculateVectorEndPoint() {
        Vector3 vectorEndPoint = _vector;
        return vectorEndPoint;
    }

    void SaveState() {
        _previousVector = _vector;
    }

    public void GrabXFromInputField(string value)
    {
        float num = value == "" ? 0 : float.Parse(value);
        _vector = new Vector3(num, _vector.y, _vector.z);
    }
    public void GrabYFromInputField(string value)
    {
        float num = value == "" ? 0 : float.Parse(value);
        _vector = new Vector3(_vector.x, num, _vector.z);
    }
    public void GrabZFromInputField(string value)
    {
        float num = value == "" ? 0 : float.Parse(value);
        _vector = new Vector3(_vector.x, _vector.y, num);
    }
}

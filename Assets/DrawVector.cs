using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawVector : MonoBehaviour
{
    [SerializeField] private float angleInDegrees;
    [SerializeField] private float magnitude;

    private enum Direction { Positive, Negative };
    [SerializeField] private Direction xDirection = Direction.Positive;
    [SerializeField] private Direction yDirection = Direction.Positive;

    private float previousAngleInDegrees;
    private float previousMagnitude;
    private Direction previousXDirection;
    private Direction previousYDirection;

    void Update() {
        // if angle, magnitude have not changed, do not compute vector end point
        if (
            angleInDegrees == previousAngleInDegrees && 
            magnitude == previousMagnitude &&
            xDirection == previousXDirection &&
            yDirection == previousYDirection
        ) 
        return;

        // calculate vector end point
        float angleInRadians = angleInDegrees * (Mathf.PI / 180f);
        Vector3 vectorEndPoint = new Vector3 {
            x = (xDirection == Direction.Positive ? 1 : -1) * magnitude * Mathf.Cos(angleInRadians),
            y = (yDirection == Direction.Positive ? 1 : -1) * magnitude * Mathf.Sin(angleInRadians)
        };
        GetComponent<DrawLine>().EndPoint = vectorEndPoint;

        // save angle and magnitude
        previousAngleInDegrees = angleInDegrees;
        previousMagnitude = magnitude;
        previousXDirection = xDirection;
        previousYDirection = yDirection;
    }
}

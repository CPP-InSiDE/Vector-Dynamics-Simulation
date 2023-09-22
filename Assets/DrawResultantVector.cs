using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class DrawResultantVector : MonoBehaviour
{
    [SerializeField] private DrawLine vector1Line;
    [SerializeField] private DrawLine vector2Line;

    private enum Operation { Addition, Subtraction, CrossProduct };
    [SerializeField] private Operation selectedOperation = Operation.Addition;

    [SerializeField] private bool reverseOperands = false;

    private Vector3 previousVector1EndPoint;
    private Vector3 previousVector2EndPoint;
    private Operation previousSelectedOperation;
    
    void Update() {
        // get vector 1 and 2 end points (or reverse them if reverse operands is set to true)
        Vector3 vector1EndPoint = !reverseOperands ? vector1Line.EndPoint : vector2Line.EndPoint;
        Vector3 vector2EndPoint = !reverseOperands ? vector2Line.EndPoint : vector1Line.EndPoint;

        // if previous vector endpoints and selected operation are unchanged, do not compute resultant vector and dot product
        if (
            vector1EndPoint == previousVector1EndPoint && 
            vector2EndPoint == previousVector2EndPoint && 
            selectedOperation == previousSelectedOperation
        ) 
        return;

        // vector calculations
        float vectorMagnitudeAddition = vector1EndPoint.magnitude + vector2EndPoint.magnitude;
        Vector3 vectorAddition = vector1EndPoint + vector2EndPoint;
        Vector3 vectorSubtraction = vector1EndPoint - vector2EndPoint;
        Vector3 vectorCrossProduct = Vector3.Cross(vector1EndPoint, vector2EndPoint);
        float vectorDotProduct = Vector3.Dot(vector1EndPoint, vector2EndPoint);

        // print to console
        ClearLog();
        print((!reverseOperands ? "|V1| + |V2|" : "|V2| + |V1|") + ": " + vectorMagnitudeAddition);
        print((!reverseOperands ? "V1 + V2" : "V2 + V1") + ": " + vectorAddition);
        print((!reverseOperands ? "V1 - V2" : "V2 - V1") + ": " + vectorSubtraction);
        print((!reverseOperands ? "V1 x V2" : "V2 x V1") + ": " + vectorCrossProduct);
        print((!reverseOperands ? "V1 . V2" : "V2 . V1") + ": " + vectorDotProduct);

        // draw resultant vector based on selected operation
        Vector3 resultantVectorEndPoint = new Vector3();
        switch(selectedOperation) {
            case Operation.Addition:
                resultantVectorEndPoint = vectorAddition;
                break;
            case Operation.Subtraction:
                resultantVectorEndPoint = vectorSubtraction;
                break;
            case Operation.CrossProduct:
                resultantVectorEndPoint = vectorCrossProduct;
                break;
        }
        GetComponent<DrawLine>().EndPoint = resultantVectorEndPoint;
        
        // save vector 1 & 2 endpoints and selected operation
        previousVector1EndPoint = vector1EndPoint;
        previousVector2EndPoint = vector2EndPoint;
        previousSelectedOperation = selectedOperation;
    }

    public static void ClearLog() {
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.ActiveEditorTracker));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
    }
}

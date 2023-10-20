using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArrow : MonoBehaviour
{
    // PRIVATE VARIABLES
    private GameObject _arrowHead;
    private LineRenderer _arrowBodyRenderer;
    private LineRenderer _arrowHeadRenderer;
    private Vector3 _previousStartPoint;
    private Vector3 _previousEndPoint;
    private string _previousLabel;
    private bool _is3DMode = true;
    private bool _previousIs3DMode = false;

    // SERIALIZABLE VARIABLES
    [SerializeField] private Vector3 _startPoint;
    [SerializeField] private Vector3 _endPoint;
    [SerializeField] private float _arrowBodyThickness = 0.025f;
    [SerializeField] private float _arrowHeadThickness = 0.25f;
    [SerializeField] private string _label;
    [SerializeField] private Color _color;

    // ACCESSORS & MUTATORS
    public Vector3 _StartPoint { get; set; }
    public Vector3 _EndPoint { get; set; }
    public string _Label { get; set; }

    // LIFECYCLE METHODS
    void Start() {
        InitializeArrowBody();
        InitializeArrowHead();
        InitializePublicStateVariables();
    }

    void Update() {
        _is3DMode = CameraModeToggle._Instance.IsCamera3D;
        if (IsStateUnChanged()) return;
        DrawArrowBody();
        DrawArrowHead();
        SaveState();
    }

    void OnGUI() {
        DrawLabel();
    }

    // CLASS METHODS
    void InitializeArrowBody() {
        _arrowBodyRenderer = GetComponent<LineRenderer>();
        _arrowBodyRenderer.useWorldSpace = false;
        _arrowBodyRenderer.startWidth = _arrowBodyThickness;
        _arrowBodyRenderer.endWidth = _arrowBodyThickness;
    }

    void InitializeArrowHead() {
        _arrowHead = new GameObject(gameObject.name + " arrow head");
        _arrowHeadRenderer = _arrowHead.AddComponent<LineRenderer>();
        _arrowHeadRenderer.material = _arrowBodyRenderer.material;
        _arrowHeadRenderer.startWidth = _arrowHeadThickness;
        _arrowHeadRenderer.endWidth = _arrowBodyThickness;
    }

    void InitializePublicStateVariables() {
        _StartPoint = _startPoint;
        _EndPoint = _endPoint;
        _Label = _label;
    }

    bool IsStateUnChanged() {
        return _StartPoint == _previousStartPoint 
        && _EndPoint == _previousEndPoint
        && _Label == _previousLabel
        && _is3DMode == _previousIs3DMode;
    }

    void DrawArrowBody() {
        _arrowBodyRenderer.startColor = DetermineColorOpacityBasedOnCameraModeAndDistance();
        _arrowBodyRenderer.endColor = DetermineColorOpacityBasedOnCameraModeAndDistance();
        Vector3[] linePositions = { _StartPoint, _EndPoint };
        _arrowBodyRenderer.positionCount = linePositions.Length;
        _arrowBodyRenderer.SetPositions(linePositions);
    }

    Vector3 GetPointAlongSlopeFromEndPointUsingDistance(float distance) {
        Vector3 slopeDirection = _EndPoint - _StartPoint;
        slopeDirection.Normalize();
        return _EndPoint + slopeDirection * distance;
    }

    void DrawArrowHead() {
        _arrowHeadRenderer.startColor = DetermineColorOpacityBasedOnCameraModeAndDistance();
        _arrowHeadRenderer.endColor = DetermineColorOpacityBasedOnCameraModeAndDistance();
        Vector3 newPoint = GetPointAlongSlopeFromEndPointUsingDistance(-0.25f);
        Vector3[] arrowHeadPositions = { newPoint, _EndPoint };
        _arrowHeadRenderer.positionCount = arrowHeadPositions.Length;
        _arrowHeadRenderer.SetPositions(arrowHeadPositions);
    }

    Color DetermineColorOpacityBasedOnCameraModeAndDistance() {
        if ((!_is3DMode && gameObject.name.Equals("z-axis")) || Vector3.Distance(_StartPoint, _EndPoint) < 0.1f) return Color.clear;
        return _color;
    }

    void DrawLabel() {
        Vector3 labelPositionInWorldSpace = GetPointAlongSlopeFromEndPointUsingDistance(0.25f);
        Vector3 labelPositionOnScreen = Camera.main.WorldToScreenPoint(labelPositionInWorldSpace);
        string labelWithEndPointCoordinates = _Label + (gameObject.name.Contains("axis") ? "" : (" " + (_is3DMode ? _EndPoint : (Vector2) _EndPoint)));
        Vector2 labelTextSize = GUI.skin.label.CalcSize(new GUIContent(_Label));
        Vector2 labelWithEndPointCoordinatesTextSize = GUI.skin.label.CalcSize(new GUIContent(labelWithEndPointCoordinates));
        GUI.Label(
            new Rect(
                labelPositionOnScreen.x - labelTextSize.x / 2, 
                Screen.height - labelPositionOnScreen.y - labelTextSize.y / 2, 
                labelWithEndPointCoordinatesTextSize.x, 
                labelWithEndPointCoordinatesTextSize.y
            ), 
            labelWithEndPointCoordinates,
            new GUIStyle {
                fontSize = 20,
                normal = new GUIStyleState { textColor = DetermineColorOpacityBasedOnCameraModeAndDistance() }
            }
        );
    }

    void SaveState() {
        _previousStartPoint = _StartPoint;
        _previousEndPoint = _EndPoint;
        _previousLabel = _Label;
    }
}

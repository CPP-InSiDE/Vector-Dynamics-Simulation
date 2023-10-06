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
    private Color _previousColor;

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
    public Color _Color { get; set; }

    // LIFECYCLE METHODS
    void Start() {
        InitializeArrowBody();
        InitializeArrowHead();
        InitializePublicStateVariables();
    }

    void Update() {
        if (IsStateUnChanged()) return;
        DrawArrowBody();
        DrawArrowHead();
        MakeTransparentBasedOnDistance();
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
        _arrowBodyRenderer.startColor = _color;
        _arrowBodyRenderer.endColor = _color;
    }

    void InitializeArrowHead() {
        _arrowHead = new GameObject(gameObject.name + " arrow head");
        _arrowHeadRenderer = _arrowHead.AddComponent<LineRenderer>();
        _arrowHeadRenderer.material = _arrowBodyRenderer.material;
        _arrowHeadRenderer.startWidth = _arrowHeadThickness;
        _arrowHeadRenderer.endWidth = _arrowBodyThickness;
        _arrowHeadRenderer.startColor = _color;
        _arrowHeadRenderer.endColor = _color;
    }

    void InitializePublicStateVariables() {
        _Color = _color;
        _StartPoint = _startPoint;
        _EndPoint = _endPoint;
    }

    bool IsStateUnChanged() {
        return _StartPoint == _previousStartPoint 
        && _EndPoint == _previousEndPoint 
        && _Color == _previousColor;
    }

    void DrawArrowBody() {
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
        Vector3 newPoint = GetPointAlongSlopeFromEndPointUsingDistance(-0.25f);
        Vector3[] arrowHeadPositions = { newPoint, _EndPoint };
        _arrowHeadRenderer.positionCount = arrowHeadPositions.Length;
        _arrowHeadRenderer.SetPositions(arrowHeadPositions);
    }

    void MakeTransparentBasedOnDistance() {
        _Color = Vector3.Distance(_StartPoint, _EndPoint) < 0.1f ? new Color { a = 0 } : _color;
    }

    void DrawLabel() {
        Vector3 labelPositionInWorldSpace = GetPointAlongSlopeFromEndPointUsingDistance(0.25f);
        Vector3 labelPositionOnScreen = Camera.main.WorldToScreenPoint(labelPositionInWorldSpace);
        string labelWithEndPointCoordinates = _label + (gameObject.name.Contains("axis") ? "" : (" " + _EndPoint));
        Vector2 labelTextSize = GUI.skin.label.CalcSize(new GUIContent(_label));
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
                normal = new GUIStyleState { textColor = _Color }
            }
        );
    }

    void SaveState() {
        _previousStartPoint = _StartPoint;
        _previousEndPoint = _EndPoint;
        _previousColor = _Color;
    }
}

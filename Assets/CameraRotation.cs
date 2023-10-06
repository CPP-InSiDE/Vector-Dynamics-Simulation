using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraRotation : MonoBehaviour
{
    // PRIVATE VARIABLES
    private enum RotateMethod { Mouse, Touch };
    private Vector2 _swipeDirection;
    private Quaternion _cameraRotation;
    private Touch _touch;
    private float _distanceBetweenCameraAndTarget;
    private float _minimumXRotationAngle = -80;
    private float _maximumXRotationAngle  = 80;
    private float _rotationX;
    private float _rotationY;

    // SERIALIZABLE VARIABLES
    [SerializeField] private Transform _target;

    [Range(0.1f, 5f)]
    [Tooltip("How sensitive the mouse drag to camera rotation")]
    [SerializeField] private float _mouseRotateSpeed = 5f;
    
    [Range(0.01f, 100)]
    [Tooltip("How sensitive the touch drag to camera rotation")]
    [SerializeField] private float _touchRotateSpeed = 17.5f;
    
    [Tooltip("Smaller positive value means smoother rotation, 1 means no smooth apply")]
    [SerializeField] private float _slerpValue = 0.25f; 
    
    [Tooltip("How do you like to rotate the camera")]
    [SerializeField] private RotateMethod _rotateMethod = RotateMethod.Mouse;

    // LIFECYCLE METHODS
    void Start() {
        _distanceBetweenCameraAndTarget = Vector3.Distance(transform.position, _target.position);
    }

    void Update() {
        switch(_rotateMethod) {
            case RotateMethod.Mouse:
                CaptureMouseRotation();
                break;
            case RotateMethod.Touch:
                CaptureTouchRotation();
                break;
        }
    }

    void LateUpdate() {
        Vector3 direction = new Vector3(0, 0, -_distanceBetweenCameraAndTarget);
        Quaternion newQuarternion;
        if (_rotateMethod == RotateMethod.Mouse)
            newQuarternion  = Quaternion.Euler(_rotationX , _rotationY, 0);
        else
            newQuarternion = Quaternion.Euler(_swipeDirection.y , -_swipeDirection.x, 0);
        _cameraRotation = Quaternion.Slerp(_cameraRotation, newQuarternion, _slerpValue);
        transform.position = _target.position + _cameraRotation * direction;
        transform.LookAt(_target.position);
    }

    // CLASS METHODS
    void CaptureMouseRotation() {
        if (Input.GetMouseButton(0)) {
            _rotationX += -Input.GetAxis("Mouse Y") * _mouseRotateSpeed;
            _rotationY += Input.GetAxis("Mouse X") * _mouseRotateSpeed;
        }
        if (_rotationX < _minimumXRotationAngle)
            _rotationX = _minimumXRotationAngle;
        else if (_rotationX > _maximumXRotationAngle)
            _rotationX = _maximumXRotationAngle;
    }

    void CaptureTouchRotation() {
        if (Input.touchCount > 0) {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Moved)
                _swipeDirection += _touch.deltaPosition * Time.deltaTime * _touchRotateSpeed;
        }
        if (_swipeDirection.y < _minimumXRotationAngle)
            _swipeDirection.y = _minimumXRotationAngle;
        else if (_swipeDirection.y > _maximumXRotationAngle)
            _swipeDirection.y = _maximumXRotationAngle;
    }

    void SetCameraPosition() {
        transform.position = new Vector3(0, 0, -_distanceBetweenCameraAndTarget);
    }
}
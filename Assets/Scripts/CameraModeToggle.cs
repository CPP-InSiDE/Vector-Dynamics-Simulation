using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraModeToggle : MonoBehaviour
{
    private static CameraModeToggle _instance;
    public static CameraModeToggle _Instance {
        get {
            if (_instance == null) Debug.LogError("Camera Mode Toggle instance is null!");
            return _instance;
        }
    }
    
    void Awake() {
        _instance = this;
    }

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    
    [SerializeField] private Toggle toggle;
    [SerializeField] private MonoBehaviour cameraRotationScript;
    [SerializeField] private MonoBehaviour cameraMovementScript;
    
    public bool IsCamera3D { get; set; }

    void Start() {
        originalCameraPosition = gameObject.transform.position;
        originalCameraRotation = gameObject.transform.rotation;
        gameObject.GetComponent<Camera>().orthographic = true;
        IsCamera3D = false;
    }

    public void OnToggleValueChanged(bool is3DMode) {
        IsCamera3D = is3DMode;
        gameObject.transform.position = originalCameraPosition;
        gameObject.transform.rotation = originalCameraRotation;
        gameObject.GetComponent<Camera>().orthographic = !is3DMode;
        cameraRotationScript.enabled = is3DMode;
        cameraMovementScript.enabled = !is3DMode;
    }

    public void Force3DMode() {
        toggle.isOn = true;
        toggle.interactable = false;
    }

    public void EnableToggle() {
        toggle.interactable = true;
    }
}
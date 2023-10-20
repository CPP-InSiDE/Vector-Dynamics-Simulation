using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraMovement : MonoBehaviour
{
    private Vector2 mouseClickPos;
    private Vector2 mouseCurrentPos;
 
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKey(KeyCode.Mouse0)) {
            mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;
            transform.position += new Vector3(-distance.x, -distance.y, 0);
            transform.position = new Vector3 {
                x = Mathf.Clamp(transform.position.x, -8f, 8f),
                y = Mathf.Clamp(transform.position.y, -8f, 8f),
                z = transform.position.z
            };
        }
    }
}
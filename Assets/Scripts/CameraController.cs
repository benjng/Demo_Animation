using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float sensitivity;
    [SerializeField] private float distance;
    [SerializeField] private float height;
    private float x = 0.0f;
    private float y = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // transform: self's (camera) transform 
        // x = transform.eulerAngles.x;
        // y = transform.eulerAngles.y;
        // Init camera
        x = 270;
        y = 20;
    }

    private void LateUpdate()
    {
        x += Input.GetAxis("Mouse X") * sensitivity;
        y -= Input.GetAxis("Mouse Y") * sensitivity;
        y = Mathf.Clamp(y, 0f, 50f);

        Quaternion rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, height, -distance) + target.position;

        transform.rotation = rotation;
        transform.position = position;
    }
}

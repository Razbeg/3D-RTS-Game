using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    
    public float moveSpeed;
    public float zoomSpeed;

    public float minZoomDist;
    public float maxZoomDist;

    private Camera _cam;

    private void Awake()
    {
        Instance = this;
        _cam = Camera.main;
    }

    private void Update()
    {
        Move();
        Zoom();
    }

    public void FocusOnPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    private void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 dir = transform.forward * zInput + transform.right * xInput;

        transform.position += dir * (moveSpeed * Time.deltaTime);
    }

    private void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float dist = Vector3.Distance(transform.position, _cam.transform.position);

        if (dist < minZoomDist && scrollInput > 0.0f)
        {
            return;
        }
        else if (dist > maxZoomDist && scrollInput < 0.0f)
        {
            return;
        }

        _cam.transform.position += _cam.transform.forward * (scrollInput * zoomSpeed);
    }
}

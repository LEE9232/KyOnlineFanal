using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamZoom : MonoBehaviour
{
    public GameObject stage;
    public float scrollSpeed = 3000f;
    public float smoothTime = 0.1f;

    private float MinZoom = 10f;
    private float MaxZoom = 45f;
    private float targetFOV;
    private float currentFOV;
    private float velocity = 0.0f;

    private void Start()
    {
        targetFOV = Camera.main.fieldOfView;
        currentFOV = Camera.main.fieldOfView;
    }

    private void Update()
    {
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        targetFOV -= scrollWheel * Time.deltaTime * scrollSpeed;

        if (targetFOV <= MinZoom)
        {
            targetFOV = MinZoom;
        }
        else if (targetFOV >= MaxZoom)
        {
            targetFOV = MaxZoom;
        }

        // ¡‹¿Œ/¡‹æ∆øÙ ∫ŒµÂ∑¥∞‘
        currentFOV = Mathf.SmoothDamp(currentFOV, targetFOV, ref velocity, smoothTime);
        Camera.main.fieldOfView = currentFOV;

        //Vector3 cameraDirection = this.transform.localRotation * Vector3.forward;
        //Camera.main.fieldOfView -= scrollWheel * Time.deltaTime * scrollSpeed;
    }
}

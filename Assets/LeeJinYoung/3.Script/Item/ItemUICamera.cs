using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUICamera : MonoBehaviour
{
    // ������ Canvas�� �׻� ����ī�޶� �ٶ󺸰� ��
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            // UI�� ī�޶� �׻� �ٶ󺸵��� ����
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}

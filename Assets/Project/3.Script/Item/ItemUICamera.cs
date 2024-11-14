using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUICamera : MonoBehaviour
{
    // 아이템 Canvas가 항상 메인카메라를 바라보게 함
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            // UI가 카메라를 항상 바라보도록 설정
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);
        }
    }
}

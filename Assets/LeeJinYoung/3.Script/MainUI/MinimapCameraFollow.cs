using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform playerTransform;  // 플레이어의 위치를 참조
    public Vector3 offset = new Vector3(0, 60, 0);  // 플레이어로부터의 오프셋
    public float cameraRotationSpeed = 10f;  // 카메라 회전 속도
    void LateUpdate()
    {
        Vector3 newPosition = playerTransform.position + offset;
        transform.position = newPosition;
    }
}

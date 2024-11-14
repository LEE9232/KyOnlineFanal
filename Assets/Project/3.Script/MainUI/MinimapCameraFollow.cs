using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform playerTransform;  // �÷��̾��� ��ġ�� ����
    public Vector3 offset = new Vector3(0, 60, 0);  // �÷��̾�κ����� ������
    public float cameraRotationSpeed = 10f;  // ī�޶� ȸ�� �ӵ�
    void LateUpdate()
    {
        Vector3 newPosition = playerTransform.position + offset;
        transform.position = newPosition;
    }
}

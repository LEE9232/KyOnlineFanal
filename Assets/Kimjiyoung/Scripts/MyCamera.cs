using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

    #region ����
    public Transform player; // �÷��̾��� Transform
    public float mouseSensitivity = 100.0f; // ���콺 ����
    public float distanceFromPlayer = 8.0f; // ī�޶�� �÷��̾� ������ �Ÿ�
    public float heightOffset = 0.0f; // ī�޶��� ���� ������
    public float rotationX = 10.0f;
    public float rotationY = 10.0f;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0; // ī�޶� �̵��� �ε巯�� ���� 
    private bool isCursorLocked = false;
    private bool isDragging = false;
    #endregion

    // ���콺 ���� ���� �޼���
    public void SetMouseSensitivity(float sensitivity)
    {
        mouseSensitivity = sensitivity;
    }
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            isDragging = true;
            LockCursor();
        }

        // ���콺 ������ ��ư�� ������ �巡�� ���� ����
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            UnlockCursor();
        }

        if (!isDragging)
        {
            FollowPlayer();
        }

        // �巡�� ������ ���� ȸ��
        if (isDragging)
        {
            Rotate();
        }
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorLocked = true;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isCursorLocked = false;
    }


    public void Rotate()
    {
        //rotationY = Mathf.Clamp(rotationY, -35, 60);
        // ���콺 �Է��� ����
        float sensitivityMultiplier = 2.0f;  // ���� ���� �߰�
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * sensitivityMultiplier * Time.deltaTime;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * sensitivityMultiplier * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -5, 50); // Y�� ȸ�� ������ �����Ͽ� ī�޶� �������� �ʵ��� ��
        // ī�޶��� ȸ���� ����
        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 targetPosition;

        targetPosition = player.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void Follow()
    {
        Vector3 targetPosition;

        targetPosition = player.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void FollowPlayer()
    {
        // �÷��̾��� ��ġ�� ���� �Ÿ����� ���󰡵��� ����
        Vector3 targetPosition = player.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // �÷��̾ �ٶ󺸵��� ī�޶��� ȸ�� ����
        transform.LookAt(player.position + Vector3.up * heightOffset);
    }

}




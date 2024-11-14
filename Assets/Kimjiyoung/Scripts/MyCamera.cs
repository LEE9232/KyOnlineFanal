using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MyCamera : MonoBehaviour
{

    #region 변수
    public Transform player; // 플레이어의 Transform
    public float mouseSensitivity = 100.0f; // 마우스 감도
    public float distanceFromPlayer = 8.0f; // 카메라와 플레이어 사이의 거리
    public float heightOffset = 0.0f; // 카메라의 높이 오프셋
    public float rotationX = 10.0f;
    public float rotationY = 10.0f;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0; // 카메라 이동의 부드러움 정도 
    private bool isCursorLocked = false;
    private bool isDragging = false;
    #endregion

    // 마우스 감도 변경 메서드
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

        // 마우스 오른쪽 버튼을 놓으면 드래그 상태 해제
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            UnlockCursor();
        }

        if (!isDragging)
        {
            FollowPlayer();
        }

        // 드래그 상태일 때만 회전
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
        // 마우스 입력을 받음
        float sensitivityMultiplier = 2.0f;  // 감도 배율 추가
        rotationX += Input.GetAxis("Mouse X") * mouseSensitivity * sensitivityMultiplier * Time.deltaTime;
        rotationY -= Input.GetAxis("Mouse Y") * mouseSensitivity * sensitivityMultiplier * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY, -5, 50); // Y축 회전 각도를 제한하여 카메라가 뒤집히지 않도록 함
        // 카메라의 회전을 설정
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
        // 플레이어의 위치와 일정 거리에서 따라가도록 설정
        Vector3 targetPosition = player.position - transform.forward * distanceFromPlayer + Vector3.up * heightOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // 플레이어를 바라보도록 카메라의 회전 설정
        transform.LookAt(player.position + Vector3.up * heightOffset);
    }

}




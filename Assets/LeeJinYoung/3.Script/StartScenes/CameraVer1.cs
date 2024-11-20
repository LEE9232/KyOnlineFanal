using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVer1 : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // 시네마카메라
    public Transform targetTransform; // 이동할 위치
    public float moveSpeed = 2f; // 이동 속도
    private bool shouldMove = false; // 카메라 이동중 체크
    private bool isShaking = false; // 흔들림이 시작되었는지 확인하는 플래그
    public float shakeAmplitude = 0.01f; // 흔들림 강도
    public float shakeFrequency = 0.01f; // 흔들림 빈도
    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        // 가상 카메라에서 노이즈 컴포넌트 가져오기
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (shouldMove)
        {
            // 카메라를 타겟 위치로 천천히 이동
            virtualCamera.transform.position =
            Vector3.Lerp(virtualCamera.transform.position,
            targetTransform.position,
            moveSpeed * Time.deltaTime);
            // 카메라가 목표 위치에 도달하면 움직임 멈춤
            if (Vector3.Distance(virtualCamera.transform.position, targetTransform.position) < 0.1f)
            {
                shouldMove = false;
                isShaking = true; // 흔들림 상태로 전환
                StartShake();
            }
        }
        // 카메라가 흔들리고 있을 때 위치를 고정
        if (isShaking)
        {
            virtualCamera.transform.position = targetTransform.position;
        }
    }

    private void StartShake()
    {
        if (noise != null)
        {
            noise.m_AmplitudeGain = shakeAmplitude;
            noise.m_FrequencyGain = shakeFrequency;
        }
    }
    public void CameraMove()
    {
        shouldMove = true;
    }

}


using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVer1 : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera; // �ó׸�ī�޶�
    public Transform targetTransform; // �̵��� ��ġ
    public float moveSpeed = 2f; // �̵� �ӵ�
    private bool shouldMove = false; // ī�޶� �̵��� üũ
    private bool isShaking = false; // ��鸲�� ���۵Ǿ����� Ȯ���ϴ� �÷���
    public float shakeAmplitude = 0.01f; // ��鸲 ����
    public float shakeFrequency = 0.01f; // ��鸲 ��
    private CinemachineBasicMultiChannelPerlin noise;

    private void Start()
    {
        // ���� ī�޶󿡼� ������ ������Ʈ ��������
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    private void Update()
    {
        if (shouldMove)
        {
            // ī�޶� Ÿ�� ��ġ�� õõ�� �̵�
            virtualCamera.transform.position =
            Vector3.Lerp(virtualCamera.transform.position,
            targetTransform.position,
            moveSpeed * Time.deltaTime);
            // ī�޶� ��ǥ ��ġ�� �����ϸ� ������ ����
            if (Vector3.Distance(virtualCamera.transform.position, targetTransform.position) < 0.1f)
            {
                shouldMove = false;
                isShaking = true; // ��鸲 ���·� ��ȯ
                StartShake();
            }
        }
        // ī�޶� ��鸮�� ���� �� ��ġ�� ����
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


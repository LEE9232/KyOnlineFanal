using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAreaCollision : MonoBehaviour
{
    public ParticleSystem particleSystem;

    private void Start()
    {
        // ��ƼŬ �ý����� Collision ��� ����
        var collisionModule = particleSystem.collision;
        collisionModule.enabled = true;
        //collisionModule.collidesWith = LayerMask.GetMask("Player"); // �浹�� ������ ���̾� ����
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player")) // �ʿ��� �±׷� ����
        {
            Debug.Log("��ƼŬ �浹 �߻�: " + other.name);
            // �浹 ó�� ���� �߰�
        }
    }
}

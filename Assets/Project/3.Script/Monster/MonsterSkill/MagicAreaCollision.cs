using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAreaCollision : MonoBehaviour
{
    public ParticleSystem particleSystem;

    private void Start()
    {
        // 파티클 시스템의 Collision 모듈 설정
        var collisionModule = particleSystem.collision;
        collisionModule.enabled = true;
        //collisionModule.collidesWith = LayerMask.GetMask("Player"); // 충돌을 감지할 레이어 설정
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player")) // 필요한 태그로 변경
        {
            Debug.Log("파티클 충돌 발생: " + other.name);
            // 충돌 처리 로직 추가
        }
    }
}

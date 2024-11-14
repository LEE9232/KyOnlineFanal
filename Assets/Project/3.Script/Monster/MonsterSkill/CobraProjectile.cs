using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraProjectile : MonoBehaviour
{
    public float speed = 15f; // 발사체 속도
    public int damage = 15;   // 발사체가 줄 피해량
    public GameObject impactEffect; // 충돌 시 시각적 효과

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    // 트리거를 사용하여 충돌 감지
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어와 충돌 시
        {
            // 플레이어에게 데미지 적용
            other.GetComponent<PlayerManagement>().TakeDamage(damage);
            // 충돌 효과 생성
            if (impactEffect != null)
            {
                GameObject impact =  Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact ,1f);
            }
            Destroy(gameObject);
            // 발사체 제거
        }
        if (other.CompareTag("Guard"))
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact, 1f);
            }
            // 발사체 제거
            Destroy(gameObject);
        }
        if (other.CompareTag("Untagged")) // 장애물에 닿았을 때 발사체 제거
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact , 1f);
            }
            Destroy(gameObject);
        }
    }
}

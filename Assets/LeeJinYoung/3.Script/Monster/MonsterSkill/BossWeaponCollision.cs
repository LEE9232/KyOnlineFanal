using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponCollision : MonoBehaviour
{
    private bool colliderCheck = true; // 충돌할 수 있는 상태를 나타냄
    private float damageCooldown = 1f; // 기본 쿨다운 시간
    public MonsterStatus monsterStatus;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player")&& colliderCheck)
        {
            Debug.Log("무기와 충돌함");
            //ApplyDamage();
            other.GetComponent<PlayerManagement>().TakeDamage(monsterStatus.monsDamage);
            // 다시 충돌할 수 없도록 상태 변경
            StartCoroutine(DamageCooldown());
        }
    }
    private void ApplyDamage()
    {

       // Debug.Log("데미지 적용");
    }

    // 충돌 쿨다운을 처리하는 코루틴
    private IEnumerator DamageCooldown()
    {
        colliderCheck = false;
        yield return new WaitForSeconds(damageCooldown); // 지정된 쿨다운 시간만큼 대기
        colliderCheck = true;
    }

    // 공격마다 충돌 쿨다운 시간을 설정하는 메서드
    public void SetDamageCooldown(float cooldown)
    {
        damageCooldown = cooldown;
    }
}

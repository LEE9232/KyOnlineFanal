using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollisionHandler : MonoBehaviour
{
    private int damage;
    private Transform targetMonster;

    public void SetDamage(int damageValue)
    {
        damage = damageValue;
    }

    public void SetTargetMonster(Transform monster)
    {
        targetMonster = monster;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Monster")) // 몬스터와 충돌 시
        if(other.transform==targetMonster)
        {
            // 몬스터의 상태를 가져와서 데미지 적용
            MonsterStatus monsterStatus = other.GetComponent<MonsterStatus>();
            if (monsterStatus != null)
            {
                monsterStatus.TakeDamage(damage); // 기본 데미지 적용
                Debug.Log($"Monster hit! {damage} damage dealt.");

                // 몬스터의 HPBar 업데이트
                MonsterHPbar monsterHPbar = other.GetComponent<MonsterHPbar>();
                if (monsterHPbar != null)
                {
                    monsterHPbar.UpdateHpbar(monsterStatus.currentHp, monsterStatus.monsmaxHp);
                }
            }

            //Destroy(gameObject); // 충돌 후 스킬 오브젝트 제거
        }
    }
}

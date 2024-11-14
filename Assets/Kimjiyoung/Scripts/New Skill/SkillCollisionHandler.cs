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
        //if (other.CompareTag("Monster")) // ���Ϳ� �浹 ��
        if(other.transform==targetMonster)
        {
            // ������ ���¸� �����ͼ� ������ ����
            MonsterStatus monsterStatus = other.GetComponent<MonsterStatus>();
            if (monsterStatus != null)
            {
                monsterStatus.TakeDamage(damage); // �⺻ ������ ����
                Debug.Log($"Monster hit! {damage} damage dealt.");

                // ������ HPBar ������Ʈ
                MonsterHPbar monsterHPbar = other.GetComponent<MonsterHPbar>();
                if (monsterHPbar != null)
                {
                    monsterHPbar.UpdateHpbar(monsterStatus.currentHp, monsterStatus.monsmaxHp);
                }
            }

            //Destroy(gameObject); // �浹 �� ��ų ������Ʈ ����
        }
    }
}

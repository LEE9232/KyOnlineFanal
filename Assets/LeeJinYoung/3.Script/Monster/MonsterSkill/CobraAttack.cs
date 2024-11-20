using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraAttack : MonoBehaviour
{
    public GameObject attackPrefab;
    public Transform attackPos;
    public float cooldownTime = 2.5f;     // ��ų ��Ÿ��
    private float nextFireTime = 0f;    // ���� ��ų ��� ���� �ð�

    public void CobraSkill()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject projectile = Instantiate(attackPrefab, attackPos.position, attackPos.rotation);
            Destroy(projectile, 3f);
            nextFireTime = Time.time + cooldownTime; // ��Ÿ�� ����
        }
    }
}

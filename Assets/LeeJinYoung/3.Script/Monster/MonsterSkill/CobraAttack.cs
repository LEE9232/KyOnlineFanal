using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraAttack : MonoBehaviour
{
    public GameObject attackPrefab;
    public Transform attackPos;
    public float cooldownTime = 2.5f;     // 스킬 쿨타임
    private float nextFireTime = 0f;    // 다음 스킬 사용 가능 시간

    public void CobraSkill()
    {
        if (Time.time >= nextFireTime)
        {
            GameObject projectile = Instantiate(attackPrefab, attackPos.position, attackPos.rotation);
            Destroy(projectile, 3f);
            nextFireTime = Time.time + cooldownTime; // 쿨타임 설정
        }
    }
}

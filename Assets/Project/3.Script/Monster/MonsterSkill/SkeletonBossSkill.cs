using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossSkill : MonoBehaviour
{
    private float Dash = 10f;
    public BossAI bossAi;
    public BossSkillParticle skillParticle;
    public BossWeaponCollision weaponCollision; // 무기 충돌 스크립트 참조
    public GameObject JumpAttackPos;
    private void Start()
    {
        bossAi = GetComponent<BossAI>();
        skillParticle = GetComponent<BossSkillParticle>();
    }

    private void ExecuteSkill(string animBoolName, float duration)
    {
        bossAi.agent.SetDestination(bossAi.GetTargetPlayerPosition().position);
        bossAi.agent.velocity = Vector3.zero;
        bossAi.anim.SetTrigger(animBoolName);
        StartCoroutine(ResetSkill(duration));
    }
    private IEnumerator ResetSkill(float duration)
    {
        bossAi.agent.isStopped = true;
        yield return new WaitForSeconds(duration);
        bossAi.agent.speed = bossAi.monsterStatus.monsSpeed;
        bossAi.agent.SetDestination(bossAi.GetTargetPlayerPosition().position);
        bossAi.agent.isStopped = false;
    }
    public void JumpDashAttack()
    {
        bossAi.agent.speed = Dash;
        bossAi.anim.SetTrigger("JumpAttack");
        StartCoroutine(ResetSpeedAfterDash());
    }
    public IEnumerator ResetSpeedAfterDash()
    {  
        yield return new WaitForSeconds(1.8f);
        bossAi.agent.updatePosition = false;  
        bossAi.agent.updateRotation = false;  
        bossAi.agent.isStopped = true;
        bossAi.agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.3f);
        Vector3 forwardir = JumpAttackPos.transform.forward;
        Vector3 spawnPos = JumpAttackPos.transform.position;
        skillParticle.CreateParticleEffect(0, spawnPos , 3f, forwardir , 0);
        yield return new WaitForSeconds(1.5f);
        bossAi.agent.speed = bossAi.monsterStatus.monsSpeed;
        bossAi.agent.updatePosition = true;  
        bossAi.agent.updateRotation = true;  
        bossAi.agent.isStopped = false;
    }
    // 두번 공격
    public void FirstSkill()
    {
        weaponCollision.SetDamageCooldown(0.3f);
        ExecuteSkill("Attack1", 3f);
    }

    // 세번 공격
    public void SecondSkill()
    {
        weaponCollision.SetDamageCooldown(0.3f);
        ExecuteSkill("Attack2", 3f);
    }

    // 360도 공격
    public void AreaSwdAttack()
    {
        StartCoroutine(AreaSwdAttackPaticlePlay());
        ExecuteSkill("Attack3", 3f);   
    }
    // 광역 마법 공격
    public void AreaMgAttack()
    {
        //bossAi.agent.isStopped = true;
        //bossAi.agent.updatePosition = false;  
        //bossAi.agent.updateRotation = false;
        Vector3 forwardir = transform.position.normalized;
        skillParticle.CreateParticleEffect(2, transform.position, 6f, forwardir, 2);
        bossAi.agent.SetDestination(bossAi.GetTargetPlayerPosition().position);
        //bossAi.anim.SetTrigger("AreaAttack");
        //StartCoroutine(ResetAreaMgAttack());
    }
    public IEnumerator AreaSwdAttackPaticlePlay()
    {
        yield return new WaitForSeconds(1.0f);
        Vector3 forwardir = transform.position.normalized;
        skillParticle.CreateParticleEffect(1, transform.position, 2f, forwardir ,1);
    }
    public IEnumerator ResetAreaMgAttack()
    {     
        yield return new WaitForSeconds(5.2f);
        // 에이전트 이동 및 회전 업데이트 다시 활성화
        //bossAi.agent.updatePosition = true;
        //bossAi.agent.updateRotation = true;
        //bossAi.agent.isStopped = false;
        bossAi.agent.SetDestination(bossAi.GetTargetPlayerPosition().position);
        //bossAi.agent.speed = bossAi.monsterStatus.monsSpeed;
    }
}

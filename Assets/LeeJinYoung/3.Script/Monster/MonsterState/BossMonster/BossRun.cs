using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRun : IBossState
{
    private bool jumpAttack = false;
    private bool MageAreaAtc = false;
    private Transform playerTrans;
    public void StartState(BossAI b_mons)
    {
        //CancelAllSkills(b_mons);
        b_mons.anim.SetBool("IsRun", true);
        if (b_mons.GetTargetPlayerPosition() != null)
        {
            playerTrans = b_mons.GetTargetPlayerPosition();
            Vector3 direction = (playerTrans.position - b_mons.transform.position).normalized;
            direction.y = 0; // Y축 회전은 무시
            if (direction != Vector3.zero)
            {
                b_mons.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    public void UpdateState(BossAI b_mons)
    {
        b_mons.agent.SetDestination(b_mons.GetTargetPlayerPosition().position);
        if (b_mons.monsterStatus.currentHp < b_mons.monsterStatus.monsmaxHp * 0.5)
        {
            if (MageAreaAtc == false)
            {
                CancelAllSkills(b_mons);
                MageAreaAtc = true;
                //jumpAttack = true;
                b_mons.AreaMagicAttack();
                b_mons.StartCoroutine(MageAttackTime(b_mons));
                //b_mons.agent.isStopped = true;
                //if (b_mons.CheckAttacRange() && MageAreaAtc == false)
                //{
                //    b_mons.ChangeState(new BossAttack());                 
                //}
            }
        }
        float distanceToPlayer = Vector3.Distance(b_mons.transform.position, b_mons.GetTargetPlayerPosition().position);
        float minRange = 10f;
        // 점프 공격 로직
        if (distanceToPlayer > minRange && distanceToPlayer < 60f && !b_mons.isAttacking && !jumpAttack)
        {
            jumpAttack = true;

            b_mons.JumpAttack();
            b_mons.StartCoroutine(JumpAttackTime(b_mons));
        }
        if (b_mons.CheckAttacRange())
        {
            b_mons.ChangeState(new BossAttack());
        }
    }
    public void ExitState(BossAI b_mons)
    {
        b_mons.anim.SetBool("IsRun", false);
        b_mons.agent.ResetPath();
    }

    private IEnumerator JumpAttackTime(BossAI b_mons)
    {
        yield return new WaitForSeconds(13f);
        jumpAttack = false;
    }
    private IEnumerator MageAttackTime(BossAI b_mons)
    {
        //yield return new WaitForSeconds(5.2f);
        //b_mons.agent.isStopped = false;
        //jumpAttack = false;
        yield return new WaitForSeconds(20f);
        MageAreaAtc = false;
    }
    // 스킬 취소 메서드
    private void CancelAllSkills(BossAI b_mons)
    {
        b_mons.anim.ResetTrigger("JumpAttack");
        b_mons.anim.ResetTrigger("Attack1");
        b_mons.anim.ResetTrigger("Attack2");
        b_mons.anim.ResetTrigger("Attack3");
        jumpAttack = false; // 점프 어택 상태 초기화
        b_mons.isAttacking = false; // 공격 상태 초기화
    }


}

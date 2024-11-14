using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : IBossState
{
    private Transform playerTrans;

    public void StartState(BossAI b_mons)
    {
        if (b_mons.GetTargetPlayerPosition() != null)
        {
            playerTrans = b_mons.GetTargetPlayerPosition();
            Vector3 direction = (playerTrans.position - b_mons.transform.position).normalized;
            direction.y = 0; // Y축 회전은 무시
            if (direction != Vector3.zero)
            {
                b_mons.transform.rotation = Quaternion.LookRotation(direction);
            }
            if (b_mons.anim.GetBool("Attack1") || b_mons.anim.GetBool("Attack2") ||
                b_mons.anim.GetBool("Attack3") || b_mons.anim.GetBool("JumpAttack"))
                //|| b_mons.anim.GetBool("AreaAttack"))
            {
                b_mons.agent.isStopped = true;
            }
        }
    }

    public void UpdateState(BossAI b_mons)
    {
        Transform playerTrans = b_mons.GetTargetPlayerPosition();
        if (playerTrans != null)
        {
            if (b_mons.CheckAttacRange())
            {
                if (b_mons.isAttacking == false)
                {
                    int randomAttack = Random.Range(1, 4);
                    switch (randomAttack)
                    {
                        case 1:
                            b_mons.AttackOne();
                            break;
                        case 2:
                            b_mons.AttackTwo();
                            break;
                        case 3:
                            b_mons.AreaSwordAttack();
                            break;
                            //default:
                            //    b_mons.ChangeState(new BossRun());
                            //    break;
                    }
                }
            }
            if (b_mons.anim.GetBool("Attack1") || b_mons.anim.GetBool("Attack2") ||
                b_mons.anim.GetBool("Attack3") || b_mons.anim.GetBool("JumpAttack") ||
                b_mons.anim.GetBool("Attack1") || b_mons.anim.GetBool("Attack1"))
            {
                b_mons.agent.isStopped = true;
            }
            if (!b_mons.CheckAttacRange())
            {
                b_mons.ChangeState(new BossRun());
            }
        }
    }
    public void ExitState(BossAI b_mons)
    {

    }
    private void CancelAllSkills(BossAI b_mons)
    {
        b_mons.anim.ResetTrigger("AreaAttack");
        b_mons.anim.ResetTrigger("Attack1");
        b_mons.anim.ResetTrigger("Attack2");
        b_mons.anim.ResetTrigger("Attack3");
        b_mons.isAttacking = false; // 공격 상태 초기화
    }
}

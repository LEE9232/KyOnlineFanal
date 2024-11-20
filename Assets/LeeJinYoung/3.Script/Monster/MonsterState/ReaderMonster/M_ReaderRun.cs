using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ReaderRun : IReaderState
{
    // 전환대기전 위치
    private Vector3 originPosition;
    public void StartState(ReaderMonsAI r_Monster)
    {
        r_Monster.agent.SetDestination(r_Monster.GetTargetPlayerPosition().position);
        r_Monster.anim.SetBool("IsRuning", true);
        r_Monster.agent.isStopped = false;
        originPosition = r_Monster.transform.position;
        r_Monster.agent.speed = r_Monster.monsterStatus.monsSpeed;
        // 타겟이 있다면 타겟을 추적
    }

    public void UpdateState(ReaderMonsAI r_Monster)
    {
        // 공격 범위에 들어오면 공격 상태로 전환
        if (r_Monster.CheckAttacRange())
        {
            r_Monster.ChangeState(new M_ReaderAttack());
            return;
        }

        // 리더가 설정한 타겟이 있을 경우 그 타겟을 따라감
        if (r_Monster.sharedTarget != null)
        {
            r_Monster.agent.SetDestination(r_Monster.sharedTarget.transform.position);
            if (r_Monster.CheckAttacRange())
            {
                r_Monster.ChangeState(new M_ReaderAttack());
            }
        }
        float distanceToPlayer = Vector3.Distance(r_Monster.transform.position, r_Monster.GetTargetPlayerPosition().
            position);
        float minRange = 10f;
        // 리더일 경우 대쉬 조건 추가: 특정 거리 내에서 점프
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Reader)
        {
            if (distanceToPlayer > minRange && distanceToPlayer < 30f)
            {
                r_Monster.readerJump(); // 대쉬 스킬 실행
            }
        }
        r_Monster.agent.SetDestination(r_Monster.GetTargetPlayerPosition().position);
        if (r_Monster.CheckAttacRange())
        {
            r_Monster.ChangeState(new M_ReaderAttack());
            //return;
        }
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Reader)
        {
            if (distanceToPlayer > minRange && distanceToPlayer < 30f)
            {
                r_Monster.readerJump();
            }
        }
        if (distanceToPlayer > 40f)
        {
            r_Monster.ChangeState(new M_ReaderReTurn(originPosition));
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {
        r_Monster.anim.SetBool("IsRuning", false);
        r_Monster.agent.ResetPath();
    }
}

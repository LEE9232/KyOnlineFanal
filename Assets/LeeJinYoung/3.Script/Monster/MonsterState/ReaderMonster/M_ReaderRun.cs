using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ReaderRun : IReaderState
{
    // ��ȯ����� ��ġ
    private Vector3 originPosition;
    public void StartState(ReaderMonsAI r_Monster)
    {
        r_Monster.agent.SetDestination(r_Monster.GetTargetPlayerPosition().position);
        r_Monster.anim.SetBool("IsRuning", true);
        r_Monster.agent.isStopped = false;
        originPosition = r_Monster.transform.position;
        r_Monster.agent.speed = r_Monster.monsterStatus.monsSpeed;
        // Ÿ���� �ִٸ� Ÿ���� ����
    }

    public void UpdateState(ReaderMonsAI r_Monster)
    {
        // ���� ������ ������ ���� ���·� ��ȯ
        if (r_Monster.CheckAttacRange())
        {
            r_Monster.ChangeState(new M_ReaderAttack());
            return;
        }

        // ������ ������ Ÿ���� ���� ��� �� Ÿ���� ����
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
        // ������ ��� �뽬 ���� �߰�: Ư�� �Ÿ� ������ ����
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Reader)
        {
            if (distanceToPlayer > minRange && distanceToPlayer < 30f)
            {
                r_Monster.readerJump(); // �뽬 ��ų ����
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

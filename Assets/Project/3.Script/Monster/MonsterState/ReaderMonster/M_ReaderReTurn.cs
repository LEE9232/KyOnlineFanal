using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M_ReaderReTurn : IReaderState
{
    private Vector3 originPosition;
    public M_ReaderReTurn(Vector3 originPosition)
    {
        this.originPosition = originPosition;
    }
    public void StartState(ReaderMonsAI r_Monster)
    {
        r_Monster.anim.SetBool("IsWalk", true);
        r_Monster.agent.speed = r_Monster.monsterStatus.monsWalkSpeed;
        r_Monster.agent.SetDestination(originPosition);
    }
    public void UpdateState(ReaderMonsAI r_Monster)
    {
        if (!r_Monster.agent.pathPending && r_Monster.agent.remainingDistance <= r_Monster.agent.stoppingDistance)
        {
            r_Monster.ChangeState(new M_ReaderIdle());
        }
        if (r_Monster.checkPlayer())
        {
            r_Monster.ChangeState(new M_ReaderRun());
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {
        r_Monster.anim.SetBool("IsWalk", false);
    }
}

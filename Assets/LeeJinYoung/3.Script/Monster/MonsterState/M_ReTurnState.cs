using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ReTurnState : IMonsterState
{
    private Vector3 originPosition;
    public M_ReTurnState(Vector3 originPosition)
    { 
        this.originPosition = originPosition;
    }
    public void StartState(MonsterAI monster)
    {
        monster.anim.SetBool("IsWalk", true);
        monster.agent.speed = monster.monsterStatus.monsWalkSpeed;
        monster.agent.SetDestination(originPosition);
    }
    public void UpdateState(MonsterAI monster)
    {
        if (!monster.agent.pathPending && monster.agent.remainingDistance <= monster.agent.stoppingDistance)
        {
            monster.ChangeState(new M_IdleState());
        }
        if (monster.checkPlayer())
        {
            monster.ChangeState(new M_RunState());
        }
    }
    public void ExitState(MonsterAI monster)
    {
        monster.anim.SetBool("IsWalk", false);
    }
}

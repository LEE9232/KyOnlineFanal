using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_RunState : IMonsterState
{
    private Vector3 originPosition;
    private float attackDelay = 2.0f;
    private float attackTimer = 0f;

    public void StartState(MonsterAI monster)
    {
        monster.agent.SetDestination(monster.GetTargetPlayerPosition().position);
        monster.anim.SetBool("IsRuning", true);
        monster.agent.isStopped = false; // 상태가 종료되면 이동을 다시 허용
        originPosition = monster.transform.position;
        monster.agent.speed = monster.monsterStatus.monsSpeed;
    }
    public void UpdateState(MonsterAI monster)
    {
        attackTimer += Time.deltaTime;
        monster.agent.SetDestination(monster.GetTargetPlayerPosition().position);
        if (monster.CheckAttacRange())
        {
            monster.ChangeState(new M_AttackState());
            attackTimer = 0f;
        }
        float distanceToPlayer = Vector3.Distance(monster.transform.position, monster.GetTargetPlayerPosition().position);
        if (distanceToPlayer > 40f)
        {
            monster.ChangeState(new M_ReTurnState(originPosition));
        }
    }
    public void ExitState(MonsterAI monster)
    {
        monster.anim.SetBool("IsRuning", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class M_WalkState : IMonsterState
{
    private bool isWalking = false;
    private Vector3 randomTarget;

    public void StartState(MonsterAI monster)
    {
        monster.anim.SetBool("IsWalk", true);
        monster.agent.speed = monster.monsterStatus.monsWalkSpeed;  // 속도
        randomTarget = RandomPoition(monster);
        monster.agent.SetDestination(randomTarget);
        isWalking = true;
    }
    public void UpdateState(MonsterAI monster)
    {
        if (isWalking && !monster.agent.pathPending && 
            monster.agent.remainingDistance <= monster.agent.stoppingDistance)
        {
            if (!monster.agent.hasPath || monster.agent.velocity.sqrMagnitude == 0f)
            {
                isWalking = false;
                monster.ChangeState(new M_IdleState());
            }
            if (monster.checkPlayer())
            {
                monster.ChangeState(new M_RunState());
            }
            monster.transform.position = Vector3.MoveTowards(
                monster.transform.position,
                randomTarget,
                Time.deltaTime * monster.monsterStatus.monsSpeed
            );
        }
        // 지점에 도착시 아이들로 전환
        if (Vector3.Distance(monster.transform.position, randomTarget) < 1f)
        {
            isWalking = false;
            monster.ChangeState(new M_IdleState());
        }

        if (monster.checkPlayer())
        {
            isWalking = false;
            monster.ChangeState(new M_RunState());
        }
    }
    public void ExitState(MonsterAI monster)
    {
        //Debug.Log("걷기 종료");
        monster.anim.SetBool("IsWalk", false);
        monster.agent.ResetPath();
    }
    private Vector3 RandomPoition(MonsterAI monster)
    {

        float randomDistance = Random.Range(10f, 15f);
        float randomAngle = Random.Range(0f, 360f);
        float radian = randomAngle * Mathf.Deg2Rad;
        float offsetX = randomDistance * Mathf.Cos(radian);
        float offsetZ = randomDistance * Mathf.Sin(radian);
        Vector3 targetPosition = new Vector3(
            monster.transform.position.x + offsetX,
            monster.transform.position.y,
            monster.transform.position.z + offsetZ
            );
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return monster.transform.position;
    }
}

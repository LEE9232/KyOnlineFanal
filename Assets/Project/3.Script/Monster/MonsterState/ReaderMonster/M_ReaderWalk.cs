using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class M_ReaderWalk : IReaderState
{
    private bool isWalking = false;
    private Vector3 randomTarget;
    public void StartState(ReaderMonsAI r_Monster)
    {
        r_Monster.anim.SetBool("IsWalk", true);
        r_Monster.agent.speed = r_Monster.monsterStatus.monsWalkSpeed;  // 속도
        randomTarget = RandomPoition(r_Monster);
        r_Monster.agent.SetDestination(randomTarget);
        isWalking = true;
    }
    public void UpdateState(ReaderMonsAI r_Monster)
    {
        // 플레이어를 발견하면 즉시 Run 상태로 전환
        if (r_Monster.checkPlayer())
        {
            isWalking = false;
            r_Monster.ChangeState(new M_ReaderRun());
            return;
        }

        // 리더와의 거리 체크
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Member && r_Monster.leader != null)
        {
            float distanceToLeader = Vector3.Distance(r_Monster.transform.position, r_Monster.leader.transform.position);

            // 리더와의 거리가 너무 가까워지면 멈춤
            if (distanceToLeader <= 5f) // 예를 들어 2m 이하로 가까워지면
            {
                r_Monster.agent.SetDestination(r_Monster.transform.position); // 멈춤
                isWalking = false;
                r_Monster.anim.SetBool("IsWalk", false);
                r_Monster.ChangeState(new M_ReaderIdle()); // Idle 상태로 전환
                return;
            }
        }
        // 걷는 상태에서 목표 지점에 도착했는지 확인
        if (isWalking && !r_Monster.agent.pathPending && r_Monster.agent.remainingDistance <= r_Monster.agent.stoppingDistance)
        {
            if (!r_Monster.agent.hasPath || r_Monster.agent.velocity.sqrMagnitude == 0f)
            {
                isWalking = false;
                r_Monster.anim.SetBool("IsWalk", false);
                r_Monster.ChangeState(new M_ReaderIdle()); // 목적지 도착 시 Idle 상태로 전환
            }
        }
        // 만약 지점에 도착했다면 상태 전환
        if (Vector3.Distance(r_Monster.transform.position, randomTarget) < 1f)
        {
            isWalking = false;
            r_Monster.ChangeState(new M_ReaderIdle());
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {
        //Debug.Log("걷기 종료");
        r_Monster.anim.SetBool("IsWalk", false);
        r_Monster.agent.ResetPath();
    }

    private Vector3 RandomPoition(ReaderMonsAI r_Monster)
    {

        float randomDistance = Random.Range(10f, 15f);
        float randomAngle = Random.Range(0f, 360f);
        float radian = randomAngle * Mathf.Deg2Rad;
        float offsetX = randomDistance * Mathf.Cos(radian);
        float offsetZ = randomDistance * Mathf.Sin(radian);
        Vector3 targetPosition = new Vector3(
            r_Monster.transform.position.x + offsetX,
            r_Monster.transform.position.y,
            r_Monster.transform.position.z + offsetZ
            );
        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return r_Monster.transform.position;
    }


}

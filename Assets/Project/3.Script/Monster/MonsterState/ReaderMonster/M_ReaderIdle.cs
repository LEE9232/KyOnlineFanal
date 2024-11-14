using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_ReaderIdle : IReaderState
{
    private float chageTime = 5f;
    private float idleTime;
    public void StartState(ReaderMonsAI r_Monster)
    {
        idleTime = 0f;
    }
    public void UpdateState(ReaderMonsAI r_Monster)
    {
        idleTime += Time.deltaTime;
        if (r_Monster.checkPlayer())
        {
            r_Monster.agent.SetDestination(r_Monster.GetTargetPlayerPosition().position);
            r_Monster.ChangeState(new M_ReaderRun());

        }
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Reader)
        {
            if (idleTime >= chageTime)
            {
                r_Monster.ChangeState(new M_ReaderWalk());
            }
        }
        // 멤버 관련
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Member && r_Monster.leader != null)
        {
            float distanceToLeader = Vector3.Distance(r_Monster.transform.position, r_Monster.leader.transform.position);
            // 스프레딩 (Spreading) 기술:
            // 맴버들이 리더를 따라가는 동안 서로 충돌하지 않도록 간격을 두고 이동하도록 합니다.
            Vector3 offset = new Vector3(
            Random.Range(-3f, 3f),
            0f,
            Random.Range(-3f, 3f)
            ).normalized * 3f;
            // Offset 
            Vector3 destination = r_Monster.leader.transform.position + offset;
            if (r_Monster.leader.anim.GetBool("IsRuning"))
            {
                if (r_Monster.checkPlayer())
                {
                    r_Monster.ChangeState(new M_ReaderRun());
                    r_Monster.anim.SetBool("IsWalk", false);
                }
                r_Monster.anim.SetBool("IsRuning", true);
                r_Monster.agent.SetDestination(destination);
                r_Monster.agent.speed = r_Monster.monsterStatus.monsSpeed;
            }
            else //if (distanceToLeader > 15f)
            {
                r_Monster.anim.SetBool("IsRuning", false);
                if (distanceToLeader > 5f)
                {
                    r_Monster.agent.SetDestination(destination);
                    //r_Monster.agent.SetDestination(r_Monster.transform.position); // 멈춤
                    r_Monster.anim.SetBool("IsWalk", true);
                }
                else if (distanceToLeader < 5f)
                {
                    r_Monster.agent.SetDestination(destination);
                    r_Monster.agent.SetDestination(r_Monster.transform.position); // 멈춤
                    r_Monster.anim.SetBool("IsWalk", false);
                }
                if (r_Monster.checkPlayer())
                {
                    r_Monster.ChangeState(new M_ReaderRun());
                    r_Monster.anim.SetBool("IsWalk", false);
                }

            }
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {

        //Debug.Log("아이들 종료");
    }
}

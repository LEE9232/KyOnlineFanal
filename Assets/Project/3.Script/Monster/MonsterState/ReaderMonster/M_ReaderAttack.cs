using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M_ReaderAttack : IReaderState
{
    private Vector3 originPosition;
    private Transform playerTrans;
    private float attackDelay = 1.0f; // 공격 후 잠시 멈추는 시간
    private float attackTimer = 0f;
    private float attackEndTimer = 1.0f;

    public void StartState(ReaderMonsAI r_Monster)
    {
        // 공격 상태로 들어갈 때 플레이어를 바라보도록 설정
        playerTrans = r_Monster.GetTargetPlayerPosition();
        if (playerTrans != null)
        {
            // 플레이어 쪽으로 몬스터를 회전
            Vector3 direction = (playerTrans.position - r_Monster.transform.position).normalized;
            direction.y = 0; // Y축 회전은 무시
            if (direction != Vector3.zero)
            {
                r_Monster.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        r_Monster.agent.isStopped = true; // 공격 중에는 이동을 멈춤
        r_Monster.anim.SetTrigger("IsAttack");
        attackTimer = 0f;
    }

    public void UpdateState(ReaderMonsAI r_Monster)
    {
        playerTrans = r_Monster.GetTargetPlayerPosition();
        attackTimer += Time.deltaTime; // 공격 타이머 업데이트
                                       // 공격용 콜라이더 활성화
        if (r_Monster.attackCollider != null)
        {
            r_Monster.attackCollider.enabled = true;
        }
        // 공격 후 대기 시간 경과 후 상태 전환을 위한 체크
        if (attackTimer >= attackDelay)
        {
            if (playerTrans != null && r_Monster.CheckAttacRange())
            {
                // 공격 범위 내에 플레이어가 있으면 다시 공격
                attackTimer = 0f;
                r_Monster.anim.SetTrigger("IsAttack"); // 재공격
            }
            else
            {
                // 공격 범위를 벗어나면 Run 상태로 전환하여 다시 추적
                r_Monster.agent.isStopped = false;
                r_Monster.ChangeState(new M_ReaderRun());
            }
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {
        attackEndTimer += Time.deltaTime;
        if (attackEndTimer >= attackDelay)
        {
            r_Monster.anim.ResetTrigger("IsAttack");
            r_Monster.agent.isStopped = false; // 상태가 종료되면 이동을 다시 허용
            attackEndTimer = 0f;
        }
    }
}

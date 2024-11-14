using System.Threading;
using UnityEngine;

public class M_AttackState : IMonsterState
{
    private Vector3 originPosition;
    //private Transform playerTrans;
    private Transform targetTrans;
    private float attackDelay = 1.0f; // 공격 후 잠시 멈추는 시간
    private float attackTimer = 0f;
    private float attackEndTimer = 1.0f;
    public void StartState(MonsterAI monster)
    {
        if (monster.GetTargetPlayerPosition() != null)
        {
            Transform targetTrans = monster.GetTargetPlayerPosition();
            //Transform playerTrans = monster.GetTargetPlayerPosition();
            Vector3 direction = (targetTrans.position - monster.transform.position).normalized;
            //Vector3 direction = (playerTrans.position - monster.transform.position).normalized;
            direction.y = 0; // Y축 회전은 무시
            if (direction != Vector3.zero)
            {
                monster.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        monster.agent.isStopped = true;

        //// 공격용 콜라이더 활성화
        //if (monster.attackCollider != null)
        //{
        //    monster.attackCollider.enabled = true;
        //}
        monster.anim.SetTrigger("IsAttack");
        // 코브라인 경우에만 스킬 발사
        if (monster.monsterType == MonsterType.Cobra)
        {
            monster.cobraAttack.CobraSkill();
        }
    }
    public void UpdateState(MonsterAI monster)
    {

        //Transform playerTrans = monster.GetTargetPlayerPosition();
        Transform targetTrans = monster.GetTargetPlayerPosition();
        attackTimer += Time.deltaTime;
        if (targetTrans != null)
        //if (playerTrans != null)
        {
            Vector3 direction = (targetTrans.position - monster.transform.position).normalized;
            //Vector3 direction = (playerTrans.position - monster.transform.position).normalized;
            direction.y = 0; // Y축 회전은 무시
            if (direction != Vector3.zero)
            {
                monster.transform.rotation = Quaternion.LookRotation(direction);
            }
            if (monster.CheckAttacRange())
            {
                monster.ChangeState(new M_AttackState());
                return;
            }
            else if (!monster.CheckAttacRange())
            {
                if (attackDelay <= attackTimer)
                {
                    monster.agent.isStopped = false; // 상태가 종료되면 이동을 다시 허용
                    monster.ChangeState(new M_RunState());
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            // 플레이어가 더 이상 존재하지 않는 경우 돌아가는 상태로 전환
            monster.ChangeState(new M_ReTurnState(originPosition));
            monster.agent.ResetPath();
        }
    }
    public void ExitState(MonsterAI monster)
    {
        attackEndTimer += Time.deltaTime;
        if (attackEndTimer >= attackDelay)
        {
            monster.agent.isStopped = false;
            attackEndTimer = 0f;
        }
        // 공격이 끝났으므로 공격용 콜라이더 비활성화
        //if (monster.attackCollider != null)
        //{
        //    monster.attackCollider.enabled = false;
        //}
    }
}

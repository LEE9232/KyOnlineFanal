using System.Threading;
using UnityEngine;

public class M_AttackState : IMonsterState
{
    private Vector3 originPosition;
    //private Transform playerTrans;
    private Transform targetTrans;
    private float attackDelay = 1.0f; // ���� �� ��� ���ߴ� �ð�
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
            direction.y = 0; // Y�� ȸ���� ����
            if (direction != Vector3.zero)
            {
                monster.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        monster.agent.isStopped = true;

        //// ���ݿ� �ݶ��̴� Ȱ��ȭ
        //if (monster.attackCollider != null)
        //{
        //    monster.attackCollider.enabled = true;
        //}
        monster.anim.SetTrigger("IsAttack");
        // �ں���� ��쿡�� ��ų �߻�
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
            direction.y = 0; // Y�� ȸ���� ����
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
                    monster.agent.isStopped = false; // ���°� ����Ǹ� �̵��� �ٽ� ���
                    monster.ChangeState(new M_RunState());
                    attackTimer = 0f;
                }
            }
        }
        else
        {
            // �÷��̾ �� �̻� �������� �ʴ� ��� ���ư��� ���·� ��ȯ
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
        // ������ �������Ƿ� ���ݿ� �ݶ��̴� ��Ȱ��ȭ
        //if (monster.attackCollider != null)
        //{
        //    monster.attackCollider.enabled = false;
        //}
    }
}

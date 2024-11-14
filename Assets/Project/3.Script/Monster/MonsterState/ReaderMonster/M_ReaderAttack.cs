using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class M_ReaderAttack : IReaderState
{
    private Vector3 originPosition;
    private Transform playerTrans;
    private float attackDelay = 1.0f; // ���� �� ��� ���ߴ� �ð�
    private float attackTimer = 0f;
    private float attackEndTimer = 1.0f;

    public void StartState(ReaderMonsAI r_Monster)
    {
        // ���� ���·� �� �� �÷��̾ �ٶ󺸵��� ����
        playerTrans = r_Monster.GetTargetPlayerPosition();
        if (playerTrans != null)
        {
            // �÷��̾� ������ ���͸� ȸ��
            Vector3 direction = (playerTrans.position - r_Monster.transform.position).normalized;
            direction.y = 0; // Y�� ȸ���� ����
            if (direction != Vector3.zero)
            {
                r_Monster.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        r_Monster.agent.isStopped = true; // ���� �߿��� �̵��� ����
        r_Monster.anim.SetTrigger("IsAttack");
        attackTimer = 0f;
    }

    public void UpdateState(ReaderMonsAI r_Monster)
    {
        playerTrans = r_Monster.GetTargetPlayerPosition();
        attackTimer += Time.deltaTime; // ���� Ÿ�̸� ������Ʈ
                                       // ���ݿ� �ݶ��̴� Ȱ��ȭ
        if (r_Monster.attackCollider != null)
        {
            r_Monster.attackCollider.enabled = true;
        }
        // ���� �� ��� �ð� ��� �� ���� ��ȯ�� ���� üũ
        if (attackTimer >= attackDelay)
        {
            if (playerTrans != null && r_Monster.CheckAttacRange())
            {
                // ���� ���� ���� �÷��̾ ������ �ٽ� ����
                attackTimer = 0f;
                r_Monster.anim.SetTrigger("IsAttack"); // �����
            }
            else
            {
                // ���� ������ ����� Run ���·� ��ȯ�Ͽ� �ٽ� ����
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
            r_Monster.agent.isStopped = false; // ���°� ����Ǹ� �̵��� �ٽ� ���
            attackEndTimer = 0f;
        }
    }
}

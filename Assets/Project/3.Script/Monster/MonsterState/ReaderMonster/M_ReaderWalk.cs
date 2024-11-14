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
        r_Monster.agent.speed = r_Monster.monsterStatus.monsWalkSpeed;  // �ӵ�
        randomTarget = RandomPoition(r_Monster);
        r_Monster.agent.SetDestination(randomTarget);
        isWalking = true;
    }
    public void UpdateState(ReaderMonsAI r_Monster)
    {
        // �÷��̾ �߰��ϸ� ��� Run ���·� ��ȯ
        if (r_Monster.checkPlayer())
        {
            isWalking = false;
            r_Monster.ChangeState(new M_ReaderRun());
            return;
        }

        // �������� �Ÿ� üũ
        if (r_Monster.monsterGroup == ReaderMonsAI.MonsterGroup.Member && r_Monster.leader != null)
        {
            float distanceToLeader = Vector3.Distance(r_Monster.transform.position, r_Monster.leader.transform.position);

            // �������� �Ÿ��� �ʹ� ��������� ����
            if (distanceToLeader <= 5f) // ���� ��� 2m ���Ϸ� ���������
            {
                r_Monster.agent.SetDestination(r_Monster.transform.position); // ����
                isWalking = false;
                r_Monster.anim.SetBool("IsWalk", false);
                r_Monster.ChangeState(new M_ReaderIdle()); // Idle ���·� ��ȯ
                return;
            }
        }
        // �ȴ� ���¿��� ��ǥ ������ �����ߴ��� Ȯ��
        if (isWalking && !r_Monster.agent.pathPending && r_Monster.agent.remainingDistance <= r_Monster.agent.stoppingDistance)
        {
            if (!r_Monster.agent.hasPath || r_Monster.agent.velocity.sqrMagnitude == 0f)
            {
                isWalking = false;
                r_Monster.anim.SetBool("IsWalk", false);
                r_Monster.ChangeState(new M_ReaderIdle()); // ������ ���� �� Idle ���·� ��ȯ
            }
        }
        // ���� ������ �����ߴٸ� ���� ��ȯ
        if (Vector3.Distance(r_Monster.transform.position, randomTarget) < 1f)
        {
            isWalking = false;
            r_Monster.ChangeState(new M_ReaderIdle());
        }
    }
    public void ExitState(ReaderMonsAI r_Monster)
    {
        //Debug.Log("�ȱ� ����");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolGuard : MonoBehaviour
{
    public Transform[] patrolPoints; // ����� ������ ����Ʈ��
    private int currentPatrolIndex = 0; // ���� ���� ���� ����Ʈ �ε���
    private NavMeshAgent agent; // NavMeshAgent ������Ʈ
    public Animator Anim;
    public float waitTime = 2f; // �� ���� ����Ʈ���� ����ϴ� �ð�
    private float waitTimer = 0f; // ��� Ÿ�̸�
    private bool waiting = false; // ����� ���� ��� ������ ����
    private MinimapIconManager minimapIconManager;  // MinimapIconManager ����
    private void Start()
    {
        Anim.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position); // ù ��° ���� ����Ʈ�� �̵�
        }
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCNoQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }
    }

    private void Update()
    {
        // ����� ���� ����Ʈ�� �����ߴ��� Ȯ��
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // ��� Ÿ�̸Ӹ� Ȱ��ȭ�ϰ� ���� ���� ����Ʈ�� �̵��ϱ� ���� ��� ���
            if (!waiting)
            {
                waiting = true;
                Anim.SetBool("Walking" , false);
                waitTimer = waitTime;
            }

            // ��� Ÿ�̸Ӱ� ������ ���� ���� ����Ʈ�� �̵�
            if (waiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    GoToNextPatrolPoint();
                    Anim.SetBool("Walking", true);
                    waiting = false; // ��Ⱑ ������ �ٽ� �̵�
                }
            }
        }
    }
    public void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // ���� ����Ʈ �迭���� ���� ����Ʈ�� �̵�
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
}

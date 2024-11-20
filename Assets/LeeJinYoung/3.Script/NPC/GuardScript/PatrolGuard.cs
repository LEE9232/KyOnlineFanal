using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolGuard : MonoBehaviour
{
    public Transform[] patrolPoints; // 경비병이 순찰할 포인트들
    private int currentPatrolIndex = 0; // 현재 순찰 중인 포인트 인덱스
    private NavMeshAgent agent; // NavMeshAgent 컴포넌트
    public Animator Anim;
    public float waitTime = 2f; // 각 순찰 포인트에서 대기하는 시간
    private float waitTimer = 0f; // 대기 타이머
    private bool waiting = false; // 경비병이 현재 대기 중인지 여부
    private MinimapIconManager minimapIconManager;  // MinimapIconManager 참조
    private void Start()
    {
        Anim.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position); // 첫 번째 순찰 포인트로 이동
        }
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCNoQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager 가 널입니다.");
        }
    }

    private void Update()
    {
        // 경비병이 순찰 포인트에 도착했는지 확인
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // 대기 타이머를 활성화하고 다음 순찰 포인트로 이동하기 전에 잠시 대기
            if (!waiting)
            {
                waiting = true;
                Anim.SetBool("Walking" , false);
                waitTimer = waitTime;
            }

            // 대기 타이머가 끝나면 다음 순찰 포인트로 이동
            if (waiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    GoToNextPatrolPoint();
                    Anim.SetBool("Walking", true);
                    waiting = false; // 대기가 끝나면 다시 이동
                }
            }
        }
    }
    public void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        // 순찰 포인트 배열에서 다음 포인트로 이동
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }
}

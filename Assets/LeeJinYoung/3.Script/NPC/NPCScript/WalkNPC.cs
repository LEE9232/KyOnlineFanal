using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkNPC : MonoBehaviour
{
    public Transform[] waypoints; // 이동할 경로의 포인트들
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    public Animator anim;
    private MinimapIconManager minimapIconManager;  // MinimapIconManager 참조
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        MoveToNextWaypoint(); // 첫 번째 경로 포인트로 이동
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
        // 현재 경로 포인트에 도착하면 다음 포인트로 이동
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
            anim.SetBool("Walk", true);
        }
    }

    private void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        anim.SetBool("Walk", true);
        agent.SetDestination(waypoints[currentWaypointIndex].position); // 다음 포인트로 이동
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // 경로 반복
    }
}

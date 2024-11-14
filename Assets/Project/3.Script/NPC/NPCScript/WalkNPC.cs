using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkNPC : MonoBehaviour
{
    public Transform[] waypoints; // �̵��� ����� ����Ʈ��
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;
    public Animator anim;
    private MinimapIconManager minimapIconManager;  // MinimapIconManager ����
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        MoveToNextWaypoint(); // ù ��° ��� ����Ʈ�� �̵�
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
        // ���� ��� ����Ʈ�� �����ϸ� ���� ����Ʈ�� �̵�
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
        agent.SetDestination(waypoints[currentWaypointIndex].position); // ���� ����Ʈ�� �̵�
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length; // ��� �ݺ�
    }
}

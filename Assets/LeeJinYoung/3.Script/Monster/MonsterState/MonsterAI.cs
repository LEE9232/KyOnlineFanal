using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterType
{
    Cobra,
    OtherMonster // �ٸ� ���� Ÿ�� �߰�
}

public class MonsterAI : MonoBehaviour
{
    public Animator anim;
    private IMonsterState currentState;
    public Collider attackCollider;  // ���ݿ� ���� �ݶ��̴�
    // �÷��̾� �Ÿ� ����
    //public float playerCheckRange;
    //public float playerAttackRange;
    // �÷��̾� �� ��� �Ÿ� ����
    public float targetCheckRange;
    public float targetAttackRange;
    public Transform currentTarget { get; set; } // ���� Ÿ�� (�÷��̾ ���)
    private GateGuard gateGuard; // ��� ��ü ����
    public MonsterType monsterType;
    public CobraAttack cobraAttack;
    public NavMeshAgent agent;
    // �÷��̾�� ���� ��ġ ����
    public Transform MonsterPos;
    //private Transform playerPos { get; set; }
    public MonsterStatus monsterStatus;
    public List<GameObject> players; // �÷��̾� ��ü ����
    public float attackDelay = 1.0f; // ���� �� ��� ���ߴ� �ð�
    public float attackTimer = 0f;   // ���� ��Ÿ��
    public bool canAttack = true;    // ���� ���� ����


    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        agent = GetComponent<NavMeshAgent>();
        if (monsterType == MonsterType.Cobra)
        {
            cobraAttack = GetComponent<CobraAttack>();
        }
        // �ʱ� ���� ����
        ChangeState(new M_IdleState());
    }
    private void Update()
    {
        if (currentState != null)
        {
            if (monsterStatus.currentHp <= 0)
            {
                agent.isStopped = true;
                anim.SetBool("Die", true);
                monsterStatus.Die(currentTarget.gameObject);
            }
            else
            {
                currentState.UpdateState(this);
            }
        }
        // ���� Ÿ�̸� ������Ʈ
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                canAttack = true;
                attackTimer = 0f;  // Ÿ�̸� ����
            }
        }
    }
    public void ChangeState(IMonsterState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.StartState(this);
    }
    public bool checkPlayer()
    {
        players = PlayerManager.Instance.GetPlayers();
        foreach (GameObject player in players)
        {
            if (player != null && player.transform != null) // �߰��� null üũ
            {
                float distance = Vector3.Distance(player.transform.position, MonsterPos.position);
                if (distance <= targetCheckRange)
                {
                    currentTarget = player.transform;
                    //playerPos = player.transform;
                    return true;
                }
            }
        }
        // 2. ��� ����
        Collider[] guards = Physics.OverlapSphere(transform.position, targetCheckRange, LayerMask.GetMask("Guard"));
        if (guards.Length > 0)
        {
            gateGuard = guards[0].GetComponent<GateGuard>();
            if (gateGuard != null)
            {
                currentTarget = gateGuard.transform;
                return true;
            }
        }

        // �÷��̾ ã�� ���� ���
        currentTarget = null;
        //playerPos = null;
        return false;
    }
    public bool CheckAttacRange()
    {
        //if (playerPos == null)
        //{
        //    return false;
        //}
        if (currentTarget == null)
        {
            return false;
        }
        //float distance = Vector3.Distance(playerPos.position, MonsterPos.position);
        float distance = Vector3.Distance(currentTarget.position, MonsterPos.position);
        //return distance <= playerAttackRange;
        return distance <= targetAttackRange;
    }
    public Transform GetTargetPlayerPosition()
    {
        return currentTarget;
        //return playerPos;
    }
    public void ResetTargetPlayer()
    {

        currentTarget = null;
        //playerPos = null;
    }
    // �ݶ��̴��� �÷��̾�� �浹�� �� ȣ��Ǵ� �޼���
    private void OnTriggerEnter(Collider other)
    {
        if (canAttack && attackCollider != null) // ���� ������ ����
        {
            if (other.CompareTag("Player"))  // �÷��̾�� �浹 Ȯ��
            {
                PlayerManagement player = other.GetComponent<PlayerManagement>();
                if (player != null)
                {
                    // �÷��̾�� �������� ��
                    player.TakeDamage(monsterStatus.monsDamage);
                    Debug.Log($"�÷��̾ {monsterStatus.monsDamage} ��ŭ�� �������� �޾ҽ��ϴ�.");
                    canAttack = false;  // ���� ��Ÿ�� ����
                }
            }
        }
    }
}

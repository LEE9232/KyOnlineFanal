using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReaderMonsAI : MonoBehaviour
{
    public enum MonsterGroup { Reader, Member }  // ������ �ɹ�
    public MonsterGroup monsterGroup;  // �׷�ȭ
    public Animator anim;
    private IReaderState currentState;
    // �÷��̾� �Ÿ� ����
    public float playerCheckRange;
    public float playerAttackRange;
    public NavMeshAgent agent;
    // �÷��̾�� ���� ��ġ ����
    public Transform MonsterPos; // ���� ���� ��ġ
    private Transform playerPos { get; set; } // �÷��̾� ��ġ
    public GameObject sharedTarget;
    public MonsterStatus monsterStatus;
    public List<GameObject> players; // �÷��̾� ��ü ����
    private TrollReaderSkill readerSkill;
    public ReaderMonsAI leader;
    public Collider attackCollider;  // ���� �ݶ��̴�
    public float attackDelay = 1.0f; // ���� �� ��� ���ߴ� �ð�
    public float attackTimer = 0f;   // ���� ��Ÿ��
    public bool canAttack = true;    // ���� ���� ����
    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        agent = GetComponent<NavMeshAgent>();
        readerSkill = GetComponent<TrollReaderSkill>();


        // �ʱ� ���� ����
        ChangeState(new M_ReaderIdle());
    }
    private void Start()
    {
        // ������ ������ ������ ã�� �Ҵ�
        if (monsterGroup == MonsterGroup.Member && leader == null)
        {
            // ������ ã�� �Ҵ� (���� ���� �׷� ������ ������ ã��)
            ReaderMonsAI[] potentialLeaders = FindObjectsOfType<ReaderMonsAI>();
            foreach (var potentialLeader in potentialLeaders)
            {
                if (potentialLeader.monsterGroup == MonsterGroup.Reader)
                {
                    leader = potentialLeader;
                    break;
                }
            }
        }
        // ���ݿ� �ݶ��̴��� ��Ȱ��ȭ ���·� ���� (������ ���� Ȱ��ȭ)
        if (attackCollider != null)
        {
            attackCollider.enabled = false;
        }
    }
    private void Update()
    {
        if (currentState != null)
        {
            if (monsterStatus.currentHp <= 0)
            {
                agent.isStopped = true;
                anim.SetBool("Die", true);
                monsterStatus.Die(playerPos.gameObject);
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
    public void ChangeState(IReaderState newState)
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
        // �÷��̾� �Ŵ��� ��ȿ�� �˻�
        //if (PlayerManager.Instance == null)
        //{
        //    Debug.LogError("PlayerManager.Instance is null.");
        //    return false;
        //}
        //if (playerPos == null || players.Count == 0)
        //{
        //    Debug.LogWarning("No players found in PlayerManager.");
        //    return false;
        //}
        players = PlayerManager.Instance.GetPlayers();
        foreach (GameObject player in players)
        {
            if (player != null && player.transform != null) // �߰��� null üũ
            {
                float distance = Vector3.Distance(player.transform.position, MonsterPos.position);
                //playerPos.position
                if (distance <= playerCheckRange)
                {
                    playerPos = player.transform;
                    return true;
                }
            }
        }
        // �÷��̾ ã�� ���� ���
        playerPos = null;
        return false;
    }
    public bool CheckAttacRange()
    {
        // �÷��̾ �����Ǿ� �ִ��� Ȯ��
        if (playerPos == null)
        {
            return false;
        }
        float distance = Vector3.Distance(playerPos.position, MonsterPos.position);
        return distance <= playerAttackRange;
    }
    public Transform GetTargetPlayerPosition()
    {
        return playerPos;
    }
    public void ResetTargetPlayer()
    {
        playerPos = null;
    }

    public void readerJump()
    {
        if (readerSkill.IsJumpDash == false)
        {
            readerSkill.ReaderJumpDash();
        }
    }

    public void SetTarget(GameObject target)
    {
        if (monsterGroup == MonsterGroup.Reader) // ������ ���
        {
            sharedTarget = target; // ������ Ÿ���� ����
            ShareTargetWithMembers();
        }
    }
    //�ɹ����� ������ Ÿ���� ��������
    private void ShareTargetWithMembers()
    {
        if (monsterGroup == MonsterGroup.Reader)
        {
            ReaderMonsAI[] members = FindObjectsOfType<ReaderMonsAI>();
            foreach (ReaderMonsAI member in members)
            {
                if (member.monsterGroup == MonsterGroup.Member && member.leader == this)
                {
                    member.sharedTarget = sharedTarget;
                }
            }
        }
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

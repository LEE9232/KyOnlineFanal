using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Animator anim;
    private IBossState currentState;
    public float playerCheckRange;
    public float playerAttackRange;
    public NavMeshAgent agent;
    private Transform playerPos { get; set; }
    public Transform monsPos;
    public MonsterStatus monsterStatus;
    public List<GameObject> players;
    public bool isAttacking = false;
    public bool isAreaAttacking = false;

    // ��Ÿ�� ���� ����
    private float attack1Cooldown = 5f;
    private float attack2Cooldown = 6f;
    private float areaSwordCooldown = 5f;
    private float jumpAttackCooldown = 8f;
    private float areaMagicCooldown = 15f;
    // ��Ÿ�� �ʱ�ȭ ����
    private float lastAttack1Time = 0;
    private float lastAttack2Time = 0;
    private float lastAreaSwordTime = 0;
    private float lastJumpAttackTime = 0;
    private float lastAreaMagicTime = 0;

    //��ų�� ��������
    private SkeletonBossSkill bossSkill;

    // �׽�Ʈ ����
    public int testdamage = 0;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        agent = GetComponent<NavMeshAgent>();
        bossSkill = GetComponent<SkeletonBossSkill>();
        ChangeState(new BossStartIdle());
        checkPlayer();
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
    }
    public void ChangeState(IBossState newState)
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
            if (player != null && player.transform != null)
            {
                float distance = Vector3.Distance(player.transform.position, monsPos.position);
                if (distance <= playerCheckRange)
                {
                    playerPos = player.transform;
                    return true;
                }
            }
        }
        playerPos = null;
        return false;
    }
    public bool CheckAttacRange()
    {
        if (playerPos == null)
        {
            return false;
        }
        float distance = Vector3.Distance(playerPos.position, monsPos.position);
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
    // ���� ����
    public void JumpAttack()
    {
        if (!isAttacking && Time.time - lastJumpAttackTime > jumpAttackCooldown)
        {
            isAttacking = true; 
            lastJumpAttackTime = Time.time; // ������ ���� ���� �ð� ����
            bossSkill.JumpDashAttack();
            StartCoroutine(ResetAttackState(3.3f));
        }
    }
    // 360�� �ֵθ���
    public void AreaSwordAttack()
    {
        if (!isAttacking && Time.time - lastAreaSwordTime > areaSwordCooldown)
        {
            isAttacking = true;
            lastAreaSwordTime = Time.time;  
            bossSkill.AreaSwdAttack();
            StartCoroutine(ResetAttackState(3.3f));
        }
    }

    // ü�� 50�ۺ��� ���� ��ų
    // ���� ���� ����
    public void AreaMagicAttack()
    {
        if (!isAreaAttacking && Time.time - lastAreaMagicTime > areaMagicCooldown)
        {
            isAreaAttacking = true;
            lastAreaMagicTime = Time.time;
            bossSkill.AreaMgAttack();
            StartCoroutine(ResetAreaAttack(20f));
        }
    }
    private IEnumerator ResetAreaAttack(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAreaAttacking = false;
    }

    // �ι� �ֵθ���
    public void AttackOne()
    {
        if (!isAttacking && Time.time - lastAttack1Time > attack1Cooldown)
        {

            isAttacking = true;
            lastAttack1Time = Time.time;
            bossSkill.FirstSkill();
            StartCoroutine(ResetAttackState(3.3f));

        }
    }
    // ���� �ֵθ���
    public void AttackTwo()
    {
        if (!isAttacking && Time.time - lastAttack2Time > attack2Cooldown)
        {
            isAttacking = true;
            lastAttack2Time = Time.time;    
            bossSkill.SecondSkill();
            StartCoroutine(ResetAttackState(3.3f));
        }
    }
    private IEnumerator ResetAttackState(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAttacking = false;
    }
}

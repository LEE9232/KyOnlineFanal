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

    // 쿨타임 관리 변수
    private float attack1Cooldown = 5f;
    private float attack2Cooldown = 6f;
    private float areaSwordCooldown = 5f;
    private float jumpAttackCooldown = 8f;
    private float areaMagicCooldown = 15f;
    // 쿨타임 초기화 변수
    private float lastAttack1Time = 0;
    private float lastAttack2Time = 0;
    private float lastAreaSwordTime = 0;
    private float lastJumpAttackTime = 0;
    private float lastAreaMagicTime = 0;

    //스킬들 가져오기
    private SkeletonBossSkill bossSkill;

    // 테스트 변수
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
    // 점프 공격
    public void JumpAttack()
    {
        if (!isAttacking && Time.time - lastJumpAttackTime > jumpAttackCooldown)
        {
            isAttacking = true; 
            lastJumpAttackTime = Time.time; // 마지막 점프 공격 시간 갱신
            bossSkill.JumpDashAttack();
            StartCoroutine(ResetAttackState(3.3f));
        }
    }
    // 360도 휘두르기
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

    // 체력 50퍼부터 사용될 스킬
    // 광역 마법 공격
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

    // 두번 휘두르기
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
    // 세번 휘두르기
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

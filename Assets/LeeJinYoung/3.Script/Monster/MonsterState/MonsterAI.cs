using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterType
{
    Cobra,
    OtherMonster // 다른 몬스터 타입 추가
}

public class MonsterAI : MonoBehaviour
{
    public Animator anim;
    private IMonsterState currentState;
    public Collider attackCollider;  // 공격에 사용될 콜라이더
    // 플레이어 거리 변수
    //public float playerCheckRange;
    //public float playerAttackRange;
    // 플레이어 및 경비병 거리 변수
    public float targetCheckRange;
    public float targetAttackRange;
    public Transform currentTarget { get; set; } // 현재 타겟 (플레이어나 경비병)
    private GateGuard gateGuard; // 경비병 객체 저장
    public MonsterType monsterType;
    public CobraAttack cobraAttack;
    public NavMeshAgent agent;
    // 플레이어와 몬스터 위치 변수
    public Transform MonsterPos;
    //private Transform playerPos { get; set; }
    public MonsterStatus monsterStatus;
    public List<GameObject> players; // 플레이어 객체 저장
    public float attackDelay = 1.0f; // 공격 후 잠시 멈추는 시간
    public float attackTimer = 0f;   // 공격 쿨타임
    public bool canAttack = true;    // 공격 가능 여부


    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        agent = GetComponent<NavMeshAgent>();
        if (monsterType == MonsterType.Cobra)
        {
            cobraAttack = GetComponent<CobraAttack>();
        }
        // 초기 상태 설정
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
        // 공격 타이머 업데이트
        if (!canAttack)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackDelay)
            {
                canAttack = true;
                attackTimer = 0f;  // 타이머 리셋
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
            if (player != null && player.transform != null) // 추가된 null 체크
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
        // 2. 경비병 감지
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

        // 플레이어를 찾지 못한 경우
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
    // 콜라이더가 플레이어와 충돌할 때 호출되는 메서드
    private void OnTriggerEnter(Collider other)
    {
        if (canAttack && attackCollider != null) // 공격 가능할 때만
        {
            if (other.CompareTag("Player"))  // 플레이어와 충돌 확인
            {
                PlayerManagement player = other.GetComponent<PlayerManagement>();
                if (player != null)
                {
                    // 플레이어에게 데미지를 줌
                    player.TakeDamage(monsterStatus.monsDamage);
                    Debug.Log($"플레이어가 {monsterStatus.monsDamage} 만큼의 데미지를 받았습니다.");
                    canAttack = false;  // 공격 쿨타임 설정
                }
            }
        }
    }
}

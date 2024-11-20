using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReaderMonsAI : MonoBehaviour
{
    public enum MonsterGroup { Reader, Member }  // 리더와 맴버
    public MonsterGroup monsterGroup;  // 그룹화
    public Animator anim;
    private IReaderState currentState;
    // 플레이어 거리 변수
    public float playerCheckRange;
    public float playerAttackRange;
    public NavMeshAgent agent;
    // 플레이어와 몬스터 위치 변수
    public Transform MonsterPos; // 몬스터 본인 위치
    private Transform playerPos { get; set; } // 플레이어 위치
    public GameObject sharedTarget;
    public MonsterStatus monsterStatus;
    public List<GameObject> players; // 플레이어 객체 저장
    private TrollReaderSkill readerSkill;
    public ReaderMonsAI leader;
    public Collider attackCollider;  // 공격 콜라이더
    public float attackDelay = 1.0f; // 공격 후 잠시 멈추는 시간
    public float attackTimer = 0f;   // 공격 쿨타임
    public bool canAttack = true;    // 공격 가능 여부
    private void Awake()
    {
        anim = GetComponent<Animator>();
        monsterStatus = GetComponent<MonsterStatus>();
        agent = GetComponent<NavMeshAgent>();
        readerSkill = GetComponent<TrollReaderSkill>();


        // 초기 상태 설정
        ChangeState(new M_ReaderIdle());
    }
    private void Start()
    {
        // 리더가 없으면 리더를 찾아 할당
        if (monsterGroup == MonsterGroup.Member && leader == null)
        {
            // 리더를 찾아 할당 (같은 몬스터 그룹 내에서 리더를 찾음)
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
        // 공격용 콜라이더는 비활성화 상태로 시작 (공격할 때만 활성화)
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
        // 플레이어 매니저 유효성 검사
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
            if (player != null && player.transform != null) // 추가된 null 체크
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
        // 플레이어를 찾지 못한 경우
        playerPos = null;
        return false;
    }
    public bool CheckAttacRange()
    {
        // 플레이어가 설정되어 있는지 확인
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
        if (monsterGroup == MonsterGroup.Reader) // 리더일 경우
        {
            sharedTarget = target; // 리더가 타겟을 설정
            ShareTargetWithMembers();
        }
    }
    //맴버들이 리더의 타겟을 공유받음
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

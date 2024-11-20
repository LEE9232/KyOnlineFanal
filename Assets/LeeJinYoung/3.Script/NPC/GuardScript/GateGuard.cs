using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GateGuard : MonoBehaviour
{
    public Animator Anim;
    public Transform guardPosition; // 경비병이 지킬 위치
    public float detectionRange = 10f; // 경비병이 반응할 범위
    public float attackRange = 3f; // 경비병이 공격할 범위
    public float attackInterval = 1.5f; // 공격 간격
    public int guardDamage = 200; // 경비병의 공격력
    private NavMeshAgent agent;
    private GameObject currentTarget; // 현재 추적 중인 몬스터
    private float attackCooldown = 0f; // 공격 대기 시간
    private bool isReturningToGuard = false; // 경비병이 원래 위치로 복귀 중인지 여부
    private MinimapIconManager minimapIconManager;  // MinimapIconManager 참조
    public TextMeshProUGUI popupText;
    private List<string> popchat = new List<string>();
    public GameObject popUI;
    private bool hasDisplayedPopup = false; // 팝업이 표시되었는지 여부

    private void Start()
    {
        Anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ReturnToGuardPosition(); // 처음에는 지정된 위치로 이동
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCNoQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager 가 널입니다.");
        }

        // 팝업
        popchat.Add("몬스터가 공격해왔다!!!\n모두 공격해라!");
        popchat.Add("마을을 지켜라!!!\n몬스터를 막아라!!");
        popchat.Add("여기서에서 막아야한다!!\n모두를 지켜라!!");
        //popchat.Add("원래 자리로 복귀 한다!!");
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            // 감지 범위 내 몬스터 탐색
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Monster"));
            if (hits.Length > 0)
            {
                currentTarget = hits[0].gameObject; // 첫 번째로 감지된 몬스터 추적
                agent.SetDestination(currentTarget.transform.position);
                Anim.SetBool("Running", true);
                if (!hasDisplayedPopup) // 팝업이 표시되지 않았다면
                {
                    popUI.SetActive(true);
                    int randomIndex = Random.Range(0, popchat.Count);
                    popupText.text = popchat[randomIndex]; // 랜덤 텍스트 출력
                    hasDisplayedPopup = true; // 팝업이 한 번만 표시되도록 설정
                }
            }
            else if (!isReturningToGuard)
            {
                // 몬스터가 없으면 원래 위치로 복귀
                ReturnToGuardPosition();
                Anim.SetBool("Walking", true);
            }
        }
        else
        {
            // 경비병이 몬스터를 추적 중일 때
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
            MonsterStatus monsterStatus = currentTarget.GetComponent<MonsterStatus>();

            if (distanceToTarget <= attackRange)
            {
                AttackTarget();
            }
            else
            {
                // 추적 범위에서 벗어나지 않았다면 계속 추적
                agent.SetDestination(currentTarget.transform.position);
                Anim.SetBool("Walking", false);
                Anim.SetBool("Running", true);
                Anim.ResetTrigger("Attacking2");


            }
            if (monsterStatus != null && monsterStatus.currentHp <= 0)
            {
                currentTarget = null; // 몬스터가 죽었으면 추적 중지
                Anim.SetBool("Walking", true);     
                Anim.SetBool("Running", false);
                Anim.ResetTrigger("Attacking2");
                ReturnToGuardPosition(); // 원래 위치로 복귀
                popupText.text = "원래 자리로 복귀 한다!!";

                //Debug.Log("디버그 2");
            }
        }
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        // 원래 자리 도착 여부 확인
        if (isReturningToGuard && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isReturningToGuard = false;
                // 경비병이 원래 자리에서 멈추게 하기 위해 NavMeshAgent 비활성화
                agent.isStopped = true;
                agent.ResetPath(); // 경로 초기화
                Anim.ResetTrigger("Attacking2");
                Anim.SetBool("Running", false);
                Anim.SetBool("Walking", false);
                popupText.text = ""; // 랜덤 텍스트 출력
                popUI.SetActive(false);
                hasDisplayedPopup = false; // 다음 몬스터를 위해 팝업 다시 표시 가능하게 설정
            }
        }
    }

    private void AttackTarget()
    {
        if (attackCooldown <= 0f && currentTarget != null)
        {
            //Debug.Log("경비병: 몬스터를 공격합니다!");
            Anim.SetTrigger("Attacking2");
            Anim.SetBool("Running", false);
            Anim.SetBool("Walking", false);
            // 공격 로직 추가
            MonsterStatus monsterStatus = currentTarget.GetComponent<MonsterStatus>();
            if (monsterStatus != null)
            {
                monsterStatus.TakeDamage(guardDamage); // 몬스터에게 데미지 적용
            }
            attackCooldown = attackInterval; // 다음 공격까지의 대기 시간 설정
        }
    }

    private void ReturnToGuardPosition()
    {
        Anim.SetBool("Walking", true);
        Anim.SetBool("Running", false);
        isReturningToGuard = true;
        agent.isStopped = false; // NavMeshAgent 다시 활성화
        agent.SetDestination(guardPosition.position);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //
    //    if (other.CompareTag("Monster"))
    //    {
    //        currentTarget = other.gameObject;
    //        agent.isStopped = false; // 경비병이 몬스터를 추적할 수 있도록 NavMeshAgent 활성화
    //        agent.SetDestination(currentTarget.transform.position); // 몬스터 추적 시작
    //        isReturningToGuard = false;
    //    }
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Monster") && currentTarget == other.gameObject)
    //    {
    //        popUI.SetActive(true);
    //        currentTarget = null; // 몬스터가 범위를 벗어나면 추적 중지
    //        ReturnToGuardPosition(); // 원래 위치로 복귀
    //    }
    //}
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance { get; private set; }
    private List<Transform> monsters = new List<Transform>();
    public MonsterHPbar hpBar;  // HP바 관리 스크립트
    public GameObject PlayerRay;
    public Image aim;
    public Transform targetMonster;  //  몬스터 자체
    public Transform targetPoition;  // 에임 위치

    public float targetRadius = 1f;
    public float sphereCastRadius = 5f;

    public Color rayColor = Color.red;
    public Color sphereColor = Color.green;

    private void Awake()
    {
        AimScript AimUI = FindObjectOfType<AimScript>();
        aim = AimUI.Aim;
        Instance = this;
        if (PlayerRay == null)
        {
            PlayerRay = GameObject.FindWithTag("Player");  // "Player" 태그를 가진 오브젝트를 찾아서 할당
            if (PlayerRay == null)
            {
                Debug.LogError("PlayerRay가 할당되지 않았습니다. 'Player' 태그를 확인하세요.");
            }

        }
        if(aim == null)
        {
            GameObject aimObj = GameObject.FindWithTag("AimUI");
            if(aimObj != null )
            { 
                aim = aimObj.GetComponentInChildren<Image>();    
            }
            if (aim == null)
            {
                Debug.LogError("Aim 할당 안됌");
            }
        }
    }

    private void Start()
    {
        aim.gameObject.SetActive(false);
    }


    public float playerTargetRange;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
                if (monster != null)
                {
                    //targetMonster = monster.transform;
                    //hpBar.SetTargetmonster(monster); // 클릭된 몬스터를 타겟팅
                    //aim.gameObject.SetActive(true);
                    SetTarget(monster);
                }
                else
                {
                    ClearTarget();
                }
            }
            else
            {
                ClearTarget();
            }
        }
        //UpdateTarget();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //aim.gameObject.SetActive(false);
            //hpBar.hpBarUI.SetActive(false);
            //targetMonster = null;
            ClearTarget();
        }

        if (targetMonster != null && targetPoition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPoition.position);
            aim.transform.position = screenPosition;
        }
        else
        {
            aim.gameObject.SetActive(false);
        }

    }
    // 몬스터를 타겟팅 시스템에 등록
    public void RegisterMonster(Transform monster)
    {
        if (!monsters.Contains(monster))
        {
            monsters.Add(monster);
            Debug.Log("Monster registered to targeting system: " + monster.name);
        }
    }

    private void SetTarget(MonsterStatus monster)
    {
        targetMonster = monster.transform; //몬스터 자체의 Transform으로 설정
        targetPoition = monster.targetPosition != null ? monster.targetPosition : monster.transform;

        aim.gameObject.SetActive(true);
        hpBar.SetTargetmonster(monster); // HP 바 타겟 설정
        monster.onHpChange.AddListener(OnMonsterHpChanged);
    }
    //private void SetTarget(MonsterStatus monster)
    //{
    //
    //    //targetMonster = monster.transform; //몬스터 자체의 Transform으로 설정
    //    //targetPoition = monster.targetPosition != null ? monster.targetPosition : monster.transform;
    //    //
    //    //aim.gameObject.SetActive(true);
    //    //hpBar.SetTargetmonster(monster); // HP 바 타겟 설정
    //    //monster.onHpChange.AddListener(OnMonsterHpChanged);
    //    //
    //    //
    //    //targetMonster = monster.targetPosition;
    //
    //    // targetPosition이 null일 경우 기본값을 할당하거나 로그 출력
    //    if (monster.targetPosition != null)
    //    {
    //        targetPoition = monster.targetPosition;
    //    }
    //    else
    //    {
    //        Debug.LogError("Target position is null for the selected monster.");
    //        targetPoition = targetMonster; // targetPosition이 null일 경우, 몬스터 자체의 Transform으로 대체
    //    }
    //    
    //    aim.gameObject.SetActive(true);
    //    hpBar.SetTargetmonster(monster);
    //    monster.onHpChange.AddListener(OnMonsterHpChanged);
    //
    //    // TODO: 수정중...
    //    //targetMonster = monster.transform;
    //    //targetPoition = monster.targetPosition;
    //    //aim.gameObject.SetActive(true);
    //    //hpBar.SetTargetmonster(monster);
    //    //monster.onHpChange.AddListener(OnMonsterHpChanged);
    //}

    private void ClearTarget()
    {
        aim.gameObject.SetActive(false);
        hpBar.hpBarUI.SetActive(false);
        targetMonster = null;
    }



    private void UpdateTarget()
    {
        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);
        RaycastHit hit;
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRadius);
        if (Physics.SphereCast(ray, 20f, out hit, 20f))
        {
            MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
            if (monster != null)
            {
                //타겟 몬스터가 설정된 경우
                if (targetMonster == null || targetMonster != monster.transform)
                {
                    targetMonster = monster.transform;
                    targetPoition = monster.targetPosition; // 타겟 위치를 설정
                    aim.gameObject.SetActive(true);
                    hpBar.SetTargetmonster(monster); // 클릭된 몬스터를 타겟팅
                                                     //체력 변화 이벤트를 구독하여 체력 0일 때 UI 비활성화
                    monster.onHpChange.AddListener(OnMonsterHpChanged);
                }
                else
                {
                    //몬스터가 아닌 경우 조준 이미지를 비활성화합니다.
                    aim.gameObject.SetActive(false);
                    hpBar.hpBarUI.SetActive(false);
                }
            }
        }
        else
        {
            // SphereCast에 몬스터가 없을 경우
            if (targetMonster != null)
            {
                // 타겟 몬스터가 현재 존재하지만 탐지되지 않는 경우
                targetMonster = null;
                targetPoition = null;
                hpBar.hpBarUI.SetActive(false);
                aim.gameObject.SetActive(false);
            }
        }



    }

    private void OnMonsterHpChanged(int currentHp, int maxHP)
    {
        if (currentHp <= 0)
        {
            aim.gameObject.SetActive(false);
            hpBar.hpBarUI.SetActive(false);
            targetMonster = null; // 타겟 초기화
            //targetPoition = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (PlayerRay == null)
            return;

        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.position);
        //float sphereCastRadius = 1f; // 레이의 반경 (조절 가능)

        // 레이의 방향을 시각적으로 표시합니다.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(ray.origin, ray.direction * targetRadius);

        // 레이의 범위를 시각적으로 표시합니다.targetRadius
        Vector3 sphereCastEndPoint = ray.origin + ray.direction * targetRadius;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(sphereCastEndPoint, sphereCastRadius);
    }
}
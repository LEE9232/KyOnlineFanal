using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MTarget : MonoBehaviour
{
    public float targetRadius = 1f;
    //public MonsterHPbar hpBar;  // HP바 관리 스크립트
    public GameObject PlayerRay;
    public Image aim;
    private Transform targetMonster;
    private Transform targetPoition;


    public Color rayColor = Color.red;
    public Color sphereColor = Color.green;

    private void Start()
    {
        aim.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // 주변 몬스터를 탐색하여 자동으로 타겟팅을 수행합니다.
            UpdateTarget();
        }
        // 타겟이 설정된 경우 조준 이미지를 화면에 표시합니다.
        if (targetPoition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPoition.position);

            aim.transform.position = screenPosition;
        }
        else
        {
            aim.gameObject.SetActive(false); // 타겟이 없을 경우 조준 이미지를 비활성화합니다.
        }
    }
    private void UpdateTarget()
    {
        // 레이의 시작 위치와 방향을 설정합니다.
        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);

        RaycastHit hit;
        float sphereCastRadius = 5f; // 레이의 반경 (조절 가능)

        // SphereCast 범위를 시각적으로 표시합니다.

        // SphereCast를 사용하여 반경 내의 오브젝트를 탐지합니다.

        //if (Physics.SphereCast(ray, sphereCastRadius, out hit, targetRadius))
        //{
        //    //MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
        //    if (monster != null)
        //    {
        //        // 타겟 몬스터가 설정된 경우
        //        if (targetMonster == null || targetMonster != monster.transform)
        //        {
        //            targetMonster = monster.transform;
        //            targetPoition = monster.targetPosition; // 타겟 위치를 설정
        //            aim.gameObject.SetActive(true);
        //            hpBar.SetTargetmonster(monster); // 클릭된 몬스터를 타겟팅
        //            // 체력 변화 이벤트를 구독하여 체력 0일 때 UI 비활성화
        //            monster.onHpChange.AddListener(OnMonsterHpChanged);
        //        }
        //        else
        //        {
        //            // 몬스터가 아닌 경우 조준 이미지를 비활성화합니다.
        //            aim.gameObject.SetActive(false);
        //            hpBar.hpBarUI.SetActive(false);
        //        }
        //    }
        //}
        //else
        //{
        //    // SphereCast에 몬스터가 없을 경우
        //    if (targetMonster != null)
        //    {
        //        // 타겟 몬스터가 현재 존재하지만 탐지되지 않는 경우
        //        targetMonster = null;
        //        targetPoition = null;
        //        hpBar.hpBarUI.SetActive(false);
        //        aim.gameObject.SetActive(false);
        //    }
        //}
    }
    //private void OnMonsterHpChanged(int currentHp, int maxHP)
    //{
    //    if (currentHp <= 0)
    //    {
    //        aim.gameObject.SetActive(false);
    //        hpBar.hpBarUI.SetActive(false);
    //    }
    //}
    private void OnDrawGizmos()
    {
        if (PlayerRay == null)
            return;

        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);
        float sphereCastRadius = 1f; // 레이의 반경 (조절 가능)

        // 레이의 방향을 시각적으로 표시합니다.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(ray.origin, ray.direction * targetRadius);

        // 레이의 범위를 시각적으로 표시합니다.
        Vector3 sphereCastEndPoint = ray.origin + ray.direction * targetRadius;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(sphereCastEndPoint, sphereCastRadius);
    }

}



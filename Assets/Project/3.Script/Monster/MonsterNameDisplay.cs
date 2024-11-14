using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterNameDisplay : MonoBehaviour
{
    public MonsterStatus mons_Stat;
    public Transform nameTagPosition; // 이름을 표시할 위치 (몬스터의 머리 위에 위치 설정)
    public Camera mainCamera; // 플레이어의 메인 카메라
    public TextMeshProUGUI nameText; // TextMeshPro 텍스트 UI
    private void Start()
    {
        mainCamera = Camera.main; // 메인 카메라를 자동으로 할당
        // 몬스터 이름 설정
        nameText.text = mons_Stat.monsName;
        SetTextColorByGrade(mons_Stat.monsterGrade);
        nameText.raycastTarget = false; // 코드로 설정하는 방법
    }

    private void LateUpdate()
    {
        // 이름이 항상 카메라를 향하도록 설정
        if (mainCamera != null)
        {
            nameTagPosition.rotation = Quaternion.LookRotation(nameTagPosition.position - mainCamera.transform.position);
        }
    }
    private void SetTextColorByGrade(MonsterStatus.MonsterGrade grade)
    {
        switch (grade)
        {
            case MonsterStatus.MonsterGrade.Normal:
                nameText.color = Color.white; // 기본 색상
                break;
            case MonsterStatus.MonsterGrade.Rare:
                nameText.color = Color.blue; // 희귀 몬스터의 색상
                break;
            case MonsterStatus.MonsterGrade.Epic:
                nameText.color = Color.magenta; // 에픽 몬스터의 색상
                break;
            case MonsterStatus.MonsterGrade.Legendary:
                nameText.color = Color.yellow; // 전설 몬스터의 색상
                break;
            default:
                nameText.color = Color.white;
                break;
        }
    }
}

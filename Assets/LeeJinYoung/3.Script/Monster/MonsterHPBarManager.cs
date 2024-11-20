using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBarManager : MonoBehaviour
{
    public Slider hpBar;  // HP Bar Slider
    public TextMeshProUGUI nameText;  // 몬스터 이름
    public TextMeshProUGUI hpText;  // HP 텍스트
    public GameObject hpBarUI;  // 전체 HP Bar UI
    //private MonsterStatus monsterStatus;
    private void Awake()
    {
        // 해당 컴포넌트들이 제대로 할당되었는지 체크
        if (hpBar == null || nameText == null || hpText == null || hpBarUI == null)
        {
            Debug.LogError("HPBarManager: 모든 UI 요소가 할당되지 않았습니다.");
        }
        //monsterStatus = GetComponent<MonsterStatus>();
    }

    // HP Bar와 관련된 데이터를 초기화하는 메서드
    public void SetupHPBar(MonsterStatus monster)
    {
        hpBar.maxValue = monster.monsmaxHp;
        hpBar.value = monster.currentHp;
        nameText.text = monster.monsName;
        hpText.text = $"{monster.currentHp} / {monster.monsmaxHp}";
        hpBarUI.SetActive(true);  // HP Bar UI 활성화
        monster.onHpChange.AddListener((currentHP, maxHP) => UpdateHPBar(currentHP, maxHP, name));
    }

    // HP Bar를 업데이트하는 메서드
    public void UpdateHPBar(int currentHP, int maxHP, string monsterName)
    {
        hpBar.value = currentHP;
        hpText.text = $"{currentHP} / {maxHP}";
        nameText.text = monsterName;
    }

    // HP Bar UI를 숨기는 메서드
    public void HideHPBar()
    {
        hpBarUI.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPBarManager : MonoBehaviour
{
    public Slider hpBar;  // HP Bar Slider
    public TextMeshProUGUI nameText;  // ���� �̸�
    public TextMeshProUGUI hpText;  // HP �ؽ�Ʈ
    public GameObject hpBarUI;  // ��ü HP Bar UI
    //private MonsterStatus monsterStatus;
    private void Awake()
    {
        // �ش� ������Ʈ���� ����� �Ҵ�Ǿ����� üũ
        if (hpBar == null || nameText == null || hpText == null || hpBarUI == null)
        {
            Debug.LogError("HPBarManager: ��� UI ��Ұ� �Ҵ���� �ʾҽ��ϴ�.");
        }
        //monsterStatus = GetComponent<MonsterStatus>();
    }

    // HP Bar�� ���õ� �����͸� �ʱ�ȭ�ϴ� �޼���
    public void SetupHPBar(MonsterStatus monster)
    {
        hpBar.maxValue = monster.monsmaxHp;
        hpBar.value = monster.currentHp;
        nameText.text = monster.monsName;
        hpText.text = $"{monster.currentHp} / {monster.monsmaxHp}";
        hpBarUI.SetActive(true);  // HP Bar UI Ȱ��ȭ
        monster.onHpChange.AddListener((currentHP, maxHP) => UpdateHPBar(currentHP, maxHP, name));
    }

    // HP Bar�� ������Ʈ�ϴ� �޼���
    public void UpdateHPBar(int currentHP, int maxHP, string monsterName)
    {
        hpBar.value = currentHP;
        hpText.text = $"{currentHP} / {maxHP}";
        nameText.text = monsterName;
    }

    // HP Bar UI�� ����� �޼���
    public void HideHPBar()
    {
        hpBarUI.SetActive(false);
    }
}

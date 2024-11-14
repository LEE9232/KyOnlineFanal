using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHPbar : MonoBehaviour
{

    private MonsterStatus mons_Stat;
    public Slider hpBar;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public GameObject hpBarUI;

    private void Awake()
    {
        // �� ���� ������ UI ������Ʈ �Ҵ�
        MonsterHPBarManager hpBarManager = FindObjectOfType<MonsterHPBarManager>();
        if (hpBarManager != null)
        {
            hpBar = hpBarManager.hpBar;
            nameText = hpBarManager.nameText;
            hpText = hpBarManager.hpText;
            hpBarUI = hpBarManager.hpBarUI;
        }
        mons_Stat = GetComponent<MonsterStatus>();
        //UpdateHpbar(mons_Stat.currentHp, mons_Stat.monsmaxHp);
        if (mons_Stat != null)
        {
            //SetTextColorByGrade(mons_Stat.monsterGrade);
            mons_Stat.onHpChange.AddListener(UpdateHpbar);
        }
        //mons_Stat.onHpChange.AddListener(UpdateHpbar);
    }

    private void Start()
    {
        hpBarUI.SetActive(false);
    }

    public void SetTargetmonster(MonsterStatus monster)
    {
        if (mons_Stat != null)
        { 
            mons_Stat.onHpChange.RemoveListener(UpdateHpbar);
        }
        mons_Stat = monster;

        if (monster != null)
        {
            if (monster.onHpChange != null)
            {
                monster.onHpChange.AddListener(UpdateHpbar);
            }
            // �ٸ� Ÿ�� ����
            //mons_Stat.onHpChange.AddListener(UpdateHpbar);

            nameText.text = mons_Stat.monsName;
            hpBar.maxValue = mons_Stat.monsmaxHp;
            hpBar.value = mons_Stat.currentHp;
            hpText.text = $"{mons_Stat.currentHp} / {mons_Stat.monsmaxHp}";
            SetTextColorByGrade(mons_Stat.monsterGrade);
            hpBarUI.SetActive(true);
        }
        else
        {
            hpBarUI.SetActive(false);
        }
    }

    public void UpdateHpbar(int currentHP, int maxHP)
    {
        hpBar.minValue = 0;
        hpBar.maxValue = maxHP;
        hpBar.value = (float)currentHP;
        hpText.text = $"{currentHP} / {maxHP}";
        SetNameText(mons_Stat.monsName);
    }
    public void SetNameText(string name)
    {
        if (nameText != null)
        {
            nameText.text = name;
        }
        else
        {
            Debug.LogError("�̸��� �����ϴ�.");
        }
    }
    // ��޿� ���� �ؽ�Ʈ ������ �����ϴ� �޼���
    private void SetTextColorByGrade(MonsterStatus.MonsterGrade grade)
    {
        switch (grade)
        {
            case MonsterStatus.MonsterGrade.Normal:
                nameText.color = Color.white; // �⺻ ����
                break;
            case MonsterStatus.MonsterGrade.Rare:
                nameText.color = Color.blue; // ��� ������ ����
                break;
            case MonsterStatus.MonsterGrade.Epic:
                nameText.color = Color.magenta; // ���� ������ ����
                break;
            case MonsterStatus.MonsterGrade.Legendary:
                nameText.color = Color.yellow; // ���� ������ ����
                break;
            default:
                nameText.color = Color.white;
                break;
        }
    }
}

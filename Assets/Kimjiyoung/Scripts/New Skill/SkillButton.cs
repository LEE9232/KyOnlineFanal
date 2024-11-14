using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ��ų�� ����, �̹��� ��ũ��Ʈ
public class SkillButton : MonoBehaviour
{
    public int skillIndex; // �� ��ư�� �ش��ϴ� ��ų �ε���
    public Button button;
    private SkillListUI skillListUI;

    private void Start()
    {
        skillListUI = FindObjectOfType<SkillListUI>();
        button.onClick.AddListener(() => OnButtonClick());
    }

    private void OnButtonClick()
    {
        // ��ų ����Ʈ���� �ش� ��ų�� ����
        //FindObjectOfType<SkillListUI>().OnSkillSelected(skillIndex);
        if (skillListUI != null)
        {
            // ��ų ����Ʈ���� �ش� ��ų�� ����
            skillListUI.OnSkillSelected(skillIndex);
        }
        else
        {
            Debug.LogWarning("Can Not Find SkillListUI");
        }
    }
}
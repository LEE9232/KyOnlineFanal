using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스킬의 정보, 이미지 스크립트
public class SkillButton : MonoBehaviour
{
    public int skillIndex; // 이 버튼에 해당하는 스킬 인덱스
    public Button button;
    private SkillListUI skillListUI;

    private void Start()
    {
        skillListUI = FindObjectOfType<SkillListUI>();
        button.onClick.AddListener(() => OnButtonClick());
    }

    private void OnButtonClick()
    {
        // 스킬 리스트에서 해당 스킬을 선택
        //FindObjectOfType<SkillListUI>().OnSkillSelected(skillIndex);
        if (skillListUI != null)
        {
            // 스킬 리스트에서 해당 스킬을 선택
            skillListUI.OnSkillSelected(skillIndex);
        }
        else
        {
            Debug.LogWarning("Can Not Find SkillListUI");
        }
    }
}
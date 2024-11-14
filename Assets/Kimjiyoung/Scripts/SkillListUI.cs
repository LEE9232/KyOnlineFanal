using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListUI : MonoBehaviour
{
    public List<Button> skillButtons; // 스킬 버튼들
    private MSkillSlot selectedSlot;

    public void Start()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            int skillIndex = i; // 클로저 문제 방지
            skillButtons[i].onClick.AddListener(() => OnSkillSelected(skillIndex));
        }
    }

    public void SetSelectedSlot(MSkillSlot slot)
    {
        selectedSlot = slot; // 어떤 슬롯에 스킬을 할당할지 설정
    }

    public void OnSkillSelected(int skillIndex)
    {
        // 선택된 스킬을 스킬 슬롯에 할당
        //MSkillSlot skillSlot = FindObjectOfType<MSkillSlot>();
        if (selectedSlot != null)
        {
            selectedSlot.AssignSkill(skillIndex); // 선택된 스킬을 슬롯에 할당
        }

        gameObject.SetActive(false); // 스킬 선택 후 스킬 리스트 UI 비활성화
    }

    //public void OnSkillButtonClick(int skillIndex)
    //{
    //    MSkillSlot.OnSkillSelected(skillIndex);
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListUI : MonoBehaviour
{
    public List<Button> skillButtons; // ��ų ��ư��
    private MSkillSlot selectedSlot;

    public void Start()
    {
        for (int i = 0; i < skillButtons.Count; i++)
        {
            int skillIndex = i; // Ŭ���� ���� ����
            skillButtons[i].onClick.AddListener(() => OnSkillSelected(skillIndex));
        }
    }

    public void SetSelectedSlot(MSkillSlot slot)
    {
        selectedSlot = slot; // � ���Կ� ��ų�� �Ҵ����� ����
    }

    public void OnSkillSelected(int skillIndex)
    {
        // ���õ� ��ų�� ��ų ���Կ� �Ҵ�
        //MSkillSlot skillSlot = FindObjectOfType<MSkillSlot>();
        if (selectedSlot != null)
        {
            selectedSlot.AssignSkill(skillIndex); // ���õ� ��ų�� ���Կ� �Ҵ�
        }

        gameObject.SetActive(false); // ��ų ���� �� ��ų ����Ʈ UI ��Ȱ��ȭ
    }

    //public void OnSkillButtonClick(int skillIndex)
    //{
    //    MSkillSlot.OnSkillSelected(skillIndex);
    //}
}

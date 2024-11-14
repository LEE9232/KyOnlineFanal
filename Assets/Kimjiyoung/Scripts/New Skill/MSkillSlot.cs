using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 셋팅한 스킬 사용하는 스크립트
public class MSkillSlot : MonoBehaviour
{
    public int slotIndex;
    private MagicManager magicManager;
    private int assignedSkillIndex = -1; // 할당된 스킬 인덱스 (-1이면 할당 안 된 상태)
    public GameObject skillListUI;
    //private static MSkillSlot selectedSlot; // 현재 선택된 슬롯을 저장(공유)

    public Image cooldownImage;
    public TextMeshProUGUI cooldownText;
    public Button skillSlotButton;

    private bool isCooldown = false;

    private static Dictionary<int, MSkillSlot> skillSlots = new Dictionary<int, MSkillSlot>();

    private void Start()
    {
        magicManager = FindObjectOfType<MagicManager>();
        if (magicManager != null)
        {
            Debug.Log(magicManager); // MagicManager가 제대로 할당되는지 확인

        }
        skillListUI.SetActive(false);
        cooldownImage.fillAmount = 0;
        cooldownText.text = "";

        skillSlots[slotIndex] = this;
        magicManager.RegisterSkillSlot(slotIndex, this);
    }

    private void Update()
    {
        if (slotIndex == 0 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseAssignedSkill();
        }
        if (slotIndex == 1 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseAssignedSkill();
        }
        if (slotIndex == 2 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseAssignedSkill();
        }
        if (slotIndex == 3 && Input.GetKeyDown(KeyCode.Alpha4))
        {
            UseAssignedSkill();
        }
    }

    public bool IsAvailable()
    {
        return !isCooldown;  // 쿨타임이 돌아가지 않는 슬롯이면 사용 가능
    }

    public void OnSlotClick()
    {

        if (skillListUI != null)
        {
            skillListUI.SetActive(true);
            //selectedSlot = this; // 클릭한 슬롯 저장
            FindObjectOfType<SkillListUI>().SetSelectedSlot(this);
        }
    }

    public void AssignSkill(int skillIndex)
    {
        assignedSkillIndex = skillIndex; // 슬롯에 스킬 인덱스 할당
        Debug.Log($"스킬 {skillIndex} 할당됨"); // 확인용 로그 추가
        // UI 업데이트 (예 : 스킬 이미지 변경)
        UpdateSkillIcon(skillIndex);
        magicManager.UpdateSkillAssignment(slotIndex, skillIndex);
        if (skillListUI != null)
        {
            skillListUI.SetActive(false);
        }
        Debug.Log($"스킬 {skillIndex}가 슬롯 {slotIndex}에 할당되었습니다.");
    }

    // 스킬 사용 예시
    public void UseAssignedSkill()
    {
        //if (assignedSkillIndex != -1)
        //{
        //    magicManager.UseSkill(assignedSkillIndex, cooldownImage, cooldownText);
        //}
        // null 체크 추가
        if (magicManager == null)
        {
            Debug.LogError("MagicManager가 null입니다. 스킬을 사용할 수 없습니다.");
            return;
        }

        if (cooldownImage == null)
        {
            Debug.LogError("cooldownImage가 null입니다.");
            return;
        }

        if (cooldownText == null)
        {
            Debug.LogError("cooldownText가 null입니다.");
            return;
        }
        if (assignedSkillIndex == -1)
        {
            Debug.LogWarning("할당된 스킬이 없습니다.");
            return;
        }
        Debug.Log($"사용할 스킬 인덱스: {assignedSkillIndex}");
        if (assignedSkillIndex != -1)
        {
            magicManager.UseSkill(assignedSkillIndex, cooldownImage, cooldownText); 
        }

        //Debug.Log($"UseAssignedSkill: {assignedSkillIndex} 스킬을 사용합니다.");
    }

    private void UpdateSkillIcon(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < magicManager.skillImage.Count)
        {
            GetComponent<Image>().sprite = magicManager.skillImage[skillIndex];
        }
    }

    public static MSkillSlot GetSlotBySkillIndex(int slotIndex)
    {
        return skillSlots.TryGetValue(slotIndex, out var slot) ? slot : null;
    }

    // 스킬 리스트 UI에서 스킬을 선택했을 때 호출(정적 메서드)
    //public static void OnSkillSelected(int skillIndex)
    //{
    //    if (selectedSlot != null)
    //    {
    //        selectedSlot.AssignSkill(skillIndex); // 선택된 슬롯에 스킬 할당
    //    }
    //}
}

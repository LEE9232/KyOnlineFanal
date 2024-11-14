using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// ������ ��ų ����ϴ� ��ũ��Ʈ
public class MSkillSlot : MonoBehaviour
{
    public int slotIndex;
    private MagicManager magicManager;
    private int assignedSkillIndex = -1; // �Ҵ�� ��ų �ε��� (-1�̸� �Ҵ� �� �� ����)
    public GameObject skillListUI;
    //private static MSkillSlot selectedSlot; // ���� ���õ� ������ ����(����)

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
            Debug.Log(magicManager); // MagicManager�� ����� �Ҵ�Ǵ��� Ȯ��

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
        return !isCooldown;  // ��Ÿ���� ���ư��� �ʴ� �����̸� ��� ����
    }

    public void OnSlotClick()
    {

        if (skillListUI != null)
        {
            skillListUI.SetActive(true);
            //selectedSlot = this; // Ŭ���� ���� ����
            FindObjectOfType<SkillListUI>().SetSelectedSlot(this);
        }
    }

    public void AssignSkill(int skillIndex)
    {
        assignedSkillIndex = skillIndex; // ���Կ� ��ų �ε��� �Ҵ�
        Debug.Log($"��ų {skillIndex} �Ҵ��"); // Ȯ�ο� �α� �߰�
        // UI ������Ʈ (�� : ��ų �̹��� ����)
        UpdateSkillIcon(skillIndex);
        magicManager.UpdateSkillAssignment(slotIndex, skillIndex);
        if (skillListUI != null)
        {
            skillListUI.SetActive(false);
        }
        Debug.Log($"��ų {skillIndex}�� ���� {slotIndex}�� �Ҵ�Ǿ����ϴ�.");
    }

    // ��ų ��� ����
    public void UseAssignedSkill()
    {
        //if (assignedSkillIndex != -1)
        //{
        //    magicManager.UseSkill(assignedSkillIndex, cooldownImage, cooldownText);
        //}
        // null üũ �߰�
        if (magicManager == null)
        {
            Debug.LogError("MagicManager�� null�Դϴ�. ��ų�� ����� �� �����ϴ�.");
            return;
        }

        if (cooldownImage == null)
        {
            Debug.LogError("cooldownImage�� null�Դϴ�.");
            return;
        }

        if (cooldownText == null)
        {
            Debug.LogError("cooldownText�� null�Դϴ�.");
            return;
        }
        if (assignedSkillIndex == -1)
        {
            Debug.LogWarning("�Ҵ�� ��ų�� �����ϴ�.");
            return;
        }
        Debug.Log($"����� ��ų �ε���: {assignedSkillIndex}");
        if (assignedSkillIndex != -1)
        {
            magicManager.UseSkill(assignedSkillIndex, cooldownImage, cooldownText); 
        }

        //Debug.Log($"UseAssignedSkill: {assignedSkillIndex} ��ų�� ����մϴ�.");
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

    // ��ų ����Ʈ UI���� ��ų�� �������� �� ȣ��(���� �޼���)
    //public static void OnSkillSelected(int skillIndex)
    //{
    //    if (selectedSlot != null)
    //    {
    //        selectedSlot.AssignSkill(skillIndex); // ���õ� ���Կ� ��ų �Ҵ�
    //    }
    //}
}

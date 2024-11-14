using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MagicSkillSlot : MonoBehaviour, IDropHandler
{
    public Image skillIcon;
    public int slotIndex;

    private MagicController magicController;


    private void Start()
    {
        magicController = FindObjectOfType<MagicController>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        SkillDragHandler draggedSkill = eventData.pointerDrag.GetComponent<SkillDragHandler>();
        if (draggedSkill != null)
        {
            AssignSKill(draggedSkill.skillIndex);
        }
    }

    private void AssignSKill(int skillIndex)
    {
        //skillIcon.sprite=magicController
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public int skillIndex;
    private Image skillIcon;
    private CanvasGroup CanvasGroup;
    private Transform originalParent;

    private void Start()
    {
        skillIcon = GetComponent<Image>();
        CanvasGroup = GetComponent<CanvasGroup>();
        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        CanvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition; // 마우스 따라다니게
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CanvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
    }
}

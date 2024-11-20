using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private QuestData questData;
    private QuestTooltip questTooltip;

    public void Initialize(QuestData data, QuestTooltip tooltip)
    {
        questData = data;
        questTooltip = tooltip;
        // 널 체크 추가
        if (questTooltip == null)
        {
            Debug.LogError("QuestTooltip이 할당되지 않았습니다.");
        }

        if (questData == null)
        {
            Debug.LogError("QuestData가 할당되지 않았습니다.");
        }

    }

    // 마우스가 버튼 위로 올라갔을 때 툴팁을 보여줌
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (questData != null)
        {
            questTooltip.ShowTooltip(questData);
        }
    }

    // 마우스가 버튼에서 벗어났을 때 툴팁을 숨김
    public void OnPointerExit(PointerEventData eventData)
    {
        questTooltip.HideTooltip();
    }
}

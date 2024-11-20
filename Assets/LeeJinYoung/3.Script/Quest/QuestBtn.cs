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
        // �� üũ �߰�
        if (questTooltip == null)
        {
            Debug.LogError("QuestTooltip�� �Ҵ���� �ʾҽ��ϴ�.");
        }

        if (questData == null)
        {
            Debug.LogError("QuestData�� �Ҵ���� �ʾҽ��ϴ�.");
        }

    }

    // ���콺�� ��ư ���� �ö��� �� ������ ������
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (questData != null)
        {
            questTooltip.ShowTooltip(questData);
        }
    }

    // ���콺�� ��ư���� ����� �� ������ ����
    public void OnPointerExit(PointerEventData eventData)
    {
        questTooltip.HideTooltip();
    }
}

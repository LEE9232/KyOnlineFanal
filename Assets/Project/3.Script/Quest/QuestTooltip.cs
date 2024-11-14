using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTooltip : MonoBehaviour
{
    public TextMeshProUGUI QuestNameText;
    public TextMeshProUGUI QuestInfoText;
    public GameObject tooltipPanel;  // ���� UI �г�

    // ������ ������Ʈ�ϰ� ǥ���ϴ� �޼���
    public void ShowTooltip(QuestData questData)
    {
        if (questData != null)
        {
            QuestNameText.text = questData.questName;
            QuestInfoText.text = questData.description;
            tooltipPanel.SetActive(true);  // ���� �г��� Ȱ��ȭ
        }
    }

    // ������ ����� �޼���
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

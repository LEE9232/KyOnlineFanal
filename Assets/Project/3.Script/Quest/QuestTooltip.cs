using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestTooltip : MonoBehaviour
{
    public TextMeshProUGUI QuestNameText;
    public TextMeshProUGUI QuestInfoText;
    public GameObject tooltipPanel;  // 툴팁 UI 패널

    // 툴팁을 업데이트하고 표시하는 메서드
    public void ShowTooltip(QuestData questData)
    {
        if (questData != null)
        {
            QuestNameText.text = questData.questName;
            QuestInfoText.text = questData.description;
            tooltipPanel.SetActive(true);  // 툴팁 패널을 활성화
        }
    }

    // 툴팁을 숨기는 메서드
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

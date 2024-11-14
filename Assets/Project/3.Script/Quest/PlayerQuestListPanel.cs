using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerQuestListPanel : MonoBehaviour
{
    public GameObject questSlotPrefab;
    public Transform questParent;
    private List<GameObject> questSlots = new List<GameObject>();

    public void UpdataQuestUI()
    {
        foreach (GameObject slot in questSlots)
        {
            Destroy(slot);
        }
        questSlots.Clear();

        List<QuestData> acceptdQuests = PlayerQuestManager.Instance.acceptedQuests;

        foreach (QuestData quest in acceptdQuests)
        {
            GameObject questSlot = Instantiate(questSlotPrefab, questParent);
            TextMeshProUGUI questText = questSlot.GetComponentInChildren<TextMeshProUGUI>();
            questText.text = $"{quest.questName}\n{quest.description} : {quest.monsterKillCount} / {quest.targetKillCount}";
            questSlots.Add(questSlot);
        }
    }
}

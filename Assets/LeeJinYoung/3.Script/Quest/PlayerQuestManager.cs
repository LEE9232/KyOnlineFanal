using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestManager : MonoBehaviour
{
    public static PlayerQuestManager Instance { get; set; }
    public List<QuestData> acceptedQuests = new List<QuestData>();  // �÷��̾ ������ ����Ʈ ���

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddQuest(QuestData quest)
    {
        if (!HasAcceptedQuest(quest))  // �ߺ� ����Ʈ ����
        {
            acceptedQuests.Add(quest);
            GameManager.Instance.logUI.AddMessage($"<color=blue>{quest.questName} </color>����Ʈ�� �����Ͽ����ϴ�.");
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();  // ����Ʈ �α� UI ������Ʈ
        }
    }
    // �̹� ������ ����Ʈ���� Ȯ��
    public bool HasAcceptedQuest(QuestData quest)
    {
        return acceptedQuests.Contains(quest);
    }
    // ����Ʈ ��Ͽ��� �Ϸ��� ����Ʈ ����
    public void RemoveQuest(QuestData quest)
    {
        if (acceptedQuests.Contains(quest))
        {
            acceptedQuests.Remove(quest);
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();  // UI ������Ʈ
            GameManager.Instance.logUI.AddMessage($"{quest.questName} �Ϸ�! ����Ʈ ��Ͽ��� ���ŵǾ����ϴ�.");
        }
    }

    // ���� óġ �� ����Ʈ ���� ��Ȳ ������Ʈ
    public void UpdateQuestProgress()
    {
        foreach (QuestData quest in acceptedQuests)
        {
            quest.monsterKillCount++;
            if (quest.monsterKillCount >= quest.targetKillCount)
            {
                quest.isCompleted = true;
                GameManager.Instance.logUI.AddMessage($"{quest.questName} �Ϸ�!");
                CompleteQuest(quest);  // ����Ʈ �Ϸ� ó��
            }
            //GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
        }
        GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
    }
    // ����Ʈ �Ϸ� ó�� (���� ����)
    private void CompleteQuest(QuestData quest)
    {
        if (quest.isCompleted)
        {
            GameManager.Instance.PlayerData.Gold += quest.goldReward;
            GameManager.Instance.PlayerData.EXP += quest.experienceReward;
            GameManager.Instance.logUI.AddMessage($"<color=yellow>{quest.goldReward} Gold</color>, <color=yellow>{quest.experienceReward} EXP</color> UP!");
            RemoveQuest(quest);  // �Ϸ�� ����Ʈ�� ��Ͽ��� ����
                                 // UI ������Ʈ
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
        }
    }
}

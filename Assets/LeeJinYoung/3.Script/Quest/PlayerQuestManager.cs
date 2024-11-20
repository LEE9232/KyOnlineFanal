using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuestManager : MonoBehaviour
{
    public static PlayerQuestManager Instance { get; set; }
    public List<QuestData> acceptedQuests = new List<QuestData>();  // 플레이어가 수락한 퀘스트 목록

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
        if (!HasAcceptedQuest(quest))  // 중복 퀘스트 방지
        {
            acceptedQuests.Add(quest);
            GameManager.Instance.logUI.AddMessage($"<color=blue>{quest.questName} </color>퀘스트를 수락하였습니다.");
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();  // 퀘스트 로그 UI 업데이트
        }
    }
    // 이미 수락한 퀘스트인지 확인
    public bool HasAcceptedQuest(QuestData quest)
    {
        return acceptedQuests.Contains(quest);
    }
    // 퀘스트 목록에서 완료한 퀘스트 제거
    public void RemoveQuest(QuestData quest)
    {
        if (acceptedQuests.Contains(quest))
        {
            acceptedQuests.Remove(quest);
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();  // UI 업데이트
            GameManager.Instance.logUI.AddMessage($"{quest.questName} 완료! 퀘스트 목록에서 제거되었습니다.");
        }
    }

    // 몬스터 처치 시 퀘스트 진행 상황 업데이트
    public void UpdateQuestProgress()
    {
        foreach (QuestData quest in acceptedQuests)
        {
            quest.monsterKillCount++;
            if (quest.monsterKillCount >= quest.targetKillCount)
            {
                quest.isCompleted = true;
                GameManager.Instance.logUI.AddMessage($"{quest.questName} 완료!");
                CompleteQuest(quest);  // 퀘스트 완료 처리
            }
            //GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
        }
        GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
    }
    // 퀘스트 완료 처리 (보상 지급)
    private void CompleteQuest(QuestData quest)
    {
        if (quest.isCompleted)
        {
            GameManager.Instance.PlayerData.Gold += quest.goldReward;
            GameManager.Instance.PlayerData.EXP += quest.experienceReward;
            GameManager.Instance.logUI.AddMessage($"<color=yellow>{quest.goldReward} Gold</color>, <color=yellow>{quest.experienceReward} EXP</color> UP!");
            RemoveQuest(quest);  // 완료된 퀘스트는 목록에서 제거
                                 // UI 업데이트
            GameManager.Instance.playerQuestListPanel.UpdataQuestUI();
        }
    }
}

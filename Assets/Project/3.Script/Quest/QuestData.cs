using System;
using UnityEngine;

[Serializable]
public class QuestData
{
    public string questName;
    public string description;
    public int targetKillCount; // 목표 몬스터 처치 수
    public int monsterKillCount; // 현재 처치한 몬스터 수
    public bool isCompleted = false;
    public int experienceReward;
    public int goldReward;

    public QuestData(string questName, string description, int experienceReward, int goldReward, int targetKillCount)
    {
        this.questName = questName;
        this.description = description;
        //this.isCompleted = false;
        this.experienceReward = experienceReward;
        this.goldReward = goldReward;
        this.targetKillCount = targetKillCount;
        this.monsterKillCount = 0;
    }
}

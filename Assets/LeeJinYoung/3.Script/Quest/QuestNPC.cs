using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestNPC : MonoBehaviour
{
    public List<QuestData> availableQuests;  // NPC가 줄 수 있는 퀘스트 목록
    public GameObject questUI;
    public GameObject PopUpUI;
    public GameObject QuestCheckPopup;
    public GameObject questButtonPrefab; // 퀘스트 목록 버튼 프리팹
    public GameObject questListParant;
    public TextMeshProUGUI popupText;
    public Button questNPCBtn;
    private MinimapIconManager minimapIconManager;
    public QuestTooltip questTooltip; // 퀘스트 툴팁 스크립트
    public QuestPopup questpopupUI;  // 팝업 UI 스크립트
    private void Start()
    {
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCWithQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager 가 널입니다.");
        }
        questNPCBtn.onClick.AddListener(QuestBtnClick);

        // 임시로 테스트용 퀘스트 추가
        availableQuests = new List<QuestData>();
        availableQuests.Add(new QuestData("마을을 지켜줘", "몬스터 5마리 처치", 100, 200, 5));
        availableQuests.Add(new QuestData("마을을 지켜줘 2", "몬스터 20마리 처치", 300, 500, 20));
        availableQuests.Add(new QuestData("마을을 지켜줘 3", "몬스터 30마리 처치", 300, 500, 20));
        ShowQuestList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 범위 안에 들어왔을 때
        {
            PopUpUI.SetActive(true);
            questNPCBtn.gameObject.SetActive(true);
            popupText.text = "무슨 퀘스트를 받겠는가?";
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // 플레이어가 범위를 나갔을 때
        {
            questNPCBtn.gameObject.SetActive(false);
            questUI.SetActive(false);
            popupText.text = "완료하면 보상을 받으러 오게 !";
            StartCoroutine(PopupClose());

            
        }
    }

    public void QuestBtnClick()
    {
        questUI.SetActive(true);
    }

    // 퀘스트 목록 UI 생성
    public void ShowQuestList()
    {
        // 이미 생성된 버튼을 정리
        foreach (Transform child in questListParant.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (QuestData quest in availableQuests)
        {
            if (!PlayerQuestManager.Instance.HasAcceptedQuest(quest))  // 중복 방지
            {
                GameObject questButton = Instantiate(questButtonPrefab, questListParant.transform);
                TextMeshProUGUI buttonText = questButton.GetComponentInChildren<TextMeshProUGUI>();
                // 버튼 텍스트에 퀘스트 이름과 내용 표시
                buttonText.text = $"{quest.questName}";
                // QuestButton 스크립트에서 툴팁을 처리하도록 설정
                QuestBtn buttonComponent = questButton.GetComponent<QuestBtn>();
                if (buttonComponent != null)
                {
                    buttonComponent.Initialize(quest, questTooltip);

                }

                // 퀘스트 버튼 클릭 이벤트 설정
                questButton.GetComponent<Button>().onClick.AddListener(() => ShowQuestPopup(quest));
            }
        }
    }

    public void ShowQuestPopup(QuestData quest)
    {
        questpopupUI.ShowPopup(
            $"퀘스트 수락: {quest.questName}",
            quest.description,
            () => AcceptQuest(quest),  // 수락 콜백
            () => Debug.Log("퀘스트 거절")  // 거절 콜백
        );
    }
    public void AcceptQuest(QuestData quest)
    {
        PlayerQuestManager.Instance.AddQuest(quest);
        QuestCheckPopup.SetActive(false);
    }
    // 퀘스트 거절 처리

    IEnumerator PopupClose()
    {
        yield return new WaitForSeconds(4.0f);
        PopUpUI.SetActive(false);
    }
    // 완료된 퀘스트를 체크하고 보상 지급
    public void CompleteQuest(QuestData quest)
    {
        if (quest.isCompleted)
        {
            PlayerInfo player = GameManager.Instance.PlayerData;
            player.Gold += quest.goldReward;
            player.EXP += quest.experienceReward;

            GameManager.Instance.logUI.AddMessage($"{quest.questName} 완료! {quest.goldReward} 골드와 {quest.experienceReward} 경험치를 받았습니다.");
            PlayerQuestManager.Instance.RemoveQuest(quest);
        }
    }
}

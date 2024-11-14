using Photon.Pun.Demo.Cockpit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestNPC : MonoBehaviour
{
    public List<QuestData> availableQuests;  // NPC�� �� �� �ִ� ����Ʈ ���
    public GameObject questUI;
    public GameObject PopUpUI;
    public GameObject QuestCheckPopup;
    public GameObject questButtonPrefab; // ����Ʈ ��� ��ư ������
    public GameObject questListParant;
    public TextMeshProUGUI popupText;
    public Button questNPCBtn;
    private MinimapIconManager minimapIconManager;
    public QuestTooltip questTooltip; // ����Ʈ ���� ��ũ��Ʈ
    public QuestPopup questpopupUI;  // �˾� UI ��ũ��Ʈ
    private void Start()
    {
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCWithQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }
        questNPCBtn.onClick.AddListener(QuestBtnClick);

        // �ӽ÷� �׽�Ʈ�� ����Ʈ �߰�
        availableQuests = new List<QuestData>();
        availableQuests.Add(new QuestData("������ ������", "���� 5���� óġ", 100, 200, 5));
        availableQuests.Add(new QuestData("������ ������ 2", "���� 20���� óġ", 300, 500, 20));
        availableQuests.Add(new QuestData("������ ������ 3", "���� 30���� óġ", 300, 500, 20));
        ShowQuestList();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ���� �ȿ� ������ ��
        {
            PopUpUI.SetActive(true);
            questNPCBtn.gameObject.SetActive(true);
            popupText.text = "���� ����Ʈ�� �ްڴ°�?";
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾ ������ ������ ��
        {
            questNPCBtn.gameObject.SetActive(false);
            questUI.SetActive(false);
            popupText.text = "�Ϸ��ϸ� ������ ������ ���� !";
            StartCoroutine(PopupClose());

            
        }
    }

    public void QuestBtnClick()
    {
        questUI.SetActive(true);
    }

    // ����Ʈ ��� UI ����
    public void ShowQuestList()
    {
        // �̹� ������ ��ư�� ����
        foreach (Transform child in questListParant.transform)
        {
            Destroy(child.gameObject);
        }


        foreach (QuestData quest in availableQuests)
        {
            if (!PlayerQuestManager.Instance.HasAcceptedQuest(quest))  // �ߺ� ����
            {
                GameObject questButton = Instantiate(questButtonPrefab, questListParant.transform);
                TextMeshProUGUI buttonText = questButton.GetComponentInChildren<TextMeshProUGUI>();
                // ��ư �ؽ�Ʈ�� ����Ʈ �̸��� ���� ǥ��
                buttonText.text = $"{quest.questName}";
                // QuestButton ��ũ��Ʈ���� ������ ó���ϵ��� ����
                QuestBtn buttonComponent = questButton.GetComponent<QuestBtn>();
                if (buttonComponent != null)
                {
                    buttonComponent.Initialize(quest, questTooltip);

                }

                // ����Ʈ ��ư Ŭ�� �̺�Ʈ ����
                questButton.GetComponent<Button>().onClick.AddListener(() => ShowQuestPopup(quest));
            }
        }
    }

    public void ShowQuestPopup(QuestData quest)
    {
        questpopupUI.ShowPopup(
            $"����Ʈ ����: {quest.questName}",
            quest.description,
            () => AcceptQuest(quest),  // ���� �ݹ�
            () => Debug.Log("����Ʈ ����")  // ���� �ݹ�
        );
    }
    public void AcceptQuest(QuestData quest)
    {
        PlayerQuestManager.Instance.AddQuest(quest);
        QuestCheckPopup.SetActive(false);
    }
    // ����Ʈ ���� ó��

    IEnumerator PopupClose()
    {
        yield return new WaitForSeconds(4.0f);
        PopUpUI.SetActive(false);
    }
    // �Ϸ�� ����Ʈ�� üũ�ϰ� ���� ����
    public void CompleteQuest(QuestData quest)
    {
        if (quest.isCompleted)
        {
            PlayerInfo player = GameManager.Instance.PlayerData;
            player.Gold += quest.goldReward;
            player.EXP += quest.experienceReward;

            GameManager.Instance.logUI.AddMessage($"{quest.questName} �Ϸ�! {quest.goldReward} ���� {quest.experienceReward} ����ġ�� �޾ҽ��ϴ�.");
            PlayerQuestManager.Instance.RemoveQuest(quest);
        }
    }
}

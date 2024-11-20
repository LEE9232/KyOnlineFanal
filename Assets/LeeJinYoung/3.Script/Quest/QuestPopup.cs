using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    public TextMeshProUGUI popupTitleText;  // �˾� ���� �ؽ�Ʈ
    //public TextMeshProUGUI popupDescriptionText;  // �˾� ���� �ؽ�Ʈ
    public Button acceptButton;  // ���� ��ư
    public Button declineButton;  // ���� ��ư
    public GameObject popupPanel;  // �˾� �г�

    private Action onAcceptAction;  // ���� ��ư�� ������ �� ����� �ݹ�
    private Action onDeclineAction;  // ���� ��ư�� ������ �� ����� �ݹ�

    private void Start()
    {
        // �⺻������ �˾��� ��Ȱ��ȭ
        popupPanel.SetActive(false);

        // ���� �� ���� ��ư�� �⺻ �̺�Ʈ�� ����
        acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        declineButton.onClick.AddListener(OnDeclineButtonClicked);
    }

    // �˾��� �����ִ� �޼���
    public void ShowPopup(string title, string description, Action onAccept = null, Action onDecline = null)
    {
        popupTitleText.text = title;
        //popupDescriptionText.text = description;
        onAcceptAction = onAccept;
        onDeclineAction = onDecline;
        popupPanel.SetActive(true);
    }

    // ���� ��ư Ŭ�� �� ����� �޼���
    private void OnAcceptButtonClicked()
    {
        onAcceptAction?.Invoke();  // ���� �ݹ� ����
        ClosePopup();
    }

    // ���� ��ư Ŭ�� �� ����� �޼���
    private void OnDeclineButtonClicked()
    {
        onDeclineAction?.Invoke();  // ���� �ݹ� ����
        ClosePopup();
    }

    // �˾��� �ݴ� �޼���
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}

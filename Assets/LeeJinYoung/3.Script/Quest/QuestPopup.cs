using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPopup : MonoBehaviour
{
    public TextMeshProUGUI popupTitleText;  // 팝업 제목 텍스트
    //public TextMeshProUGUI popupDescriptionText;  // 팝업 설명 텍스트
    public Button acceptButton;  // 수락 버튼
    public Button declineButton;  // 거절 버튼
    public GameObject popupPanel;  // 팝업 패널

    private Action onAcceptAction;  // 수락 버튼을 눌렀을 때 실행될 콜백
    private Action onDeclineAction;  // 거절 버튼을 눌렀을 때 실행될 콜백

    private void Start()
    {
        // 기본적으로 팝업을 비활성화
        popupPanel.SetActive(false);

        // 수락 및 거절 버튼에 기본 이벤트를 설정
        acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        declineButton.onClick.AddListener(OnDeclineButtonClicked);
    }

    // 팝업을 보여주는 메서드
    public void ShowPopup(string title, string description, Action onAccept = null, Action onDecline = null)
    {
        popupTitleText.text = title;
        //popupDescriptionText.text = description;
        onAcceptAction = onAccept;
        onDeclineAction = onDecline;
        popupPanel.SetActive(true);
    }

    // 수락 버튼 클릭 시 실행될 메서드
    private void OnAcceptButtonClicked()
    {
        onAcceptAction?.Invoke();  // 수락 콜백 실행
        ClosePopup();
    }

    // 거절 버튼 클릭 시 실행될 메서드
    private void OnDeclineButtonClicked()
    {
        onDeclineAction?.Invoke();  // 거절 콜백 실행
        ClosePopup();
    }

    // 팝업을 닫는 메서드
    public void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}

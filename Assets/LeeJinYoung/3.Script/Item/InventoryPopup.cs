using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    public GameObject popupPanel;  // 팝업 패널
    public TextMeshProUGUI popupMessage;      // 팝업 메시지 텍스트
    public Button yesButton;       // Yes 버튼
    public Button noButton;        // No 버튼

    // 팝업을 띄우는 메서드
    public void ShowPopup(string message, Action onConfirm)
    {
        popupPanel.SetActive(true);
        popupMessage.text = message;

        // Yes 버튼 클릭 이벤트 등록
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke(); // 확인 버튼 클릭 시 콜백 호출
            popupPanel.SetActive(false); // 팝업 닫기
        });

        // No 버튼 클릭 시 팝업 닫기
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() =>
        {
            popupPanel.SetActive(false); // 팝업 닫기
        });
    }

    // 팝업을 숨기는 메서드
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}

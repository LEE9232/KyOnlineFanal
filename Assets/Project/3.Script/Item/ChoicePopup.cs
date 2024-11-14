using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePopup : MonoBehaviour
{
    public GameObject popupPanel;  // 팝업 패널
    public TextMeshProUGUI popupMessage;      // 팝업 메시지 텍스트
    public Button yesButton;       // Yes 버튼
    public Button noButton;        // No 버튼
    private Action confirmAction;  // 확인 시 호출되는 콜백


    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    private void Update()
    {
        
        // Enter 키 또는 마우스 클릭 시 Yes 버튼 작동
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnYesButtonClicked();
        }
        // Esc 키 또는 마우스 클릭 시 No 버튼 작동
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnNoButtonClicked();
        }
    }
    // 팝업을 띄우는 메서드
    public void ShowPopup(string message, Action onConfirm)
    {
        popupPanel.SetActive(true);
        popupMessage.text = message;
        confirmAction = onConfirm; // 콜백 저장

        // Yes 버튼 클릭 이벤트 등록
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesButtonClicked); 
        noButton.onClick.AddListener(OnNoButtonClicked);
    }
    private void OnYesButtonClicked()
    {
        Debug.Log("Yes 버튼 클릭됨");
        confirmAction?.Invoke(); // 확인 버튼 클릭 시 콜백 호출
        popupPanel.SetActive(false); // 팝업 닫기
    }

    private void OnNoButtonClicked()
    {
        Debug.Log("No 버튼 클릭됨");
        popupPanel.SetActive(false); // 팝업 닫기
    }
    // 팝업을 숨기는 메서드
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}

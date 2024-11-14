using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance { get; private set; }
    // 로그인패널
    public Button startButton;
    public GameObject loginPanel;
    public GameObject signPanel;
    public GameObject choicePanel;
    //public GameObject optionPanel;
    public GameObject CharPanel;
    public GameObject panelList;
    public CameraVer1 cameraVer1;

    public GameObject PopupPanel;
    // 팝업
    public GameObject popupPrefab; // 팝업 프리팹에 대한 참조
    private GameObject currentPopup;

    // 캐릭터 삭제를 위한 변수
    public GameObject confirmationPopup; // 확인 팝업 오브젝트
    public TextMeshProUGUI confirmationText; // 팝업에 표시될 텍스트
    public Button confirmButton; // 확인 버튼
    public Button cancelButton; // 취소 버튼
    private Action confirmAction; // 확인 버튼을 눌렀을 때 실행할 액션



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;      
    }

    private void Start()
    {
        if (GameManager.Instance.IsLoggedIn == true &&
            GameManager.Instance.IsMultiplayer == false ||
            GameManager.Instance.IsMultiplayer == true)
        {
            panelList.SetActive(true);
            startButton.gameObject.SetActive(false);
            loginPanel.SetActive(false);
            choicePanel.SetActive(true);
        }
        startButton.onClick.AddListener(StartClick);
        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }
    private void OnDestroy()
    {
        // 씬 전환 시 Instance를 null로 초기화
        if (Instance == this)
        {
            Instance = null;
        }
    }
    public void StartClick()
    {
        cameraVer1.CameraMove();
        startButton.gameObject.SetActive(false);
        loginPanel.SetActive(true);
    }
    public void LoginClick()
    {
        GameManager.Instance.IsLoggedIn = true;
        loginPanel.SetActive(false);
        choicePanel.SetActive(true);
        signPanel.SetActive(false);
    }
    public void SignClick()
    {
        loginPanel.SetActive(false);
        signPanel.SetActive(true);
    }
    public void SignOkClick()
    {
        loginPanel.SetActive(true);
        signPanel.SetActive(false);
    }
    public void SignCancelClick()
    {
        loginPanel.SetActive(true);
        signPanel.SetActive(false);
    }
    //public void OptionClick()
    //{
    //    choicePanel.SetActive(false);
    //    optionPanel.SetActive(true);
    //}
    public void SingleClick()
    {
        choicePanel.SetActive(false);
        CharPanel.SetActive(true);
    }

    public void MultipleClick()
    {
        GameManager.Instance.IsMultiplayer = true;
        choicePanel.SetActive(false);
        CharPanel.SetActive(true);
    }
    public void LogOutClick()
    {
        loginPanel.SetActive(true);
        CharPanel.SetActive(false);
        choicePanel.SetActive(false);
    }
    // 옵션 
    //public void OptionSaveClick()
    //{
    //    //optionPanel.SetActive(false);
    //    choicePanel.SetActive(true);
    //}
    //public void OptionExitClick()
    //{
    //    //optionPanel.SetActive(false);
    //    choicePanel.SetActive(true);
    //}
    public void ChoiceBackClick()
    {
        GameManager.Instance.IsMultiplayer = false;
        choicePanel.SetActive(true);
        CharPanel.SetActive(false);
    }

    public void ShowPopup(string message)
    {

        //// 현재 팝업이 이미 존재하면 제거
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }
        // 팝업 인스턴스 생성 및 설정
        var popupPanels = Instantiate(popupPrefab, PopupPanel.transform);
        StartUIPopup popupPanel = popupPanels.GetComponent<StartUIPopup>();
        popupPanel.PopupMessage(message);
        popupPanel.okBtn.onClick.AddListener(() =>
        {
            Destroy(popupPanels);
            currentPopup = null;
        });
        currentPopup = popupPanels;
    }

    public void ShowConfirmationPopup(string message, Action onConfirm)
    {
        confirmationText.text = message;
        confirmAction = onConfirm;
        confirmationPopup.SetActive(true);
    }

    private void OnConfirm()
    {
        confirmAction?.Invoke(); // 확인 액션 실행
        confirmationPopup.SetActive(false); // 팝업 닫기
    }

    private void OnCancel()
    {
        confirmationPopup.SetActive(false); // 팝업 닫기
    }



}

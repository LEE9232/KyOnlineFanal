using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance { get; private set; }
    // �α����г�
    public Button startButton;
    public GameObject loginPanel;
    public GameObject signPanel;
    public GameObject choicePanel;
    //public GameObject optionPanel;
    public GameObject CharPanel;
    public GameObject panelList;
    public CameraVer1 cameraVer1;

    public GameObject PopupPanel;
    // �˾�
    public GameObject popupPrefab; // �˾� �����տ� ���� ����
    private GameObject currentPopup;

    // ĳ���� ������ ���� ����
    public GameObject confirmationPopup; // Ȯ�� �˾� ������Ʈ
    public TextMeshProUGUI confirmationText; // �˾��� ǥ�õ� �ؽ�Ʈ
    public Button confirmButton; // Ȯ�� ��ư
    public Button cancelButton; // ��� ��ư
    private Action confirmAction; // Ȯ�� ��ư�� ������ �� ������ �׼�



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
        // �� ��ȯ �� Instance�� null�� �ʱ�ȭ
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
    // �ɼ� 
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

        //// ���� �˾��� �̹� �����ϸ� ����
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }
        // �˾� �ν��Ͻ� ���� �� ����
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
        confirmAction?.Invoke(); // Ȯ�� �׼� ����
        confirmationPopup.SetActive(false); // �˾� �ݱ�
    }

    private void OnCancel()
    {
        confirmationPopup.SetActive(false); // �˾� �ݱ�
    }



}

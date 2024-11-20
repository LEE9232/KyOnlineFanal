using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    public UIPanelManager panelManager;
    public Button singleButton;
    public Button multipleButton;
    public Button optionButton;
    public Button logoutButton;
    private void Awake()
    {
        singleButton.onClick.AddListener(SingleButtonClick);
        multipleButton.onClick.AddListener(MultipleButtonClick);
        optionButton.onClick.AddListener(OptionButtonClick);
        logoutButton.onClick.AddListener(LogoutButtonClick);
    }

    public void SingleButtonClick()
    { 
        panelManager.SingleClick();
        Debug.Log("�̱��÷��� ��ư Ŭ��");
    }
    public void MultipleButtonClick()
    {
        panelManager.MultipleClick();
        Debug.Log("��Ƽ�÷��� ��ư Ŭ��");
    }
    public void OptionButtonClick()
    {
        //panelManager.OptionClick();
        Debug.Log("�ɼ� â ��ư Ŭ��");
    }
    public void LogoutButtonClick()
    {
        panelManager.LogOutClick();

        Debug.Log("�α׾ƿ� ��ư Ŭ��");
    }

}

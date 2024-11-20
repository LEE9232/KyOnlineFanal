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
        Debug.Log("싱글플레이 버튼 클릭");
    }
    public void MultipleButtonClick()
    {
        panelManager.MultipleClick();
        Debug.Log("멀티플레이 버튼 클릭");
    }
    public void OptionButtonClick()
    {
        //panelManager.OptionClick();
        Debug.Log("옵션 창 버튼 클릭");
    }
    public void LogoutButtonClick()
    {
        panelManager.LogOutClick();

        Debug.Log("로그아웃 버튼 클릭");
    }

}

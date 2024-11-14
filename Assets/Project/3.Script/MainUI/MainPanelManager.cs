using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPanelManager : MonoBehaviour
{
    public static MainPanelManager Instance { get; private set; }
    public OptionPanel optionPanelScript;
    public GameObject menuPanel;
    public GameObject statusPanel;
    public GameObject optionPanel;
    public GameObject inventoryPanel;
    //public GameObject EquipPanel;
    public GameObject skillPanel;
    public GameObject shopPanel;

    public GameObject partyPanel;
    public GameObject QuestPanel;

    public GameObject questNPCpanel;


    public Button menuBtn;
    public Button optionsaveBtn;
    public Button optioncancelBtn;
    public Button statuscancelBtn;
    public Button invencancelBtn;
    public Button skillcloseBtn;
    public Button shopcloseBtn;
    public Button questcloseBtn;
    private void Awake()
    {
        // 중복 방지 로직
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        menuBtn.onClick.AddListener(MenuBtnClick);
        optionsaveBtn.onClick.AddListener(OptionSaveClick);
        optioncancelBtn.onClick.AddListener(OptionCloseClick);
        statuscancelBtn.onClick.AddListener(StatusCloseBtnClick);
        invencancelBtn.onClick.AddListener(InventoryCloseClick);
        skillcloseBtn.onClick.AddListener(SkillCloseClick);
        shopcloseBtn.onClick.AddListener(ShopCloseClick);
        questcloseBtn.onClick.AddListener(QuestCloseClick);
    }


    private void Update()
    {
        KeyValueInput();
    }

    public void MenuBtnClick()
    {
        menuPanel.SetActive(true);
    }

    public void StatusCloseBtnClick()
    {
        statusPanel.SetActive(false);
    }
    public void OptionSaveClick()
    {
        optionPanel.SetActive(false);
    }
    public void OptionCloseClick()
    {
        optionPanel.SetActive(false);
    }
    public void InventoryCloseClick()
    { 
        inventoryPanel.SetActive(false);
        //EquipPanel.SetActive(false);
    }
    public void SkillCloseClick()
    { 
        skillPanel.SetActive(false);
    }
    public void ShopCloseClick()
    {
        shopPanel.SetActive(false);
    }
    public void QuestCloseClick()
    {
        questNPCpanel.SetActive(false);
    }

    public void KeyValueInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(!menuPanel.activeSelf);
            OptionCloseClick();
            StatusCloseBtnClick();
            InventoryCloseClick();
            SkillCloseClick();
            ShopCloseClick();
            QuestCloseClick();
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
            //EquipPanel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            //optionPanel.SetActive(!optionPanel.activeSelf);
            if (!optionPanel.activeSelf)
            {
                optionPanelScript.OptionBtnClick();  // 옵션 패널 열기
            }
            else
            {   
                optionPanelScript.OptionSaveBtClick();
                optionPanelScript.OptionExitBtClick();  // 옵션 패널 닫기
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            statusPanel.SetActive(!statusPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            skillPanel.SetActive(!skillPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.P))
        { 
            partyPanel.SetActive(!partyPanel.activeSelf);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            QuestPanel.SetActive(!QuestPanel.activeSelf);
        }
    }


}

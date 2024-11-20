using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
public class LobbyPanelManager : MonoBehaviour
{
    //public LobbyPopUpPanel popupPanel;
    public GameObject popupPrefab;
    public Transform popupPos;

    public LobbyRoomPanel roomPanel;
    public LobbyCreateRoomPanel createRoomPanel;
    public Button roomcreateBtn;
    public Button roomupdateBtn;
    public Button backBtn;

    [SerializeField] private LobbyPopUpPanel popUp;
    private GameObject currentPopup;
    private void Awake()
    {
        roomcreateBtn.onClick.AddListener(RoomCreateBtnClick);
        roomupdateBtn.onClick.AddListener(RoomUpdateBtnClick);
        backBtn.onClick.AddListener(BackBtnClick);
    }

    public void RoomCreateBtnClick()
    {
        createRoomPanel.gameObject.SetActive(true);

    }

    public void RoomUpdateBtnClick()
    {

    }

    public void BackBtnClick()
    {
        DestroyCurrentPopup();
        GameObject popupObj = Instantiate(popupPrefab, popupPos.position, Quaternion.identity);
        LobbyPopUpPanel popUpPanel = popupObj.GetComponent<LobbyPopUpPanel>();
        popupObj.transform.parent = popupPos;
        popupObj.transform.position = popupPos.transform.position;
        popUpPanel.yesBtn.onClick.AddListener(popUpPanel.LobbyYesBtnClick);
        popUpPanel.noBtn.onClick.AddListener(popUpPanel.LobbyNoBtnClick);
        popUpPanel.popupText.text = "로비로 나가시겠습니까?";
        currentPopup = popupObj;
        popUpPanel.yesBtn.onClick.AddListener(() => DestroyCurrentPopup());
        popUpPanel.noBtn.onClick.AddListener(() => DestroyCurrentPopup());
    }


    // 입장 버튼 클릭시 
    public void ButtonRoomInClick()
    {
        DestroyCurrentPopup();
        //if (currentPopup != null) return;
        //ShowPopup("방에 입장하시겠습니까?", popUp.RoomInYesBtn, popUp.RoomInNoBtn);
        //popUp.yesBtn.onClick.AddListener(() => DestroyCurrentPopup());
        //popUp.noBtn.onClick.AddListener(() => DestroyCurrentPopup());
        GameObject popupObj = Instantiate(popupPrefab, popupPos.transform.position, Quaternion.identity);
        LobbyPopUpPanel popUpPanel = popupObj.GetComponent<LobbyPopUpPanel>();
        popupObj.transform.parent = popupPos;
        popupObj.transform.position = popupPos.transform.position;
        popUpPanel.yesBtn.onClick.AddListener(popUpPanel.RoomInYesBtn);
        popUpPanel.noBtn.onClick.AddListener(popUpPanel.RoomInNoBtn);
        popUpPanel.popupText.text = "방에 입장하시겠습니까?";   
        currentPopup = popupObj;
        popUpPanel.yesBtn.onClick.AddListener(() => DestroyCurrentPopup());
        popUpPanel.noBtn.onClick.AddListener(() => DestroyCurrentPopup());
    }

    private void DestroyCurrentPopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            currentPopup = null;
        }
    }
    //private void ShowPopup(string message, UnityAction yesAction, UnityAction noAction)
    //{
    //    //DestroyCurrentPopup();
    //    GameObject popupObj = Instantiate(popupPrefab, popupPos.position, Quaternion.identity);
    //    LobbyPopUpPanel popUpPanel = popupObj.GetComponent<LobbyPopUpPanel>();
    //    popupObj.transform.parent = popupPos;
    //    popupObj.transform.position = popupPos.transform.position;
    //    popUpPanel.yesBtn.onClick.AddListener(yesAction);
    //    popUpPanel.noBtn.onClick.AddListener(noAction);
    //    popUpPanel.popupText.text = message;
    //    currentPopup = popupObj;
    //    // 팝업이 닫힐 때 플래그를 초기화
    //
    //    //popUpPanel.yesBtn.onClick.AddListener(() => DestroyCurrentPopup());
    //    //popUpPanel.noBtn.onClick.AddListener(() => DestroyCurrentPopup());
    //}



}

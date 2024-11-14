using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyRoomPanel : MonoBehaviour
{
    public GameObject RoomSlotPrefeb;
    public Transform Contentpos;
    [SerializeField] private LobbyPanelManager lobbyPanelManager;



    public TextMeshProUGUI RoomInfoText;
    public GameObject tooltip;
    public LobbyCreateRoomPanel roomInfoSlot;
   
    
    //public LobbyPopUpPanel popupPanel;



    public void CreateRoom()
    { 
        GameObject roomslotObj = Instantiate(RoomSlotPrefeb, Contentpos);
        RoomSlot roomSlot = roomslotObj.GetComponent<RoomSlot>();
        // 방 정보를 roomInfoSlot에서 받아 설정
        string roomMessage = roomInfoSlot.roomInfoInput.text;
        string roomName = roomInfoSlot.roomNameInput.text;
        roomSlot.roomInfo = roomMessage; // RoomSlot에 방 정보 저장
        roomSlot.SetTooltip(tooltip, RoomInfoText);
        roomSlot.SetRoomName(roomName);
        roomSlot.Initialize(lobbyPanelManager);

        //lobbyPanelManager.ButtonRoomInClick();

        //roomSlot.SetPopupPanel(popupPanel);
    }
    
}

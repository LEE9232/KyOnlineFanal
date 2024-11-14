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
        // �� ������ roomInfoSlot���� �޾� ����
        string roomMessage = roomInfoSlot.roomInfoInput.text;
        string roomName = roomInfoSlot.roomNameInput.text;
        roomSlot.roomInfo = roomMessage; // RoomSlot�� �� ���� ����
        roomSlot.SetTooltip(tooltip, RoomInfoText);
        roomSlot.SetRoomName(roomName);
        roomSlot.Initialize(lobbyPanelManager);

        //lobbyPanelManager.ButtonRoomInClick();

        //roomSlot.SetPopupPanel(popupPanel);
    }
    
}

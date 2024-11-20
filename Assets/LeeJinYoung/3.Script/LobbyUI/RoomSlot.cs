using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSlot : MonoBehaviour
{
    public Button roominBtn;
    public GameObject popupPrefab;
    //public Transform popupPos;


    [SerializeField] private LobbyPanelManager lobbyPanelManager;



    //private LobbyPopUpPanel popupPanel;





    public string roomInfo;                 // 방 정보 문자열

    private CharTooltip charTooltip;        // CharTooltip 레퍼런스

    private TextMeshProUGUI buttonText;     // 버튼의 텍스트 컴포넌트




    private void Awake()
    {
        roominBtn.onClick.AddListener(RoomInBtnClick);
        // CharTooltip 컴포넌트를 가져와서 참조
        charTooltip = gameObject.AddComponent<CharTooltip>();
        // 버튼의 텍스트 컴포넌트를 찾아서 설정
        buttonText = roominBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = roomInfo;  // 방 정보로 버튼 텍스트 설정
        }
        // roomInfo 필드를 CharTooltip에 설정
        charTooltip.message = roomInfo;

    }

    public void Initialize(LobbyPanelManager manager)
    {
        lobbyPanelManager = manager;
    }



    public void RoomInBtnClick()
    {
        lobbyPanelManager.ButtonRoomInClick();      
    }
    //public void SetPopupPanel(LobbyPopUpPanel panel)
    //{
    //    popupPanel = panel;
    //}

    // 툴팁 설정을 위한 메서드 추가 (동적으로 설정할 수 있도록)
    public void SetTooltip(GameObject tooltip, TextMeshProUGUI tooltipText)
    {
        if (charTooltip != null)
        {
            charTooltip.tooltip = tooltip;
            charTooltip.tooltipText = tooltipText;
            charTooltip.message = roomInfo; // 저장된 roomInfo를 툴팁 메시지로 설정
        }
    }
    public void SetRoomName(string roomName)
    {
        if (buttonText != null)
        {
            buttonText.text = roomName;
        }
    }




}

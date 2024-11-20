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





    public string roomInfo;                 // �� ���� ���ڿ�

    private CharTooltip charTooltip;        // CharTooltip ���۷���

    private TextMeshProUGUI buttonText;     // ��ư�� �ؽ�Ʈ ������Ʈ




    private void Awake()
    {
        roominBtn.onClick.AddListener(RoomInBtnClick);
        // CharTooltip ������Ʈ�� �����ͼ� ����
        charTooltip = gameObject.AddComponent<CharTooltip>();
        // ��ư�� �ؽ�Ʈ ������Ʈ�� ã�Ƽ� ����
        buttonText = roominBtn.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = roomInfo;  // �� ������ ��ư �ؽ�Ʈ ����
        }
        // roomInfo �ʵ带 CharTooltip�� ����
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

    // ���� ������ ���� �޼��� �߰� (�������� ������ �� �ֵ���)
    public void SetTooltip(GameObject tooltip, TextMeshProUGUI tooltipText)
    {
        if (charTooltip != null)
        {
            charTooltip.tooltip = tooltip;
            charTooltip.tooltipText = tooltipText;
            charTooltip.message = roomInfo; // ����� roomInfo�� ���� �޽����� ����
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

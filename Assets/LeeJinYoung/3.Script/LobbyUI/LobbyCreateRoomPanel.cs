using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateRoomPanel : MonoBehaviour
{
    public LobbyPanelManager lobbyPanelManager;
    public LobbyRoomPanel roomPanel;
    public TMP_InputField roomNameInput;  // �� �̸�
    public TMP_InputField roomInfoInput;  // �� ����
    public TextMeshProUGUI roomNameText;
    //public TextMeshProUGUI roomInfoText; // �� ���� �ؽ�Ʈ

    public Button createRoomokBtn;
    public Button createRoomcancelBtn;

    private void Awake()
    {
        createRoomokBtn.onClick.AddListener(CreateOKBtnClick);
        createRoomcancelBtn.onClick.AddListener(CreateCancelBtnClick);
    }

    public void RoomName()
    {
        roomNameText.text = roomNameText.text;
    }
    //public void RoomInfo()
    //{
    //    roomInfoText.text = roomInfoInput.text;
    //}



    public void CreateOKBtnClick()
    {
        roomPanel.CreateRoom();
        lobbyPanelManager.createRoomPanel.gameObject.SetActive(false);
    }
    public void CreateCancelBtnClick()
    {
        lobbyPanelManager.createRoomPanel.gameObject.SetActive(false);
    }
}

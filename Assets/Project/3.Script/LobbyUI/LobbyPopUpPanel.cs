using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPopUpPanel : MonoBehaviour
{
    public Button yesBtn;
    public Button noBtn;
    public GameObject popupPanel;

    public TextMeshProUGUI popupText;
    
    public void LobbyYesBtnClick()
    {
        GameManager.Instance.IsMultiplayer = false;
        
        Changescenemaneger.Instance.StartScecn();

    }
    public void LobbyNoBtnClick()
    {
        //popupPanel.SetActive(false);
        Destroy(popupPanel);



    }

    public void CreateRoomClick()
    {
        
    }


    public void RoomInYesBtn()
    {       
        Changescenemaneger.Instance.StageOneScene();

    }
    public void RoomInNoBtn()
    {
        Destroy(popupPanel);
        //popupPanel.SetActive(false);
        //Changescenemaneger.Instance.StartScecn();
    }

    public void PopupMessage(string msg)
    {
        popupText.text = msg;
    }

}

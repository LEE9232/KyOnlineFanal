using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUIPopup : MonoBehaviour
{
    public Button okBtn;
    public GameObject popupPanel;
    public TextMeshProUGUI popupText;

    public void OkBtnClick()
    {
        Destroy(popupPanel);
    }

    public void PopupMessage(string msg)
    {
        popupText.text = msg;
    }
}

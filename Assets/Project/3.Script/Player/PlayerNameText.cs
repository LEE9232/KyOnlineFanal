using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNameText : MonoBehaviour
{
    public Transform nameTagPosition;
    public Camera mainCamera; 
    public TextMeshProUGUI nameText;
    private void Start()
    {
        mainCamera = Camera.main;
        nameText.text = GameManager.Instance.PlayerData.NickName;
        nameText.raycastTarget = false; // 코드로 설정하는 방법
    }
    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            nameTagPosition.rotation = Quaternion.LookRotation(nameTagPosition.position - mainCamera.transform.position);
        }
    }
}

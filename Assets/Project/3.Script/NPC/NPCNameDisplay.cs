using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCNameDisplay : MonoBehaviour
{
    public string NPCname;
    public Transform nameTagPosition;
    public Camera mainCamera;
    public TextMeshProUGUI nameText;
    private void Start()
    {
        mainCamera = Camera.main;
        nameText.text = NPCname;
    }

    private void LateUpdate()
    {
        if (mainCamera != null)
        {
            nameTagPosition.rotation = Quaternion.LookRotation(nameTagPosition.position - mainCamera.transform.position);
        }
    }
}

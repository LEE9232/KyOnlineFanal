using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject shopUI; // ���� UI
    public GameObject inventoryPanel;
    public GameObject PopUpUI;
    public Button shopBtn;
    public TextMeshProUGUI popupText;
    private MinimapIconManager minimapIconManager; 

    private void Start()
    {
        shopPanel.SetActive(true);
        shopBtn.onClick.AddListener(OpenShop);
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCNoQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            shopBtn.gameObject.SetActive(true); // ���� �̸� ��ư Ȱ��ȭ
            PopUpUI.SetActive(true);
            popupText.text = "������ ���͵帱���?";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseShop();
            shopBtn.gameObject.SetActive(false); // ���� �̸� ��ư ��Ȱ��ȭ
            popupText.text = "�ٽ� ã���ּ���";
            StartCoroutine(PopupClose());
        }
    }

    private void OpenShop()
    {
        shopUI.SetActive(true); // ���� UI Ȱ��ȭ
        inventoryPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false); // ���� UI �ݱ�
        inventoryPanel.SetActive(false);
    }
    IEnumerator PopupClose()
    {
        yield return new WaitForSeconds(4.0f);
        PopUpUI.SetActive(false);
    }
}

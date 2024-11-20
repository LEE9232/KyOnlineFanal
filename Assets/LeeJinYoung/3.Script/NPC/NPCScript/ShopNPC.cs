using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject shopUI; // 상점 UI
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
            Debug.LogError("minimapIconManager 가 널입니다.");
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            shopBtn.gameObject.SetActive(true); // 상인 이름 버튼 활성화
            PopUpUI.SetActive(true);
            popupText.text = "무엇을 도와드릴까요?";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CloseShop();
            shopBtn.gameObject.SetActive(false); // 상인 이름 버튼 비활성화
            popupText.text = "다시 찾아주세요";
            StartCoroutine(PopupClose());
        }
    }

    private void OpenShop()
    {
        shopUI.SetActive(true); // 상점 UI 활성화
        inventoryPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopUI.SetActive(false); // 상점 UI 닫기
        inventoryPanel.SetActive(false);
    }
    IEnumerator PopupClose()
    {
        yield return new WaitForSeconds(4.0f);
        PopUpUI.SetActive(false);
    }
}

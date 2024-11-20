using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemDeletePopup : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TMP_InputField quantityInput;
    public Button YesButton;
    public Button NoButton;
    private ItemData currentItem;
    private InventorySlot currentSlot;
    private bool isListenerRegistered = false;
    public void InitializePopup(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;
        itemNameText.text = $"{item.itemName} 을(를)\n버리시겠습니까?";
        quantityInput.text = "1"; // 기본값은 1로 설정
        YesButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        YesButton.onClick.AddListener(OnOkBtnClick);
        NoButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        NoButton.onClick.AddListener(OnNoBtnClick); // 새로운 리스너 등록
    }

    public void InitializeSellPopup(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;
        itemNameText.text = $"{item.itemName} 을(를)\n판매하시겠습니까?";
        quantityInput.text = "1"; // 기본값은 1로 설정
        YesButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        YesButton.onClick.AddListener(OnSaleOkBtnClick);
        NoButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
        NoButton.onClick.AddListener(OnSaleNoBtnClick); // 새로운 리스너 등록
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnOkBtnClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnNoBtnClick();
        }
    }

    private void OnOkBtnClick()
    {
        int quantityToDiscard = int.Parse(quantityInput.text);
        if (quantityToDiscard > 0 && quantityToDiscard <= currentItem.itemquantity)
        {
            // 선택한 수량만큼 아이템 버리기
            currentSlot.DiscardItemWithQuantity(quantityToDiscard);
            gameObject.SetActive(false); // 팝업 창 닫기
            GameManager.Instance.inventoryUI.UpdateInventoryUI();
        }
        else
        {
            Debug.LogError("잘못된 수량 입력");
        }
    }
    private void OnNoBtnClick()
    {
        gameObject.SetActive(false); // 팝업 창 닫기
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
    }

    private void OnSaleOkBtnClick()
    {
        int quantityToSell = int.Parse(quantityInput.text);
        if (quantityToSell > 0 && quantityToSell <= currentItem.itemquantity)
        {
            // 선택한 수량만큼 아이템 버리기
            currentSlot.DiscardItemWithQuantity(quantityToSell);

            // 판매 수량을 고려한 판매 처리
            GameManager.Instance.shopManager.SellItemWithQuantity(currentItem, quantityToSell);


            //GameManager.Instance.shopManager.SellItem(currentItem); // 판매 처리
            gameObject.SetActive(false); // 팝업 창 닫기
            //GameManager.Instance.inventoryUI.UpdateInventoryUI();
            GameManager.Instance.inventoryUI.UpdateGoldUI();
        }
        else
        {
            Debug.LogError("잘못된 수량 입력");
        }
    }
    private void OnSaleNoBtnClick()
    {
        gameObject.SetActive(false); // 팝업 창 닫기
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
    }


}

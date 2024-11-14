using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;     // 아이템 이미지
    public Button buyButton;    // 구매 버튼

    private ItemData currentItem;  // 현재 슬롯에 표시된 아이템 정보
    private ShopManager shopManager;  // 상점 매니저
    private ItemTooltip itemTooltip;  // 아이템 툴팁 참조

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClick);
        // 상점 매니저에서 ItemTooltip을 가져옴
        itemTooltip = FindObjectOfType<ItemTooltip>();
    }

    // 슬롯에 아이템 정보를 표시하는 함수
    public void SetSlot(ItemData item, ShopManager manager)
    {
        currentItem = item;
        shopManager = manager;
        // 아이템 이미지를 로드하여 설정
        itemImage.sprite = Resources.Load<Sprite>(item.itemImage);  // Resources 폴더에 있는 이미지 경로
    }
    // 구매 버튼이 클릭되었을 때 호출되는 함수
    private void OnBuyButtonClick()
    {
        if (currentItem != null && shopManager != null)
        {
            shopManager.BuyItem(currentItem);
            //Debug.Log($"{currentItem} 클릭 ");
        }
    }
    // 마우스가 슬롯 위로 올라갔을 때 호출되는 함수
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemTooltip != null && currentItem != null)
        {
            itemTooltip.ShowTooltip(currentItem); // 툴팁을 표시
        }
    }

    // 마우스가 슬롯에서 벗어났을 때 호출되는 함수
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemTooltip != null)
        {
            itemTooltip.HideTooltip(); // 툴팁을 숨김
        }
    }
}

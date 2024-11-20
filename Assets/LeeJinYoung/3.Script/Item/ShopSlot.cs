using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;     // ������ �̹���
    public Button buyButton;    // ���� ��ư

    private ItemData currentItem;  // ���� ���Կ� ǥ�õ� ������ ����
    private ShopManager shopManager;  // ���� �Ŵ���
    private ItemTooltip itemTooltip;  // ������ ���� ����

    private void Start()
    {
        buyButton.onClick.AddListener(OnBuyButtonClick);
        // ���� �Ŵ������� ItemTooltip�� ������
        itemTooltip = FindObjectOfType<ItemTooltip>();
    }

    // ���Կ� ������ ������ ǥ���ϴ� �Լ�
    public void SetSlot(ItemData item, ShopManager manager)
    {
        currentItem = item;
        shopManager = manager;
        // ������ �̹����� �ε��Ͽ� ����
        itemImage.sprite = Resources.Load<Sprite>(item.itemImage);  // Resources ������ �ִ� �̹��� ���
    }
    // ���� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ�
    private void OnBuyButtonClick()
    {
        if (currentItem != null && shopManager != null)
        {
            shopManager.BuyItem(currentItem);
            //Debug.Log($"{currentItem} Ŭ�� ");
        }
    }
    // ���콺�� ���� ���� �ö��� �� ȣ��Ǵ� �Լ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemTooltip != null && currentItem != null)
        {
            itemTooltip.ShowTooltip(currentItem); // ������ ǥ��
        }
    }

    // ���콺�� ���Կ��� ����� �� ȣ��Ǵ� �Լ�
    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemTooltip != null)
        {
            itemTooltip.HideTooltip(); // ������ ����
        }
    }
}

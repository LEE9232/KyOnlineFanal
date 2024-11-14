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
        itemNameText.text = $"{item.itemName} ��(��)\n�����ðڽ��ϱ�?";
        quantityInput.text = "1"; // �⺻���� 1�� ����
        YesButton.onClick.RemoveAllListeners(); // ���� ������ ����
        YesButton.onClick.AddListener(OnOkBtnClick);
        NoButton.onClick.RemoveAllListeners(); // ���� ������ ����
        NoButton.onClick.AddListener(OnNoBtnClick); // ���ο� ������ ���
    }

    public void InitializeSellPopup(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;
        itemNameText.text = $"{item.itemName} ��(��)\n�Ǹ��Ͻðڽ��ϱ�?";
        quantityInput.text = "1"; // �⺻���� 1�� ����
        YesButton.onClick.RemoveAllListeners(); // ���� ������ ����
        YesButton.onClick.AddListener(OnSaleOkBtnClick);
        NoButton.onClick.RemoveAllListeners(); // ���� ������ ����
        NoButton.onClick.AddListener(OnSaleNoBtnClick); // ���ο� ������ ���
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
            // ������ ������ŭ ������ ������
            currentSlot.DiscardItemWithQuantity(quantityToDiscard);
            gameObject.SetActive(false); // �˾� â �ݱ�
            GameManager.Instance.inventoryUI.UpdateInventoryUI();
        }
        else
        {
            Debug.LogError("�߸��� ���� �Է�");
        }
    }
    private void OnNoBtnClick()
    {
        gameObject.SetActive(false); // �˾� â �ݱ�
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
    }

    private void OnSaleOkBtnClick()
    {
        int quantityToSell = int.Parse(quantityInput.text);
        if (quantityToSell > 0 && quantityToSell <= currentItem.itemquantity)
        {
            // ������ ������ŭ ������ ������
            currentSlot.DiscardItemWithQuantity(quantityToSell);

            // �Ǹ� ������ ����� �Ǹ� ó��
            GameManager.Instance.shopManager.SellItemWithQuantity(currentItem, quantityToSell);


            //GameManager.Instance.shopManager.SellItem(currentItem); // �Ǹ� ó��
            gameObject.SetActive(false); // �˾� â �ݱ�
            //GameManager.Instance.inventoryUI.UpdateInventoryUI();
            GameManager.Instance.inventoryUI.UpdateGoldUI();
        }
        else
        {
            Debug.LogError("�߸��� ���� �Է�");
        }
    }
    private void OnSaleNoBtnClick()
    {
        gameObject.SetActive(false); // �˾� â �ݱ�
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
    }


}

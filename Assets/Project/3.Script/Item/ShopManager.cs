using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public List<int> weaponItemIDs;  // �������� �Ǹ��� ���� �������� ID ����Ʈ
    public List<int> armorItemIDs;   // �������� �Ǹ��� �� �������� ID ����Ʈ
    public List<int> accessoryItemIDs;  // �������� �Ǹ��� ��ű� �������� ID ����Ʈ

    public Button buyBtn;
    public Button saleBtn;

    public GameObject buyPanel;
    public GameObject salePanel;

    //public List<ShopSlot> shopSlots;  // ���� ���Ե�
    public Transform shopSlotParent;  // ���� ������ ��ġ�� �θ� ��ü
    public GameObject shopSlotPrefab; // ���� ���� ������
    private ItemDatabase itemDatabase; // ������ �����͸� �ҷ��� �����ͺ��̽�
    private PlayerInfo playerInfo;      // �÷��̾� ���� ����

    public GameObject deleteZone;
    public InventoryItemDeletePopup quantityPopup; // �˾� ����
    public ChoicePopup choicePopup;

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyBtnClick);
        saleBtn.onClick.AddListener(SaleBtnClick);
        // ������ �����ͺ��̽� ����
        itemDatabase = ItemDatabase.Instance;
        playerInfo = GameManager.Instance.PlayerData;  // GameManager���� PlayerInfo ��������
        SetupShopSlots();  // ���� ���� ����
    }

    // ���� ������ �����ϴ� �Լ�
    public void SetupShopSlots()
    {
        foreach (int id in weaponItemIDs)
        {
            ItemData weapon = itemDatabase.GetItemById(id);  // ID�� ���� ������ ��������
            if (weapon != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // ���� ����
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(weapon, this);  // ���Կ� ������ ���� ����
            }
            else
            {
                Debug.LogError($"������ ID {id}�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
            }
        }

        // �� ������ ���� ����
        foreach (int id in armorItemIDs)
        {
            ItemData armor = itemDatabase.GetItemById(id);  // ID�� �� ������ ��������
            if (armor != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // ���� ����
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(armor, this);  // ���Կ� ������ ���� ����
            }
            else
            {
                Debug.LogError($"������ ID {id}�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
            }
        }
        // ��ű� ������ ���� ����
        foreach (int id in accessoryItemIDs)
        {
            ItemData accessory = itemDatabase.GetItemById(id);  // ID�� ��ű� ������ ��������
            if (accessory != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // ���� ����
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(accessory, this);  // ���Կ� ������ ���� ����
            }
            else
            {
                Debug.LogError($"������ ID {id}�� �ش��ϴ� �������� ã�� �� �����ϴ�.");
            }
        }
    }

    // �������� �����ϴ� �Լ�
    public void BuyItem(ItemData item)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        // �κ��丮 �Ŵ����� null�� ��� ���� �α�
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is null!");
            return;
        }
        // �κ��丮�� �������� �߰��� ������ �ִ��� Ȯ��
        if (!inventoryManager.HasSpaceForItem(item))
        {
            GameManager.Instance.logUI.AddMessage($"<color=red>�κ��丮�� {item.itemName}��(��) �߰��� ������ �����մϴ�!</color>");
            return;
        }
        // ��尡 ������� Ȯ�� �� ����
        if (playerInfo.SpendGold(item.buyPrice))
        {
            // �κ��丮�� ������ �߰�
            inventoryManager.AddItem(item);
            GameManager.Instance.inventoryUI.UpdateInventoryUI();  // �κ��丮 UI ������Ʈ
        }
        else
        {
            // ��尡 ������ ��� �޽��� ���
            GameManager.Instance.logUI.AddMessage($"<color=red>��尡 �����մϴ�!</color>");
        }
        //if (playerInfo.SpendGold(item.buyPrice))  // ��尡 ������� Ȯ�� �� ����
        //{
        //    InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        //    // �κ��丮�� ������ �߰�
        //    if (inventoryManager != null)
        //    {
        //        inventoryManager.AddItem(item);
        //        GameManager.Instance.inventoryUI.UpdateInventoryUI();  // �κ��丮 UI ������Ʈ
        //    }
        //    else
        //    {
        //        Debug.LogError("InventoryManager is null!");
        //    }
        //}
        //else
        //{
        //    // �ؽ�ƮȮ��
        //    GameManager.Instance.logUI.AddMessage($"<color=green> Gold </color> �� �����մϴ�!");
        //}
    }
    // ������ �Ǹ� �޼���
    public void SellItem(ItemData item)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;
        if (inventoryManager != null)
        {
            if (inventoryManager.RemoveItem(item))  // �κ��丮���� ������ ����
            {
                //playerInfo.SellItemGold(item.sellPrice);  // �Ǹ� ���ݸ�ŭ ��� ����
                playerInfo.SellItemGold(item.sellPrice * item.itemquantity);  // �Ǹ� ���ݸ�ŭ ��� ����
                GameManager.Instance.inventoryUI.UpdateGoldUI();  // ��� UI ������Ʈ
                GameManager.Instance.inventoryUI.UpdateInventoryUI();  // ��� UI ������Ʈ
                GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color>��(��) {item.sellPrice} ��忡 �Ǹ��߽��ϴ�!");
            }
        }
    }
    // �Ǹ� ���Կ� ��ӵǾ��� �� ó��
    public void OnItemDroppedToSellSlot(ItemData item)
    {
        SellItemWithQuantity(item, item.itemquantity);
    }


    public void ShowQuantityPopup(ItemData item, InventorySlot slot)
    {
        // ���� ���� �˾� â�� Ȱ��ȭ�ϰ�, �����۰� ���� ������ ����
        quantityPopup.gameObject.SetActive(true);
        quantityPopup.InitializeSellPopup(item, slot);
    }
    public void SellItemWithQuantity(ItemData item, int quantityToSell)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;

        if (inventoryManager != null)
        {
            // �κ��丮���� ������ŭ ����
            if (inventoryManager.RemoveItemWithQuantity(item, quantityToSell))
            {
                playerInfo.SellItemGold(item.sellPrice * quantityToSell);
                GameManager.Instance.inventoryUI.UpdateGoldUI();
                GameManager.Instance.inventoryUI.UpdateInventoryUI();
                GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color> {quantityToSell}���� {item.sellPrice * quantityToSell} ��忡 �Ǹ��߽��ϴ�!");
            }
        }
    }


    public void BuyBtnClick()
    {
        buyPanel.SetActive(true);
        salePanel.SetActive(false);
    }
    public void SaleBtnClick()
    {
        salePanel.SetActive(true);
        buyPanel.SetActive(false);
    }

}

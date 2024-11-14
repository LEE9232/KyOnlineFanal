using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemInfo : MonoBehaviour
{
    public int itemID; // ������ ID
    public ItemData itemData; // ������ ������ ��ü
    public Button itemBtn;
    public TextMeshProUGUI itemText;
    public InventoryType itemInventoryType;
    private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� ����

    private void Start()
    {
        //ItemData
        itemData = ItemDatabase.Instance.GetItemById(itemID);
        itemText = GetComponentInChildren<TextMeshProUGUI>();
        if (itemData != null)
        {
            InitializeItem(itemData);
        }
        if (itemBtn != null)
        {
            itemBtn.onClick.AddListener(PickupItem);
        }
        itemBtn.interactable = false; // ó������ ��ư�� ��Ȱ��ȭ
    }

    public void InitializeItem(ItemData data)
    {
        itemData = data;
        if (itemData != null)
        {
            // �������� �����̳� ���¸� ������ �����Ϳ� ���� ����
            gameObject.name = itemData.itemName;
            if (itemText != null)
            {
                itemText.text = itemData.itemName;
            }
        }
    }

    // �÷��̾ ���� �ȿ� ������ �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �±װ� 'Player'�� ������Ʈ�� ����
        {
            isPlayerInRange = true;
            if (itemBtn != null)
            {
                itemBtn.interactable = true;
            }


        }
    }

    // �÷��̾ �������� ������ �� ȣ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (itemBtn != null)
            {
                itemBtn.interactable = false;
            }
        }
    }


    public void PickupItem()
    {
        if (itemData == null)
        {
            Debug.LogError("������ �����Ͱ� null�Դϴ�.");
            return;
        }
        InventoryManager inventory = GameManager.Instance.PlayerData.Inventory;
        // �κ��丮 ���� üũ
        if (itemData is WeaponData && inventory.Weapons.Count >= InventoryManager.MaxWeaponSlots)
        {
            GameManager.Instance.logUI.AddMessage("���� �κ��丮�� ���� á���ϴ�! �������� ȹ���� �� �����ϴ�.");
            return; // ��� �������� �ı����� �ʰ� �״�� ����
        }
        else if (itemData is ArmorData && inventory.Armors.Count >= InventoryManager.MaxArmorSlots)
        {
            GameManager.Instance.logUI.AddMessage("�� �κ��丮�� ���� á���ϴ�! �������� ȹ���� �� �����ϴ�.");
            return; // ��� �������� �ı����� �ʰ� �״�� ����
        }
        else if (itemData is AccessoryData && inventory.Accessories.Count >= InventoryManager.MaxAccessorySlots)
        {
            GameManager.Instance.logUI.AddMessage("��ű� �κ��丮�� ���� á���ϴ�! �������� ȹ���� �� �����ϴ�.");
            return; // ��� �������� �ı����� �ʰ� �״�� ����
        }
        else if (!(itemData is WeaponData) && !(itemData is ArmorData) && !(itemData is AccessoryData) && inventory.Materials.Count >= InventoryManager.MaxMaterialSlots)
        {
            GameManager.Instance.logUI.AddMessage("��� �κ��丮�� ���� á���ϴ�! �������� ȹ���� �� �����ϴ�.");
            return; // ��� �������� �ı����� �ʰ� �״�� ����
        }
        inventory.AddItem(itemData);
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        GameManager.Instance.PlayerData.Inventory.SetInventoryChanged(true);
        inventory.SetInventoryChanged(true);
        Destroy(gameObject);
    }
}

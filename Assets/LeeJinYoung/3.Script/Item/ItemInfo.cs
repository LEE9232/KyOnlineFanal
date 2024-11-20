using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemInfo : MonoBehaviour
{
    public int itemID; // 아이템 ID
    public ItemData itemData; // 아이템 데이터 객체
    public Button itemBtn;
    public TextMeshProUGUI itemText;
    public InventoryType itemInventoryType;
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부

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
        itemBtn.interactable = false; // 처음에는 버튼을 비활성화
    }

    public void InitializeItem(ItemData data)
    {
        itemData = data;
        if (itemData != null)
        {
            // 아이템의 외형이나 상태를 아이템 데이터에 맞춰 설정
            gameObject.name = itemData.itemName;
            if (itemText != null)
            {
                itemText.text = itemData.itemName;
            }
        }
    }

    // 플레이어가 범위 안에 들어왔을 때 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 태그가 'Player'인 오브젝트만 반응
        {
            isPlayerInRange = true;
            if (itemBtn != null)
            {
                itemBtn.interactable = true;
            }


        }
    }

    // 플레이어가 범위에서 나갔을 때 호출
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
            Debug.LogError("아이템 데이터가 null입니다.");
            return;
        }
        InventoryManager inventory = GameManager.Instance.PlayerData.Inventory;
        // 인벤토리 슬롯 체크
        if (itemData is WeaponData && inventory.Weapons.Count >= InventoryManager.MaxWeaponSlots)
        {
            GameManager.Instance.logUI.AddMessage("무기 인벤토리가 가득 찼습니다! 아이템을 획득할 수 없습니다.");
            return; // 드랍 아이템을 파괴하지 않고 그대로 유지
        }
        else if (itemData is ArmorData && inventory.Armors.Count >= InventoryManager.MaxArmorSlots)
        {
            GameManager.Instance.logUI.AddMessage("방어구 인벤토리가 가득 찼습니다! 아이템을 획득할 수 없습니다.");
            return; // 드랍 아이템을 파괴하지 않고 그대로 유지
        }
        else if (itemData is AccessoryData && inventory.Accessories.Count >= InventoryManager.MaxAccessorySlots)
        {
            GameManager.Instance.logUI.AddMessage("장신구 인벤토리가 가득 찼습니다! 아이템을 획득할 수 없습니다.");
            return; // 드랍 아이템을 파괴하지 않고 그대로 유지
        }
        else if (!(itemData is WeaponData) && !(itemData is ArmorData) && !(itemData is AccessoryData) && inventory.Materials.Count >= InventoryManager.MaxMaterialSlots)
        {
            GameManager.Instance.logUI.AddMessage("재료 인벤토리가 가득 찼습니다! 아이템을 획득할 수 없습니다.");
            return; // 드랍 아이템을 파괴하지 않고 그대로 유지
        }
        inventory.AddItem(itemData);
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        GameManager.Instance.PlayerData.Inventory.SetInventoryChanged(true);
        inventory.SetInventoryChanged(true);
        Destroy(gameObject);
    }
}

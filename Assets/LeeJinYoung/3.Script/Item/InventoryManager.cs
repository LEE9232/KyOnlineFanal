using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public List<ItemData> inventoryItems; // { get; set; } // 통합된 아이템 리스트
    public List<WeaponData> Weapons { get; set; }
    public List<ArmorData> Armors { get; set; }
    public List<AccessoryData> Accessories { get; set; }
    public List<ItemData> Materials { get; set; } // 재료 아이템
    public WeaponData EquippedWeapon { get; private set; }
    public ArmorData EquippedArmor { get; private set; }
    public AccessoryData EquippedAccessory { get; private set; }

    private bool isInventoryChanged; // 변경 여부를 나타내는 플래그
    public const int MaxWeaponSlots = 20;
    public const int MaxArmorSlots = 20;
    public const int MaxAccessorySlots = 20;
    public const int MaxMaterialSlots = 20;
    private InventoryUI cachedInventoryUI { get; set; }

    public InventoryManager()
    {
        inventoryItems = new List<ItemData>();
        Weapons = new List<WeaponData>();
        Armors = new List<ArmorData>();
        Accessories = new List<AccessoryData>();
        Materials = new List<ItemData>(); // 재료 아이템 리스트 초기화
        isInventoryChanged = false; // 초기값은 변경되지 않음
        cachedInventoryUI = GameManager.Instance.inventoryUI;
    }
    public void EquipItem(ItemData item, PlayerInfo player, InventoryType type)
    {
        Debug.Log("아이템 삭제됨");
        RemoveItem(item); // 인벤토리에서 아이템 제거
        GameManager.Instance.equippedItemsManager.EquipItem(item, player, type); // 장착 처리
        //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, type); // 장착 UI 업데이트
        GameManager.Instance.inventoryUI.UpdateInventoryUI(); // UI 업데이트
        player.MarkDataAsChanged(); // 데이터 변경 플래그 설정
        player.EquippedItemsManager.MarkEquippedItemsAsChanged();
    }

    public void UnEquipItem(ItemData item, PlayerInfo player, InventoryType type)
    {
        Debug.Log("아이템 생성");
        AddItem(item); // 인벤토리에 아이템 추가
        GameManager.Instance.equippedItemsManager.UnEquipItem(player, type); // 장착 해제 처리
        //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, type); // 장착 UI 업데이트
        GameManager.Instance.inventoryUI.UpdateInventoryUI(); // UI 업데이트
        player.MarkDataAsChanged(); // 데이터 변경 플래그 설정
        player.EquippedItemsManager.MarkEquippedItemsAsChanged();
    }
   
    // 기존의 AddItem 메서드
    public void AddItem(ItemData item)
    {
        // 재료 아이템인 경우
        if (item is ItemData && !(item is WeaponData) && !(item is ArmorData) && !(item is AccessoryData))
        {
            var existingMaterial = Materials.Find(m => m.itemID == item.itemID);
            if (existingMaterial != null)
            {
                if (Materials.Count >= MaxMaterialSlots)
                {
                    // 텍스트확인
                    GameManager.Instance.logUI.AddMessage($"재료아이템 인벤토리 공간이 부족합니다!");    
                    return;
                }
                int newQuantity = existingMaterial.itemquantity + item.itemquantity;
                existingMaterial.itemquantity = Mathf.Min(newQuantity, 999);
                if (newQuantity > 999)
                {
                    existingMaterial.itemquantity = 999;
                    int excess = newQuantity - 999;
                    Materials.Add(new ItemData(item.itemID, item.itemName, item.buyPrice, item.sellPrice, item.itemProfile, item.itemImage, excess, item.itemdropChance));   
                }
            }
            else
            {
                if (Materials.Count >= MaxMaterialSlots)
                {
                    // 텍스트확인
                    GameManager.Instance.logUI.AddMessage($"재료아이템 인벤토리 공간이 부족합니다!");
                    return;
                }
                Materials.Add(item);           
            }
        }

        else if (item is WeaponData weapon)
        {
            if (Weapons.Count >= MaxWeaponSlots)
            {
                // 텍스트확인
                GameManager.Instance.logUI.AddMessage($"무기 인벤토리 공간이 부족합니다!");
                return;
            }
            Weapons.Add(weapon);          
        }
        else if (item is ArmorData armor)
        {
            if (Armors.Count >= MaxArmorSlots)
            {
                // 텍스트확인
                GameManager.Instance.logUI.AddMessage($"방어구 인벤토리 공간이 부족합니다!");
                return;
            }
            Armors.Add(armor);         
        }
        else if (item is AccessoryData accessory)
        {
            if (Accessories.Count >= MaxAccessorySlots)
            {
                // 텍스트확인
                GameManager.Instance.logUI.AddMessage($"장신구 인벤토리 공간이 부족합니다!");
                return;
            }
            Accessories.Add(accessory);        
        }
        // 텍스트확인
        GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color>을(를) 획득했습니다!");
        SetInventoryChanged(true);
        UpdateInventoryUI();
        Debug.Log($"아이템 추가: {item.itemName} | 인벤토리 상태: Weapons: {Weapons.Count}, Armors: {Armors.Count}, Accessories: {Accessories.Count}, Materials: {Materials.Count}");
        //Debug.Log($"isInventoryChanged : {isInventoryChanged}");
    }

    public bool RemoveItem(ItemData item)
    {
        bool result = false;
        if (item is WeaponData weapon)
        {
            result = Weapons.Remove(weapon);
        }
        else if (item is ArmorData armor)
        {      
            result = Armors.Remove(armor);
        }
        else if (item is AccessoryData accessory)
        {     
            result = Accessories.Remove(accessory);
        }
        else if (item is ItemData materials) // 재료 아이템
        {
            result = Materials.Remove(materials);
        }
        if (result)
        {
            // 텍스트확인
            GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName} </color>을(를) 삭제했습니다!");
            SetInventoryChanged(true);
            UpdateInventoryUI();  
        }
        return result;
    }
    public bool HasSpaceForItem(ItemData item)
    {
        if (item is WeaponData)
        {
            return Weapons.Count < MaxWeaponSlots;
        }
        else if (item is ArmorData)
        {
            return Armors.Count < MaxArmorSlots;
        }
        else if (item is AccessoryData)
        {
            return Accessories.Count < MaxAccessorySlots;
        }
        //else if (item is ItemData && !(item is WeaponData) && !(item is ArmorData) && !(item is AccessoryData))
        //{
        //    return Materials.Count < MaxMaterialSlots;
        //}

        return false; // 만약 아이템 유형이 알 수 없을 경우 공간이 없는 것으로 처리
    }
    public void UpdateInventoryUI()
    {
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        Debug.Log("인벤토리매니저 호출함");
    }

    public void MoveItemInCategory(InventoryType category, int fromIndex, int toIndex)
    {
        switch (category)
        {
            case InventoryType.Weapon:
                if (fromIndex >= 0 && fromIndex < Weapons.Count && toIndex >= 0 && toIndex < Weapons.Count) // 인덱스 체크
                {
                    WeaponData weapon = Weapons[fromIndex];
                    Weapons[toIndex] = weapon;
                    Weapons[fromIndex] = null;
                }
                break;
            case InventoryType.Armor:
                if (fromIndex >= 0 && fromIndex < Armors.Count && toIndex >= 0 && toIndex < Armors.Count) // 인덱스 체크
                {
                    ArmorData armor = Armors[fromIndex];
                    Armors[toIndex] = armor;
                    Armors[fromIndex] = null;
                }
                break;
            case InventoryType.Accessory:
                if (fromIndex >= 0 && fromIndex < Accessories.Count && toIndex >= 0 && toIndex < Accessories.Count) // 인덱스 체크
                {
                    AccessoryData accessory = Accessories[fromIndex];
                    Accessories[toIndex] = accessory;
                    Accessories[fromIndex] = null;
                }
                break;
            case InventoryType.Material:
                if (fromIndex >= 0 && fromIndex < Materials.Count && toIndex >= 0 && toIndex < Materials.Count) // 인덱스 체크
                {
                    ItemData material = Materials[fromIndex];
                    Materials[toIndex] = material;
                    Materials[fromIndex] = null;
                }
                break;
        }

        UpdateInventoryUI();  // 아이템 이동 후 UI 업데이트
    }

    public void SwapItemsInCategory(InventoryType category, int indexA, int indexB)
    {
        switch (category)
        {
            case InventoryType.Weapon:
                if (indexA >= 0 && indexA < Weapons.Count && indexB >= 0 && indexB < Weapons.Count) // 인덱스 체크
                {
                    WeaponData tempWeapon = Weapons[indexA];
                    Weapons[indexA] = Weapons[indexB];
                    Weapons[indexB] = tempWeapon;
                }
                break;
            case InventoryType.Armor:
                if (indexA >= 0 && indexA < Armors.Count && indexB >= 0 && indexB < Armors.Count) // 인덱스 체크
                {
                    ArmorData tempArmor = Armors[indexA];
                    Armors[indexA] = Armors[indexB];
                    Armors[indexB] = tempArmor;
                }
                break;
            case InventoryType.Accessory:
                if (indexA >= 0 && indexA < Accessories.Count && indexB >= 0 && indexB < Accessories.Count) // 인덱스 체크
                {
                    AccessoryData tempAccessory = Accessories[indexA];
                    Accessories[indexA] = Accessories[indexB];
                    Accessories[indexB] = tempAccessory;
                }
                break;
            case InventoryType.Material:
                if (indexA >= 0 && indexA < Materials.Count && indexB >= 0 && indexB < Materials.Count) // 인덱스 체크
                {
                    ItemData tempMaterial = Materials[indexA];
                    Materials[indexA] = Materials[indexB];
                    Materials[indexB] = tempMaterial;
                }
                break;
        }

        UpdateInventoryUI();  // 아이템 스왑 후 UI 업데이트
    }
    public void SetInventoryChanged(bool changed)
    {
        isInventoryChanged = changed;
        Debug.Log($"인벤토리매니저{isInventoryChanged}");
    }
    public bool IsInventoryChanged()
    {
        return isInventoryChanged;
    }

    public void ResetInventoryChangeFlag()
    {
        isInventoryChanged = false;
        Debug.Log(isInventoryChanged);
    }

    //  상점 판매 시 삭제
    public bool RemoveItemWithQuantity(ItemData item, int quantityToRemove)
    {
        if (item.itemquantity >= quantityToRemove)
        {
            item.itemquantity -= quantityToRemove;

            if (item.itemquantity <= 0)
            {
                // 남은 수량이 없으면 아이템 삭제
                return RemoveItem(item);
            }
            UpdateInventoryUI();
            return true;
        }

        return false;
    }

}

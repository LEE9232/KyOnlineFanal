using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public List<ItemData> inventoryItems; // { get; set; } // ���յ� ������ ����Ʈ
    public List<WeaponData> Weapons { get; set; }
    public List<ArmorData> Armors { get; set; }
    public List<AccessoryData> Accessories { get; set; }
    public List<ItemData> Materials { get; set; } // ��� ������
    public WeaponData EquippedWeapon { get; private set; }
    public ArmorData EquippedArmor { get; private set; }
    public AccessoryData EquippedAccessory { get; private set; }

    private bool isInventoryChanged; // ���� ���θ� ��Ÿ���� �÷���
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
        Materials = new List<ItemData>(); // ��� ������ ����Ʈ �ʱ�ȭ
        isInventoryChanged = false; // �ʱⰪ�� ������� ����
        cachedInventoryUI = GameManager.Instance.inventoryUI;
    }
    public void EquipItem(ItemData item, PlayerInfo player, InventoryType type)
    {
        Debug.Log("������ ������");
        RemoveItem(item); // �κ��丮���� ������ ����
        GameManager.Instance.equippedItemsManager.EquipItem(item, player, type); // ���� ó��
        //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, type); // ���� UI ������Ʈ
        GameManager.Instance.inventoryUI.UpdateInventoryUI(); // UI ������Ʈ
        player.MarkDataAsChanged(); // ������ ���� �÷��� ����
        player.EquippedItemsManager.MarkEquippedItemsAsChanged();
    }

    public void UnEquipItem(ItemData item, PlayerInfo player, InventoryType type)
    {
        Debug.Log("������ ����");
        AddItem(item); // �κ��丮�� ������ �߰�
        GameManager.Instance.equippedItemsManager.UnEquipItem(player, type); // ���� ���� ó��
        //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, type); // ���� UI ������Ʈ
        GameManager.Instance.inventoryUI.UpdateInventoryUI(); // UI ������Ʈ
        player.MarkDataAsChanged(); // ������ ���� �÷��� ����
        player.EquippedItemsManager.MarkEquippedItemsAsChanged();
    }
   
    // ������ AddItem �޼���
    public void AddItem(ItemData item)
    {
        // ��� �������� ���
        if (item is ItemData && !(item is WeaponData) && !(item is ArmorData) && !(item is AccessoryData))
        {
            var existingMaterial = Materials.Find(m => m.itemID == item.itemID);
            if (existingMaterial != null)
            {
                if (Materials.Count >= MaxMaterialSlots)
                {
                    // �ؽ�ƮȮ��
                    GameManager.Instance.logUI.AddMessage($"�������� �κ��丮 ������ �����մϴ�!");    
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
                    // �ؽ�ƮȮ��
                    GameManager.Instance.logUI.AddMessage($"�������� �κ��丮 ������ �����մϴ�!");
                    return;
                }
                Materials.Add(item);           
            }
        }

        else if (item is WeaponData weapon)
        {
            if (Weapons.Count >= MaxWeaponSlots)
            {
                // �ؽ�ƮȮ��
                GameManager.Instance.logUI.AddMessage($"���� �κ��丮 ������ �����մϴ�!");
                return;
            }
            Weapons.Add(weapon);          
        }
        else if (item is ArmorData armor)
        {
            if (Armors.Count >= MaxArmorSlots)
            {
                // �ؽ�ƮȮ��
                GameManager.Instance.logUI.AddMessage($"�� �κ��丮 ������ �����մϴ�!");
                return;
            }
            Armors.Add(armor);         
        }
        else if (item is AccessoryData accessory)
        {
            if (Accessories.Count >= MaxAccessorySlots)
            {
                // �ؽ�ƮȮ��
                GameManager.Instance.logUI.AddMessage($"��ű� �κ��丮 ������ �����մϴ�!");
                return;
            }
            Accessories.Add(accessory);        
        }
        // �ؽ�ƮȮ��
        GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color>��(��) ȹ���߽��ϴ�!");
        SetInventoryChanged(true);
        UpdateInventoryUI();
        Debug.Log($"������ �߰�: {item.itemName} | �κ��丮 ����: Weapons: {Weapons.Count}, Armors: {Armors.Count}, Accessories: {Accessories.Count}, Materials: {Materials.Count}");
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
        else if (item is ItemData materials) // ��� ������
        {
            result = Materials.Remove(materials);
        }
        if (result)
        {
            // �ؽ�ƮȮ��
            GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName} </color>��(��) �����߽��ϴ�!");
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

        return false; // ���� ������ ������ �� �� ���� ��� ������ ���� ������ ó��
    }
    public void UpdateInventoryUI()
    {
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        Debug.Log("�κ��丮�Ŵ��� ȣ����");
    }

    public void MoveItemInCategory(InventoryType category, int fromIndex, int toIndex)
    {
        switch (category)
        {
            case InventoryType.Weapon:
                if (fromIndex >= 0 && fromIndex < Weapons.Count && toIndex >= 0 && toIndex < Weapons.Count) // �ε��� üũ
                {
                    WeaponData weapon = Weapons[fromIndex];
                    Weapons[toIndex] = weapon;
                    Weapons[fromIndex] = null;
                }
                break;
            case InventoryType.Armor:
                if (fromIndex >= 0 && fromIndex < Armors.Count && toIndex >= 0 && toIndex < Armors.Count) // �ε��� üũ
                {
                    ArmorData armor = Armors[fromIndex];
                    Armors[toIndex] = armor;
                    Armors[fromIndex] = null;
                }
                break;
            case InventoryType.Accessory:
                if (fromIndex >= 0 && fromIndex < Accessories.Count && toIndex >= 0 && toIndex < Accessories.Count) // �ε��� üũ
                {
                    AccessoryData accessory = Accessories[fromIndex];
                    Accessories[toIndex] = accessory;
                    Accessories[fromIndex] = null;
                }
                break;
            case InventoryType.Material:
                if (fromIndex >= 0 && fromIndex < Materials.Count && toIndex >= 0 && toIndex < Materials.Count) // �ε��� üũ
                {
                    ItemData material = Materials[fromIndex];
                    Materials[toIndex] = material;
                    Materials[fromIndex] = null;
                }
                break;
        }

        UpdateInventoryUI();  // ������ �̵� �� UI ������Ʈ
    }

    public void SwapItemsInCategory(InventoryType category, int indexA, int indexB)
    {
        switch (category)
        {
            case InventoryType.Weapon:
                if (indexA >= 0 && indexA < Weapons.Count && indexB >= 0 && indexB < Weapons.Count) // �ε��� üũ
                {
                    WeaponData tempWeapon = Weapons[indexA];
                    Weapons[indexA] = Weapons[indexB];
                    Weapons[indexB] = tempWeapon;
                }
                break;
            case InventoryType.Armor:
                if (indexA >= 0 && indexA < Armors.Count && indexB >= 0 && indexB < Armors.Count) // �ε��� üũ
                {
                    ArmorData tempArmor = Armors[indexA];
                    Armors[indexA] = Armors[indexB];
                    Armors[indexB] = tempArmor;
                }
                break;
            case InventoryType.Accessory:
                if (indexA >= 0 && indexA < Accessories.Count && indexB >= 0 && indexB < Accessories.Count) // �ε��� üũ
                {
                    AccessoryData tempAccessory = Accessories[indexA];
                    Accessories[indexA] = Accessories[indexB];
                    Accessories[indexB] = tempAccessory;
                }
                break;
            case InventoryType.Material:
                if (indexA >= 0 && indexA < Materials.Count && indexB >= 0 && indexB < Materials.Count) // �ε��� üũ
                {
                    ItemData tempMaterial = Materials[indexA];
                    Materials[indexA] = Materials[indexB];
                    Materials[indexB] = tempMaterial;
                }
                break;
        }

        UpdateInventoryUI();  // ������ ���� �� UI ������Ʈ
    }
    public void SetInventoryChanged(bool changed)
    {
        isInventoryChanged = changed;
        Debug.Log($"�κ��丮�Ŵ���{isInventoryChanged}");
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

    //  ���� �Ǹ� �� ����
    public bool RemoveItemWithQuantity(ItemData item, int quantityToRemove)
    {
        if (item.itemquantity >= quantityToRemove)
        {
            item.itemquantity -= quantityToRemove;

            if (item.itemquantity <= 0)
            {
                // ���� ������ ������ ������ ����
                return RemoveItem(item);
            }
            UpdateInventoryUI();
            return true;
        }

        return false;
    }

}

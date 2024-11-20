using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems
{
    public WeaponData EquippedWeapon { get; private set; }
    public ArmorData EquippedArmor { get; private set; }
    public AccessoryData EquippedAccessory { get; private set; }

    private bool isEquippedItemsChanged; // ���� ���θ� ��Ÿ���� �÷���

    public EquippedItems()
    {
        EquippedWeapon = null;
        EquippedArmor = null;
        EquippedAccessory = null;
        isEquippedItemsChanged = false; // �ʱⰪ�� ������� ����
    }
    public void EquipItem(ItemData item, PlayerInfo player, InventoryType type)
    {
        switch (type)
        {
            case InventoryType.Weapon:
                player.ResetWeaponData();
                if (EquippedWeapon != null)
                {
                    player.Inventory.AddItem(EquippedWeapon);
                    UnequipWeapon(player); // ���� ���� ���� ����
                }
                EquipWeapon(item as WeaponData, player);
                //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, InventoryType.Weapon);
                break;
            case InventoryType.Armor:
                player.ResetArmorData();
                if (EquippedArmor != null)
                {
                    player.Inventory.AddItem(EquippedArmor);
                    UnequipArmor(player); // ���� �� ���� ����
                }
                EquipArmor(item as ArmorData, player);
                //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, InventoryType.Armor);
                break;
            case InventoryType.Accessory:
                player.ResetAccessoryData();
                if (EquippedAccessory != null)
                {
                    player.Inventory.AddItem(EquippedAccessory);
                    UnequipAccessory(player); // ���� ��ű� ���� ����
                }
                EquipAccessory(item as AccessoryData, player);
                //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, InventoryType.Accessory);
                break;
        }
        MarkEquippedItemsAsChanged();
    }

    public void UnEquipItem(PlayerInfo player, InventoryType type)
    {
        switch (type)
        {
            case InventoryType.Weapon:
                //if (EquippedWeapon != null)
                //{
                //    player.Inventory.AddItem(EquippedWeapon); // ������ ���⸦ �κ��丮�� ��ȯ
                //}
                UnequipWeapon(player);
                player.ResetWeaponData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Weapon); // ������ null�� ������Ʈ
                break;
            case InventoryType.Armor:
                //if (EquippedArmor != null)
                //{
                //    player.Inventory.AddItem(EquippedArmor); // ������ ���� �κ��丮�� ��ȯ
                //}
                UnequipArmor(player);
                player.ResetArmorData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Armor); // ������ null�� ������Ʈ
                break;
            case InventoryType.Accessory:
                //if (EquippedAccessory != null)
                //{
                //    player.Inventory.AddItem(EquippedAccessory); // ������ ��ű��� �κ��丮�� ��ȯ
                //}
                UnequipAccessory(player);
                player.ResetAccessoryData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Accessory); // ������ null�� ������Ʈ
                break;
        }
        MarkEquippedItemsAsChanged();
    }

    public void EquipWeapon(WeaponData weapon, PlayerInfo player)
    {
        if (EquippedWeapon != null && EquippedWeapon.itemID == weapon.itemID)
        {
            Debug.Log("�̹� ������ ���Ⱑ �����Ǿ� �ֽ��ϴ�.");
            return; // ������ ������ �������� ����
        }
        EquippedWeapon = weapon;
        player.Damage += weapon.attackPower;
    }

    public void EquipArmor(ArmorData armor, PlayerInfo player)
    {
        if (EquippedArmor != null && EquippedArmor.itemID == armor.itemID)
        {
            Debug.Log("�̹� ������ ���Ⱑ �����Ǿ� �ֽ��ϴ�.");
            return; // ������ ������ �������� ����
        }
        EquippedArmor = armor;
        player.Defensive += EquippedArmor.defensePower;
        player.MaxHP += EquippedArmor.healthPoints;

        //player.HP = Mathf.Clamp(player.HP, 0, player.MaxHP);
    }

    public void EquipAccessory(AccessoryData accessory, PlayerInfo player)
    {
        if (EquippedAccessory != null && EquippedAccessory.itemID == accessory.itemID)
        {
            Debug.Log("�̹� ������ ���Ⱑ �����Ǿ� �ֽ��ϴ�.");
            return; // ������ ������ �������� ����
        }
        EquippedAccessory = accessory;
        player.HpRegenPerSecond += EquippedAccessory.hpRegenPerSecond;
        player.MpRegenPerSecond += EquippedAccessory.mpRegenPerSecond;
        player.MaxMP += EquippedAccessory.manaPoints;
        // ���� MP�� ���ο� MaxMP�� ���� �ʵ��� Ŭ���� ó��
        //player.MP = Mathf.Clamp(player.MP, 0, player.MaxMP);
    }

    public void UnequipWeapon(PlayerInfo player)
    {
        if (EquippedWeapon == null) return;
        player.Damage = Mathf.Max(player.Damage - EquippedWeapon.attackPower, player.BaseDamage);
        EquippedWeapon = null;
    }

    public void UnequipArmor(PlayerInfo player)
    {
        if (EquippedArmor == null) return;
        player.MaxHP = Mathf.Max(player.MaxHP - EquippedArmor.healthPoints, player.BaseMaxHP);
        player.Defensive = Mathf.Max(player.Defensive - EquippedArmor.defensePower, player.BaseDefensive);
        // ���� MP�� ���ο� MaxMP�� ���� �ʵ��� Ŭ���� ó��
        player.HP = Mathf.Clamp(player.HP, 0, player.MaxHP);
        EquippedArmor = null;
    }

    public void UnequipAccessory(PlayerInfo player)
    {
        if (EquippedAccessory == null) return;
        player.MaxMP = Mathf.Max(player.MaxMP - EquippedAccessory.manaPoints, player.BaseMaxMP);
        player.HpRegenPerSecond = Mathf.Max(player.HpRegenPerSecond - EquippedAccessory.hpRegenPerSecond, player.BaseHpRegenPerSecond);
        player.MpRegenPerSecond = Mathf.Max(player.MpRegenPerSecond - EquippedAccessory.mpRegenPerSecond, player.BaseMpRegenPerSecond);
        // ���� MP�� ���ο� MaxMP�� ���� �ʵ��� Ŭ���� ó��
        player.MP = Mathf.Clamp(player.MP, 0, player.MaxMP);
        EquippedAccessory = null;
    }

    private void UpdateWeaponStats(PlayerInfo player)
    {
        if (EquippedWeapon != null)
        {
            player.Damage += EquippedWeapon.attackPower;
        }
    }

    private void UpdateArmorStats(PlayerInfo player)
    {
        if (EquippedArmor != null)
        {
            player.Defensive += EquippedArmor.defensePower;
            player.MaxHP += EquippedArmor.healthPoints;
        }
    }

    private void UpdateAccessoryStats(PlayerInfo player)
    {
        if (EquippedAccessory != null)
        {
            player.HpRegenPerSecond += EquippedAccessory.hpRegenPerSecond;
            player.MpRegenPerSecond += EquippedAccessory.mpRegenPerSecond;
            player.MaxMP += EquippedAccessory.manaPoints;
        }
    }
    public bool IsEquippedItemsChanged()
    {
        return isEquippedItemsChanged;
    }

    public void MarkEquippedItemsAsChanged()
    {
        isEquippedItemsChanged = true;
    }

    public void ResetEquippedItemsChangeFlag()
    {
        isEquippedItemsChanged = false;
    }
}

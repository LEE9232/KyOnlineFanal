using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItems
{
    public WeaponData EquippedWeapon { get; private set; }
    public ArmorData EquippedArmor { get; private set; }
    public AccessoryData EquippedAccessory { get; private set; }

    private bool isEquippedItemsChanged; // 변경 여부를 나타내는 플래그

    public EquippedItems()
    {
        EquippedWeapon = null;
        EquippedArmor = null;
        EquippedAccessory = null;
        isEquippedItemsChanged = false; // 초기값은 변경되지 않음
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
                    UnequipWeapon(player); // 기존 무기 스탯 제거
                }
                EquipWeapon(item as WeaponData, player);
                //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, InventoryType.Weapon);
                break;
            case InventoryType.Armor:
                player.ResetArmorData();
                if (EquippedArmor != null)
                {
                    player.Inventory.AddItem(EquippedArmor);
                    UnequipArmor(player); // 기존 방어구 스탯 제거
                }
                EquipArmor(item as ArmorData, player);
                //GameManager.Instance.inventoryUI.UpdateEquipSlots(item, InventoryType.Armor);
                break;
            case InventoryType.Accessory:
                player.ResetAccessoryData();
                if (EquippedAccessory != null)
                {
                    player.Inventory.AddItem(EquippedAccessory);
                    UnequipAccessory(player); // 기존 장신구 스탯 제거
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
                //    player.Inventory.AddItem(EquippedWeapon); // 해제된 무기를 인벤토리로 반환
                //}
                UnequipWeapon(player);
                player.ResetWeaponData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Weapon); // 슬롯을 null로 업데이트
                break;
            case InventoryType.Armor:
                //if (EquippedArmor != null)
                //{
                //    player.Inventory.AddItem(EquippedArmor); // 해제된 방어구를 인벤토리로 반환
                //}
                UnequipArmor(player);
                player.ResetArmorData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Armor); // 슬롯을 null로 업데이트
                break;
            case InventoryType.Accessory:
                //if (EquippedAccessory != null)
                //{
                //    player.Inventory.AddItem(EquippedAccessory); // 해제된 장신구를 인벤토리로 반환
                //}
                UnequipAccessory(player);
                player.ResetAccessoryData();
                GameManager.Instance.inventoryUI.UpdateEquipSlots(null, InventoryType.Accessory); // 슬롯을 null로 업데이트
                break;
        }
        MarkEquippedItemsAsChanged();
    }

    public void EquipWeapon(WeaponData weapon, PlayerInfo player)
    {
        if (EquippedWeapon != null && EquippedWeapon.itemID == weapon.itemID)
        {
            Debug.Log("이미 동일한 무기가 장착되어 있습니다.");
            return; // 동일한 무기라면 갱신하지 않음
        }
        EquippedWeapon = weapon;
        player.Damage += weapon.attackPower;
    }

    public void EquipArmor(ArmorData armor, PlayerInfo player)
    {
        if (EquippedArmor != null && EquippedArmor.itemID == armor.itemID)
        {
            Debug.Log("이미 동일한 무기가 장착되어 있습니다.");
            return; // 동일한 무기라면 갱신하지 않음
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
            Debug.Log("이미 동일한 무기가 장착되어 있습니다.");
            return; // 동일한 무기라면 갱신하지 않음
        }
        EquippedAccessory = accessory;
        player.HpRegenPerSecond += EquippedAccessory.hpRegenPerSecond;
        player.MpRegenPerSecond += EquippedAccessory.mpRegenPerSecond;
        player.MaxMP += EquippedAccessory.manaPoints;
        // 현재 MP가 새로운 MaxMP를 넘지 않도록 클램프 처리
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
        // 현재 MP가 새로운 MaxMP를 넘지 않도록 클램프 처리
        player.HP = Mathf.Clamp(player.HP, 0, player.MaxHP);
        EquippedArmor = null;
    }

    public void UnequipAccessory(PlayerInfo player)
    {
        if (EquippedAccessory == null) return;
        player.MaxMP = Mathf.Max(player.MaxMP - EquippedAccessory.manaPoints, player.BaseMaxMP);
        player.HpRegenPerSecond = Mathf.Max(player.HpRegenPerSecond - EquippedAccessory.hpRegenPerSecond, player.BaseHpRegenPerSecond);
        player.MpRegenPerSecond = Mathf.Max(player.MpRegenPerSecond - EquippedAccessory.mpRegenPerSecond, player.BaseMpRegenPerSecond);
        // 현재 MP가 새로운 MaxMP를 넘지 않도록 클램프 처리
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

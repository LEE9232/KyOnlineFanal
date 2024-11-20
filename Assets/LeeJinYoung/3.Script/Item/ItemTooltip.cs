using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDetailsText;
    public GameObject tooltipPanel;  // 툴팁 UI 패널

    // 툴팁을 업데이트하고 표시하는 메서드
    public void ShowTooltip(ItemData itemData, PlayerInfo player = null, bool isEquipped = false)
    {
        if (itemData != null)
        {
            itemNameText.text = itemData.itemName;
            // 장착된 아이템이면 플레이어 스탯과 함께 표시, 그렇지 않으면 아이템 정보만 표시
            if (isEquipped && player != null)
            {
                itemDetailsText.text = GetEquippedItemDetails(itemData, player);  // 장착 아이템 + 플레이어 스탯
            }
            else
            {
                itemDetailsText.text = GetItemDetails(itemData);  // 아이템 데이터만
            }
            tooltipPanel.SetActive(true);  // 툴팁 패널을 활성화
        }
    }
    // 장착 아이템 + 플레이어 스탯 정보를 포함하는 툴팁
    private string GetEquippedItemDetails(ItemData itemData, PlayerInfo player)
    {
        string tooltipText = "";
        // 아이템 타입에 따른 추가 정보 표시
        if (itemData is WeaponData weapon)
        {
            int weaponDamage = weapon.attackPower;
            int baseDamage = player.BaseDamage;  // 플레이어의 기본 공격력
            int totalDamage = baseDamage + weaponDamage;  // 합산된 공격력
            tooltipText += $"\n <color=#FF5733>공격력 : {totalDamage}</color>  (<color=#33FF57>{baseDamage}</color>  +  <color=#3375FF>{weaponDamage}</color>)\n";
        }
        else if (itemData is ArmorData armor)
        {
            int armorDefense = armor.defensePower;
            int baseDefense = player.BaseDefensive;
            int totalDefense = baseDefense + armorDefense;
            int armorHP = armor.healthPoints;
            int baseHP = player.BaseMaxHP;
            int totalHP = baseHP + armorHP;
            tooltipText += $"\n <color=#FF5733>방어력 : {totalDefense}</color>  (<color=#33FF57>{baseDefense}</color>  +  <color=#3375FF>{armorDefense}</color>)\n";
            tooltipText += $" <color=#FF5733>체력 : {totalHP}</color>  (<color=#33FF57>{baseHP}</color>  +  <color=#3375FF>{armorHP}</color>)";
        }
        else if (itemData is AccessoryData accessory)
        {
            int accessoryMP = accessory.manaPoints;
            int baseMP = player.BaseMaxMP;
            int totalMP = baseMP + accessoryMP;
            int accessoryHPRegen = accessory.hpRegenPerSecond;
            int baseHPRegen = player.BaseHpRegenPerSecond;
            int totalHPRegen = baseHPRegen + accessoryHPRegen;
            int accessoryMPRegen = accessory.mpRegenPerSecond;
            int baseMPRegen = player.BaseMpRegenPerSecond;
            int totalMPRegen = baseMPRegen + accessoryMPRegen;
            tooltipText += $"\n <color=#FF5733>마나 : {totalMP}</color>  (<color=#33FF57>{baseMP}</color>  +  <color=#3375FF>{accessoryMP}</color>)\n";
            tooltipText += $" <color=#FF5733>HP 재생 : {totalHPRegen}/초</color>  (<color=#33FF57>{baseHPRegen}</color>  +  <color=#3375FF>{accessoryHPRegen}</color>)\n";
            tooltipText += $" <color=#FF5733>MP 재생 : {totalMPRegen}/초</color>  (<color=#33FF57>{baseMPRegen}</color>  +  <color=#3375FF>{accessoryMPRegen}</color>)";
        }
        tooltipText += $"\n\n {itemData.itemProfile}";
        return tooltipText;
    }
    // 툴팁에 표시될 아이템 정보를 문자열로 생성하는 메서드
    private string GetItemDetails(ItemData itemData, PlayerInfo player = null)
    {
        string tooltipText = $" 구매 가격 : {itemData.buyPrice}\n 판매 가격 : {itemData.sellPrice}\n";

        // 각 아이템 타입에 맞는 추가 정보를 표시
        if (itemData is WeaponData weapon)
        {
            tooltipText += $" 공격력 : {weapon.attackPower}\n";
        }
        else if (itemData is ArmorData armor)
        {
            tooltipText += $" 방어력 : {armor.defensePower}\n 체력 : {armor.healthPoints}\n";
        }
        else if (itemData is AccessoryData accessory)
        {
            tooltipText += $" 마나 : {accessory.manaPoints}\n HP 재생 : {accessory.hpRegenPerSecond} 초\n MP 재생:  {accessory.mpRegenPerSecond} 초\n";
        }
        tooltipText += $"\n {itemData.itemProfile}";
        return tooltipText;
    }

    // 툴팁을 숨기는 메서드
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

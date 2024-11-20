using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemDetailsText;
    public GameObject tooltipPanel;  // ���� UI �г�

    // ������ ������Ʈ�ϰ� ǥ���ϴ� �޼���
    public void ShowTooltip(ItemData itemData, PlayerInfo player = null, bool isEquipped = false)
    {
        if (itemData != null)
        {
            itemNameText.text = itemData.itemName;
            // ������ �������̸� �÷��̾� ���Ȱ� �Բ� ǥ��, �׷��� ������ ������ ������ ǥ��
            if (isEquipped && player != null)
            {
                itemDetailsText.text = GetEquippedItemDetails(itemData, player);  // ���� ������ + �÷��̾� ����
            }
            else
            {
                itemDetailsText.text = GetItemDetails(itemData);  // ������ �����͸�
            }
            tooltipPanel.SetActive(true);  // ���� �г��� Ȱ��ȭ
        }
    }
    // ���� ������ + �÷��̾� ���� ������ �����ϴ� ����
    private string GetEquippedItemDetails(ItemData itemData, PlayerInfo player)
    {
        string tooltipText = "";
        // ������ Ÿ�Կ� ���� �߰� ���� ǥ��
        if (itemData is WeaponData weapon)
        {
            int weaponDamage = weapon.attackPower;
            int baseDamage = player.BaseDamage;  // �÷��̾��� �⺻ ���ݷ�
            int totalDamage = baseDamage + weaponDamage;  // �ջ�� ���ݷ�
            tooltipText += $"\n <color=#FF5733>���ݷ� : {totalDamage}</color>  (<color=#33FF57>{baseDamage}</color>  +  <color=#3375FF>{weaponDamage}</color>)\n";
        }
        else if (itemData is ArmorData armor)
        {
            int armorDefense = armor.defensePower;
            int baseDefense = player.BaseDefensive;
            int totalDefense = baseDefense + armorDefense;
            int armorHP = armor.healthPoints;
            int baseHP = player.BaseMaxHP;
            int totalHP = baseHP + armorHP;
            tooltipText += $"\n <color=#FF5733>���� : {totalDefense}</color>  (<color=#33FF57>{baseDefense}</color>  +  <color=#3375FF>{armorDefense}</color>)\n";
            tooltipText += $" <color=#FF5733>ü�� : {totalHP}</color>  (<color=#33FF57>{baseHP}</color>  +  <color=#3375FF>{armorHP}</color>)";
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
            tooltipText += $"\n <color=#FF5733>���� : {totalMP}</color>  (<color=#33FF57>{baseMP}</color>  +  <color=#3375FF>{accessoryMP}</color>)\n";
            tooltipText += $" <color=#FF5733>HP ��� : {totalHPRegen}/��</color>  (<color=#33FF57>{baseHPRegen}</color>  +  <color=#3375FF>{accessoryHPRegen}</color>)\n";
            tooltipText += $" <color=#FF5733>MP ��� : {totalMPRegen}/��</color>  (<color=#33FF57>{baseMPRegen}</color>  +  <color=#3375FF>{accessoryMPRegen}</color>)";
        }
        tooltipText += $"\n\n {itemData.itemProfile}";
        return tooltipText;
    }
    // ������ ǥ�õ� ������ ������ ���ڿ��� �����ϴ� �޼���
    private string GetItemDetails(ItemData itemData, PlayerInfo player = null)
    {
        string tooltipText = $" ���� ���� : {itemData.buyPrice}\n �Ǹ� ���� : {itemData.sellPrice}\n";

        // �� ������ Ÿ�Կ� �´� �߰� ������ ǥ��
        if (itemData is WeaponData weapon)
        {
            tooltipText += $" ���ݷ� : {weapon.attackPower}\n";
        }
        else if (itemData is ArmorData armor)
        {
            tooltipText += $" ���� : {armor.defensePower}\n ü�� : {armor.healthPoints}\n";
        }
        else if (itemData is AccessoryData accessory)
        {
            tooltipText += $" ���� : {accessory.manaPoints}\n HP ��� : {accessory.hpRegenPerSecond} ��\n MP ���:  {accessory.mpRegenPerSecond} ��\n";
        }
        tooltipText += $"\n {itemData.itemProfile}";
        return tooltipText;
    }

    // ������ ����� �޼���
    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

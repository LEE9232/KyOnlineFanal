using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public enum Class
{
    Paladine = 1,
    Mage = 2
}

public class PlayerInfo
{
    public string Key { get; set; } // Firebase�� ���� Ű�� �����ϴ� ������Ƽ �߰�
    public int Level { get; set; }
    public string NickName { get; set; }
    public int HP { get; set; }
    public int MaxHP { get; set; }
    public int MP { get; set; }
    public int MaxMP { get; set; }
    public int Damage { get; set; }
    public int Defensive { get; set; }
    public int EXP { get; set; }
    public int MaxEXP { get; set; }
    public int SkillPoint { get; set; }
    public int Gold { get; set; }
    public Class classes { get; set; }
    // �߰��� �Ӽ���
    public int HpRegenPerSecond { get; set; } // �ʴ� HP ȸ����
    public int MpRegenPerSecond { get; set; } // �ʴ� MP ȸ����
    private bool isDataChanged = false;

    
    //�⺻ ������ ������ ���� �߰�
    public int BaseMaxHP { get;  set; }
    public int BaseMaxMP { get;  set; }
    public int BaseDamage { get;  set; }
    public int BaseDefensive { get;  set; }
    public int BaseHpRegenPerSecond { get;  set; }
    public int BaseMpRegenPerSecond { get;  set; }
// ������ ��� �߰�
public List<WeaponData> Weapons { get; set; }         // ���� ���
    public List<ArmorData> Armors { get; set; }           // �� ���
    public List<AccessoryData> Accessories { get; set; }  // ��ű� ���
    public EquippedItems EquippedItemsManager { get; set; } // ���� ������ ���� �Ŵ��� �߰�
    public InventoryManager Inventory { get; set; }


    public PlayerInfo(int level, string nickName, int hP, int maxHP, int mP,int maxMP, int damage, int defensive, int exp, int maxEXP, int skillPoint, int gold, Class classes, int hpRegen = 1, int mpRegen = 2)
    {
        Level = level;
        NickName = nickName;
        HP = hP;
        MaxHP = maxHP;
        MP = mP;
        MaxMP = maxMP;
        Damage = damage;
        Defensive = defensive;
        EXP = exp;
        MaxEXP = maxEXP;
        SkillPoint = skillPoint;
        Gold = gold;
        this.classes = classes;
        HpRegenPerSecond = hpRegen; // �⺻ �ʴ� HP ȸ���� ����
        MpRegenPerSecond = mpRegen; // �⺻ �ʴ� MP ȸ���� ����

        // �⺻ ���� ����
        BaseMaxHP = maxHP;
        BaseMaxMP = maxMP;
        BaseDamage = damage;
        BaseDefensive = defensive;
        BaseHpRegenPerSecond = hpRegen;
        BaseMpRegenPerSecond = mpRegen;

        Inventory = new InventoryManager();
        EquippedItemsManager = new EquippedItems(); // ���� ������ �Ŵ��� �ʱ�ȭ
    }

    public PlayerInfo()
    {
        Level = 1;
        NickName = "";
        HP = 100;
        MaxHP = 100;
        MP = 50;
        MaxMP = 50;
        Damage = 10;
        Defensive = 5;
        EXP = 0;
        MaxEXP = 500;
        this.classes = classes;
        SkillPoint = SkillPoint;
        Gold = 10000;
        // �⺻�� ����
        HpRegenPerSecond = 1; // �⺻ �ʴ� HP ȸ����
        MpRegenPerSecond = 2; // �⺻ �ʴ� MP ȸ����

        // �⺻ ���� ����
        BaseMaxHP = MaxHP;
        BaseMaxMP = MaxMP;
        BaseDamage = Damage;
        BaseDefensive = Defensive;
        BaseHpRegenPerSecond = HpRegenPerSecond;
        BaseMpRegenPerSecond = MpRegenPerSecond;


        EquippedItemsManager = new EquippedItems(); // ���� ������ �Ŵ��� �ʱ�ȭ
        Inventory = new InventoryManager();
    }
    public void ResetToBaseStats()
    {
        // �⺻������ �ٽ� ����
        MaxHP = BaseMaxHP;
        MaxMP = BaseMaxMP;
        Damage = BaseDamage;
        Defensive = BaseDefensive;
        HpRegenPerSecond = BaseHpRegenPerSecond;
        MpRegenPerSecond = BaseMpRegenPerSecond;
    }
    // ������ �������̳� �������� ���� ���� ���ʽ� ����
    public void ApplyEquippedItemsStats(PlayerInfo player, EquippedItems equippedItems)
    {
        if (equippedItems.EquippedWeapon != null)
        {
            player.Damage += equippedItems.EquippedWeapon.attackPower;
        }
        if (equippedItems.EquippedArmor != null)
        {
            player.Defensive += equippedItems.EquippedArmor.defensePower;
            player.MaxHP += equippedItems.EquippedArmor.healthPoints;
        }
        if (equippedItems.EquippedAccessory != null)
        {
            player.MaxMP += equippedItems.EquippedAccessory.manaPoints;
            player.HpRegenPerSecond += equippedItems.EquippedAccessory.hpRegenPerSecond;
            player.MpRegenPerSecond += equippedItems.EquippedAccessory.mpRegenPerSecond;
        }
    }
    public void ResetWeaponData()
    {
        Damage = BaseDamage;
    }
    public void ResetArmorData()
    {
        MaxHP = BaseMaxHP;
        Defensive = BaseDefensive;
    }
    public void ResetAccessoryData()
    {
        MaxMP = BaseMaxMP;
        HpRegenPerSecond = BaseHpRegenPerSecond;
        MpRegenPerSecond = BaseMpRegenPerSecond;
    }
    public void ResetMPData()
    {
        MaxMP = BaseMaxMP;
    }

    //public void GainEXP(int amount)
    //{ 
    //    EXP += amount;
    //    while (EXP >= MaxEXP)
    //    {
    //        LevelUp();
    //
    //    }
    //    //isDataChanged = true;
    //    MarkDataAsChanged(); // ������ ���� �÷��� ����
    //}

    // ������ �޼���
    //public void LevelUp()
    //{
    //    Level++;
    //    SkillPoint += 1;  // ���������� ��ų ����Ʈ 1 ����
    //    EXP -= MaxEXP;  // ���� ����ġ�� ���� ������ �̾���
    //    UpdateStats();
    //    HP = MaxHP;  // ������ �� HP�� �ִ�ġ�� ȸ��
    //    MP = MaxMP;  // ������ �� MP�� �ִ�ġ�� ȸ��
    //    MarkDataAsChanged(); // ������ ���� �÷��� ����
    //}

    //private void UpdateStats()
    //{
    //    MaxHP += 10; // ���÷� ������ �� HP 10 ����
    //    MaxMP += 5;  // MP 5 ����
    //    MaxEXP += Level * 20;
    //    Damage += 2;  // ���ݷ� 2 ����
    //    Defensive += 1;  // ���� 1 ����
    //}
    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            GameManager.Instance.logUI.AddMessage($"<color=green>{amount} </color> Gold ���");
            GameManager.Instance.inventoryUI.UpdateGoldUI();  // ��� UI ������Ʈ
            return true;  // ���������� ��带 ���
        }
        else
        {
            GameManager.Instance.logUI.AddMessage("Gold �� �����մϴ�.");
            return false;  // ��尡 ������ ��
        }
    }
    public void SellItemGold(int amount)
    {
        Gold += amount;
        GameManager.Instance.inventoryUI.UpdateGoldUI();  // ��� UI ������Ʈ
    }

    // ������ ���� �÷��׸� �����ϴ� �޼���
    public void MarkDataAsChanged()
    {
        isDataChanged = true;
        Debug.Log(isDataChanged);
    }
    
    // �����Ͱ� ����Ǿ����� ���θ� Ȯ���ϴ� �޼���
    public bool IsDataChanged()
    {
        return isDataChanged;
    }
    
    // ������ ���� �÷��׸� �����ϴ� �޼���
    public void ResetDataChangeFlag()
    {
        isDataChanged = false;
        Debug.Log(isDataChanged);
    }
}
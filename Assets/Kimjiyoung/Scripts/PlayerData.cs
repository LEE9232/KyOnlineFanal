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
    public string Key { get; set; } // Firebase의 고유 키를 저장하는 프로퍼티 추가
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
    // 추가된 속성들
    public int HpRegenPerSecond { get; set; } // 초당 HP 회복량
    public int MpRegenPerSecond { get; set; } // 초당 MP 회복량
    private bool isDataChanged = false;

    
    //기본 스탯을 저장할 변수 추가
    public int BaseMaxHP { get;  set; }
    public int BaseMaxMP { get;  set; }
    public int BaseDamage { get;  set; }
    public int BaseDefensive { get;  set; }
    public int BaseHpRegenPerSecond { get;  set; }
    public int BaseMpRegenPerSecond { get;  set; }
// 아이템 목록 추가
public List<WeaponData> Weapons { get; set; }         // 무기 목록
    public List<ArmorData> Armors { get; set; }           // 방어구 목록
    public List<AccessoryData> Accessories { get; set; }  // 장신구 목록
    public EquippedItems EquippedItemsManager { get; set; } // 장착 아이템 관리 매니저 추가
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
        HpRegenPerSecond = hpRegen; // 기본 초당 HP 회복량 설정
        MpRegenPerSecond = mpRegen; // 기본 초당 MP 회복량 설정

        // 기본 스탯 설정
        BaseMaxHP = maxHP;
        BaseMaxMP = maxMP;
        BaseDamage = damage;
        BaseDefensive = defensive;
        BaseHpRegenPerSecond = hpRegen;
        BaseMpRegenPerSecond = mpRegen;

        Inventory = new InventoryManager();
        EquippedItemsManager = new EquippedItems(); // 장착 아이템 매니저 초기화
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
        // 기본값 설정
        HpRegenPerSecond = 1; // 기본 초당 HP 회복량
        MpRegenPerSecond = 2; // 기본 초당 MP 회복량

        // 기본 스탯 설정
        BaseMaxHP = MaxHP;
        BaseMaxMP = MaxMP;
        BaseDamage = Damage;
        BaseDefensive = Defensive;
        BaseHpRegenPerSecond = HpRegenPerSecond;
        BaseMpRegenPerSecond = MpRegenPerSecond;


        EquippedItemsManager = new EquippedItems(); // 장착 아이템 매니저 초기화
        Inventory = new InventoryManager();
    }
    public void ResetToBaseStats()
    {
        // 기본값으로 다시 리셋
        MaxHP = BaseMaxHP;
        MaxMP = BaseMaxMP;
        Damage = BaseDamage;
        Defensive = BaseDefensive;
        HpRegenPerSecond = BaseHpRegenPerSecond;
        MpRegenPerSecond = BaseMpRegenPerSecond;
    }
    // 장착된 아이템이나 레벨업에 따른 스탯 보너스 적용
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
    //    MarkDataAsChanged(); // 데이터 변경 플래그 설정
    //}

    // 레벨업 메서드
    //public void LevelUp()
    //{
    //    Level++;
    //    SkillPoint += 1;  // 레벨업마다 스킬 포인트 1 증가
    //    EXP -= MaxEXP;  // 남은 경험치는 다음 레벨로 이어짐
    //    UpdateStats();
    //    HP = MaxHP;  // 레벨업 시 HP를 최대치로 회복
    //    MP = MaxMP;  // 레벨업 시 MP를 최대치로 회복
    //    MarkDataAsChanged(); // 데이터 변경 플래그 설정
    //}

    //private void UpdateStats()
    //{
    //    MaxHP += 10; // 예시로 레벨업 시 HP 10 증가
    //    MaxMP += 5;  // MP 5 증가
    //    MaxEXP += Level * 20;
    //    Damage += 2;  // 공격력 2 증가
    //    Defensive += 1;  // 방어력 1 증가
    //}
    public bool SpendGold(int amount)
    {
        if (Gold >= amount)
        {
            Gold -= amount;
            GameManager.Instance.logUI.AddMessage($"<color=green>{amount} </color> Gold 사용");
            GameManager.Instance.inventoryUI.UpdateGoldUI();  // 골드 UI 업데이트
            return true;  // 성공적으로 골드를 사용
        }
        else
        {
            GameManager.Instance.logUI.AddMessage("Gold 가 부족합니다.");
            return false;  // 골드가 부족할 때
        }
    }
    public void SellItemGold(int amount)
    {
        Gold += amount;
        GameManager.Instance.inventoryUI.UpdateGoldUI();  // 골드 UI 업데이트
    }

    // 데이터 변경 플래그를 설정하는 메서드
    public void MarkDataAsChanged()
    {
        isDataChanged = true;
        Debug.Log(isDataChanged);
    }
    
    // 데이터가 변경되었는지 여부를 확인하는 메서드
    public bool IsDataChanged()
    {
        return isDataChanged;
    }
    
    // 데이터 변경 플래그를 리셋하는 메서드
    public void ResetDataChangeFlag()
    {
        isDataChanged = false;
        Debug.Log(isDataChanged);
    }
}
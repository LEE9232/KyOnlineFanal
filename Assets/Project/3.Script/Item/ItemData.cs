using System;
using UnityEngine;


[Serializable]
public class ItemData
{
    // 공통적인 아이템데이터들
    public int itemID;          // 아이템 고유 ID
    public string itemName;     // 아이템 이름
    public int buyPrice;        // 구매가격
    public int sellPrice;       // 판매가격
    public string itemProfile;  // 아이템 내용
    public string itemImage;      // 아이템 이미지

    public int itemquantity = 1;         // 아이템 수량 (재료 아이템의 경우 여러 개, 무기나 방어구는 1)
    public float itemdropChance = 0f;    // 아이템 드랍 확률 (0~1 사이의 값)


    public ItemData() { }  // 기본 생성자 추가

    public ItemData(int id, string name, int buy, int sell, string profile, string image, int quantity, float dropChance)
    {
        itemID = id;
        itemName = name;
        buyPrice = buy;
        sellPrice = sell;
        itemProfile = profile;
        itemImage = image;      // 아이템 이미지

        itemquantity = quantity;
        itemdropChance = dropChance;
    }

    // 이미지 리소스를 불러오는 메서드
    public Sprite GetItemSprite()
    {
        return Resources.Load<Sprite>(itemImage);  // Resources 폴더에서 이미지 불러오기
    }
}
[Serializable]
public class WeaponData : ItemData
{
    // 무기 데이터
    public int attackPower; // 공격력

    public WeaponData() { }  // 기본 생성자 추가
    public WeaponData(int id, string name, int attack, int buy, int sell, string profile, string image)
        : base(id, name, buy, sell, profile, image, 1, 0.15f)
    {
        attackPower = attack;
    }
}
[Serializable]
public class ArmorData : ItemData
{
    // 방어구
    public int defensePower; // 방어력
    public int healthPoints; // 체력

    public ArmorData() { }  // 기본 생성자 추가
    public ArmorData(int id, string name, int defense, int hp, int buy, int sell, string profile, string image)
        : base(id, name, buy, sell, profile, image ,1, 0.2f)
    {
        defensePower = defense;
        healthPoints = hp;
    }
}
[Serializable]
public class AccessoryData : ItemData
{
    // 장신구
    public int manaPoints;          // 마나
    public int hpRegenPerSecond;    // 초당 HP 재생력
    public int mpRegenPerSecond;    // 초당 MP 재생력

    public AccessoryData() { }  // 기본 생성자 추가
    public AccessoryData(int id, string name, int mana, int hpRegen, int mpRegen, int buy, int sell, string profile, string image)
        : base(id, name, buy, sell, profile, image, 1, 0.1f)
    {
        manaPoints = mana;
        hpRegenPerSecond = hpRegen;
        mpRegenPerSecond = mpRegen;
    }
}
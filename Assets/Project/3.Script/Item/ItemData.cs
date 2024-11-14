using System;
using UnityEngine;


[Serializable]
public class ItemData
{
    // �������� �����۵����͵�
    public int itemID;          // ������ ���� ID
    public string itemName;     // ������ �̸�
    public int buyPrice;        // ���Ű���
    public int sellPrice;       // �ǸŰ���
    public string itemProfile;  // ������ ����
    public string itemImage;      // ������ �̹���

    public int itemquantity = 1;         // ������ ���� (��� �������� ��� ���� ��, ���⳪ ���� 1)
    public float itemdropChance = 0f;    // ������ ��� Ȯ�� (0~1 ������ ��)


    public ItemData() { }  // �⺻ ������ �߰�

    public ItemData(int id, string name, int buy, int sell, string profile, string image, int quantity, float dropChance)
    {
        itemID = id;
        itemName = name;
        buyPrice = buy;
        sellPrice = sell;
        itemProfile = profile;
        itemImage = image;      // ������ �̹���

        itemquantity = quantity;
        itemdropChance = dropChance;
    }

    // �̹��� ���ҽ��� �ҷ����� �޼���
    public Sprite GetItemSprite()
    {
        return Resources.Load<Sprite>(itemImage);  // Resources �������� �̹��� �ҷ�����
    }
}
[Serializable]
public class WeaponData : ItemData
{
    // ���� ������
    public int attackPower; // ���ݷ�

    public WeaponData() { }  // �⺻ ������ �߰�
    public WeaponData(int id, string name, int attack, int buy, int sell, string profile, string image)
        : base(id, name, buy, sell, profile, image, 1, 0.15f)
    {
        attackPower = attack;
    }
}
[Serializable]
public class ArmorData : ItemData
{
    // ��
    public int defensePower; // ����
    public int healthPoints; // ü��

    public ArmorData() { }  // �⺻ ������ �߰�
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
    // ��ű�
    public int manaPoints;          // ����
    public int hpRegenPerSecond;    // �ʴ� HP �����
    public int mpRegenPerSecond;    // �ʴ� MP �����

    public AccessoryData() { }  // �⺻ ������ �߰�
    public AccessoryData(int id, string name, int mana, int hpRegen, int mpRegen, int buy, int sell, string profile, string image)
        : base(id, name, buy, sell, profile, image, 1, 0.1f)
    {
        manaPoints = mana;
        hpRegenPerSecond = hpRegen;
        mpRegenPerSecond = mpRegen;
    }
}
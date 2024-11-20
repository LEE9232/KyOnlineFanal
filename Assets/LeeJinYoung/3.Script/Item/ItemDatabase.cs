using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
[Serializable]
public class ItemDatabase : MonoBehaviour
{
    // �̱��� �ν��Ͻ�
    public static ItemDatabase Instance { get; private set; }

    //public List<ItemData> AllItems { get; private set; } // ��� ������ ������ ���
    public List<WeaponData> weapons;  // ���� ������ ���
    public List<ArmorData> armors;    // �� ������ ���
    public List<AccessoryData> accessories;  // �Ǽ��縮 ������ ���
    public List<ItemData> materials;  // ��� ������ ���  // �߰���: ��� ������ ����Ʈ �߰�

    private Dictionary<int, ItemData> itemDictionary; // �������� ID�� ������ �˻��ϱ� ���� ��ųʸ�

    private void Awake()
    {
        // �̱��� �ν��Ͻ� �ʱ�ȭ
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // �ʱ�ȭ

        //AllItems = new List<ItemData> ();
        weapons = new List<WeaponData>();
        armors = new List<ArmorData>();
        accessories = new List<AccessoryData>();
        materials = new List<ItemData>();  // �߰���: ��� ������ �ʱ�ȭ

        itemDictionary = new Dictionary<int, ItemData>();
    }
    private void Start()
    {
        LoadItemsFromJson();
        UploadItemsToFirebase();
    }
    private void LoadItemsFromJson()
    {
        LoadWeapons();
        LoadArmors();
        LoadAccessories();
        LoadMaterials();
        // ��� �������� AllItems ����Ʈ�� �߰�

    }

    //private void LoadItemListFromJson<T>(string fileName, ref List<T> itemList) where T : ItemData
    //{
    //    TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
    //    if (jsonFile != null)
    //    {
    //        itemList = JsonConvert.DeserializeObject<List<T>>(jsonFile.text);
    //        foreach (var item in itemList)
    //        {
    //            AllItems.Add(item);
    //            itemDictionary[item.itemID] = item;  // ��ųʸ��� ������ �߰�
    //        }
    //        Debug.Log($"Loaded {itemList.Count} items from {fileName}");
    //    }
    //    else
    //    {
    //        Debug.LogWarning($"{fileName}.json not found in Resources");
    //    }
    //}


    private void LoadWeapons()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Weapons");
        // Resources ������ "weapons.json" 
        if (jsonFile != null)
        {
            var jsonData = JsonConvert.DeserializeObject<List<WeaponData>>(jsonFile.text);
            weapons = jsonData;
            foreach (var weapon in weapons)
            {

                itemDictionary[weapon.itemID] = weapon;
            }
        }
    }

    private void LoadArmors()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Armors");
        // Resources ������ "armors.json"
        if (jsonFile != null)
        {
            var jsonData = JsonConvert.DeserializeObject<List<ArmorData>>(jsonFile.text);
            armors = jsonData;
            foreach (var armor in armors)
            {

                itemDictionary[armor.itemID] = armor;
            }
        }
    }

    private void LoadAccessories()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Accessories");
        // Resources ������ "accessories.json" ������ �ε�
        if (jsonFile != null)
        {
            var jsonData = JsonConvert.DeserializeObject<List<AccessoryData>>(jsonFile.text);
            accessories = jsonData;
            foreach (var accessory in accessories)
            {

                itemDictionary[accessory.itemID] = accessory;
            }
        }
    }
    // �߰���: ��� ������ �ε� �޼���
    private void LoadMaterials()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Materials");
        if (jsonFile != null)
        {
            var jsonData = JsonConvert.DeserializeObject<List<ItemData>>(jsonFile.text);
            materials = jsonData;
            foreach (var material in materials)
            {

                itemDictionary[material.itemID] = material;
            }
            
        }
    }

    // Firebase�� ������ �����͸� ���ε��ϴ� �޼���
    public void UploadItemsToFirebase()
    {
        UploadItemsListToFirebase("Weapons", weapons);
        UploadItemsListToFirebase("Armors", armors);
        UploadItemsListToFirebase("Accessories", accessories);
        UploadItemsListToFirebase("Materials", materials);
        // �߰���: ��� ������ ���ε�
    }

    private void UploadItemsListToFirebase<T>(string path, List<T> items) where T : ItemData
    {
        DatabaseReference itemsRef = FirebaseDatabase.DefaultInstance.GetReference($"items/{path}");
        foreach (T item in items)
        {
            string json = JsonConvert.SerializeObject(item);
            itemsRef.Child(item.itemID.ToString()).SetRawJsonValueAsync(json).ContinueWith(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    Debug.Log($"{typeof(T).Name} {item.itemName} uploaded successfully!");
                }
                else
                {
                    Debug.LogError($"Failed to upload {typeof(T).Name} {item.itemName}: {task.Exception}");
                }
            });
        }
    }
    public List<ItemData> GetAllItem()
    {
        List<ItemData> allitems = new List<ItemData>();
        allitems.AddRange(weapons);
        allitems.AddRange(accessories);
        allitems.AddRange(armors);
        allitems.AddRange(materials);  // �߰���: ��� ������ �߰�
        return allitems;
    }


    // ������ ID�� �˻��ϴ� �޼���
    public ItemData GetItemById(int id)
    {
        if (itemDictionary.ContainsKey(id))
        {
            return itemDictionary[id];
        }
        return null;
    }
}


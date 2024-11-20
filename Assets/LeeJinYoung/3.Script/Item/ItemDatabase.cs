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
    // 싱글톤 인스턴스
    public static ItemDatabase Instance { get; private set; }

    //public List<ItemData> AllItems { get; private set; } // 모든 아이템 데이터 목록
    public List<WeaponData> weapons;  // 무기 데이터 목록
    public List<ArmorData> armors;    // 방어구 데이터 목록
    public List<AccessoryData> accessories;  // 악세사리 데이터 목록
    public List<ItemData> materials;  // 재료 아이템 목록  // 추가됨: 재료 아이템 리스트 추가

    private Dictionary<int, ItemData> itemDictionary; // 아이템을 ID로 빠르게 검색하기 위한 딕셔너리

    private void Awake()
    {
        // 싱글톤 인스턴스 초기화
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 초기화

        //AllItems = new List<ItemData> ();
        weapons = new List<WeaponData>();
        armors = new List<ArmorData>();
        accessories = new List<AccessoryData>();
        materials = new List<ItemData>();  // 추가됨: 재료 아이템 초기화

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
        // 모든 아이템을 AllItems 리스트에 추가

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
    //            itemDictionary[item.itemID] = item;  // 딕셔너리에 아이템 추가
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
        // Resources 폴더의 "weapons.json" 
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
        // Resources 폴더의 "armors.json"
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
        // Resources 폴더의 "accessories.json" 파일을 로드
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
    // 추가됨: 재료 아이템 로드 메서드
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

    // Firebase에 아이템 데이터를 업로드하는 메서드
    public void UploadItemsToFirebase()
    {
        UploadItemsListToFirebase("Weapons", weapons);
        UploadItemsListToFirebase("Armors", armors);
        UploadItemsListToFirebase("Accessories", accessories);
        UploadItemsListToFirebase("Materials", materials);
        // 추가됨: 재료 아이템 업로드
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
        allitems.AddRange(materials);  // 추가됨: 재료 아이템 추가
        return allitems;
    }


    // 아이템 ID로 검색하는 메서드
    public ItemData GetItemById(int id)
    {
        if (itemDictionary.ContainsKey(id))
        {
            return itemDictionary[id];
        }
        return null;
    }
}


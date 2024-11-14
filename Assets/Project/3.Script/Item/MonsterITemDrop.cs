using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MonsterITemDrop : MonoBehaviour
{
    public MonsterStatus MonsterStatus;   
    public List<GameObject> Drop = new List<GameObject>();
    public GameObject goldPrefabs;
    public int goldDropAmount = 100; // ����� ��� ��
    public Camera mainCamera;

    private void Awake()
    {
        MonsterStatus = GetComponent<MonsterStatus>();
        mainCamera = Camera.main;
    }

    public void DropSystem()
    {
        DropRamdomITem();
        DropGoldItem();
    }

    private void DropGoldItem()
    { 
        if(goldPrefabs != null)
        {
            GameObject goldItem = Instantiate(goldPrefabs, MonsterStatus.transform.position, Quaternion.identity);
            GoldItem goldItemInfo = goldItem.GetComponent<GoldItem>();
            if(goldItemInfo != null)
            {
                goldItemInfo.goldAmount = goldDropAmount;
                Debug.Log("�� ����.");
            }
        }
    }



    private void DropRamdomITem()
    {
        GameObject itemPrefab = Drop[0]; // ����� ������ ������ ����
        var item = Instantiate(itemPrefab, MonsterStatus.transform.position, transform.rotation);


        // ���⼭ ������ ������ �ۼ��ؾ���
        // ������ �����ۿ� ���� Item ������Ʈ�� ������
        ItemData RandomItemData = GetRamdomITemData();
        if (RandomItemData != null)
        {
            ItemInfo itemComponent = item.GetComponent<ItemInfo>();
            if (itemComponent != null)
            {
                itemComponent.itemID = RandomItemData.itemID; // ���� ������ ID ����
                itemComponent.InitializeItem(RandomItemData);
            }
        }

        SetCanvasCamera(item);
    }
    private ItemData GetRamdomITemData()
    {
        List<ItemData> allItem = ItemDatabase.Instance.GetAllItem();
        Debug.Log($"{allItem}");
        // Ȯ���� 0�� ������ ����
        List<ItemData> validItems = allItem.FindAll(item => item.itemdropChance > 0);
        if (allItem.Count > 0)
        {
            // �������� �� ��� Ȯ�� ���
            float totalChance = 0f;
            foreach (var item in allItem)
            {
                totalChance += item.itemdropChance; // �����ۺ� Ȯ���� ����
            }

            // �� Ȯ�� ���� ������ ���� �� ����
            float randomValue = Random.Range(0f, totalChance);
            float cumulativeChance = 0f;

            // ���� ���� �ش��ϴ� ������ ��ȯ
            foreach (var item in allItem)
            {
                cumulativeChance += item.itemdropChance;
                if (randomValue <= cumulativeChance)
                {
                    Debug.Log($"������ ���õ�: {item.itemName}, Ȯ��: {item.itemdropChance}, ���� ��: {randomValue}");
                    return item; // �ش� ������ ��ȯ
                }
            }
        }
        else
        {
            Debug.LogError("��ȿ�� �������� �����ϴ�.");
        }
        return null;
    }


    private void SetCanvasCamera(GameObject item)
    { 
        // UI�� ī�޶� ���� ����
        Canvas itemCanvas = item.GetComponentInChildren<Canvas>();
        if (itemCanvas != null)
        {
            itemCanvas.renderMode = RenderMode.WorldSpace;
            itemCanvas.worldCamera = mainCamera;
            itemCanvas.gameObject.AddComponent<ItemUICamera>();
        }
    }
}

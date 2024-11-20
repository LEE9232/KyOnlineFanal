using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class MonsterITemDrop : MonoBehaviour
{
    public MonsterStatus MonsterStatus;   
    public List<GameObject> Drop = new List<GameObject>();
    public GameObject goldPrefabs;
    public int goldDropAmount = 100; // 드롭할 골드 양
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
                Debug.Log("돈 떨굼.");
            }
        }
    }



    private void DropRamdomITem()
    {
        GameObject itemPrefab = Drop[0]; // 드랍할 아이템 프리팹 선택
        var item = Instantiate(itemPrefab, MonsterStatus.transform.position, transform.rotation);


        // 여기서 아이템 정보를 작성해야함
        // 생성된 아이템에 대한 Item 컴포넌트를 가져옴
        ItemData RandomItemData = GetRamdomITemData();
        if (RandomItemData != null)
        {
            ItemInfo itemComponent = item.GetComponent<ItemInfo>();
            if (itemComponent != null)
            {
                itemComponent.itemID = RandomItemData.itemID; // 랜덤 아이템 ID 설정
                itemComponent.InitializeItem(RandomItemData);
            }
        }

        SetCanvasCamera(item);
    }
    private ItemData GetRamdomITemData()
    {
        List<ItemData> allItem = ItemDatabase.Instance.GetAllItem();
        Debug.Log($"{allItem}");
        // 확률이 0인 아이템 제거
        List<ItemData> validItems = allItem.FindAll(item => item.itemdropChance > 0);
        if (allItem.Count > 0)
        {
            // 아이템의 총 드랍 확률 계산
            float totalChance = 0f;
            foreach (var item in allItem)
            {
                totalChance += item.itemdropChance; // 아이템별 확률을 더함
            }

            // 총 확률 범위 내에서 랜덤 값 생성
            float randomValue = Random.Range(0f, totalChance);
            float cumulativeChance = 0f;

            // 랜덤 값에 해당하는 아이템 반환
            foreach (var item in allItem)
            {
                cumulativeChance += item.itemdropChance;
                if (randomValue <= cumulativeChance)
                {
                    Debug.Log($"아이템 선택됨: {item.itemName}, 확률: {item.itemdropChance}, 랜덤 값: {randomValue}");
                    return item; // 해당 아이템 반환
                }
            }
        }
        else
        {
            Debug.LogError("유효한 아이템이 없습니다.");
        }
        return null;
    }


    private void SetCanvasCamera(GameObject item)
    { 
        // UI가 카메라 보게 설정
        Canvas itemCanvas = item.GetComponentInChildren<Canvas>();
        if (itemCanvas != null)
        {
            itemCanvas.renderMode = RenderMode.WorldSpace;
            itemCanvas.worldCamera = mainCamera;
            itemCanvas.gameObject.AddComponent<ItemUICamera>();
        }
    }
}

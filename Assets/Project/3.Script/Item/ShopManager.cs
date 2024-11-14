using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopManager : MonoBehaviour
{
    public List<int> weaponItemIDs;  // 상점에서 판매할 무기 아이템의 ID 리스트
    public List<int> armorItemIDs;   // 상점에서 판매할 방어구 아이템의 ID 리스트
    public List<int> accessoryItemIDs;  // 상점에서 판매할 장신구 아이템의 ID 리스트

    public Button buyBtn;
    public Button saleBtn;

    public GameObject buyPanel;
    public GameObject salePanel;

    //public List<ShopSlot> shopSlots;  // 상점 슬롯들
    public Transform shopSlotParent;  // 상점 슬롯이 배치될 부모 객체
    public GameObject shopSlotPrefab; // 상점 슬롯 프리팹
    private ItemDatabase itemDatabase; // 아이템 데이터를 불러올 데이터베이스
    private PlayerInfo playerInfo;      // 플레이어 정보 참조

    public GameObject deleteZone;
    public InventoryItemDeletePopup quantityPopup; // 팝업 참조
    public ChoicePopup choicePopup;

    private void Start()
    {
        buyBtn.onClick.AddListener(BuyBtnClick);
        saleBtn.onClick.AddListener(SaleBtnClick);
        // 아이템 데이터베이스 참조
        itemDatabase = ItemDatabase.Instance;
        playerInfo = GameManager.Instance.PlayerData;  // GameManager에서 PlayerInfo 가져오기
        SetupShopSlots();  // 상점 슬롯 설정
    }

    // 상점 슬롯을 설정하는 함수
    public void SetupShopSlots()
    {
        foreach (int id in weaponItemIDs)
        {
            ItemData weapon = itemDatabase.GetItemById(id);  // ID로 무기 아이템 가져오기
            if (weapon != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // 슬롯 생성
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(weapon, this);  // 슬롯에 아이템 정보 설정
            }
            else
            {
                Debug.LogError($"아이템 ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
            }
        }

        // 방어구 아이템 슬롯 설정
        foreach (int id in armorItemIDs)
        {
            ItemData armor = itemDatabase.GetItemById(id);  // ID로 방어구 아이템 가져오기
            if (armor != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // 슬롯 생성
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(armor, this);  // 슬롯에 아이템 정보 설정
            }
            else
            {
                Debug.LogError($"아이템 ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
            }
        }
        // 장신구 아이템 슬롯 설정
        foreach (int id in accessoryItemIDs)
        {
            ItemData accessory = itemDatabase.GetItemById(id);  // ID로 장신구 아이템 가져오기
            if (accessory != null)
            {
                GameObject slot = Instantiate(shopSlotPrefab, shopSlotParent);  // 슬롯 생성
                ShopSlot shopSlot = slot.GetComponent<ShopSlot>();
                shopSlot.SetSlot(accessory, this);  // 슬롯에 아이템 정보 설정
            }
            else
            {
                Debug.LogError($"아이템 ID {id}에 해당하는 아이템을 찾을 수 없습니다.");
            }
        }
    }

    // 아이템을 구매하는 함수
    public void BuyItem(ItemData item)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        // 인벤토리 매니저가 null인 경우 에러 로그
        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is null!");
            return;
        }
        // 인벤토리에 아이템을 추가할 공간이 있는지 확인
        if (!inventoryManager.HasSpaceForItem(item))
        {
            GameManager.Instance.logUI.AddMessage($"<color=red>인벤토리에 {item.itemName}을(를) 추가할 공간이 부족합니다!</color>");
            return;
        }
        // 골드가 충분한지 확인 후 구매
        if (playerInfo.SpendGold(item.buyPrice))
        {
            // 인벤토리에 아이템 추가
            inventoryManager.AddItem(item);
            GameManager.Instance.inventoryUI.UpdateInventoryUI();  // 인벤토리 UI 업데이트
        }
        else
        {
            // 골드가 부족할 경우 메시지 출력
            GameManager.Instance.logUI.AddMessage($"<color=red>골드가 부족합니다!</color>");
        }
        //if (playerInfo.SpendGold(item.buyPrice))  // 골드가 충분한지 확인 후 구매
        //{
        //    InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        //    // 인벤토리에 아이템 추가
        //    if (inventoryManager != null)
        //    {
        //        inventoryManager.AddItem(item);
        //        GameManager.Instance.inventoryUI.UpdateInventoryUI();  // 인벤토리 UI 업데이트
        //    }
        //    else
        //    {
        //        Debug.LogError("InventoryManager is null!");
        //    }
        //}
        //else
        //{
        //    // 텍스트확인
        //    GameManager.Instance.logUI.AddMessage($"<color=green> Gold </color> 가 부족합니다!");
        //}
    }
    // 아이템 판매 메서드
    public void SellItem(ItemData item)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;
        if (inventoryManager != null)
        {
            if (inventoryManager.RemoveItem(item))  // 인벤토리에서 아이템 제거
            {
                //playerInfo.SellItemGold(item.sellPrice);  // 판매 가격만큼 골드 증가
                playerInfo.SellItemGold(item.sellPrice * item.itemquantity);  // 판매 가격만큼 골드 증가
                GameManager.Instance.inventoryUI.UpdateGoldUI();  // 골드 UI 업데이트
                GameManager.Instance.inventoryUI.UpdateInventoryUI();  // 골드 UI 업데이트
                GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color>을(를) {item.sellPrice} 골드에 판매했습니다!");
            }
        }
    }
    // 판매 슬롯에 드롭되었을 때 처리
    public void OnItemDroppedToSellSlot(ItemData item)
    {
        SellItemWithQuantity(item, item.itemquantity);
    }


    public void ShowQuantityPopup(ItemData item, InventorySlot slot)
    {
        // 수량 선택 팝업 창을 활성화하고, 아이템과 슬롯 정보를 전달
        quantityPopup.gameObject.SetActive(true);
        quantityPopup.InitializeSellPopup(item, slot);
    }
    public void SellItemWithQuantity(ItemData item, int quantityToSell)
    {
        InventoryManager inventoryManager = GameManager.Instance.inventoryManager;
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;

        if (inventoryManager != null)
        {
            // 인벤토리에서 수량만큼 제거
            if (inventoryManager.RemoveItemWithQuantity(item, quantityToSell))
            {
                playerInfo.SellItemGold(item.sellPrice * quantityToSell);
                GameManager.Instance.inventoryUI.UpdateGoldUI();
                GameManager.Instance.inventoryUI.UpdateInventoryUI();
                GameManager.Instance.logUI.AddMessage($"<color=green>{item.itemName}</color> {quantityToSell}개를 {item.sellPrice * quantityToSell} 골드에 판매했습니다!");
            }
        }
    }


    public void BuyBtnClick()
    {
        buyPanel.SetActive(true);
        salePanel.SetActive(false);
    }
    public void SaleBtnClick()
    {
        salePanel.SetActive(true);
        buyPanel.SetActive(false);
    }

}

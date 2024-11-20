using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;  // 인벤토리 패널
    public GameObject slotPrefab;      // 슬롯 프리팹

    public Transform weaponView;
    public Transform armorsView;
    public Transform accessoriesView;
    public Transform materialsView;
    public TextMeshProUGUI GoldText;

    public Button equipWeaponSlot;      // 무기 장착 슬롯
    public Button equipArmorSlot;       // 방어구 장착 슬롯
    public Button equipAccessorySlot;   // 장신구 장착 슬롯
    public ItemTooltip itemTooltip;     // 한 개의 툴팁 오브젝트



    private List<GameObject> weaponSlots = new List<GameObject>();
    private List<GameObject> armorSlots = new List<GameObject>();
    private List<GameObject> accessorySlots = new List<GameObject>();
    private List<GameObject> materialSlots = new List<GameObject>();

    public InventoryManager inventoryManager { get; set; }  // 인벤토리 매니저 참조
    private int MaxInventorySlots = 20;  // 인벤토리 최대 슬롯 개수

    public Button weaponBtn;
    public Button armorsBtn;
    public Button accessoriesBtn;
    public Button materialsBtn;

    public List<GameObject> InvenListPanel = new List<GameObject>();
    public GameObject deleteZone;

    public InventoryItemDeletePopup quantityPopup; // 팝업 참조
    public ChoicePopup choicePopup;
    private void Awake()
    {
        weaponBtn.onClick.AddListener(WeaponBtnClick);
        armorsBtn.onClick.AddListener(ArmorBtnClick);
        accessoriesBtn.onClick.AddListener(AccessoryBtnClick);
        materialsBtn.onClick.AddListener(MaterialBtnClick);

        equipWeaponSlot.onClick.AddListener(OnEquipWeaponSlotClicked);
        equipArmorSlot.onClick.AddListener(OnEquipArmorSlotClicked);
        equipAccessorySlot.onClick.AddListener(OnEquipAccessorySlotClicked);
        // 마우스 오버 이벤트 추가
        AddTooltipEvents(equipWeaponSlot, InventoryType.Weapon);
        AddTooltipEvents(equipArmorSlot, InventoryType.Armor);
        AddTooltipEvents(equipAccessorySlot, InventoryType.Accessory);
    }

    // 슬롯에 툴팁을 표시하기 위한 마우스 오버 이벤트 등록
    private void AddTooltipEvents(Button slot, InventoryType inventoryType)
    {
        EventTrigger trigger = slot.gameObject.AddComponent<EventTrigger>();

        // 마우스가 슬롯 위에 있을 때
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { ShowTooltip(inventoryType); });
        trigger.triggers.Add(pointerEnter);

        // 마우스가 슬롯을 벗어났을 때
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { HideTooltip(); });
        trigger.triggers.Add(pointerExit);
    }
    // 툴팁을 보여주는 메서드
    private void ShowTooltip(InventoryType inventoryType)
    {
        ItemData equippedItem = null;

        // 장착된 아이템을 가져옴
        switch (inventoryType)
        {
            case InventoryType.Weapon:
                equippedItem = GameManager.Instance.equippedItemsManager.EquippedWeapon;
                break;
            case InventoryType.Armor:
                equippedItem = GameManager.Instance.equippedItemsManager.EquippedArmor;
                break;
            case InventoryType.Accessory:
                equippedItem = GameManager.Instance.equippedItemsManager.EquippedAccessory;
                break;
        }

        // 아이템이 있으면 툴팁을 표시
        if (equippedItem != null)
        {
            itemTooltip.ShowTooltip(equippedItem, GameManager.Instance.PlayerData, true);
        }
    }

    // 툴팁을 숨기는 메서드
    private void HideTooltip()
    {
        itemTooltip.HideTooltip();
    }

    private async void Start()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.inventoryUI = this;
        }
        if (GameManager.Instance.inventoryManager != null)
        {
            inventoryManager = GameManager.Instance.inventoryManager;
        }
        if (inventoryManager == null)
        {
            return;
        }
        CreateInventorySlots();
        var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(GameManager.Instance.PlayerData.Key);
        if (loadedInventory != null)
        {
            GameManager.Instance.PlayerData.Inventory = loadedInventory;
            inventoryManager = GameManager.Instance.PlayerData.Inventory;
            UpdateInventoryUI();
        }
    }

    // 인벤토리 데이터를 로드한 후 UI를 업데이트하는 비동기 메서드
    private async Task LoadInventoryAndUpdateUI()
    {
        var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(GameManager.Instance.PlayerData.Key);
        if (loadedInventory != null)
        {
            GameManager.Instance.PlayerData.Inventory = loadedInventory;
            UpdateInventoryUI();
        }
    }
    //인벤토리 슬롯 생성 메서드
    private void CreateInventorySlots()
    {
        CreateSlotsForView(weaponView, weaponSlots, InventoryType.Weapon);
        CreateSlotsForView(armorsView, armorSlots, InventoryType.Armor);
        CreateSlotsForView(accessoriesView, accessorySlots, InventoryType.Accessory);
        CreateSlotsForView(materialsView, materialSlots, InventoryType.Material);
    }
    // 특정 뷰에 슬롯을 생성하는 메서드
    private void CreateSlotsForView(Transform viewParent, List<GameObject> slotList, InventoryType inventoryType)
    {
        for (int i = 0; i < MaxInventorySlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, viewParent);
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            slotScript.slotIndex = i;  // 고유한 인덱스 할당
            slotScript.inventoryType = inventoryType;
            slotList.Add(slot);
        }
    }

    public void ShowItemDetails(ItemData item)
    {
        // 아이템 세부 정보 UI를 업데이트 (아이템 이름, 설명, 스탯 등 표시)
        Debug.Log($"인벤토리 UI 스크립트 아이템 {item.itemName}, 수량 : {item.itemquantity} 을(를) 클릭했습니다.");
    }

    // 각 슬롯을 아이템 데이터에 맞게 업데이트
    public void UpdateInventoryUI()
    {
        if (inventoryManager == null)
        {
            return;
        }
        InventoryManager inventory = GameManager.Instance.PlayerData.Inventory;
        ClearAllSlots();
        UpdateViewSlots(inventory.Weapons, weaponSlots, InventoryType.Weapon);
        UpdateViewSlots(inventory.Armors, armorSlots, InventoryType.Armor);
        UpdateViewSlots(inventory.Accessories, accessorySlots, InventoryType.Accessory);
        UpdateViewSlots(inventory.Materials, materialSlots, InventoryType.Material);

    }
    // 특정 뷰의 슬롯 업데이트
    private void UpdateViewSlots<T>(List<T> items, List<GameObject> slotList, InventoryType inventoryType) where T : ItemData
    {
        for (int i = 0; i < items.Count && i < slotList.Count; i++)
        {
            InventorySlot slotScript = slotList[i].GetComponent<InventorySlot>();
            slotScript.SetItem(items[i], inventoryType);
        }
    }
    private void ClearAllSlots()
    {
        ClearSlots(weaponSlots);
        ClearSlots(armorSlots);
        ClearSlots(accessorySlots);
        ClearSlots(materialSlots);
    }

    public void UpdateGoldUI()
    {
        if (GoldText != null)
        {
            ClearGold();
            GoldText.text = GameManager.Instance.PlayerData.Gold.ToString() + $" Gold";
        }
    }
    public void ClearGold()
    {
        GoldItem goldValue = GetComponent<GoldItem>();
        GoldText.text = "";
    }


    private void ClearSlots(List<GameObject> slotList)
    {
        foreach (var slot in slotList)
        {
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            slotScript.ClearSlot();
        }
    }
    public void WeaponBtnClick()
    {
        DisableAllPanels();
        InvenListPanel[0].SetActive(true);
        UpdateInventoryUI();
    }
    public void ArmorBtnClick()
    {
        DisableAllPanels();
        InvenListPanel[1].SetActive(true);
        UpdateInventoryUI();
    }
    public void AccessoryBtnClick()
    {
        DisableAllPanels();
        InvenListPanel[2].SetActive(true);
        UpdateInventoryUI();
    }
    public void MaterialBtnClick()
    {
        DisableAllPanels();
        InvenListPanel[3].SetActive(true);
        UpdateInventoryUI();
    }
    // 모든 패널을 비활성화하는 메서드
    private void DisableAllPanels()
    {
        foreach (var panel in InvenListPanel)
        {
            panel.SetActive(false);  // 모든 패널을 비활성화
        }
    }

    // 장착 슬롯 UI 업데이트
    public void UpdateEquipSlots(ItemData item, InventoryType inventoryType)
    {
        switch (inventoryType)
        {

            case InventoryType.Weapon:
                if (equipWeaponSlot != null && equipWeaponSlot.GetComponent<Image>() != null)
                {
                    if (item != null)
                    {
                        equipWeaponSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.itemImage);
                        Debug.Log("이미지 아이템");
                    }
                    else
                    {
                        equipWeaponSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/WeaponSlot");
                        Debug.Log("이미지 기본값");
                    }
                }
                break;

            case InventoryType.Armor:
                if (equipArmorSlot != null && equipArmorSlot.GetComponent<Image>() != null)
                {
                    if (item != null)
                    {
                        equipArmorSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.itemImage);
                        Debug.Log("이미지 아이템");
                    }
                    else
                    {
                        equipArmorSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ArmorSlot");
                        Debug.Log("이미지 기본값");
                    }
                }
                break;

            case InventoryType.Accessory:
                if (equipAccessorySlot != null && equipAccessorySlot.GetComponent<Image>() != null)
                {
                    if (item != null)
                    {
                        equipAccessorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.itemImage);
                        Debug.Log("이미지 아이템");
                    }
                    else
                    {
                        equipAccessorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AccessorySlot");
                        Debug.Log("이미지 기본값");
                    }
                }
                break;
        }
    }

    // 무기 장착 슬롯 클릭 시 이벤트
    private void OnEquipWeaponSlotClicked()
    {
        var equippedWeapon = GameManager.Instance.equippedItemsManager.EquippedWeapon;
        if (equippedWeapon != null)
        {
            Debug.Log("클릭된 무기 데이터: " + equippedWeapon.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedWeapon, InventoryType.Weapon);
        }
        else
        {
            Debug.LogWarning("장착된 무기 데이터가 없습니다.");
        }
    }

    // 방어구 장착 슬롯 클릭 시 이벤트
    private void OnEquipArmorSlotClicked()
    {
        var equippedArmor = GameManager.Instance.equippedItemsManager.EquippedArmor;
        if (equippedArmor != null)
        { 
            Debug.Log("클릭된 무기 데이터: " + equippedArmor.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedArmor, InventoryType.Armor);
        }
        else
        {
            Debug.LogWarning("장착된 무기 데이터가 없습니다.");
        }
    }

    // 장신구 장착 슬롯 클릭 시 이벤트
    private void OnEquipAccessorySlotClicked()
    {
        var equippedAccessory = GameManager.Instance.equippedItemsManager.EquippedAccessory;
        if (equippedAccessory != null)
        {
            Debug.Log("클릭된 무기 데이터: " + equippedAccessory.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedAccessory, InventoryType.Accessory);
        }
        else
        {
            Debug.LogWarning("장착된 무기 데이터가 없습니다.");
        }
    }
    public void ShowQuantityPopup(ItemData item, InventorySlot slot)
    {
        // 수량 선택 팝업 창을 활성화하고, 아이템과 슬롯 정보를 전달
        quantityPopup.gameObject.SetActive(true);
        quantityPopup.InitializePopup(item, slot);
    }
    public void TryDeleteItem(ItemData item, InventorySlot slot)
    {
        // 텍스트확인
        choicePopup.ShowPopup($"{item.itemName}을(를)\n버리시겠습니까?", () =>
        {
            slot.DiscardItem();
        });
    }
}

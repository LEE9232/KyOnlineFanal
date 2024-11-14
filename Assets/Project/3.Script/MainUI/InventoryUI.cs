using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;  // �κ��丮 �г�
    public GameObject slotPrefab;      // ���� ������

    public Transform weaponView;
    public Transform armorsView;
    public Transform accessoriesView;
    public Transform materialsView;
    public TextMeshProUGUI GoldText;

    public Button equipWeaponSlot;      // ���� ���� ����
    public Button equipArmorSlot;       // �� ���� ����
    public Button equipAccessorySlot;   // ��ű� ���� ����
    public ItemTooltip itemTooltip;     // �� ���� ���� ������Ʈ



    private List<GameObject> weaponSlots = new List<GameObject>();
    private List<GameObject> armorSlots = new List<GameObject>();
    private List<GameObject> accessorySlots = new List<GameObject>();
    private List<GameObject> materialSlots = new List<GameObject>();

    public InventoryManager inventoryManager { get; set; }  // �κ��丮 �Ŵ��� ����
    private int MaxInventorySlots = 20;  // �κ��丮 �ִ� ���� ����

    public Button weaponBtn;
    public Button armorsBtn;
    public Button accessoriesBtn;
    public Button materialsBtn;

    public List<GameObject> InvenListPanel = new List<GameObject>();
    public GameObject deleteZone;

    public InventoryItemDeletePopup quantityPopup; // �˾� ����
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
        // ���콺 ���� �̺�Ʈ �߰�
        AddTooltipEvents(equipWeaponSlot, InventoryType.Weapon);
        AddTooltipEvents(equipArmorSlot, InventoryType.Armor);
        AddTooltipEvents(equipAccessorySlot, InventoryType.Accessory);
    }

    // ���Կ� ������ ǥ���ϱ� ���� ���콺 ���� �̺�Ʈ ���
    private void AddTooltipEvents(Button slot, InventoryType inventoryType)
    {
        EventTrigger trigger = slot.gameObject.AddComponent<EventTrigger>();

        // ���콺�� ���� ���� ���� ��
        EventTrigger.Entry pointerEnter = new EventTrigger.Entry();
        pointerEnter.eventID = EventTriggerType.PointerEnter;
        pointerEnter.callback.AddListener((eventData) => { ShowTooltip(inventoryType); });
        trigger.triggers.Add(pointerEnter);

        // ���콺�� ������ ����� ��
        EventTrigger.Entry pointerExit = new EventTrigger.Entry();
        pointerExit.eventID = EventTriggerType.PointerExit;
        pointerExit.callback.AddListener((eventData) => { HideTooltip(); });
        trigger.triggers.Add(pointerExit);
    }
    // ������ �����ִ� �޼���
    private void ShowTooltip(InventoryType inventoryType)
    {
        ItemData equippedItem = null;

        // ������ �������� ������
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

        // �������� ������ ������ ǥ��
        if (equippedItem != null)
        {
            itemTooltip.ShowTooltip(equippedItem, GameManager.Instance.PlayerData, true);
        }
    }

    // ������ ����� �޼���
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

    // �κ��丮 �����͸� �ε��� �� UI�� ������Ʈ�ϴ� �񵿱� �޼���
    private async Task LoadInventoryAndUpdateUI()
    {
        var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(GameManager.Instance.PlayerData.Key);
        if (loadedInventory != null)
        {
            GameManager.Instance.PlayerData.Inventory = loadedInventory;
            UpdateInventoryUI();
        }
    }
    //�κ��丮 ���� ���� �޼���
    private void CreateInventorySlots()
    {
        CreateSlotsForView(weaponView, weaponSlots, InventoryType.Weapon);
        CreateSlotsForView(armorsView, armorSlots, InventoryType.Armor);
        CreateSlotsForView(accessoriesView, accessorySlots, InventoryType.Accessory);
        CreateSlotsForView(materialsView, materialSlots, InventoryType.Material);
    }
    // Ư�� �信 ������ �����ϴ� �޼���
    private void CreateSlotsForView(Transform viewParent, List<GameObject> slotList, InventoryType inventoryType)
    {
        for (int i = 0; i < MaxInventorySlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, viewParent);
            InventorySlot slotScript = slot.GetComponent<InventorySlot>();
            slotScript.slotIndex = i;  // ������ �ε��� �Ҵ�
            slotScript.inventoryType = inventoryType;
            slotList.Add(slot);
        }
    }

    public void ShowItemDetails(ItemData item)
    {
        // ������ ���� ���� UI�� ������Ʈ (������ �̸�, ����, ���� �� ǥ��)
        Debug.Log($"�κ��丮 UI ��ũ��Ʈ ������ {item.itemName}, ���� : {item.itemquantity} ��(��) Ŭ���߽��ϴ�.");
    }

    // �� ������ ������ �����Ϳ� �°� ������Ʈ
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
    // Ư�� ���� ���� ������Ʈ
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
    // ��� �г��� ��Ȱ��ȭ�ϴ� �޼���
    private void DisableAllPanels()
    {
        foreach (var panel in InvenListPanel)
        {
            panel.SetActive(false);  // ��� �г��� ��Ȱ��ȭ
        }
    }

    // ���� ���� UI ������Ʈ
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
                        Debug.Log("�̹��� ������");
                    }
                    else
                    {
                        equipWeaponSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/WeaponSlot");
                        Debug.Log("�̹��� �⺻��");
                    }
                }
                break;

            case InventoryType.Armor:
                if (equipArmorSlot != null && equipArmorSlot.GetComponent<Image>() != null)
                {
                    if (item != null)
                    {
                        equipArmorSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.itemImage);
                        Debug.Log("�̹��� ������");
                    }
                    else
                    {
                        equipArmorSlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ArmorSlot");
                        Debug.Log("�̹��� �⺻��");
                    }
                }
                break;

            case InventoryType.Accessory:
                if (equipAccessorySlot != null && equipAccessorySlot.GetComponent<Image>() != null)
                {
                    if (item != null)
                    {
                        equipAccessorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>(item.itemImage);
                        Debug.Log("�̹��� ������");
                    }
                    else
                    {
                        equipAccessorySlot.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/AccessorySlot");
                        Debug.Log("�̹��� �⺻��");
                    }
                }
                break;
        }
    }

    // ���� ���� ���� Ŭ�� �� �̺�Ʈ
    private void OnEquipWeaponSlotClicked()
    {
        var equippedWeapon = GameManager.Instance.equippedItemsManager.EquippedWeapon;
        if (equippedWeapon != null)
        {
            Debug.Log("Ŭ���� ���� ������: " + equippedWeapon.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedWeapon, InventoryType.Weapon);
        }
        else
        {
            Debug.LogWarning("������ ���� �����Ͱ� �����ϴ�.");
        }
    }

    // �� ���� ���� Ŭ�� �� �̺�Ʈ
    private void OnEquipArmorSlotClicked()
    {
        var equippedArmor = GameManager.Instance.equippedItemsManager.EquippedArmor;
        if (equippedArmor != null)
        { 
            Debug.Log("Ŭ���� ���� ������: " + equippedArmor.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedArmor, InventoryType.Armor);
        }
        else
        {
            Debug.LogWarning("������ ���� �����Ͱ� �����ϴ�.");
        }
    }

    // ��ű� ���� ���� Ŭ�� �� �̺�Ʈ
    private void OnEquipAccessorySlotClicked()
    {
        var equippedAccessory = GameManager.Instance.equippedItemsManager.EquippedAccessory;
        if (equippedAccessory != null)
        {
            Debug.Log("Ŭ���� ���� ������: " + equippedAccessory.itemName);
            GameManager.Instance.equipUI.ShowUnequipConfirmation(equippedAccessory, InventoryType.Accessory);
        }
        else
        {
            Debug.LogWarning("������ ���� �����Ͱ� �����ϴ�.");
        }
    }
    public void ShowQuantityPopup(ItemData item, InventorySlot slot)
    {
        // ���� ���� �˾� â�� Ȱ��ȭ�ϰ�, �����۰� ���� ������ ����
        quantityPopup.gameObject.SetActive(true);
        quantityPopup.InitializePopup(item, slot);
    }
    public void TryDeleteItem(ItemData item, InventorySlot slot)
    {
        // �ؽ�ƮȮ��
        choicePopup.ShowPopup($"{item.itemName}��(��)\n�����ðڽ��ϱ�?", () =>
        {
            slot.DiscardItem();
        });
    }
}

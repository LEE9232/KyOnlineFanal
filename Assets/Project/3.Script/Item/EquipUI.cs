using UnityEngine;
using UnityEngine.UI;

public class EquipUI : MonoBehaviour
{
    public GameObject equipConfirmPanel; // 장착 확인 패널
    public Button equipButton; // 장착 버튼
    public Button cancelButton; // 취소 버튼
    public Button unequipButton; // 장착 해제 버튼
    private InventorySlot currentSlot; // 현재 선택된 슬롯
    private ItemData currentItem; // 현재 선택된 아이템
    private InventoryType currentType;
    public ChoicePopup equipPopup;

    private void Awake()
    {
        equipButton.onClick.AddListener(OnEquipButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        unequipButton.onClick.AddListener(OnUnequipButtonClicked);
        HideEquipConfirmation();
    }
    // 인벤토리 슬롯에있는 슬롯 클릭시 나옴.
    public void ShowEquipConfirmation(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;
        equipConfirmPanel.SetActive(true); // 장착 확인 패널을 활성화
        equipButton.interactable = true;
        unequipButton.interactable = false;
    }
    public void ShowUnequipConfirmation(ItemData item, InventoryType type)
    {
        currentItem = item;
        currentType = type;
        equipConfirmPanel.SetActive(true); // 장착 해제 확인 패널 활성화
        equipButton.interactable=false;
        unequipButton.interactable = true;
    }
    public void HideEquipConfirmation()
    {
        equipConfirmPanel.SetActive(false); // 장착 확인 패널을 비활성화
    }

    private void OnEquipButtonClicked()
    {
        equipPopup.ShowPopup($"{currentItem.itemName}을(를) 장착하시겠습니까?", () =>
        {
            EquipItem();

        });
    }

    // 아이템 장착 처리
    private void EquipItem()
    {
        if (currentSlot != null && currentItem != null)
        {
            GameManager.Instance.inventoryUI.UpdateEquipSlots(currentItem, currentSlot.inventoryType);
            GameManager.Instance.inventoryManager.EquipItem(currentItem, GameManager.Instance.PlayerData, currentSlot.inventoryType);
            HideEquipConfirmation();
        }
    }
    // 아이템 장착 처리
    private void UnEquipItem()
    {
        if (currentSlot != null && currentItem != null)
        {
            GameManager.Instance.inventoryManager.UnEquipItem(currentItem, GameManager.Instance.PlayerData, currentType);
            HideEquipConfirmation();

        }
    }
    private void OnUnequipButtonClicked()
    {
        equipPopup.ShowPopup($"{currentItem.itemName}을(를) 해제하시겠습니까?", () =>
        {
            UnEquipItem();
        });
    }
    private void OnCancelButtonClicked()
    {
        HideEquipConfirmation(); 
    }
}

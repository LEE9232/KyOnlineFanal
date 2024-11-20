using UnityEngine;
using UnityEngine.UI;

public class EquipUI : MonoBehaviour
{
    public GameObject equipConfirmPanel; // ���� Ȯ�� �г�
    public Button equipButton; // ���� ��ư
    public Button cancelButton; // ��� ��ư
    public Button unequipButton; // ���� ���� ��ư
    private InventorySlot currentSlot; // ���� ���õ� ����
    private ItemData currentItem; // ���� ���õ� ������
    private InventoryType currentType;
    public ChoicePopup equipPopup;

    private void Awake()
    {
        equipButton.onClick.AddListener(OnEquipButtonClicked);
        cancelButton.onClick.AddListener(OnCancelButtonClicked);
        unequipButton.onClick.AddListener(OnUnequipButtonClicked);
        HideEquipConfirmation();
    }
    // �κ��丮 ���Կ��ִ� ���� Ŭ���� ����.
    public void ShowEquipConfirmation(ItemData item, InventorySlot slot)
    {
        currentItem = item;
        currentSlot = slot;
        equipConfirmPanel.SetActive(true); // ���� Ȯ�� �г��� Ȱ��ȭ
        equipButton.interactable = true;
        unequipButton.interactable = false;
    }
    public void ShowUnequipConfirmation(ItemData item, InventoryType type)
    {
        currentItem = item;
        currentType = type;
        equipConfirmPanel.SetActive(true); // ���� ���� Ȯ�� �г� Ȱ��ȭ
        equipButton.interactable=false;
        unequipButton.interactable = true;
    }
    public void HideEquipConfirmation()
    {
        equipConfirmPanel.SetActive(false); // ���� Ȯ�� �г��� ��Ȱ��ȭ
    }

    private void OnEquipButtonClicked()
    {
        equipPopup.ShowPopup($"{currentItem.itemName}��(��) �����Ͻðڽ��ϱ�?", () =>
        {
            EquipItem();

        });
    }

    // ������ ���� ó��
    private void EquipItem()
    {
        if (currentSlot != null && currentItem != null)
        {
            GameManager.Instance.inventoryUI.UpdateEquipSlots(currentItem, currentSlot.inventoryType);
            GameManager.Instance.inventoryManager.EquipItem(currentItem, GameManager.Instance.PlayerData, currentSlot.inventoryType);
            HideEquipConfirmation();
        }
    }
    // ������ ���� ó��
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
        equipPopup.ShowPopup($"{currentItem.itemName}��(��) �����Ͻðڽ��ϱ�?", () =>
        {
            UnEquipItem();
        });
    }
    private void OnCancelButtonClicked()
    {
        HideEquipConfirmation(); 
    }
}

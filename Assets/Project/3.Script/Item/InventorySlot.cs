using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum InventoryType
{
    Weapon,
    Armor,
    Accessory,
    Material
}
public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage; // ���Կ� �ִ� ������ �̹���
    public Text quantityText;
    private Canvas canvas; // ĵ������ �����ؼ� UI�� ����� ������� ��
    private RectTransform rectTransform; // �������� ��ġ�� ����
    public ItemData currentItem; // ���Կ� �ִ� ���� ������ ������
    public Button slotButton; // ���� ��ư
    private Vector2 originalPosition; // ������ ���� ��ġ ����
    public int slotIndex; // ���� �ε���
    public InventoryType inventoryType;  // ������ ���� �κ��丮 ����
    private CanvasGroup canvasGroup;
    private ItemTooltip tooltip;  // ������ ����
    public GameObject equipSlot;
    public EquipUI equipConfirmUI; // ���� Ȯ�� UI
    private void Awake()
    {
        slotButton = GetComponentInChildren<Button>(); // ���� ��ư ��������
        slotButton.onClick.AddListener(OnSlotClicked); // ��ư�� Ŭ�� �̺�Ʈ ����
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        tooltip = FindObjectOfType<ItemTooltip>(); // ������ ������ ã��
        equipConfirmUI = FindObjectOfType<EquipUI>(); // ���� Ȯ�� UI ã��
    }

    public void SetItem(ItemData itemData, InventoryType inventoryType)
    {
        this.inventoryType = inventoryType; // �κ��丮 ���� ����
        currentItem = itemData;
        // ������ �̹��� ����
        if (currentItem != null)
        {
            itemImage.sprite = Resources.Load<Sprite>(currentItem.itemImage);
            quantityText.text = currentItem.itemquantity > 1 ? currentItem.itemquantity.ToString() : "";
        }
        else
        {
            ClearSlot();  // �������� ������ ���� ����
        }
    }
    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = Resources.Load<Sprite>("Image/InventorySlotBackGround"); ;
        quantityText.text = "";
    }
    private void OnSlotClicked()
    {
        if (currentItem != null)
        {
            equipConfirmUI.ShowEquipConfirmation(currentItem, this);
        }
        else
        {
            Debug.Log("�� ���� Ŭ����");
        }
    }
    // ���콺�� �÷��� �� ���� ǥ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            tooltip.ShowTooltip(currentItem); // ������ ������
        }
    }

    // ���콺�� ����� �� ���� ����
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip(); // ������ ����
    }
    // �巡�׾� ���
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null)
        {
            // �� ���Կ����� �巡�׸� ������ �� ������ ��
            Debug.Log("�� ������ �巡���� �� �����ϴ�.");
            canvasGroup.blocksRaycasts = true; // �巡�� �Ұ� �� Raycast ���� ����
            return;
        }
        canvasGroup.alpha = 0.6f; // �������ϰ�
        canvasGroup.blocksRaycasts = false; // �巡�� �߿��� Raycast ����
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // �巡�� ���� ����ٴϰ�
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if (currentItem == null)
        {
            canvasGroup.alpha = 1.0f; // �巡�װ� ������ ���� ����
            canvasGroup.blocksRaycasts = true; // Raycast�� �ٽ� ����
            return; // �������� ���� ��� �ƹ��� ���۵� ���� �ʰ� ����
        }
        canvasGroup.alpha = 1.0f; // �巡�װ� ������ ���� ����
        canvasGroup.blocksRaycasts = true;


        // Debug �߰� - GameManager �� ���� ���� Ȯ��
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance�� null�Դϴ�.");
            return;
        }    
        if (GameManager.Instance.inventoryUI == null)
        {
            Debug.LogError("GameManager.Instance.inventoryUI�� null�Դϴ�.");
            return;
        }     
        if (GameManager.Instance.shopManager == null)
        {
            Debug.LogError("GameManager.Instance.shopManager�� null�Դϴ�.");
            return;
        }      
        if (GameManager.Instance.inventoryUI.deleteZone == null)
        {
            Debug.LogError("GameManager.Instance.inventoryUI.deleteZone�� null�Դϴ�.");
            return;
        }     
        if (GameManager.Instance.shopManager.deleteZone == null)
        {
            Debug.LogError("GameManager.Instance.shopManager.deleteZone�� null�Դϴ�.");
            return;
        }
        RectTransform trashAreaRect = GameManager.Instance.inventoryUI.deleteZone.GetComponent<RectTransform>();
        Vector2 localMousePosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(trashAreaRect, eventData.position, null, out localMousePosition))
        {
            if (trashAreaRect.rect.Contains(localMousePosition))
            {
                if (currentItem.itemquantity > 1)
                {
                    GameManager.Instance.inventoryUI.ShowQuantityPopup(currentItem, this);
                }
                else
                {
                    GameManager.Instance.inventoryUI.TryDeleteItem(currentItem, this);
                }
                return;
            }
        }
        RectTransform SellAreaRect = GameManager.Instance.shopManager.deleteZone.GetComponent<RectTransform>();
        Vector2 localMouseSellPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(SellAreaRect, eventData.position, null, out localMouseSellPosition))
        {
            if (trashAreaRect.rect.Contains(localMouseSellPosition))
            {
                Debug.Log($"�Ǹ� ������ {currentItem.itemName} �����.");
                if (currentItem.itemquantity > 1)
                {
                    GameManager.Instance.shopManager.ShowQuantityPopup(currentItem, this);
                }
                else
                {
                    GameManager.Instance.shopManager.SellItem(currentItem);
                }
                return;
            }
        }
        // �κ��丮 ���� Ȯ��
        RectTransform inventoryRect = GameManager.Instance.inventoryUI.inventoryPanel.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, eventData.position, null))
        {
            // �κ��丮 ���ο� �ִ� ��� ���� �� �̵� ó��
            InventorySlot targetSlot = FindSlotAtPosition(eventData.position);
            if (targetSlot != null)
            {
                if (targetSlot.currentItem == null)
                {
                    // �� �������� �̵�
                    MoveItemToSlot(targetSlot);
                }
                else
                {
                    // ���� ��ü
                    SwapItems(targetSlot);
                }
            }
        }
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        rectTransform.anchoredPosition = originalPosition;
    }
    // ������ ������ �޼���
    public void DiscardItem()
    {
        if (currentItem != null)
        {
            GameManager.Instance.PlayerData.Inventory.RemoveItem(currentItem);
            currentItem = null;
            ClearSlot();
        }
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
    }
    private void MoveItemToSlot(InventorySlot targetSlot)
    {
        if (currentItem == null || targetSlot.inventoryType != this.inventoryType)
            return; 
                    
        if (targetSlot.currentItem == null)
        {
            GameManager.Instance.PlayerData.Inventory.MoveItemInCategory(this.inventoryType, this.slotIndex, targetSlot.slotIndex);
            targetSlot.SetItem(currentItem, inventoryType); // ��� ���Կ� �������� ����    
            ClearSlot(); // ���� ������ ���
        }
        GameManager.Instance.PlayerData.Inventory.UpdateInventoryUI(); // UI ����
    }
    private void SwapItems(InventorySlot targetSlot)
    {
        if (targetSlot.inventoryType != this.inventoryType)
            return;  // �ٸ� ī�װ��� ��ȯ�� �õ��ϸ� ����

        GameManager.Instance.PlayerData.Inventory.SwapItemsInCategory(this.inventoryType, this.slotIndex, targetSlot.slotIndex);

        ItemData tempItem = targetSlot.currentItem;
        targetSlot.SetItem(this.currentItem, this.inventoryType); // Ÿ�� ���Կ� ���� ������ ����
        SetItem(tempItem, this.inventoryType); // ���� ���Կ� Ÿ�� ������ ������ ����
        GameManager.Instance.PlayerData.Inventory.UpdateInventoryUI();
    }

    private InventorySlot FindSlotAtPosition(Vector2 position)
    {
        foreach (InventorySlot slot in FindObjectsOfType<InventorySlot>())
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(slot.GetComponent<RectTransform>(), position))
            {
                return slot;
            }
        }
        return null;
    }
    // ������ ������ŭ �������� ������ �޼���
    public void DiscardItemWithQuantity(int quantityToDiscard)
    {
        if (currentItem != null)
        {
            if (currentItem.itemquantity > quantityToDiscard) // ������ ������ŭ�� ����
            {
                currentItem.itemquantity -= quantityToDiscard;
                SetItem(currentItem, inventoryType); // ���� ������ ������Ʈ
            }
            else
            {
                Debug.Log($"{currentItem.itemName}��(��) ��� �����ϴ�.");
                GameManager.Instance.PlayerData.Inventory.RemoveItem(currentItem);
                currentItem = null;
                ClearSlot();
            }
            GameManager.Instance.inventoryUI.UpdateInventoryUI();  // UI ������Ʈ
        }
    }
}

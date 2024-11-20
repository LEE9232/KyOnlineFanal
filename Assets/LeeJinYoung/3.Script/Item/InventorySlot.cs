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
    public Image itemImage; // 슬롯에 있는 아이템 이미지
    public Text quantityText;
    private Canvas canvas; // 캔버스를 참조해서 UI가 제대로 따라오게 함
    private RectTransform rectTransform; // 아이템의 위치를 관리
    public ItemData currentItem; // 슬롯에 있는 현재 아이템 데이터
    public Button slotButton; // 슬롯 버튼
    private Vector2 originalPosition; // 슬롯의 원래 위치 저장
    public int slotIndex; // 슬롯 인덱스
    public InventoryType inventoryType;  // 슬롯이 속한 인벤토리 종류
    private CanvasGroup canvasGroup;
    private ItemTooltip tooltip;  // 툴팁을 참조
    public GameObject equipSlot;
    public EquipUI equipConfirmUI; // 장착 확인 UI
    private void Awake()
    {
        slotButton = GetComponentInChildren<Button>(); // 슬롯 버튼 가져오기
        slotButton.onClick.AddListener(OnSlotClicked); // 버튼에 클릭 이벤트 연결
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = FindObjectOfType<Canvas>();
        originalPosition = rectTransform.anchoredPosition;
        tooltip = FindObjectOfType<ItemTooltip>(); // 씬에서 툴팁을 찾음
        equipConfirmUI = FindObjectOfType<EquipUI>(); // 장착 확인 UI 찾기
    }

    public void SetItem(ItemData itemData, InventoryType inventoryType)
    {
        this.inventoryType = inventoryType; // 인벤토리 종류 설정
        currentItem = itemData;
        // 아이템 이미지 설정
        if (currentItem != null)
        {
            itemImage.sprite = Resources.Load<Sprite>(currentItem.itemImage);
            quantityText.text = currentItem.itemquantity > 1 ? currentItem.itemquantity.ToString() : "";
        }
        else
        {
            ClearSlot();  // 아이템이 없으면 슬롯 비우기
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
            Debug.Log("빈 슬롯 클릭됨");
        }
    }
    // 마우스를 올렸을 때 툴팁 표시
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            tooltip.ShowTooltip(currentItem); // 툴팁을 보여줌
        }
    }

    // 마우스를 벗어났을 때 툴팁 숨김
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.HideTooltip(); // 툴팁을 숨김
    }
    // 드래그앤 드랍
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null)
        {
            // 빈 슬롯에서는 드래그를 시작할 수 없도록 함
            Debug.Log("빈 슬롯은 드래그할 수 없습니다.");
            canvasGroup.blocksRaycasts = true; // 드래그 불가 시 Raycast 차단 해제
            return;
        }
        canvasGroup.alpha = 0.6f; // 반투명하게
        canvasGroup.blocksRaycasts = false; // 드래그 중에는 Raycast 막기
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; // 드래그 동안 따라다니게
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {

        if (currentItem == null)
        {
            canvasGroup.alpha = 1.0f; // 드래그가 끝나면 투명도 복원
            canvasGroup.blocksRaycasts = true; // Raycast를 다시 차단
            return; // 아이템이 없을 경우 아무런 동작도 하지 않고 종료
        }
        canvasGroup.alpha = 1.0f; // 드래그가 끝나면 투명도 복원
        canvasGroup.blocksRaycasts = true;


        // Debug 추가 - GameManager 및 관련 참조 확인
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager.Instance가 null입니다.");
            return;
        }    
        if (GameManager.Instance.inventoryUI == null)
        {
            Debug.LogError("GameManager.Instance.inventoryUI가 null입니다.");
            return;
        }     
        if (GameManager.Instance.shopManager == null)
        {
            Debug.LogError("GameManager.Instance.shopManager가 null입니다.");
            return;
        }      
        if (GameManager.Instance.inventoryUI.deleteZone == null)
        {
            Debug.LogError("GameManager.Instance.inventoryUI.deleteZone이 null입니다.");
            return;
        }     
        if (GameManager.Instance.shopManager.deleteZone == null)
        {
            Debug.LogError("GameManager.Instance.shopManager.deleteZone이 null입니다.");
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
                Debug.Log($"판매 영역에 {currentItem.itemName} 드랍됨.");
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
        // 인벤토리 영역 확인
        RectTransform inventoryRect = GameManager.Instance.inventoryUI.inventoryPanel.GetComponent<RectTransform>();
        if (RectTransformUtility.RectangleContainsScreenPoint(inventoryRect, eventData.position, null))
        {
            // 인벤토리 내부에 있는 경우 슬롯 간 이동 처리
            InventorySlot targetSlot = FindSlotAtPosition(eventData.position);
            if (targetSlot != null)
            {
                if (targetSlot.currentItem == null)
                {
                    // 빈 슬롯으로 이동
                    MoveItemToSlot(targetSlot);
                }
                else
                {
                    // 슬롯 교체
                    SwapItems(targetSlot);
                }
            }
        }
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        rectTransform.anchoredPosition = originalPosition;
    }
    // 아이템 버리는 메서드
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
            targetSlot.SetItem(currentItem, inventoryType); // 대상 슬롯에 아이템을 설정    
            ClearSlot(); // 원래 슬롯을 비움
        }
        GameManager.Instance.PlayerData.Inventory.UpdateInventoryUI(); // UI 갱신
    }
    private void SwapItems(InventorySlot targetSlot)
    {
        if (targetSlot.inventoryType != this.inventoryType)
            return;  // 다른 카테고리로 교환을 시도하면 무시

        GameManager.Instance.PlayerData.Inventory.SwapItemsInCategory(this.inventoryType, this.slotIndex, targetSlot.slotIndex);

        ItemData tempItem = targetSlot.currentItem;
        targetSlot.SetItem(this.currentItem, this.inventoryType); // 타겟 슬롯에 현재 아이템 설정
        SetItem(tempItem, this.inventoryType); // 현재 슬롯에 타겟 슬롯의 아이템 설정
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
    // 선택한 수량만큼 아이템을 버리는 메서드
    public void DiscardItemWithQuantity(int quantityToDiscard)
    {
        if (currentItem != null)
        {
            if (currentItem.itemquantity > quantityToDiscard) // 선택한 수량만큼만 줄임
            {
                currentItem.itemquantity -= quantityToDiscard;
                SetItem(currentItem, inventoryType); // 남은 수량을 업데이트
            }
            else
            {
                Debug.Log($"{currentItem.itemName}을(를) 모두 버립니다.");
                GameManager.Instance.PlayerData.Inventory.RemoveItem(currentItem);
                currentItem = null;
                ClearSlot();
            }
            GameManager.Instance.inventoryUI.UpdateInventoryUI();  // UI 업데이트
        }
    }
}

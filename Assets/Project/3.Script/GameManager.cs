using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerInfo PlayerData { get; set; }
    public InventoryManager inventoryManager { get; set; } // Inventory manager
    public EquippedItems equippedItemsManager { get; set; } // 장착된 아이템 관리
    public InventoryUI inventoryUI { get; set; } // InventoryUI 참조
    public ShopManager shopManager { get; set; }
    public LogUI logUI { get; set; }  // 스크립트 인스턴스
    public PlayerManagement playerManagement { get; set; }  // PlayerManagement 인스턴스 추가
    public PlayerQuestListPanel playerQuestListPanel { get; set; }
    public EquipUI equipUI { get; set; }

    // 로그인 체크
    public bool IsLoggedIn { get; set; }

    // 멀티로 접속인지 체크 = true(멀티) false(솔로)
    public bool IsMultiplayer { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerData = new PlayerInfo();
            inventoryManager = PlayerData.Inventory;
            equippedItemsManager = PlayerData.EquippedItemsManager; // 초기화
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager 초기화 실패");
            }
            else
            {
                Debug.Log("InventoryManager 초기화 성공");
            }                                      
            // PlayerManagement를 씬에서 가져오기
            //playerManagement = FindObjectOfType<PlayerManagement>();

            //if (playerManagement != null)
            //{
            //    Debug.LogError("PlayerManagement가 널이 아닙니다");
            //}
            //
            //if (playerManagement == null)
            //{
            //    Debug.LogError("PlayerManagement가 널이 아닙니(게임매니저)");
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePlayerDataLocally(PlayerInfo updatedData)
    {
        // 로컬 데이터 갱신
        PlayerData = updatedData;
        inventoryManager = PlayerData.Inventory;
        equippedItemsManager = PlayerData.EquippedItemsManager;

        if (inventoryManager == null)
        {
            Debug.LogError("InventoryManager is null after assigning PlayerData.Inventory!");
        }
        else
        {
            Debug.Log($"InventoryManager updated - Weapons: {inventoryManager.Weapons.Count}, Materials: {inventoryManager.Materials.Count}, Armors: {inventoryManager.Armors.Count}, Accessories: {inventoryManager.Accessories.Count},");
        }

        // 로컬 데이터 갱신
        // InventoryUI가 null인지 확인
        if (inventoryUI != null)
        {
            inventoryUI.UpdateInventoryUI();
            //SavePlayerDataToServer(PlayerData.Key);
        }
    }
    public async void SavePlayerDataToServer(string characterKey)
    {
        if (string.IsNullOrEmpty(characterKey))
        {
            Debug.LogError("Character key is not set.");
            return;
        }
        // 서버에 데이터를 업데이트
        await FirebaseManeger.Instance.SaveInventoryToDatabaseAsync(characterKey);
        await FirebaseManeger.Instance.SaveCharacterDataAsync(characterKey);
        await FirebaseManeger.Instance.SaveEquippedItemsToFirebase(characterKey);
    }
}

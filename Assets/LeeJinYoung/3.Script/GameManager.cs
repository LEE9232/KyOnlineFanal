using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerInfo PlayerData { get; set; }
    public InventoryManager inventoryManager { get; set; } // Inventory manager
    public EquippedItems equippedItemsManager { get; set; } // ������ ������ ����
    public InventoryUI inventoryUI { get; set; } // InventoryUI ����
    public ShopManager shopManager { get; set; }
    public LogUI logUI { get; set; }  // ��ũ��Ʈ �ν��Ͻ�
    public PlayerManagement playerManagement { get; set; }  // PlayerManagement �ν��Ͻ� �߰�
    public PlayerQuestListPanel playerQuestListPanel { get; set; }
    public EquipUI equipUI { get; set; }

    // �α��� üũ
    public bool IsLoggedIn { get; set; }

    // ��Ƽ�� �������� üũ = true(��Ƽ) false(�ַ�)
    public bool IsMultiplayer { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PlayerData = new PlayerInfo();
            inventoryManager = PlayerData.Inventory;
            equippedItemsManager = PlayerData.EquippedItemsManager; // �ʱ�ȭ
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryManager �ʱ�ȭ ����");
            }
            else
            {
                Debug.Log("InventoryManager �ʱ�ȭ ����");
            }                                      
            // PlayerManagement�� ������ ��������
            //playerManagement = FindObjectOfType<PlayerManagement>();

            //if (playerManagement != null)
            //{
            //    Debug.LogError("PlayerManagement�� ���� �ƴմϴ�");
            //}
            //
            //if (playerManagement == null)
            //{
            //    Debug.LogError("PlayerManagement�� ���� �ƴմ�(���ӸŴ���)");
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePlayerDataLocally(PlayerInfo updatedData)
    {
        // ���� ������ ����
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

        // ���� ������ ����
        // InventoryUI�� null���� Ȯ��
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
        // ������ �����͸� ������Ʈ
        await FirebaseManeger.Instance.SaveInventoryToDatabaseAsync(characterKey);
        await FirebaseManeger.Instance.SaveCharacterDataAsync(characterKey);
        await FirebaseManeger.Instance.SaveEquippedItemsToFirebase(characterKey);
    }
}

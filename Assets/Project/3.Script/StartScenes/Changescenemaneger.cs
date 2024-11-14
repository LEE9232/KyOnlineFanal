using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Changescenemaneger : MonoBehaviour
{

    public static Changescenemaneger Instance { get; private set; }
    private List<string> gameScenes = new List<string> { "Stage1", "Dungion1", "BossRoom" };
    public Slider progressBar;
    public GameObject loadingScreen;
    public TextEffect textEffect;
    public Text progressCount;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(loadingScreen);  // loadingScreen�� �� ��ȯ �Ŀ��� ����
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        // �� �ε� �Ϸ� �̺�Ʈ ����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // �κ��丮 ���� �޼��� ȣ�� �� �� ��ȯ
    private async Task SaveDataAndChangeScene(string sceneName)
    {
        if (GameManager.Instance.PlayerData == null || string.IsNullOrEmpty(GameManager.Instance.PlayerData.Key))
        {
            Debug.LogError("Cannot save data or change scene because PlayerData or Character Key is not set.");
            return;
        }
        try
        {
            string characterKey = GameManager.Instance.PlayerData.Key;
            // �κ��丮 �����͸� ���� (������ ���� ��쿡��)
            if (GameManager.Instance.PlayerData.Inventory.IsInventoryChanged())
            {
                await FirebaseManeger.Instance.SaveInventoryToDatabaseAsync(characterKey);
                GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag(); // ���� �� �÷��� ����
            }
            // ���� �������� ����Ǿ��� ���� ����
            if (GameManager.Instance.PlayerData.EquippedItemsManager.IsEquippedItemsChanged())
            {
                await FirebaseManeger.Instance.SaveEquippedItemsToFirebase(characterKey);
                GameManager.Instance.PlayerData.EquippedItemsManager.ResetEquippedItemsChangeFlag(); // �÷��� ����
            }
            // ĳ���� �����͵� ���� ���θ� Ȯ���� �����ϴ� ������� ������ �� �ֽ��ϴ�.
            if (GameManager.Instance.PlayerData.IsDataChanged())
            {
                await FirebaseManeger.Instance.SaveCharacterDataAsync(characterKey);
                GameManager.Instance.PlayerData.ResetDataChangeFlag(); // �÷��� ����
            }
            // ���� �񵿱� �ε�
            loadingScreen.SetActive(true);
            textEffect.StartTyping("L o d i n g . . .");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;  // �� �ڵ� Ȱ��ȭ ����
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);  // ���൵ ���
                progressBar.value = progress;  // �����̴� �� ������Ʈ
                progressCount.text = $"{progress * 100} %";
                // �� ��ȯ �߿� ������Ʈ�� �ı��Ǿ����� üũ
                if (loadingScreen == null)
                {
                    Debug.LogError("Loading screen object was destroyed.");
                    return;
                }

                // �� �ε尡 �Ϸ�Ǹ� �� Ȱ��ȭ
                if (asyncLoad.progress >= 0.9f)
                {
                    // ��� �ε��� �Ϸ�Ǹ� ���� Ȱ��ȭ
                    await Task.Delay(1500);  // 2��
                    asyncLoad.allowSceneActivation = true;
                }
                await Task.Yield();        
            }
            loadingScreen.SetActive(false);
            // ���� ��ȯ�� ��, �κ��丮 �����͸� �ٽ� �ε�
            var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(characterKey);
            var equippedItems = await FirebaseManeger.Instance.LoadEquippedItemsFromFirebase(characterKey);
            if (loadedInventory != null)
            {
                GameManager.Instance.PlayerData.Inventory = loadedInventory;
            }
            else
            {
                Debug.LogWarning("No inventory data found, creating a new inventory.");
                GameManager.Instance.PlayerData.Inventory = new InventoryManager();
                await FirebaseManeger.Instance.SaveInventoryToDatabaseAsync(characterKey);
                await FirebaseManeger.Instance.SaveCharacterDataAsync(characterKey);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save data or change scene: {ex.Message}");
        }
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (gameScenes.Contains(scene.name))
        {
            string characterKey = GameManager.Instance.PlayerData.Key;
            //Debug.Log("�� �ε� �� ���� ������ �ε� ����");
            // �κ��丮 ������ �ε�
            var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(characterKey);
            var equippedItems = await FirebaseManeger.Instance.LoadEquippedItemsFromFirebase(characterKey);
            // �����Ͱ� ���������� �ε�� �Ŀ��� UI ������Ʈ
            if (loadedInventory != null)
            {
                GameManager.Instance.PlayerData.Inventory = loadedInventory;
                StartCoroutine(UpdateInventoryUIAfterLoad());
            }
            if (equippedItems != null)
            {
                // ���� �������� null �� �� �����Ƿ�, null ���θ� Ȯ�� �� ������Ʈ
                GameManager.Instance.equippedItemsManager = equippedItems;

                // 4. Base ���� �ʱ�ȭ (�⺻ �������� �ٽ� ����)
                GameManager.Instance.PlayerData.ResetToBaseStats();
                // ���� ���� UI ����
                if (GameManager.Instance.equipUI != null)
                {
                    GameManager.Instance.PlayerData.ApplyEquippedItemsStats(GameManager.Instance.PlayerData, equippedItems);
                    GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedItems.EquippedWeapon, InventoryType.Weapon);
                    GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedItems.EquippedArmor, InventoryType.Armor);
                    GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedItems.EquippedAccessory, InventoryType.Accessory);
                }
            }
            else
            {
                // ������ �������� ���� ���� ó��
                GameManager.Instance.equippedItemsManager = new EquippedItems();
            }
        }
    }
    private IEnumerator UpdateInventoryUIAfterLoad()
    {
        // ��� ��ٸ� �Ŀ� UI�� ã��
        yield return new WaitForSeconds(0.5f); // 0.5�� ��� �� ����
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        ShopManager shopManager = FindObjectOfType<ShopManager>();
        EquipUI equipUI = FindObjectOfType<EquipUI>();
        LogUI logUI = FindObjectOfType<LogUI>();

        PlayerQuestListPanel playerQuestListPanel = FindObjectOfType<PlayerQuestListPanel>();
        if (inventoryUI != null)
        {
            GameManager.Instance.inventoryUI = inventoryUI;
            GameManager.Instance.shopManager = shopManager;
            GameManager.Instance.equipUI = equipUI;
            GameManager.Instance.playerQuestListPanel = playerQuestListPanel;
            inventoryUI.UpdateInventoryUI(); // �κ��丮 UI ������Ʈ
            inventoryUI.UpdateGoldUI();
            if (logUI != null)
            {
                GameManager.Instance.logUI = logUI;  // GameManager�� LogUI �Ҵ�
                GameManager.Instance.logUI.AddMessage("������ ȯ���մϴ�!!");  // �κ��丮 �ε� �� �α� ���
            }
        }
    }
    public async void StartScecn()
    {
        await SaveDataAndChangeScene("StartScenes");
    }
    public async void StageOneScene()
    {
        await SaveDataAndChangeScene("Stage1");
    }
    public async void DungionScecn()
    {
        await SaveDataAndChangeScene("Dungion1");
    }
    public async void BossScecn()
    {
        await SaveDataAndChangeScene("BossRoom");
    }

    public async void LobbyScene()
    {
        await SaveDataAndChangeScene("LobbyScene");
    }
}

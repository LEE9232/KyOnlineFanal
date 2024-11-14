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
            DontDestroyOnLoad(loadingScreen);  // loadingScreen을 씬 전환 후에도 유지
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        // 씬 로드 완료 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // 인벤토리 저장 메서드 호출 후 씬 전환
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
            // 인벤토리 데이터를 저장 (변경이 있을 경우에만)
            if (GameManager.Instance.PlayerData.Inventory.IsInventoryChanged())
            {
                await FirebaseManeger.Instance.SaveInventoryToDatabaseAsync(characterKey);
                GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag(); // 저장 후 플래그 리셋
            }
            // 장착 아이템이 변경되었을 때만 저장
            if (GameManager.Instance.PlayerData.EquippedItemsManager.IsEquippedItemsChanged())
            {
                await FirebaseManeger.Instance.SaveEquippedItemsToFirebase(characterKey);
                GameManager.Instance.PlayerData.EquippedItemsManager.ResetEquippedItemsChangeFlag(); // 플래그 리셋
            }
            // 캐릭터 데이터도 변경 여부를 확인해 저장하는 방식으로 구현할 수 있습니다.
            if (GameManager.Instance.PlayerData.IsDataChanged())
            {
                await FirebaseManeger.Instance.SaveCharacterDataAsync(characterKey);
                GameManager.Instance.PlayerData.ResetDataChangeFlag(); // 플래그 리셋
            }
            // 씬을 비동기 로드
            loadingScreen.SetActive(true);
            textEffect.StartTyping("L o d i n g . . .");
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;  // 씬 자동 활성화 방지
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);  // 진행도 계산
                progressBar.value = progress;  // 슬라이더 값 업데이트
                progressCount.text = $"{progress * 100} %";
                // 씬 전환 중에 오브젝트가 파괴되었는지 체크
                if (loadingScreen == null)
                {
                    Debug.LogError("Loading screen object was destroyed.");
                    return;
                }

                // 씬 로드가 완료되면 씬 활성화
                if (asyncLoad.progress >= 0.9f)
                {
                    // 모든 로딩이 완료되면 씬을 활성화
                    await Task.Delay(1500);  // 2초
                    asyncLoad.allowSceneActivation = true;
                }
                await Task.Yield();        
            }
            loadingScreen.SetActive(false);
            // 씬이 전환된 후, 인벤토리 데이터를 다시 로드
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
            //Debug.Log("씬 로드 후 장착 아이템 로드 시작");
            // 인벤토리 데이터 로드
            var loadedInventory = await FirebaseManeger.Instance.LoadCharacterInventoryFromFirebase(characterKey);
            var equippedItems = await FirebaseManeger.Instance.LoadEquippedItemsFromFirebase(characterKey);
            // 데이터가 성공적으로 로드된 후에만 UI 업데이트
            if (loadedInventory != null)
            {
                GameManager.Instance.PlayerData.Inventory = loadedInventory;
                StartCoroutine(UpdateInventoryUIAfterLoad());
            }
            if (equippedItems != null)
            {
                // 장착 아이템이 null 일 수 있으므로, null 여부를 확인 후 업데이트
                GameManager.Instance.equippedItemsManager = equippedItems;

                // 4. Base 스탯 초기화 (기본 스탯으로 다시 리셋)
                GameManager.Instance.PlayerData.ResetToBaseStats();
                // 장착 슬롯 UI 갱신
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
                // 장착된 아이템이 없을 때도 처리
                GameManager.Instance.equippedItemsManager = new EquippedItems();
            }
        }
    }
    private IEnumerator UpdateInventoryUIAfterLoad()
    {
        // 잠시 기다린 후에 UI를 찾음
        yield return new WaitForSeconds(0.5f); // 0.5초 대기 후 실행
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
            inventoryUI.UpdateInventoryUI(); // 인벤토리 UI 업데이트
            inventoryUI.UpdateGoldUI();
            if (logUI != null)
            {
                GameManager.Instance.logUI = logUI;  // GameManager에 LogUI 할당
                GameManager.Instance.logUI.AddMessage("접속을 환영합니다!!");  // 인벤토리 로드 후 로그 출력
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

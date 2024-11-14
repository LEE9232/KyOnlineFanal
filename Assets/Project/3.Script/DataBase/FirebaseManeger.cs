using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Threading.Tasks;
public class FirebaseManeger : MonoBehaviour
{
    public static FirebaseManeger Instance { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public FirebaseDatabase DB { get; private set; }
    public FirebaseApp App { get; private set; }
    public bool IsInitialized { get; private set; }
    public event Action onInit; // 파이어베이스가 초기화되면 호출
    public UserDatabase userData;
    public PlayerInfo playerInfo;
    public DatabaseReference usersRef;
    public const int MaxCharacters = 4; // 최대 캐릭터 수
    private Coroutine autoSaveCoroutine; // 자동저장 코루틴
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        InitializeAsync();
        StartAutoSaveCoroutine();
    }
    private async void InitializeAsync()
    {
        DependencyStatus status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status == DependencyStatus.Available)
        {
            // 파이어베이스 초기화 성공
            App = FirebaseApp.DefaultInstance;
            Auth = FirebaseAuth.DefaultInstance;
            DB = FirebaseDatabase.DefaultInstance;
            IsInitialized = true;
            onInit?.Invoke();
        }
    }
    public async void Login(string email, string pw, Action<FirebaseUser> callback = null)
    {
        try
        {
            var result = await Auth.SignInWithEmailAndPasswordAsync(email, pw);
            usersRef = DB.GetReference($"users/{result.User.UserId}");
            DataSnapshot userDataValues = await usersRef.GetValueAsync();
            if (userDataValues.Exists)
            {
                var userNameValue = userDataValues.Child("userName").Value?.ToString();
                var userIdValue = userDataValues.Child("userId").Value?.ToString();
                if (!string.IsNullOrEmpty(userNameValue))
                {
                    UIPanelManager.Instance.ShowPopup($"{userNameValue} 님\n경일 온라인\n접속하셨습니다");
                }
                // 기존 userData 객체 초기화
                userData = new UserDatabase(userIdValue, userNameValue);
                DataSnapshot characterSnapshot = userDataValues.Child("character");
                if (characterSnapshot.Exists)
                {
                    foreach (DataSnapshot child in characterSnapshot.Children)
                    {
                        string characterJson = child.GetRawJsonValue();
                        playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(characterJson);
                        playerInfo.Key = child.Key;
                        GameManager.Instance.PlayerData = playerInfo;
                        var loadedInventory = await LoadCharacterInventoryFromFirebase(playerInfo.Key);
                        if (loadedInventory != null)
                        {
                            GameManager.Instance.PlayerData.Inventory = loadedInventory;
                        }
                    }
                }
            }
            else
            {
                userData = new UserDatabase(result.User.UserId);
                UIPanelManager.Instance.ShowPopup("로그인 실패!\n정보를 확인하세요");
            }
            callback?.Invoke(result.User);
        }
        catch (Exception ex)
        {
            HandleFirebaseAuthError(ex);
        }
    }
    private void HandleFirebaseAuthError(Exception ex)
    {
        if (ex.Message.Contains("auth/user-not-found"))
        {
            UIPanelManager.Instance.ShowPopup("이메일에 해당하는 사용자가 없습니다.");
        }
        else if (ex.Message.Contains("auth/invalid-email"))
        {
            UIPanelManager.Instance.ShowPopup("이메일 형식이 잘못되었습니다.");
        }
        else if (ex.Message.Contains("auth/wrong-password"))
        {
            UIPanelManager.Instance.ShowPopup("비밀번호가 잘못되었습니다.");
        }
        else
        {
            UIPanelManager.Instance.ShowPopup("다시 시도해주세요.");
        }
    }
    public async void Create(string email, string pw, string userName, Action<FirebaseUser> callback = null)
    {
        // 이메일 중복 확인
        bool isEmailTaken = await IsEmailTakenAsync(email);
        if (isEmailTaken)
        {
            UIPanelManager.Instance.ShowPopup("이미 사용 중인 이메일입니다.");
            return;
        }
        // 사용자 이름 중복 확인
        bool isUserNameTaken = await IsUserNameTakenAsync(userName);
        if (isUserNameTaken)
        {
            // 텍스트확인
            UIPanelManager.Instance.ShowPopup("이미 사용중인 닉네임입니다.");
            return;
        }
        try
        {
            var result = await Auth.CreateUserWithEmailAndPasswordAsync(email, pw);
            usersRef = DB.GetReference($"users/{result.User.UserId}");
            //DB의 레퍼런스
            UserDatabase userData = new UserDatabase(result.User.UserId, userName);
            string userDataJson = JsonConvert.SerializeObject(userData);
            await usersRef.SetRawJsonValueAsync(userDataJson);
            this.userData = userData;
            callback?.Invoke(result.User);

        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("email"))
            {
                // 이메일 중복 에러 처리
                UIPanelManager.Instance.ShowPopup("이미 사용 중인 이메일입니다.");
            }
            else
            {
                // 기타 인증 에러 처리
                UIPanelManager.Instance.ShowPopup("회원가입에 실패했습니다. \n다시 시도해주세요.");
            }
        }
    }
    public async Task<bool> IsEmailTakenAsync(string email)
    {
        DatabaseReference usersRef = DB.GetReference("users");
        DataSnapshot snapshot = await usersRef.GetValueAsync();

        foreach (DataSnapshot userSnapshot in snapshot.Children)
        {
            // userSnapshot에서 이메일 값을 가져와서 비교
            var existingEmail = userSnapshot.Child("email").Value?.ToString();
            if (existingEmail != null && existingEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                // 중복된 이메일 발견
                return true;
            }
        }
        return false; // 중복된 이메일 없음
    }
    public async void UpdateUser(string name, string pw, Action callback = null)
    {
        var profile = new UserProfile() { DisplayName = name };
        await Auth.CurrentUser.UpdateUserProfileAsync(profile);

        if (false == string.IsNullOrWhiteSpace(pw))
        {
            await Auth.CurrentUser.UpdatePasswordAsync(pw);
        }
        callback?.Invoke();
    }
    public async void CreateChaacter(string characterName, Class characterClass, Action callBack = null)
    {
        if (userData != null)
        {
            // 현재 캐릭터 수 확인을 위해 GetCharacterList 호출
            GetCharacterList(async characterList =>
            {
                // 캐릭터 수가 4개 이상이면 캐릭터 생성 제한
                if (characterList.Count >= 4)
                {
                    UIPanelManager.Instance.ShowPopup("캐릭터 수가 최대치입니다.");
                    return;
                }
                playerInfo = new PlayerInfo(1, characterName, 100, 100, 50, 50, 10, 5, 0, 500, 1, 10000, characterClass, 1, 2);
                string newCharacterKey = usersRef.Child("character").Push().Key;
                GameManager.Instance.UpdatePlayerDataLocally(playerInfo);
                GameManager.Instance.PlayerData.Key = newCharacterKey;  // 캐릭터 키 설정
                playerInfo.Inventory = new InventoryManager();  // 빈 인벤토리 추가
                playerInfo.Inventory.Materials.Add(new ItemData(901, "초보자 패키지", 0, 10000, "초보자를 위한 금괴", "ItemImage/material/GoldBar", 1, 1f));
                // 캐릭터 데이터 및 인벤토리 저장
                await SaveCharacterDataAsync(newCharacterKey);
                await SaveInventoryToDatabaseAsync(newCharacterKey);
                callBack?.Invoke();
            });
        }
    }
    public async Task<bool> IsUserNameTakenAsync(string userName)
    {
        // 'users' 노드에서 userName으로 검색
        DatabaseReference usersRef = DB.GetReference("users");
        // 모든 사용자 정보를 가져와서 검색
        DataSnapshot snapshot = await usersRef.GetValueAsync();
        foreach (DataSnapshot userSnapshot in snapshot.Children)
        {
            // 사용자 노드에서 userName 값을 가져오기
            var existingUserName = userSnapshot.Child("userName").Value?.ToString();
            // 가져온 userName 값과 비교
            if (existingUserName != null && existingUserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                // 중복된 사용자 이름 발견
                UIPanelManager.Instance.ShowPopup("이미 사용중인 닉네임입니다.");
                return true;
            }
        }
        return false; // 중복된 사용자 이름 없음
    }

    public async void GetCharacterList(Action<List<PlayerInfo>> callback)
    {
        if (userData == null) return;

        DatabaseReference charactersRef = DB.GetReference($"users/{Auth.CurrentUser.UserId}/character");
        DataSnapshot snapshot = await charactersRef.GetValueAsync();
        List<PlayerInfo> characterList = new List<PlayerInfo>();
        foreach (DataSnapshot child in snapshot.Children)
        {
            DataSnapshot characterInfoSnapshot = child.Child("characterInfo"); // characterInfo 자식 노드 접근
            if (characterInfoSnapshot.Exists)
            {
                string json = characterInfoSnapshot.GetRawJsonValue();
                PlayerInfo playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(json);
                playerInfo.Key = child.Key; // 각 캐릭터의 고유 키를 저장
                characterList.Add(playerInfo);
            }
        }
        callback?.Invoke(characterList);
    }
    public void DeleteCharacter(string characterKey, Action onSuccess = null, Action onFailure = null)
    {
        DatabaseReference characterRef = usersRef.Child("character").Child(characterKey);

        characterRef.RemoveValueAsync().ContinueWith(task => { });
        onSuccess?.Invoke();
    }
    public void Logout() => Auth.SignOut();
    public async Task SaveInventoryToDatabaseAsync(string characterKey)
    {
        string userId = Auth.CurrentUser.UserId;
        if (GameManager.Instance.PlayerData == null || string.IsNullOrEmpty(characterKey))
        {
            Debug.LogError("캐릭터 키가 설정되지 않았습니다.");
            return;
        }
        DatabaseReference inventoryRef = DB.GetReference($"users/{userId}/character/{characterKey}/inventory");
        var inventoryData = new
        {
            Weapons = GameManager.Instance.PlayerData.Inventory.Weapons,
            Armors = GameManager.Instance.PlayerData.Inventory.Armors,
            Accessories = GameManager.Instance.PlayerData.Inventory.Accessories,
            Materials = GameManager.Instance.PlayerData.Inventory.Materials  // 재료 아이템 추가
        };
        string inventoryJson = JsonConvert.SerializeObject(inventoryData);
        await inventoryRef.SetRawJsonValueAsync(inventoryJson); // JSON 문자열로 저장
        try
        {
            GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag(); // 변경 플래그 리셋
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update character inventory in Firebase: " + ex.Message);
        }
    }
    public async Task<InventoryManager> LoadCharacterInventoryFromFirebase(string characterKey)
    {
        string userId = Auth.CurrentUser.UserId;
        DatabaseReference itemsRef = DB.GetReference($"users/{userId}/character/{characterKey}/inventory");
        DataSnapshot snapshot = await itemsRef.GetValueAsync();
        InventoryManager inventory = new InventoryManager();
        if (snapshot.Exists)
        {
            if (snapshot.HasChild("Weapons"))
            {
                string weaponsJson = snapshot.Child("Weapons").GetRawJsonValue();
                inventory.Weapons = JsonConvert.DeserializeObject<List<WeaponData>>(weaponsJson);
            }
            if (snapshot.HasChild("Armors"))
            {
                string armorsJson = snapshot.Child("Armors").GetRawJsonValue();
                inventory.Armors = JsonConvert.DeserializeObject<List<ArmorData>>(armorsJson);
            }
            if (snapshot.HasChild("Accessories"))
            {
                string accessoriesJson = snapshot.Child("Accessories").GetRawJsonValue();
                inventory.Accessories = JsonConvert.DeserializeObject<List<AccessoryData>>(accessoriesJson);
            }
            if (snapshot.HasChild("Materials"))
            {
                string materialsJson = snapshot.Child("Materials").GetRawJsonValue();
                inventory.Materials = JsonConvert.DeserializeObject<List<ItemData>>(materialsJson);
            }
            // UI 업데이트 추가
            GameManager.Instance.PlayerData.Inventory = inventory;
            GameManager.Instance.inventoryManager = inventory;  // InventoryManager 업데이
            // UI 갱신
            if (GameManager.Instance.inventoryUI != null)
            {
                GameManager.Instance.inventoryUI.UpdateInventoryUI();
            }
            return inventory; // 성공적으로 로드한 인벤토리 반환
        }
        else
        {
            return null; // 데이터가 없으면 null 반환
        }
    }
    public async Task SaveCharacterDataAsync(string characterKey)
    {
        if (Auth.CurrentUser != null)
        {
            string userId = Auth.CurrentUser.UserId;
            if (string.IsNullOrEmpty(characterKey))
            {
                Debug.LogError("캐릭터 키가 설정되지 않았습니다.");
                return;
            }
            // 캐릭터 상태 정보 저장 경로
            DatabaseReference characterInfoRef = DB.GetReference($"users/{userId}/character/{characterKey}/characterInfo");
            // 캐릭터 데이터를 JSON으로 직렬화
            var characterData = new
            {
                Damage = GameManager.Instance.PlayerData.Damage,
                Defensive = GameManager.Instance.PlayerData.Defensive,
                EXP = GameManager.Instance.PlayerData.EXP,
                MaxEXP = GameManager.Instance.PlayerData.MaxEXP,
                Gold = GameManager.Instance.PlayerData.Gold,
                HP = GameManager.Instance.PlayerData.HP,
                MaxHP = GameManager.Instance.PlayerData.MaxHP,
                HpRegenPerSecond = GameManager.Instance.PlayerData.HpRegenPerSecond,
                Level = GameManager.Instance.PlayerData.Level,
                MP = GameManager.Instance.PlayerData.MP,
                MaxMP = GameManager.Instance.PlayerData.MaxMP,
                MpRegenPerSecond = GameManager.Instance.PlayerData.MpRegenPerSecond,
                NickName = GameManager.Instance.PlayerData.NickName,
                SkillPoint = GameManager.Instance.PlayerData.SkillPoint,
                Classes = GameManager.Instance.PlayerData.classes,
                // Base 스탯 추가
                BaseMaxHP = GameManager.Instance.PlayerData.BaseMaxHP,
                BaseMaxMP = GameManager.Instance.PlayerData.BaseMaxMP,
                BaseDamage = GameManager.Instance.PlayerData.BaseDamage,
                BaseDefensive = GameManager.Instance.PlayerData.BaseDefensive,
                BaseHpRegenPerSecond = GameManager.Instance.PlayerData.BaseHpRegenPerSecond,
                BaseMpRegenPerSecond = GameManager.Instance.PlayerData.BaseMpRegenPerSecond

            };
            string characterJson = JsonConvert.SerializeObject(characterData);
            try
            {
                await characterInfoRef.SetRawJsonValueAsync(characterJson); // 데이터를 JSON 문자열로 변환하여 저장
                GameManager.Instance.PlayerData.ResetDataChangeFlag(); // 캐릭터 저장 후 플래그 리셋
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to update character info in Firebase: " + ex.Message);
            }
        }
    }
    // 장착아이템
    public async Task SaveEquippedItemsToFirebase(string characterKey)
    {
        try
        {
            string userId = Auth.CurrentUser.UserId;
            DatabaseReference equippedItemsRef = DB.GetReference($"users/{userId}/character/{characterKey}/equippedItems");
            // 경로 확인
            Debug.Log($"저장할 경로: users/{userId}/character/{characterKey}/equippedItems");
            var equippedItemsData = new
            {
                EquippedWeapon = GameManager.Instance.equippedItemsManager.EquippedWeapon,
                EquippedArmor = GameManager.Instance.equippedItemsManager.EquippedArmor,
                EquippedAccessory = GameManager.Instance.equippedItemsManager.EquippedAccessory
            };

            string equippedItemsJson = JsonConvert.SerializeObject(equippedItemsData);
            await equippedItemsRef.SetRawJsonValueAsync(equippedItemsJson);
            GameManager.Instance.equippedItemsManager.ResetEquippedItemsChangeFlag();
        }
        catch (Exception ex)
        {
            Debug.LogError($"장착 아이템 Firebase 저장 실패: {ex.Message}");
        }
    }
    // 장착아이템 로드
    public async Task<EquippedItems> LoadEquippedItemsFromFirebase(string characterKey)
    {
        string userId = Auth.CurrentUser.UserId;
        DatabaseReference equippedItemsRef = DB.GetReference($"users/{userId}/character/{characterKey}/equippedItems");

        DataSnapshot snapshot = await equippedItemsRef.GetValueAsync();
        EquippedItems equippedItems = new EquippedItems();
        if (snapshot.Exists)
        {
            if (snapshot.HasChild("EquippedWeapon"))
            {
                WeaponData equippedWeapon = JsonConvert.DeserializeObject<WeaponData>(snapshot.Child("EquippedWeapon").GetRawJsonValue());
                equippedItems.EquipWeapon(equippedWeapon, GameManager.Instance.PlayerData); // PlayerInfo 추가
                GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedWeapon, InventoryType.Weapon);
            }
            if (snapshot.HasChild("EquippedArmor"))
            {
                ArmorData equippedArmor = JsonConvert.DeserializeObject<ArmorData>(snapshot.Child("EquippedArmor").GetRawJsonValue());
                equippedItems.EquipArmor(equippedArmor, GameManager.Instance.PlayerData);
                GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedArmor, InventoryType.Armor);
            }
            if (snapshot.HasChild("EquippedAccessory"))
            {
                AccessoryData equippedAccessory = JsonConvert.DeserializeObject<AccessoryData>(snapshot.Child("EquippedAccessory").GetRawJsonValue());
                equippedItems.EquipAccessory(equippedAccessory, GameManager.Instance.PlayerData);
                GameManager.Instance.inventoryUI.UpdateEquipSlots(equippedAccessory, InventoryType.Accessory);
            }
        }
        return equippedItems;
    }
    private async void OnApplicationQuit()
    {
        string characterKey = GameManager.Instance.PlayerData.Key;
        if (!string.IsNullOrEmpty(characterKey))
        {
            if (GameManager.Instance.PlayerData.Inventory.IsInventoryChanged())
            {
                await SaveInventoryToDatabaseAsync(characterKey); // 게임 종료 시 데이터 저장
            }

            if (GameManager.Instance.PlayerData.EquippedItemsManager.IsEquippedItemsChanged())
            {
                await SaveEquippedItemsToFirebase(characterKey);
            }
            if (GameManager.Instance.PlayerData.IsDataChanged())
            {
                await SaveCharacterDataAsync(characterKey);
            }
        }
    }
    // 5분마다 자동 저장을 위한 코루틴 시작
    private void StartAutoSaveCoroutine()
    {
        if (autoSaveCoroutine == null)
        {
            autoSaveCoroutine = StartCoroutine(AutoSaveCoroutine());
        }
    }
    // 5분마다 인벤토리 자동 저장
    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(300); // 5분 대기
            SaveCharacterAndInventory();
        }
    }
    private async void SaveCharacterAndInventory()
    {
        string characterKey = GameManager.Instance.PlayerData.Key;
        if (!string.IsNullOrEmpty(characterKey))
        {
            try
            {
                // 인벤토리 저장
                await SaveInventoryToDatabaseAsync(characterKey);
                // 캐릭터 데이터 저장
                await SaveCharacterDataAsync(characterKey);
                await SaveEquippedItemsToFirebase(characterKey);
                GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag();  // 플래그 리셋
                GameManager.Instance.PlayerData.ResetDataChangeFlag();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to auto-save data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("캐릭터 키가 설정되지않음");
        }
    }
}

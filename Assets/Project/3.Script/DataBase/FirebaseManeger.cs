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
    public event Action onInit; // ���̾�̽��� �ʱ�ȭ�Ǹ� ȣ��
    public UserDatabase userData;
    public PlayerInfo playerInfo;
    public DatabaseReference usersRef;
    public const int MaxCharacters = 4; // �ִ� ĳ���� ��
    private Coroutine autoSaveCoroutine; // �ڵ����� �ڷ�ƾ
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
            // ���̾�̽� �ʱ�ȭ ����
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
                    UIPanelManager.Instance.ShowPopup($"{userNameValue} ��\n���� �¶���\n�����ϼ̽��ϴ�");
                }
                // ���� userData ��ü �ʱ�ȭ
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
                UIPanelManager.Instance.ShowPopup("�α��� ����!\n������ Ȯ���ϼ���");
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
            UIPanelManager.Instance.ShowPopup("�̸��Ͽ� �ش��ϴ� ����ڰ� �����ϴ�.");
        }
        else if (ex.Message.Contains("auth/invalid-email"))
        {
            UIPanelManager.Instance.ShowPopup("�̸��� ������ �߸��Ǿ����ϴ�.");
        }
        else if (ex.Message.Contains("auth/wrong-password"))
        {
            UIPanelManager.Instance.ShowPopup("��й�ȣ�� �߸��Ǿ����ϴ�.");
        }
        else
        {
            UIPanelManager.Instance.ShowPopup("�ٽ� �õ����ּ���.");
        }
    }
    public async void Create(string email, string pw, string userName, Action<FirebaseUser> callback = null)
    {
        // �̸��� �ߺ� Ȯ��
        bool isEmailTaken = await IsEmailTakenAsync(email);
        if (isEmailTaken)
        {
            UIPanelManager.Instance.ShowPopup("�̹� ��� ���� �̸����Դϴ�.");
            return;
        }
        // ����� �̸� �ߺ� Ȯ��
        bool isUserNameTaken = await IsUserNameTakenAsync(userName);
        if (isUserNameTaken)
        {
            // �ؽ�ƮȮ��
            UIPanelManager.Instance.ShowPopup("�̹� ������� �г����Դϴ�.");
            return;
        }
        try
        {
            var result = await Auth.CreateUserWithEmailAndPasswordAsync(email, pw);
            usersRef = DB.GetReference($"users/{result.User.UserId}");
            //DB�� ���۷���
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
                // �̸��� �ߺ� ���� ó��
                UIPanelManager.Instance.ShowPopup("�̹� ��� ���� �̸����Դϴ�.");
            }
            else
            {
                // ��Ÿ ���� ���� ó��
                UIPanelManager.Instance.ShowPopup("ȸ�����Կ� �����߽��ϴ�. \n�ٽ� �õ����ּ���.");
            }
        }
    }
    public async Task<bool> IsEmailTakenAsync(string email)
    {
        DatabaseReference usersRef = DB.GetReference("users");
        DataSnapshot snapshot = await usersRef.GetValueAsync();

        foreach (DataSnapshot userSnapshot in snapshot.Children)
        {
            // userSnapshot���� �̸��� ���� �����ͼ� ��
            var existingEmail = userSnapshot.Child("email").Value?.ToString();
            if (existingEmail != null && existingEmail.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                // �ߺ��� �̸��� �߰�
                return true;
            }
        }
        return false; // �ߺ��� �̸��� ����
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
            // ���� ĳ���� �� Ȯ���� ���� GetCharacterList ȣ��
            GetCharacterList(async characterList =>
            {
                // ĳ���� ���� 4�� �̻��̸� ĳ���� ���� ����
                if (characterList.Count >= 4)
                {
                    UIPanelManager.Instance.ShowPopup("ĳ���� ���� �ִ�ġ�Դϴ�.");
                    return;
                }
                playerInfo = new PlayerInfo(1, characterName, 100, 100, 50, 50, 10, 5, 0, 500, 1, 10000, characterClass, 1, 2);
                string newCharacterKey = usersRef.Child("character").Push().Key;
                GameManager.Instance.UpdatePlayerDataLocally(playerInfo);
                GameManager.Instance.PlayerData.Key = newCharacterKey;  // ĳ���� Ű ����
                playerInfo.Inventory = new InventoryManager();  // �� �κ��丮 �߰�
                playerInfo.Inventory.Materials.Add(new ItemData(901, "�ʺ��� ��Ű��", 0, 10000, "�ʺ��ڸ� ���� �ݱ�", "ItemImage/material/GoldBar", 1, 1f));
                // ĳ���� ������ �� �κ��丮 ����
                await SaveCharacterDataAsync(newCharacterKey);
                await SaveInventoryToDatabaseAsync(newCharacterKey);
                callBack?.Invoke();
            });
        }
    }
    public async Task<bool> IsUserNameTakenAsync(string userName)
    {
        // 'users' ��忡�� userName���� �˻�
        DatabaseReference usersRef = DB.GetReference("users");
        // ��� ����� ������ �����ͼ� �˻�
        DataSnapshot snapshot = await usersRef.GetValueAsync();
        foreach (DataSnapshot userSnapshot in snapshot.Children)
        {
            // ����� ��忡�� userName ���� ��������
            var existingUserName = userSnapshot.Child("userName").Value?.ToString();
            // ������ userName ���� ��
            if (existingUserName != null && existingUserName.Equals(userName, StringComparison.OrdinalIgnoreCase))
            {
                // �ߺ��� ����� �̸� �߰�
                UIPanelManager.Instance.ShowPopup("�̹� ������� �г����Դϴ�.");
                return true;
            }
        }
        return false; // �ߺ��� ����� �̸� ����
    }

    public async void GetCharacterList(Action<List<PlayerInfo>> callback)
    {
        if (userData == null) return;

        DatabaseReference charactersRef = DB.GetReference($"users/{Auth.CurrentUser.UserId}/character");
        DataSnapshot snapshot = await charactersRef.GetValueAsync();
        List<PlayerInfo> characterList = new List<PlayerInfo>();
        foreach (DataSnapshot child in snapshot.Children)
        {
            DataSnapshot characterInfoSnapshot = child.Child("characterInfo"); // characterInfo �ڽ� ��� ����
            if (characterInfoSnapshot.Exists)
            {
                string json = characterInfoSnapshot.GetRawJsonValue();
                PlayerInfo playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(json);
                playerInfo.Key = child.Key; // �� ĳ������ ���� Ű�� ����
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
            Debug.LogError("ĳ���� Ű�� �������� �ʾҽ��ϴ�.");
            return;
        }
        DatabaseReference inventoryRef = DB.GetReference($"users/{userId}/character/{characterKey}/inventory");
        var inventoryData = new
        {
            Weapons = GameManager.Instance.PlayerData.Inventory.Weapons,
            Armors = GameManager.Instance.PlayerData.Inventory.Armors,
            Accessories = GameManager.Instance.PlayerData.Inventory.Accessories,
            Materials = GameManager.Instance.PlayerData.Inventory.Materials  // ��� ������ �߰�
        };
        string inventoryJson = JsonConvert.SerializeObject(inventoryData);
        await inventoryRef.SetRawJsonValueAsync(inventoryJson); // JSON ���ڿ��� ����
        try
        {
            GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag(); // ���� �÷��� ����
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
            // UI ������Ʈ �߰�
            GameManager.Instance.PlayerData.Inventory = inventory;
            GameManager.Instance.inventoryManager = inventory;  // InventoryManager ������
            // UI ����
            if (GameManager.Instance.inventoryUI != null)
            {
                GameManager.Instance.inventoryUI.UpdateInventoryUI();
            }
            return inventory; // ���������� �ε��� �κ��丮 ��ȯ
        }
        else
        {
            return null; // �����Ͱ� ������ null ��ȯ
        }
    }
    public async Task SaveCharacterDataAsync(string characterKey)
    {
        if (Auth.CurrentUser != null)
        {
            string userId = Auth.CurrentUser.UserId;
            if (string.IsNullOrEmpty(characterKey))
            {
                Debug.LogError("ĳ���� Ű�� �������� �ʾҽ��ϴ�.");
                return;
            }
            // ĳ���� ���� ���� ���� ���
            DatabaseReference characterInfoRef = DB.GetReference($"users/{userId}/character/{characterKey}/characterInfo");
            // ĳ���� �����͸� JSON���� ����ȭ
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
                // Base ���� �߰�
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
                await characterInfoRef.SetRawJsonValueAsync(characterJson); // �����͸� JSON ���ڿ��� ��ȯ�Ͽ� ����
                GameManager.Instance.PlayerData.ResetDataChangeFlag(); // ĳ���� ���� �� �÷��� ����
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to update character info in Firebase: " + ex.Message);
            }
        }
    }
    // ����������
    public async Task SaveEquippedItemsToFirebase(string characterKey)
    {
        try
        {
            string userId = Auth.CurrentUser.UserId;
            DatabaseReference equippedItemsRef = DB.GetReference($"users/{userId}/character/{characterKey}/equippedItems");
            // ��� Ȯ��
            Debug.Log($"������ ���: users/{userId}/character/{characterKey}/equippedItems");
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
            Debug.LogError($"���� ������ Firebase ���� ����: {ex.Message}");
        }
    }
    // ���������� �ε�
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
                equippedItems.EquipWeapon(equippedWeapon, GameManager.Instance.PlayerData); // PlayerInfo �߰�
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
                await SaveInventoryToDatabaseAsync(characterKey); // ���� ���� �� ������ ����
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
    // 5�и��� �ڵ� ������ ���� �ڷ�ƾ ����
    private void StartAutoSaveCoroutine()
    {
        if (autoSaveCoroutine == null)
        {
            autoSaveCoroutine = StartCoroutine(AutoSaveCoroutine());
        }
    }
    // 5�и��� �κ��丮 �ڵ� ����
    private IEnumerator AutoSaveCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(300); // 5�� ���
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
                // �κ��丮 ����
                await SaveInventoryToDatabaseAsync(characterKey);
                // ĳ���� ������ ����
                await SaveCharacterDataAsync(characterKey);
                await SaveEquippedItemsToFirebase(characterKey);
                GameManager.Instance.PlayerData.Inventory.ResetInventoryChangeFlag();  // �÷��� ����
                GameManager.Instance.PlayerData.ResetDataChangeFlag();
            }
            catch (Exception ex)
            {
                Debug.LogError("Failed to auto-save data: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("ĳ���� Ű�� ������������");
        }
    }
}

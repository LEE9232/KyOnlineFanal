using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCheckPanel : MonoBehaviour
{
    public UIPanelManager panelManager;
    // ���� ��ư
    public Button newCharBt;
    public Button charListBt;

    // ��ҹ�ư
    public Button backBt;
    public Button choicebackBt;
    public Button choiceListbackBt;

    // �ű� ĳ���� ���� ��ư
    public Button knightBtn;
    public Button sorcerBtn;

    public TMP_InputField nickNameInput;
    public Button characterCreate;
    public Button characterCancel;

    private List<GameObject> characterSlots = new List<GameObject>();
    public GameObject slotPrefab;
    public GameObject charListSlotsContainer; // �������� ���� �� ������Ʈ

    public GameObject charList;
    public GameObject titleText;
    public GameObject newChar;
    public GameObject charListChoice;

    public GameObject CharacterCreatePanel;

    private Class selectClass;
    private void Awake()
    {
        newCharBt.onClick.AddListener(NewCharBtnClick);
        charListBt.onClick.AddListener(CharListBtnClick);

        knightBtn.onClick.AddListener(NewCharKnightBtn);
        sorcerBtn.onClick.AddListener(NewCharSorcerBtn);

        characterCreate.onClick.AddListener(CharacterCreateBtn);
        characterCancel.onClick.AddListener(CharacterCancelBtn);

        backBt.onClick.AddListener(BackBtnClick);
        choicebackBt.onClick.AddListener(BackBtnClick);
        choiceListbackBt.onClick.AddListener(BackBtnClick);
    }

    // �ű� ĳ����
    public void NewCharBtnClick()
    {
        charList.SetActive(false);
        titleText.SetActive(false);
        newChar.SetActive(true);
    }
    public void NewCharKnightBtn()
    {
        selectClass = Class.Paladine;
        CharacterCreatePanel.SetActive(true);
    }
    public void NewCharSorcerBtn()
    {
        selectClass = Class.Mage;
        CharacterCreatePanel.SetActive(true);
    }

    // ���̺� ĳ����
    public void CharListBtnClick()
    {

        charList.SetActive(false);
        titleText.SetActive(false);
        charListChoice.SetActive(true);
        LoadCharacterList();
    }

    // �ڷΰ���
    public void BackBtnClick()
    {
        charList.SetActive(true);
        titleText.SetActive(true);
        charListChoice.SetActive(false);
        newChar.SetActive(false);

        panelManager.ChoiceBackClick();
    }
    public void CharacterCreateBtn()
    {
        string characterName = nickNameInput.text.Trim();
        if (string.IsNullOrEmpty(characterName))
        {
            panelManager.ShowPopup("�س����� Ȯ�����ּ���");
            return;
        }
        if (selectClass == Class.Paladine || selectClass == Class.Mage)
        {
            FirebaseManeger.Instance.CreateChaacter(characterName, selectClass, () =>
            {
                // Debug.Log("ĳ���� ���� �Ϸ�: " + characterName);
                CharacterCreatePanel.SetActive(false);
                panelManager.ShowPopup("ĳ���� �����Ǿ����ϴ�");
                LoadCharacterList();
                // ���߿� ������ ���� ĳ���� �������� �����ϵ��� ���⿡�� �ۼ��ϸ� �ȴ�.
            });
        }
    }

    public void CharacterCancelBtn()
    {
        CharacterCreatePanel.SetActive(false);
    }
    private void LoadCharacterList()
    {

        ClearCharacterSlots();
        FirebaseManeger.Instance.GetCharacterList(characterList =>
        {
            int slotCount = 0;
            foreach (var character in characterList)
            {
                if (slotCount >= 4) break;
                var slot = Instantiate(slotPrefab, charListSlotsContainer.transform);
                SetupCharacterSlot(slot, character); // ���� ���� �Լ� ȣ��
                characterSlots.Add(slot);
                slotCount++;
            }
        });
    }
    private void ClearCharacterSlots()
    {
        foreach (var slot in characterSlots)
        {
            Destroy(slot);
        }
        characterSlots.Clear();
    }
    private void SetupCharacterSlot(GameObject slot, PlayerInfo character)
    {
        var characterSlotText = slot.transform.Find("SaveInfoText").GetComponent<TextMeshProUGUI>();
        if (characterSlotText != null)
        {
            characterSlotText.text = $"{character.NickName}\n\nLevel: {character.Level}\n\nGold: {character.Gold}";
        }

        var button = slot.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                GameManager.Instance.UpdatePlayerDataLocally(character);
                LoadGameScene();
            });
        }

        var deleteButton = slot.transform.Find("DeleteButton").GetComponent<Button>();
        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(() => ConfirmAndDeleteCharacter(character, slot));
        }
    }

    private void ConfirmAndDeleteCharacter(PlayerInfo character, GameObject slot)
    {
        panelManager.ShowConfirmationPopup($"{character.NickName}\n ĳ���͸� �����Ͻðڽ��ϱ�?", () =>
        {
            FirebaseManeger.Instance.DeleteCharacter(character.Key, () =>
            {
                panelManager.ShowPopup($"{character.NickName}\n ĳ���Ͱ� �����Ǿ����ϴ�.");
                characterSlots.Remove(slot);
                Destroy(slot);
                LoadCharacterList(); // ����Ʈ ���ΰ�ħ
            },
            () =>
            {
                panelManager.ShowPopup("ĳ���� ���� ����!");
            });
        });
    }

    private void LoadGameScene()
    {
        if (GameManager.Instance.IsMultiplayer == true)
        {
            Changescenemaneger.Instance.LobbyScene();
        }
        else
        {
            Changescenemaneger.Instance.StageOneScene();
        }
    }

}

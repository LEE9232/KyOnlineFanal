using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCheckPanel : MonoBehaviour
{
    public UIPanelManager panelManager;
    // 선택 버튼
    public Button newCharBt;
    public Button charListBt;

    // 취소버튼
    public Button backBt;
    public Button choicebackBt;
    public Button choiceListbackBt;

    // 신규 캐릭터 선택 버튼
    public Button knightBtn;
    public Button sorcerBtn;

    public TMP_InputField nickNameInput;
    public Button characterCreate;
    public Button characterCancel;

    private List<GameObject> characterSlots = new List<GameObject>();
    public GameObject slotPrefab;
    public GameObject charListSlotsContainer; // 프리팹을 넣을 빈 오브젝트

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

    // 신규 캐릭터
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

    // 세이브 캐릭터
    public void CharListBtnClick()
    {

        charList.SetActive(false);
        titleText.SetActive(false);
        charListChoice.SetActive(true);
        LoadCharacterList();
    }

    // 뒤로가기
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
            panelManager.ShowPopup("넥네임을 확인해주세요");
            return;
        }
        if (selectClass == Class.Paladine || selectClass == Class.Mage)
        {
            FirebaseManeger.Instance.CreateChaacter(characterName, selectClass, () =>
            {
                // Debug.Log("캐릭터 생성 완료: " + characterName);
                CharacterCreatePanel.SetActive(false);
                panelManager.ShowPopup("캐릭터 생성되었습니다");
                LoadCharacterList();
                // 나중에 각각의 직업 캐릭터 프리팹을 생성하도록 여기에서 작성하면 된다.
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
                SetupCharacterSlot(slot, character); // 슬롯 설정 함수 호출
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
        panelManager.ShowConfirmationPopup($"{character.NickName}\n 캐릭터를 삭제하시겠습니까?", () =>
        {
            FirebaseManeger.Instance.DeleteCharacter(character.Key, () =>
            {
                panelManager.ShowPopup($"{character.NickName}\n 캐릭터가 삭제되었습니다.");
                characterSlots.Remove(slot);
                Destroy(slot);
                LoadCharacterList(); // 리스트 새로고침
            },
            () =>
            {
                panelManager.ShowPopup("캐릭터 삭제 실패!");
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

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    //private UIPanelManager uiPanelManager { get; set; }

    public Button reGameBtn;
    public Button skillBtn;
    public Button statusBtn;
    public Button invenBtn;
    public Button optionBtn;
    public Button lobbyBtn;
    public Button gameclaseBtn;
    public ChoicePopup choicePopup;


    private void Awake()
    {
        reGameBtn.onClick.AddListener(ReGameBtnClick);
        skillBtn.onClick.AddListener(SkillListBtnClick);
        statusBtn.onClick.AddListener(StatusBtnClick);
        invenBtn.onClick.AddListener(InventoryBtnClick);
        optionBtn.onClick.AddListener(OptionBtnClick);
        lobbyBtn.onClick.AddListener(LobbyBackPopup);
        gameclaseBtn.onClick.AddListener(GameEndPopup);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) InventoryBtnClick();
    }

    public void ReGameBtnClick()
    {

        MainPanelManager.Instance.menuPanel.SetActive(false);

    }
    public void SkillListBtnClick()
    {
        MainPanelManager.Instance.skillPanel.SetActive(true);
        MainPanelManager.Instance.menuPanel.SetActive(false);
    }
    public void InventoryBtnClick()
    {

        MainPanelManager.Instance.menuPanel.SetActive(false);
        MainPanelManager.Instance.inventoryPanel.SetActive(true);
    }
    public void StatusBtnClick()
    {

        MainPanelManager.Instance.menuPanel.SetActive(false);
        MainPanelManager.Instance.statusPanel.SetActive(true);
    }

    public void OptionBtnClick()
    {
        MainPanelManager.Instance.menuPanel.SetActive(false);
        MainPanelManager.Instance.optionPanel.SetActive(true);
    }



    public void LobbyBackPopup()
    {
        // �ؽ�ƮȮ��
        choicePopup.ShowPopup($"ĳ���� ����â����\n�ǵ��ư��ðڽ��ϱ�?", () =>
        {
            LobbyBtnClick();
        });
    }
    public void LobbyBtnClick()
    {
        MainPanelManager.Instance.menuPanel.SetActive(false);

        if (GameManager.Instance.IsLoggedIn == true && GameManager.Instance.IsMultiplayer == false)
        {
            Changescenemaneger.Instance.StartScecn();
        }
        else if (GameManager.Instance.IsLoggedIn == true && GameManager.Instance.IsMultiplayer == true)
        {
            // �κ�� ���� ������
            Changescenemaneger.Instance.StartScecn();
        }
    }
    public void GameEndPopup()
    {
        // �ؽ�ƮȮ��
        choicePopup.ShowPopup($"���� ���� ��(��)\n���� �Ͻðڽ��ϱ�?", () =>
        {
            GameEnd();
        });
    }
    public void GameEnd()
    {
        MainPanelManager.Instance.menuPanel.SetActive(false);
        // ������ ���� �Ұ�
#if UNITY_EDITOR
        // ����Ƽ �����Ϳ��� ���� ���� ���� �����͸� ����
        EditorApplication.isPlaying = false;
#else
        // ����� ���ӿ����� �� �ڵ带 �����Ͽ� ���ø����̼��� ����
        Application.Quit();
#endif
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }
}

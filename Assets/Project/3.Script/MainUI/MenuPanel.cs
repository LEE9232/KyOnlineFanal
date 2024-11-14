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
        // 텍스트확인
        choicePopup.ShowPopup($"캐릭터 선택창으로\n되돌아가시겠습니까?", () =>
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
            // 로비씬 갈것 적을곳
            Changescenemaneger.Instance.StartScecn();
        }
    }
    public void GameEndPopup()
    {
        // 텍스트확인
        choicePopup.ShowPopup($"정말 게임 을(를)\n종료 하시겠습니까?", () =>
        {
            GameEnd();
        });
    }
    public void GameEnd()
    {
        MainPanelManager.Instance.menuPanel.SetActive(false);
        // 에디터 종료 할곳
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때는 에디터를 종료
        EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 이 코드를 실행하여 애플리케이션을 종료
        Application.Quit();
#endif
        //EditorApplication.isPlaying = false;
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class UILoginPanel : MonoBehaviour
{
    public UIPanelManager panelManager;

    public TMP_InputField idInput;
    public TMP_InputField pwInput;

    public Button loginButton;
    public Button cancelButton;
    public Button signupButton;
    private void Awake()
    {
        pwInput.contentType = TMP_InputField.ContentType.Password;
        loginButton.onClick.AddListener(LoginButtonClick);
        signupButton.onClick.AddListener(SignUpButtonClick);
        cancelButton.onClick.AddListener(CancelButtonClick);
    }


    public void LoginButtonClick()
    {

        FirebaseManeger.Instance.Login(idInput.text, pwInput.text,
            (user) =>
            {
                panelManager.LoginClick();

                //FBPanelManager.Instance.PanelOpen<FBUserInfoPanel>().SetUserInfo(user);
            });

        //panelManager.LoginClick();
        //Debug.Log("로그인 버튼 클릭");
    }
    public void SignUpButtonClick()
    {
        panelManager.SignClick();
        //Debug.Log("회원 가입 버튼 클릭");
    }
    public void CancelButtonClick()
    {

        // 게임 종료 처리
#if UNITY_EDITOR
        // 유니티 에디터에서 실행 중일 때 에디터를 종료
        EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서는 이 코드를 실행하여 애플리케이션을 종료
        Application.Quit();
#endif
        ////Debug.Log("게임 종료 클릭");
        //EditorApplication.isPlaying = false;
        //
        //
        //
        //Application.Quit();
    }
}

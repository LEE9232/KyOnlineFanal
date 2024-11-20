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
        //Debug.Log("�α��� ��ư Ŭ��");
    }
    public void SignUpButtonClick()
    {
        panelManager.SignClick();
        //Debug.Log("ȸ�� ���� ��ư Ŭ��");
    }
    public void CancelButtonClick()
    {

        // ���� ���� ó��
#if UNITY_EDITOR
        // ����Ƽ �����Ϳ��� ���� ���� �� �����͸� ����
        EditorApplication.isPlaying = false;
#else
        // ����� ���ӿ����� �� �ڵ带 �����Ͽ� ���ø����̼��� ����
        Application.Quit();
#endif
        ////Debug.Log("���� ���� Ŭ��");
        //EditorApplication.isPlaying = false;
        //
        //
        //
        //Application.Quit();
    }
}

using Firebase.Auth;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISignPanel : MonoBehaviour
{
    public UIPanelManager panelManager;

    public TMP_InputField idInput;
    public TMP_InputField NickInput;
    public TMP_InputField pwInput;
    public TMP_InputField pwReInput;

    public Button signokButton;
    public Button signcancelButton;

    private void Awake()
    {
        // 비밀번호 입력 필드 설정
        pwInput.contentType = TMP_InputField.ContentType.Password;
        pwReInput.contentType = TMP_InputField.ContentType.Password;

        signokButton.onClick.AddListener(SignButtonClick);
        signcancelButton.onClick.AddListener(SignCancelButtonClick);
    }

    public void SignButtonClick()
    {
        if (string.IsNullOrWhiteSpace(idInput.text))
        {
            panelManager.ShowPopup("아이디를 확인해주세요");
            return;
        }
        if (string.IsNullOrWhiteSpace(NickInput.text))
        {
            panelManager.ShowPopup("닉네임을 확인해주세요");
            return;
        }
        if (string.IsNullOrWhiteSpace(pwInput.text) || string.IsNullOrWhiteSpace(pwReInput.text))
        {
            panelManager.ShowPopup("비밀번호를 확인해주세요");
            return;
        }
        if (pwInput.text != pwReInput.text)
        {
            panelManager.ShowPopup("비밀번호가 다릅니다");
            return;
        }
        FirebaseManeger.Instance.Create(idInput.text, pwInput.text,NickInput.text, SetUser);
    }
    public void SignCancelButtonClick()
    {
        panelManager.SignCancelClick();
        //Debug.Log("가입 취소 버튼 클릭");
        
    }
    private void SetUser(FirebaseUser user)
    {
        panelManager.SignOkClick();
        panelManager.ShowPopup("회원가입이 완료되었습니다");
    }
}

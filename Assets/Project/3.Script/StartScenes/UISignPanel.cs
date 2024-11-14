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
        // ��й�ȣ �Է� �ʵ� ����
        pwInput.contentType = TMP_InputField.ContentType.Password;
        pwReInput.contentType = TMP_InputField.ContentType.Password;

        signokButton.onClick.AddListener(SignButtonClick);
        signcancelButton.onClick.AddListener(SignCancelButtonClick);
    }

    public void SignButtonClick()
    {
        if (string.IsNullOrWhiteSpace(idInput.text))
        {
            panelManager.ShowPopup("���̵� Ȯ�����ּ���");
            return;
        }
        if (string.IsNullOrWhiteSpace(NickInput.text))
        {
            panelManager.ShowPopup("�г����� Ȯ�����ּ���");
            return;
        }
        if (string.IsNullOrWhiteSpace(pwInput.text) || string.IsNullOrWhiteSpace(pwReInput.text))
        {
            panelManager.ShowPopup("��й�ȣ�� Ȯ�����ּ���");
            return;
        }
        if (pwInput.text != pwReInput.text)
        {
            panelManager.ShowPopup("��й�ȣ�� �ٸ��ϴ�");
            return;
        }
        FirebaseManeger.Instance.Create(idInput.text, pwInput.text,NickInput.text, SetUser);
    }
    public void SignCancelButtonClick()
    {
        panelManager.SignCancelClick();
        //Debug.Log("���� ��� ��ư Ŭ��");
        
    }
    private void SetUser(FirebaseUser user)
    {
        panelManager.SignOkClick();
        panelManager.ShowPopup("ȸ�������� �Ϸ�Ǿ����ϴ�");
    }
}

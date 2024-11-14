using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePopup : MonoBehaviour
{
    public GameObject popupPanel;  // �˾� �г�
    public TextMeshProUGUI popupMessage;      // �˾� �޽��� �ؽ�Ʈ
    public Button yesButton;       // Yes ��ư
    public Button noButton;        // No ��ư
    private Action confirmAction;  // Ȯ�� �� ȣ��Ǵ� �ݹ�


    private void Awake()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    private void Update()
    {
        
        // Enter Ű �Ǵ� ���콺 Ŭ�� �� Yes ��ư �۵�
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnYesButtonClicked();
        }
        // Esc Ű �Ǵ� ���콺 Ŭ�� �� No ��ư �۵�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnNoButtonClicked();
        }
    }
    // �˾��� ���� �޼���
    public void ShowPopup(string message, Action onConfirm)
    {
        popupPanel.SetActive(true);
        popupMessage.text = message;
        confirmAction = onConfirm; // �ݹ� ����

        // Yes ��ư Ŭ�� �̺�Ʈ ���
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(OnYesButtonClicked); 
        noButton.onClick.AddListener(OnNoButtonClicked);
    }
    private void OnYesButtonClicked()
    {
        Debug.Log("Yes ��ư Ŭ����");
        confirmAction?.Invoke(); // Ȯ�� ��ư Ŭ�� �� �ݹ� ȣ��
        popupPanel.SetActive(false); // �˾� �ݱ�
    }

    private void OnNoButtonClicked()
    {
        Debug.Log("No ��ư Ŭ����");
        popupPanel.SetActive(false); // �˾� �ݱ�
    }
    // �˾��� ����� �޼���
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    public GameObject popupPanel;  // �˾� �г�
    public TextMeshProUGUI popupMessage;      // �˾� �޽��� �ؽ�Ʈ
    public Button yesButton;       // Yes ��ư
    public Button noButton;        // No ��ư

    // �˾��� ���� �޼���
    public void ShowPopup(string message, Action onConfirm)
    {
        popupPanel.SetActive(true);
        popupMessage.text = message;

        // Yes ��ư Ŭ�� �̺�Ʈ ���
        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke(); // Ȯ�� ��ư Ŭ�� �� �ݹ� ȣ��
            popupPanel.SetActive(false); // �˾� �ݱ�
        });

        // No ��ư Ŭ�� �� �˾� �ݱ�
        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(() =>
        {
            popupPanel.SetActive(false); // �˾� �ݱ�
        });
    }

    // �˾��� ����� �޼���
    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldItem : MonoBehaviour
{
    public int minGoldAmount = 50; // �ּ� ��差
    public int maxGoldAmount = 100; // �ִ� ��差
    public int goldAmount = 100;
    public TextMeshProUGUI goldText;
    public Button goldBtn;
    private bool isPlayerInRange = false; // �÷��̾ ���� ���� �ִ��� ����
    private void Start()
    {
        // ��差�� �ּҰ��� �ִ밪 ���̿��� �������� ����
        goldAmount = Random.Range(minGoldAmount, maxGoldAmount);
        if (goldText != null)
        {
            goldText.text = $"{goldAmount} Gold";
        }
        goldBtn.onClick.AddListener(PicupGoldClick);
        goldBtn.interactable = false; // ó������ ��ư�� ��Ȱ��ȭ
    }
    private void PicupGoldClick()
    { 
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;
        if (playerInfo != null)
        {
            playerInfo.Gold += goldAmount;
            // �ؽ�ƮȮ��
            GameManager.Instance.logUI.AddMessage($"<color=green>{goldAmount} Gold </color>��(��) ȹ���߽��ϴ�!");
        }
        GameManager.Instance.inventoryUI.UpdateGoldUI();
        Destroy(gameObject);
    }

    // �÷��̾ ���� �ȿ� ������ �� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (goldBtn != null)
            {
                goldBtn.interactable = true;
            }
        }
    }
    // �÷��̾ �������� ������ �� ȣ��
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (goldBtn != null)
            {
                goldBtn.interactable = false;
            }
        }
    }

}

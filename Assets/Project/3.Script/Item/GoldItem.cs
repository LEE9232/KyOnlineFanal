using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoldItem : MonoBehaviour
{
    public int minGoldAmount = 50; // 최소 골드량
    public int maxGoldAmount = 100; // 최대 골드량
    public int goldAmount = 100;
    public TextMeshProUGUI goldText;
    public Button goldBtn;
    private bool isPlayerInRange = false; // 플레이어가 범위 내에 있는지 여부
    private void Start()
    {
        // 골드량을 최소값과 최대값 사이에서 랜덤으로 설정
        goldAmount = Random.Range(minGoldAmount, maxGoldAmount);
        if (goldText != null)
        {
            goldText.text = $"{goldAmount} Gold";
        }
        goldBtn.onClick.AddListener(PicupGoldClick);
        goldBtn.interactable = false; // 처음에는 버튼을 비활성화
    }
    private void PicupGoldClick()
    { 
        PlayerInfo playerInfo = GameManager.Instance.PlayerData;
        if (playerInfo != null)
        {
            playerInfo.Gold += goldAmount;
            // 텍스트확인
            GameManager.Instance.logUI.AddMessage($"<color=green>{goldAmount} Gold </color>을(를) 획득했습니다!");
        }
        GameManager.Instance.inventoryUI.UpdateGoldUI();
        Destroy(gameObject);
    }

    // 플레이어가 범위 안에 들어왔을 때 호출
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
    // 플레이어가 범위에서 나갔을 때 호출
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

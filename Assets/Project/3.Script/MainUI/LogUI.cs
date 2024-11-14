using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public TextMeshProUGUI LogText;  // 채팅 로그를 표시할 텍스트(TMP)
    private int maxLines = 100;       // 최대 라인 수
    private Queue<string> messageQueue = new Queue<string>();  // 메시지 큐
    public ScrollRect scrollRect;    // ScrollRect 참조 추가
    public Button chatBtn;
    public Button logBtn;
    public GameObject LogPanel;
    public GameObject ChatPanel;
    public TMP_InputField chatInput;
    private void Awake()
    {
        chatBtn.onClick.AddListener(ChatBtnClick);
        logBtn.onClick.AddListener(LogBtnClick);
    }


    public void AddMessage(string message)
    {
        if (messageQueue.Count >= maxLines)
        {
            messageQueue.Dequeue();  // 최대 라인을 초과하면 가장 오래된 메시지 제거
        }
        messageQueue.Enqueue(message);  // 새 메시지를 큐에 추가

        LogText.text = string.Join("\n", messageQueue.ToArray());  // 큐를 하나의 문자열로 결합하여 텍스트 업데이트
        // 스크롤을 맨 아래로 이동
        StartCoroutine(ScrollToBottom());
    }
    // 메시지를 추가한 후 스크롤을 맨 아래로 이동시키는 코루틴
    private IEnumerator ScrollToBottom()
    {
        // 다음 프레임까지 기다렸다가 스크롤을 업데이트
        yield return null;
        //Canvas.ForceUpdateCanvases();  // Canvas를 강제로 업데이트하여 레이아웃이 즉시 반영되도록 함
        // 스크롤 값을 0으로 설정하여 가장 아래로 이동
        scrollRect.verticalNormalizedPosition = 0f;
    }

    public void ChatBtnClick()
    { 
        ChatPanel.SetActive(true);
        LogPanel.SetActive(false);
    }
    public void LogBtnClick()
    {
        LogPanel?.SetActive(true);
        ChatPanel?.SetActive(false);
    }


}

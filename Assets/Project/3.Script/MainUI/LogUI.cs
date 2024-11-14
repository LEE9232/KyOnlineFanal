using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    public TextMeshProUGUI LogText;  // ä�� �α׸� ǥ���� �ؽ�Ʈ(TMP)
    private int maxLines = 100;       // �ִ� ���� ��
    private Queue<string> messageQueue = new Queue<string>();  // �޽��� ť
    public ScrollRect scrollRect;    // ScrollRect ���� �߰�
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
            messageQueue.Dequeue();  // �ִ� ������ �ʰ��ϸ� ���� ������ �޽��� ����
        }
        messageQueue.Enqueue(message);  // �� �޽����� ť�� �߰�

        LogText.text = string.Join("\n", messageQueue.ToArray());  // ť�� �ϳ��� ���ڿ��� �����Ͽ� �ؽ�Ʈ ������Ʈ
        // ��ũ���� �� �Ʒ��� �̵�
        StartCoroutine(ScrollToBottom());
    }
    // �޽����� �߰��� �� ��ũ���� �� �Ʒ��� �̵���Ű�� �ڷ�ƾ
    private IEnumerator ScrollToBottom()
    {
        // ���� �����ӱ��� ��ٷȴٰ� ��ũ���� ������Ʈ
        yield return null;
        //Canvas.ForceUpdateCanvases();  // Canvas�� ������ ������Ʈ�Ͽ� ���̾ƿ��� ��� �ݿ��ǵ��� ��
        // ��ũ�� ���� 0���� �����Ͽ� ���� �Ʒ��� �̵�
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

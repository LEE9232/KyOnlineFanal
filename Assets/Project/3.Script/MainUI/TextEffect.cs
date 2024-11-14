using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI loadingText;  // Ÿ���� ȿ���� ������ �ؽ�Ʈ
    public float typingSpeed = 0.5f;  // �� ���ھ� ������ �ӵ�

    public void StartTyping(string fullText)
    {
        StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        loadingText.text = "";  // �ؽ�Ʈ�� ���� ����
        foreach (char letter in fullText.ToCharArray())
        {
            loadingText.text += letter;  // �� ���ھ� �߰�
            yield return new WaitForSeconds(typingSpeed);  // ��� �� ���� ���� ���
        }
    }
}

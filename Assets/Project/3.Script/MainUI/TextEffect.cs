using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    public TextMeshProUGUI loadingText;  // 타이핑 효과를 적용할 텍스트
    public float typingSpeed = 0.5f;  // 한 글자씩 나오는 속도

    public void StartTyping(string fullText)
    {
        StartCoroutine(TypeText(fullText));
    }

    IEnumerator TypeText(string fullText)
    {
        loadingText.text = "";  // 텍스트를 비우고 시작
        foreach (char letter in fullText.ToCharArray())
        {
            loadingText.text += letter;  // 한 글자씩 추가
            yield return new WaitForSeconds(typingSpeed);  // 대기 후 다음 글자 출력
        }
    }
}

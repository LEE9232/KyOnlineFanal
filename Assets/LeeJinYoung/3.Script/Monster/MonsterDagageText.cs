using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDagageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float moveUpSpeed = 2f; // 텍스트가 위로 올라가는 속도
    public float fadeOutTime = 1f; // 텍스트가 사라지는 시간
    public Vector3 offset = new Vector3(0, 2, 0); // 텍스트 위치 오프셋
    private Color originalColor; // 텍스트의 원래 색상
    private Vector3 startPosition; // 텍스트의 시작 위치
    private float elapsedTime = 0f;
    private MonsterTextPool textPool; // 오브젝트 풀 참조
    private void Awake()
    {
        // 텍스트의 원래 색상 저장
        originalColor = damageText.color;
    }

    private void OnEnable()
    {
        elapsedTime = 0f;
        damageText.color = originalColor;
        startPosition = transform.localPosition;
        transform.localPosition = startPosition + offset;
    }


    private void Update()
    {
        transform.localPosition += Vector3.up * moveUpSpeed * Time.deltaTime;


        elapsedTime += Time.deltaTime;
        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeOutTime);
        damageText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

        if (alpha <= 0f)
        {
            textPool.ReturnToPool(gameObject);
        }
    }
    public void SetDamageText(int damageAmount, MonsterTextPool pool)
    {
        damageText.text = damageAmount.ToString();
        textPool = pool;
                        
        gameObject.SetActive(true);
    }
    
}

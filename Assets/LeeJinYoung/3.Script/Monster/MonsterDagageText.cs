using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterDagageText : MonoBehaviour
{
    public TextMeshProUGUI damageText;
    public float moveUpSpeed = 2f; // �ؽ�Ʈ�� ���� �ö󰡴� �ӵ�
    public float fadeOutTime = 1f; // �ؽ�Ʈ�� ������� �ð�
    public Vector3 offset = new Vector3(0, 2, 0); // �ؽ�Ʈ ��ġ ������
    private Color originalColor; // �ؽ�Ʈ�� ���� ����
    private Vector3 startPosition; // �ؽ�Ʈ�� ���� ��ġ
    private float elapsedTime = 0f;
    private MonsterTextPool textPool; // ������Ʈ Ǯ ����
    private void Awake()
    {
        // �ؽ�Ʈ�� ���� ���� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextPool : MonoBehaviour
{
    public GameObject damageTextPrefab; // ������ �ؽ�Ʈ ������
    public int poolSize = 10; // ������Ʈ Ǯ ũ��
    private Queue<GameObject> damageTextPool = new Queue<GameObject>();

    // ������Ʈ Ǯ �ʱ�ȭ
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(damageTextPrefab, transform);
            obj.SetActive(false); // �ʱ⿡�� ��Ȱ��ȭ
           
            damageTextPool.Enqueue(obj); // Ǯ�� �߰�
        }
    }

    // ������ �ؽ�Ʈ ������Ʈ�� Ǯ���� ��������
    public GameObject GetDamageText(Transform parentTransform, Vector3 offset)
    {
        GameObject obj;
        if (damageTextPool.Count > 0)
        {
            obj = damageTextPool.Dequeue();
        }
        else
        {
            obj = Instantiate(damageTextPrefab); // Ǯ�� ������ ���ο� ������Ʈ ����
        }
        // �θ� ������ ���� ĵ������ ����
        obj.transform.SetParent(parentTransform, false); // ĵ����(������ �ڽ�) ������ ����
        obj.transform.localPosition = offset;
        obj.SetActive(true);
        return obj;
    }

    // ������ �ؽ�Ʈ ������Ʈ�� �ٽ� Ǯ�� ��ȯ
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // Ǯ(�� ��ũ��Ʈ�� �پ��ִ� ������Ʈ)�� �ڽ����� ����
        damageTextPool.Enqueue(obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTextPool : MonoBehaviour
{
    public GameObject damageTextPrefab; // 데미지 텍스트 프리팹
    public int poolSize = 10; // 오브젝트 풀 크기
    private Queue<GameObject> damageTextPool = new Queue<GameObject>();

    // 오브젝트 풀 초기화
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(damageTextPrefab, transform);
            obj.SetActive(false); // 초기에는 비활성화
           
            damageTextPool.Enqueue(obj); // 풀에 추가
        }
    }

    // 데미지 텍스트 오브젝트를 풀에서 가져오기
    public GameObject GetDamageText(Transform parentTransform, Vector3 offset)
    {
        GameObject obj;
        if (damageTextPool.Count > 0)
        {
            obj = damageTextPool.Dequeue();
        }
        else
        {
            obj = Instantiate(damageTextPrefab); // 풀에 없으면 새로운 오브젝트 생성
        }
        // 부모를 몬스터의 월드 캔버스로 설정
        obj.transform.SetParent(parentTransform, false); // 캔버스(몬스터의 자식) 하위로 설정
        obj.transform.localPosition = offset;
        obj.SetActive(true);
        return obj;
    }

    // 데미지 텍스트 오브젝트를 다시 풀에 반환
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.SetParent(transform); // 풀(이 스크립트가 붙어있는 오브젝트)의 자식으로 설정
        damageTextPool.Enqueue(obj);
    }
}

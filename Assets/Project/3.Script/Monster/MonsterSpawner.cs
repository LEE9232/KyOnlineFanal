using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    public GameObject monsterPrefab; // ������ ���� ������
    public Transform[] spawnPoints; // �� ���Ͱ� ������ �� �ִ� ��ġ��
    public int maxMonsters; // ���ÿ� ������ �� �ִ� �ִ� ���� ��
    public float spawnInterval = 40f;
    [HideInInspector] public int currentMonsterCount = 0; // ���� �����ϴ� ���� ��

}
public class MonsterSpawner : MonoBehaviour
{
    public MonsterSpawnInfo[] monsterSpawnInfos;
    private void Start()
    {
        // �� ���ͺ��� ���� �ڷ�ƾ�� �����Ͽ� ����
        foreach (MonsterSpawnInfo monsterSpawnInfo in monsterSpawnInfos)
        {
            StartCoroutine(SpawnMonsters(monsterSpawnInfo));
        }
    }

    // ���� ���� �ڷ�ƾ
    IEnumerator SpawnMonsters(MonsterSpawnInfo monsterSpawnInfo)
    {
        while (true)
        {

            // �� ���ͺ��� ���� ���� ���� �ִ� ���� ������ ������ ����
            if (monsterSpawnInfo.currentMonsterCount < monsterSpawnInfo.maxMonsters)
            {
                // ���� ���� ����Ʈ ����
                Transform spawnPoint = monsterSpawnInfo.spawnPoints[UnityEngine.Random.Range(0, monsterSpawnInfo.spawnPoints.Length)];

                // ���� ����
                GameObject monster = Instantiate(monsterSpawnInfo.monsterPrefab, spawnPoint.position, spawnPoint.rotation);
                monsterSpawnInfo.currentMonsterCount++; // ���� ���� �� ����
                PlayerTargeting.Instance.RegisterMonster(monster.transform);

                // ���Ͱ� �׾��� �� ī��Ʈ ����
                MonsterStatus monsterStatus = monster.GetComponent<MonsterStatus>();
                if (monsterStatus != null)
                {
                    monsterStatus.onDeath.AddListener(() =>
                    {
                        monsterSpawnInfo.currentMonsterCount--;
                    });
                }
            }
            yield return new WaitForSeconds(monsterSpawnInfo.spawnInterval);
        }
    }
}

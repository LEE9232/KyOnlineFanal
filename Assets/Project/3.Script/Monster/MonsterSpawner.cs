using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterSpawnInfo
{
    public GameObject monsterPrefab; // 스폰할 몬스터 프리팹
    public Transform[] spawnPoints; // 이 몬스터가 스폰될 수 있는 위치들
    public int maxMonsters; // 동시에 존재할 수 있는 최대 몬스터 수
    public float spawnInterval = 40f;
    [HideInInspector] public int currentMonsterCount = 0; // 현재 존재하는 몬스터 수

}
public class MonsterSpawner : MonoBehaviour
{
    public MonsterSpawnInfo[] monsterSpawnInfos;
    private void Start()
    {
        // 각 몬스터별로 개별 코루틴을 실행하여 스폰
        foreach (MonsterSpawnInfo monsterSpawnInfo in monsterSpawnInfos)
        {
            StartCoroutine(SpawnMonsters(monsterSpawnInfo));
        }
    }

    // 몬스터 스폰 코루틴
    IEnumerator SpawnMonsters(MonsterSpawnInfo monsterSpawnInfo)
    {
        while (true)
        {

            // 각 몬스터별로 현재 몬스터 수가 최대 몬스터 수보다 적으면 스폰
            if (monsterSpawnInfo.currentMonsterCount < monsterSpawnInfo.maxMonsters)
            {
                // 랜덤 스폰 포인트 선택
                Transform spawnPoint = monsterSpawnInfo.spawnPoints[UnityEngine.Random.Range(0, monsterSpawnInfo.spawnPoints.Length)];

                // 몬스터 스폰
                GameObject monster = Instantiate(monsterSpawnInfo.monsterPrefab, spawnPoint.position, spawnPoint.rotation);
                monsterSpawnInfo.currentMonsterCount++; // 현재 몬스터 수 증가
                PlayerTargeting.Instance.RegisterMonster(monster.transform);

                // 몬스터가 죽었을 때 카운트 감소
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieCheck : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public GameObject potalObj;
    private bool isPortalActivated = false; // 포탈이 활성화된 적이 있는지 체크
    private bool isPortalDeactivated = false; // 포탈이 비활성화된 적이 있는지 체크
    private void Update()
    {
        // 몬스터가 살아 있을 때 한 번만 포탈 비활성화
        if (monsterStatus.currentHp > 0 && !isPortalDeactivated)
        {
            potalObj.SetActive(false);
            isPortalDeactivated = true; // 이후 이 조건을 다시 실행하지 않음
            isPortalActivated = false;  // 활성화 플래그 초기화
        }

        // 몬스터가 죽었을 때 한 번만 포탈 활성화
        if (monsterStatus.currentHp <= 0 && !isPortalActivated)
        {
            potalObj.SetActive(true);
            isPortalActivated = true; // 이후 이 조건을 다시 실행하지 않음
            isPortalDeactivated = false; // 비활성화 플래그 초기화
        }
    }
}

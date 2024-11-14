using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IdleState : IMonsterState
{
    // 경과 시간
    private float chageTime = 5f;
    // 실시간타임
    private float idleTime;
    public void StartState(MonsterAI monster)
    {
        idleTime = 0f;
    }
    public void UpdateState(MonsterAI monster)
    {
        // 상태 업데이트
        idleTime += Time.deltaTime;
        if (idleTime >= chageTime)
        {
            monster.ChangeState(new M_WalkState());
        }
        if (monster.checkPlayer())
        {
            monster.ChangeState(new M_RunState());
        }
    }
    public void ExitState(MonsterAI monster)
    {

    }
}

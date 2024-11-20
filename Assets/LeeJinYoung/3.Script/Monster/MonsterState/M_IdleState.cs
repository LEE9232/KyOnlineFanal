using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_IdleState : IMonsterState
{
    // ��� �ð�
    private float chageTime = 5f;
    // �ǽð�Ÿ��
    private float idleTime;
    public void StartState(MonsterAI monster)
    {
        idleTime = 0f;
    }
    public void UpdateState(MonsterAI monster)
    {
        // ���� ������Ʈ
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

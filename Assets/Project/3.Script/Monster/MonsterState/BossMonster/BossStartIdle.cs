using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStartIdle : IBossState
{
    private float chageTime = 5f;
    private float StartTime;
    public void StartState(BossAI b_mons)
    {
        StartTime = 0f;      
    }

    public void UpdateState(BossAI b_mons)
    {
        StartTime += Time.deltaTime;
        if (StartTime >= chageTime && b_mons.checkPlayer())
        {
            b_mons.ChangeState(new BossIdle());
        }
     
    }
    public void ExitState(BossAI b_mons)
    {

    }
}

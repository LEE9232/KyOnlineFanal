using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : IBossState
{
    public void StartState(BossAI b_mons)
    {
        
    }
    public void UpdateState(BossAI b_mons)
    {
        if (b_mons.checkPlayer())
        {
            b_mons.agent.SetDestination(b_mons.GetTargetPlayerPosition().position);
            b_mons.ChangeState(new BossRun());
        }
    }

    public void ExitState(BossAI b_mons)
    {

    }

}

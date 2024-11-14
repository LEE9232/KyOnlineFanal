using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBossState
{
    void StartState(BossAI b_mons);

    void UpdateState(BossAI b_mons);

    void ExitState(BossAI b_mons);

}

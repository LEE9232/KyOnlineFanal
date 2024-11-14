using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    // 상태의 시작
    void StartState(MonsterAI monster);

    // 상태 진행중
    void UpdateState(MonsterAI monster);

    // 상태가 끝났을때 호출이되어서 빠져나올함수
    void ExitState(MonsterAI monster);
}

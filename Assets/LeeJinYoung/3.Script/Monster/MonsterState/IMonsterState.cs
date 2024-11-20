using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonsterState
{
    // ������ ����
    void StartState(MonsterAI monster);

    // ���� ������
    void UpdateState(MonsterAI monster);

    // ���°� �������� ȣ���̵Ǿ ���������Լ�
    void ExitState(MonsterAI monster);
}

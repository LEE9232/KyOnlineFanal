using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandChar : IPlayerSkill
{
    private ISkillManager skillManager;
    public void StartSkill(ISkillManager iSkillManager)
    {

    }

    public void UpdateSkill(ISkillManager iSkillManager)
    {
        skillManager = iSkillManager;

        //스킬1,2,3,4에 대한 조건문을 각각 캡슐화 시켜서 써줘야함
        // 1.몬스터를 타겟중인가?
        // 2.스킬 포인트에 대한 조건(ex:파이어볼2번째는 파이어볼1번째의 포인트가 5이상이어야 한다.)

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {         
            iSkillManager.ChangeSkill(new FireBall());
        }



    }

    public void ExitSkill(ISkillManager iSkillManager)
    {

    }
}

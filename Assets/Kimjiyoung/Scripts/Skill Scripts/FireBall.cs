using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : IPlayerSkill
{
    //private bool skilldds;
    //public CastingState castingState;

    public void StartSkill(ISkillManager iSkillManager)
    {
        //iSkillManager.playerManagement.pmAnim.SetBool("IsFire1", true);
        //iSkillManager.casting.skill1casting();
    }

    public void UpdateSkill(ISkillManager iSkillManager)
    {
        //iSkillManager.casting.skilldd = true;
        iSkillManager.casting.skill1casting();
        //if (iSkillManager.casting.skilldd == false)
        //{
        //iSkillManager.FireBalls();
            iSkillManager.ChangeSkill(new StandChar());
        //
        //}
    }

    public void ExitSkill(ISkillManager iSkillManager)
    {
        // Test
        //EX)
        //iSkillManager.StartCoroutine(enumerator());
        //// 대기만 가지는 코루틴 모노비를상속받은 스크립트를 가져와서 사용 
        //
        //iSkillManager.StartCoroutine(Mono(iSkillManager));
        // 특정 객체정보를 받아서 실행해야할때 모노비스크립트에있는 정보를 받아서 처리한다.
    }


    // 이아이는 단순히 대기시간만 가지고있는 코루틴
    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1f);
    }

    // 이 아이는 정보를 가진것을 처리할때 모노비를 상속받고있는 아이를 파라미터 값으로 지정해 사용한다,
    private IEnumerator Mono(ISkillManager iSkillManager)
    {
        yield return new WaitForSeconds(1f);
        iSkillManager.casting.skilldd = false;
    }
}

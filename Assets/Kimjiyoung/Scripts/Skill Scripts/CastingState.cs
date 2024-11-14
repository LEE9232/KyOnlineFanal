using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingState : IPlayerSkill
{
    private float castingTime;
    private float timer = 0f;
    private bool isCasting = false;

    public void StartSkill(ISkillManager iSkill)
    {
        //캐스팅 시작!
        //캐스팅 애니메이션
        timer = 0f;
        iSkill.playerManagement.pmAnim.SetBool("IsCasting", true);
        Debug.Log("캐스팅 시작!");
        
    }

    public void UpdateSkill(ISkillManager iSkill)
    {
        //캐스팅 중이다
        //캐스팅이 끝나면 다음 상태를 호출(ex:fireball)
        timer += Time.deltaTime;
        if (timer >= 2.5f)
        {
            iSkill.ChangeSkill(new FireBall());
        }
    }

    public void ExitSkill(ISkillManager iSkill)
    {
        //나는 캐스팅이 끝났다 애니메이션 종료
    }

    IEnumerator CastingCoroutine()
    {
        yield return new WaitForSeconds(castingTime);
    }
}

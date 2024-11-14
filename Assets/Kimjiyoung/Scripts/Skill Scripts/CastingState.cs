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
        //ĳ���� ����!
        //ĳ���� �ִϸ��̼�
        timer = 0f;
        iSkill.playerManagement.pmAnim.SetBool("IsCasting", true);
        Debug.Log("ĳ���� ����!");
        
    }

    public void UpdateSkill(ISkillManager iSkill)
    {
        //ĳ���� ���̴�
        //ĳ������ ������ ���� ���¸� ȣ��(ex:fireball)
        timer += Time.deltaTime;
        if (timer >= 2.5f)
        {
            iSkill.ChangeSkill(new FireBall());
        }
    }

    public void ExitSkill(ISkillManager iSkill)
    {
        //���� ĳ������ ������ �ִϸ��̼� ����
    }

    IEnumerator CastingCoroutine()
    {
        yield return new WaitForSeconds(castingTime);
    }
}

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
        //// ��⸸ ������ �ڷ�ƾ ���񸦻�ӹ��� ��ũ��Ʈ�� �����ͼ� ��� 
        //
        //iSkillManager.StartCoroutine(Mono(iSkillManager));
        // Ư�� ��ü������ �޾Ƽ� �����ؾ��Ҷ� ����ũ��Ʈ���ִ� ������ �޾Ƽ� ó���Ѵ�.
    }


    // �̾��̴� �ܼ��� ���ð��� �������ִ� �ڷ�ƾ
    private IEnumerator enumerator()
    {
        yield return new WaitForSeconds(1f);
    }

    // �� ���̴� ������ �������� ó���Ҷ� ���� ��ӹް��ִ� ���̸� �Ķ���� ������ ������ ����Ѵ�,
    private IEnumerator Mono(ISkillManager iSkillManager)
    {
        yield return new WaitForSeconds(1f);
        iSkillManager.casting.skilldd = false;
    }
}

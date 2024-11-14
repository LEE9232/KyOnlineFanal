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

        //��ų1,2,3,4�� ���� ���ǹ��� ���� ĸ��ȭ ���Ѽ� �������
        // 1.���͸� Ÿ�����ΰ�?
        // 2.��ų ����Ʈ�� ���� ����(ex:���̾2��°�� ���̾1��°�� ����Ʈ�� 5�̻��̾�� �Ѵ�.)

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {         
            iSkillManager.ChangeSkill(new FireBall());
        }



    }

    public void ExitSkill(ISkillManager iSkillManager)
    {

    }
}

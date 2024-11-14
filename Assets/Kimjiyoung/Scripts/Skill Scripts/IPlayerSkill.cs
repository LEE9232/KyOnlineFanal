using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerSkill
{
    void StartSkill(ISkillManager iSkillManager);

    void UpdateSkill(ISkillManager iSkillManager);

    void ExitSkill(ISkillManager iSkillManager);
}

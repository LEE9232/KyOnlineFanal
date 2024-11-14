using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSkillContext : MonoBehaviour
{
    private IMagicSkills magicSkill;
    private bool isOnCooldown = false;

    public MagicSkillContext(IMagicSkills magicSkill)
    {
        this.magicSkill = magicSkill;
    }

    public void UseSkill(Transform target, Vector3 position, IMagicSkills skill)
    {
        if (skill != null && !isOnCooldown)
        {
            // 여기서 스킬을 사용하고 쿨타임 시작
            //skill.UseSkill(target, position, skillindex);
            //StartCoroutine(MagicCooldown(skill.CooldownTime)); // 스킬의 쿨타임 가져오기
        }
        else if (isOnCooldown)
        {
            Debug.Log("Skill is on cooldown.");
        }
    }

    private IEnumerator MagicCooldown(float cooldownTime)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IMagicSkills
{
    //int skillID { get; }
    string mSkillName { get; }
    float Cooldown { get; } // 각 스킬별로 고유한 쿨타임을 설정
    int Damage { get; }
    int UseMP { get; }
    bool isOnCooldown { get; }
    //int DamageIncreasePerLevel { get; } // 레벨당 데미지 증가량
    //float CooldownReductionPerLevel { get; } // 레벨당 쿨타임 감소량

    Sprite SkillImage { get; }

    void UseSkill(Transform target, Vector3 position, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText);
}

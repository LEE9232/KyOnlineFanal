using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IMagicSkills
{
    //int skillID { get; }
    string mSkillName { get; }
    float Cooldown { get; } // �� ��ų���� ������ ��Ÿ���� ����
    int Damage { get; }
    int UseMP { get; }
    bool isOnCooldown { get; }
    //int DamageIncreasePerLevel { get; } // ������ ������ ������
    //float CooldownReductionPerLevel { get; } // ������ ��Ÿ�� ���ҷ�

    Sprite SkillImage { get; }

    void UseSkill(Transform target, Vector3 position, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText);
}

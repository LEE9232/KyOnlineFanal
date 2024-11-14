using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SkillData", menuName ="Skill/New Skill")]
public class mSkillData
{
    public string skillName;
    public float cooldown;
    public int damage;
    public int useMP;
    public int skillLevel;
    public Sprite skillImage;
    public GameObject skillPrefab;
}

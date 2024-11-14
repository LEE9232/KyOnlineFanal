using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewSkill", menuName ="Skills/Skill")]
public class Skill : ScriptableObject
{
    public string skillName;
    public GameObject skillEffect;
    public float coolTime;
    public float Speed;
    public int damage;
}
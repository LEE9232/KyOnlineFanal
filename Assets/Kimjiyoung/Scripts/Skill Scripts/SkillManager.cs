using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public class SkillManager : MonoBehaviour
//{
//    public SkillPointManager skillPointManager;
//    public PlayerManagement playerManagement;
//    SkillPointUpButton skillPointUpButton;
//
//    public List<Skill> skills = new List<Skill>();
//    public void Start()
//    {
//        //스킬 추가
//        skills.Add(new Skill("FireBall1", 5));
//        skills.Add(new Skill("FireBall2", 10)); // FireBall1 레벨 5 달성 시 해금
//        skills.Add(new Skill("FireBall3", 20)); // FireBall2 레벨 10 달성 시 해금
//        skills.Add(new Skill("IceSpear1", 5));
//        skills.Add(new Skill("IceSpear2", 10)); // IceSpear1 레벨 5 달성 시 해금
//        skills.Add(new Skill("IceSpear3", 20)); // IceSpear2 레벨 10 달성 시 해금
//        skills.Add(new Skill("SingleSkill", 3));
//        skills.Add(new Skill("WideSkill", 3));
//
//        skillPointUpButton.UpdateSkillButtons();
//    }
//
//
//
//    public void UpgradeSkill(string skillName)
//    {
//
//        Skill skill = skills.Find(s => s.Name == skillName);
//        if (skill != null)
//        {
//            if (skill.SkillLevel < skill.MaxSkillLevel)
//            {
//                if (skillPointManager.UseSkillPoints(1))
//                {
//                    if (skill.Upgrade())
//                    {
//
//                        skillPointUpButton.UpdateSkillButtons();
//                    }
//                }
//                else
//                {
//                    Debug.Log("Not enough skill points");
//                }
//            }
//            else
//            {
//                Debug.Log($"{skillName} is already at max level");
//            }
//        }
//        else
//        {
//            Debug.Log($"Skill {skillName} not found");
//        }
//
//    }
//
//}

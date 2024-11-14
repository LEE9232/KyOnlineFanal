//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SkillUnlockManager : MonoBehaviour
//{
//    public PlayerManagement playerManagement;
//    public SkillPointManager skillPointManager;

//    private void Update()
//    {
//        CheckSkillUnlockConditions();
//    }

//    //해금 조건
//    //나중에 해금된 스킬 이미지 흑백=>컬러로 바뀌게 하면 될듯
//    private void CheckSkillUnlockConditions()
//    {
//        Skill fireBall1 = skillManager.skills.Find(s => s.Name == "FireBall1");
//        Skill fireBall2 = skillManager.skills.Find(s => s.Name == "FireBall2");
//        if (fireBall1 != null && fireBall1.SkillLevel >= 5 &&
//            fireBall2 != null && fireBall2.SkillLevel == 0)
//        {
//            fireBall2.SkillLevel = 1;
//        }

//        Skill fireBall3 = skillManager.skills.Find(s => s.Name == "FireBall3");
//        if (fireBall2 != null && fireBall2.SkillLevel >= 10 &&
//            fireBall3 != null && fireBall3.SkillLevel == 0)
//        {
//            fireBall3.SkillLevel = 1;
//        }

//        Skill iceSpear1 = skillManager.skills.Find(s => s.Name == "IceSpaer1");
//        Skill iceSpear2 = skillManager.skills.Find(s => s.Name == "IceSpaer2");
//        if (iceSpear1 != null && iceSpear1.SkillLevel >= 5 &&
//            iceSpear2 != null && iceSpear2.SkillLevel == 0)
//        {
//            iceSpear2.SkillLevel = 1;
//        }

//        Skill iceSpear3 = skillManager.skills.Find(s => s.Name == "IceSpaer3");
//        if (iceSpear2 != null && iceSpear2.SkillLevel >= 10 &&
//            iceSpear3 != null && iceSpear3.SkillLevel == 0)
//        {
//            iceSpear3.SkillLevel = 1;
//        }

//        Skill singleSkill = skillManager.skills.Find(s => s.Name == "SingleSkill");
//        Skill wideSkill = skillManager.skills.Find(s => s.Name == "WideSkill");

//        bool singleSkillUnlocked = playerManagement.playerLevel >= 15;
//        bool wideSkillUnlocked = playerManagement.playerLevel >= 15;

//        if (singleSkillUnlocked && singleSkill != null &&
//            singleSkill.SkillLevel == 0)
//        {
//            singleSkill.SkillLevel = 0;
//        }

//        if (wideSkillUnlocked && wideSkill != null &&
//            wideSkill.SkillLevel == 0)
//        {
//            wideSkill.SkillLevel = 0;
//        }
//    }
//}

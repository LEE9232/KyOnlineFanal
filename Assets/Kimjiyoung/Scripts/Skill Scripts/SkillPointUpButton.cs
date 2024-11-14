using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointUpButton : MonoBehaviour
{
    public SkillPointManager skillPointManager;
    public PlayerManagement playerManagement;

    public Button FireBall1SkillButton;
    public Button FireBall2SkillButton;
    public Button FireBall3SkillButton;

    public Button IceSpear1SkillButton;
    public Button IceSpear2SkillButton;
    public Button IceSpear3SkillButton;

    public Button SingleSkillButton;
    public Button WideSkillButton;

    //private List<Skill> skills;

    public void UpdateSkillButtons()
    {
        //스킬 포인트가 있는지 확인 있으면 true가 되며 스킬 포인트를 올릴 수 있게 버튼 활성화
        bool canUpgradeFireBall1 = skillPointManager.GetTotalSkillPoints() > 0;
        bool canUpgradeIceSpear1 = skillPointManager.GetTotalSkillPoints() > 0;

        //하위 스킬의 레벨이 Max가 되었는지 확인
        //bool canUpgradeFireBall2 =
        //    skillPointManager.GetTotalSkillPoints() > 0 && skills.Find(s => s.Name == "FireBall1")?.SkillLevel >= 5;
        //bool canUpgradeFireBall3 =
        //    skillPointManager.GetTotalSkillPoints() > 0 && skills.Find(s => s.Name == "FireBall2")?.SkillLevel >= 10;
        //bool canUpgradeIceSpear2 =
        //    skillPointManager.totalSkillPoints > 0 && skills.Find(s => s.Name == "IceSpear2")?.SkillLevel >= 5;
        //bool canUpgradeIceSpear3 =
        //    skillPointManager.totalSkillPoints > 0 && skills.Find(s => s.Name == "IceSpear2")?.SkillLevel >= 10;

        int playerLevel = GameManager.Instance.PlayerData.Level;
        int totalSkillPoints = skillPointManager.GetTotalSkillPoints();

        //bool singleSkillAvailable =
        //    playerManagement.playerLevel >= 15 || playerManagement.playerLevel >= 30 || playerManagement.playerLevel >= 45;
        //bool wideSkillAvailable =
        //    playerManagement.playerLevel >= 15 || playerManagement.playerLevel >= 30 || playerManagement.playerLevel >= 45;
        bool singleSkillAvailable = GameManager.Instance.PlayerData.Level >= 15;
        bool wideSkillAvailable = GameManager.Instance.PlayerData.Level >= 15;

        // 레벨에 따라 버튼을 활성화하기 위한 세밀한 조건
        if (playerLevel >= 30)
        {
            SingleSkillButton.interactable = (totalSkillPoints > 0);
        }
        if (playerLevel >= 45)
        {
            SingleSkillButton.interactable = (totalSkillPoints > 0);
        }

        //FireBall1SkillButton.interactable = canUpgradeFireBall1;
        //IceSpear1SkillButton.interactable = canUpgradeIceSpear1;

        //FireBall2SkillButton.interactable = canUpgradeFireBall2;
        //IceSpear2SkillButton.interactable = canUpgradeIceSpear2;

        //FireBall3SkillButton.interactable = canUpgradeFireBall3;
        //IceSpear3SkillButton.interactable = canUpgradeFireBall3;
    }

}

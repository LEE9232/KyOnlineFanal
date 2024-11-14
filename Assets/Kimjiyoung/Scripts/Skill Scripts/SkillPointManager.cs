using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPointManager : MonoBehaviour
{
    public int totalSkillPoints;
    //public int MaxSkillPoints = 50;

    

    public void AddSkillPoints(int points)
    {
        //totalSkillPoints = Mathf.Clamp(totalSkillPoints + points, 0, MaxSkillPoints);
    }

    public bool UseSkillPoints(int points)
    {
        if (totalSkillPoints >= points)
        {
            totalSkillPoints -= points;
            return true;
        }
        return false;
    }

    public int GetTotalSkillPoints()
    {
        return totalSkillPoints;
    }
}

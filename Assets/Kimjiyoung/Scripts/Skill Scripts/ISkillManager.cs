using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ISkillManager : MonoBehaviour
{
    public GameObject fireball;
    public Transform pos;
    private IPlayerSkill currentSkill;
    public PlayerManagement playerManagement;
    private fireBallProjectile fireBallProjectile = new fireBallProjectile();
    public CastingManager casting { get; set; }

    public float fireCool = 3f;

    private void Awake()
    {
        //ChangeSkill(new StandChar());
        //fireBall = GetComponent<fireBallProjectile>();
        ChangeSkill(new StandChar());
        casting = GetComponent<CastingManager>();
    }

    private void Update()
    {
        if (currentSkill != null)
        {
            currentSkill.UpdateSkill(this);
        }

    }

    public void ChangeSkill(IPlayerSkill newSkill)
    {
        if (currentSkill != null)
        {
            currentSkill.ExitSkill(this);
        }
        currentSkill = newSkill;
        currentSkill.StartSkill(this);
    }
    public void FireBalls()
    {

        if (fireBallProjectile.IsFire == false)
        {
            var obj = Instantiate(fireball, pos.position, pos.rotation);
            StartCoroutine(fireBallProjectile.CoolTime_FireBall1());

        }
    }

}

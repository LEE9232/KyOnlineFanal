using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CastingManager : MonoBehaviour
{
    public bool skilldd = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void skill1casting()
    {
        //skilldd = true;
        StartCoroutine(FireBall1CastingTime());
    }
    public IEnumerator FireBall1CastingTime()
    {
        yield return new WaitForSeconds(5f);
        //skilldd = false;
        Debug.Log(skilldd);
        ISkillManager manager = GetComponent<ISkillManager>();
        manager.FireBalls();
    }
}

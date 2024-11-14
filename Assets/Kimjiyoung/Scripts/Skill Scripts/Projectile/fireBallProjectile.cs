using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireBallProjectile : MonoBehaviour
{
    private PlayerManagement playerManagement = new PlayerManagement();
    private Rigidbody rd;
    public GameObject ExplosionPrefab;//터지는 애
    public Vector3 velocity;
    public int fireBall1Damage = 0;

    public float Speed = 20f;
    public float fireCool = 3f;

    public bool IsFire { get; set; } = false;


    private void Awake()
    {
        //Destroy(gameObject, 4f);
        rd = GetComponent<Rigidbody>();
        rd.velocity = transform.forward * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        var exp = Instantiate(ExplosionPrefab, transform.position, ExplosionPrefab.transform.rotation);
        Destroy(exp, 3f);
        Transform child;
        child = transform.GetChild(0);
        transform.DetachChildren();
        Destroy(child.gameObject, 2f);
        Destroy(gameObject);
    }

    // 스킬 데미지를 몬스터 태그를 가지고 있는 애한테
    public void DamageToMonsters(int fire1damage)
    {
        
    }
    

    private void Update()
    {
        //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        //playerManagement.mDamage = 50; 스킬들은 가해자 시점으로 만들어야 한다?
        //몬스터가 피해자가 되게
    }

    public IEnumerator CoolTime_FireBall1()
    {
        IsFire = true;
        yield return new WaitForSeconds(fireCool);
        IsFire = false;
    }

}

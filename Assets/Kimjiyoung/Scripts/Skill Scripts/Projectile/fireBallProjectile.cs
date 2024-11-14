using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class fireBallProjectile : MonoBehaviour
{
    private PlayerManagement playerManagement = new PlayerManagement();
    private Rigidbody rd;
    public GameObject ExplosionPrefab;//������ ��
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

    // ��ų �������� ���� �±׸� ������ �ִ� ������
    public void DamageToMonsters(int fire1damage)
    {
        
    }
    

    private void Update()
    {
        //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        //playerManagement.mDamage = 50; ��ų���� ������ �������� ������ �Ѵ�?
        //���Ͱ� �����ڰ� �ǰ�
    }

    public IEnumerator CoolTime_FireBall1()
    {
        IsFire = true;
        yield return new WaitForSeconds(fireCool);
        IsFire = false;
    }

}

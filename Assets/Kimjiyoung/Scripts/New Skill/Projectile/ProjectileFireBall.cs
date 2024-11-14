using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFireBall : MonoBehaviour
{
    public GameObject explosionFireball;
   // public Vector3 velocity;
   // public float fireballSpeed;

   // public Rigidbody rb;

   // public bool isFireBall1 = false;

    private void Awake()
    {
       // rb = GetComponent<Rigidbody>();
       // rb.velocity = velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Monster")
        {
            var exposion = Instantiate(explosionFireball, transform.position, explosionFireball.transform.rotation);
            Destroy(explosionFireball, 5f);
            Transform child;
            child = transform.GetChild(0);
            transform.DetachChildren();
            Destroy(child.gameObject, 3f);
            Destroy(gameObject);
            //StartCoroutine(FireBall1CoolTime(3f));
            //isFireBall1 = false;
        }
    }



    

}

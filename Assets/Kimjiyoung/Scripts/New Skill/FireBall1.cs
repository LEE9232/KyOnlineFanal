using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall1 : MonoBehaviour
{
    public GameObject explosionFireball;
    //public int skillDamage = 20;
    private bool hasDealtDamage = false;

    private void OnTriggerEnter(Collider other)
    {
        //if (hasDealtDamage) return;
        //hasDealtDamage = true;

        //MonsterStatus monsterStatus = new MonsterStatus();

        MonsterStatus monsterStatus = other.GetComponent<MonsterStatus>();
        if (other.CompareTag("Monster"))
        {
            //if (monsterStatus != null)
            //{
                // 데미지 구현
                //monsterStatus.TakeDamage(skillDamage);
            //}
            var exposion = Instantiate(explosionFireball, transform.position, explosionFireball.transform.rotation);
            Destroy(exposion, 2f);
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CobraProjectile : MonoBehaviour
{
    public float speed = 15f; // �߻�ü �ӵ�
    public int damage = 15;   // �߻�ü�� �� ���ط�
    public GameObject impactEffect; // �浹 �� �ð��� ȿ��

    private void Start()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }
    // Ʈ���Ÿ� ����Ͽ� �浹 ����
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹 ��
        {
            // �÷��̾�� ������ ����
            other.GetComponent<PlayerManagement>().TakeDamage(damage);
            // �浹 ȿ�� ����
            if (impactEffect != null)
            {
                GameObject impact =  Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact ,1f);
            }
            Destroy(gameObject);
            // �߻�ü ����
        }
        if (other.CompareTag("Guard"))
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact, 1f);
            }
            // �߻�ü ����
            Destroy(gameObject);
        }
        if (other.CompareTag("Untagged")) // ��ֹ��� ����� �� �߻�ü ����
        {
            if (impactEffect != null)
            {
                GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(impact , 1f);
            }
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponCollision : MonoBehaviour
{
    private bool colliderCheck = true; // �浹�� �� �ִ� ���¸� ��Ÿ��
    private float damageCooldown = 1f; // �⺻ ��ٿ� �ð�
    public MonsterStatus monsterStatus;
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player")&& colliderCheck)
        {
            Debug.Log("����� �浹��");
            //ApplyDamage();
            other.GetComponent<PlayerManagement>().TakeDamage(monsterStatus.monsDamage);
            // �ٽ� �浹�� �� ������ ���� ����
            StartCoroutine(DamageCooldown());
        }
    }
    private void ApplyDamage()
    {

       // Debug.Log("������ ����");
    }

    // �浹 ��ٿ��� ó���ϴ� �ڷ�ƾ
    private IEnumerator DamageCooldown()
    {
        colliderCheck = false;
        yield return new WaitForSeconds(damageCooldown); // ������ ��ٿ� �ð���ŭ ���
        colliderCheck = true;
    }

    // ���ݸ��� �浹 ��ٿ� �ð��� �����ϴ� �޼���
    public void SetDamageCooldown(float cooldown)
    {
        damageCooldown = cooldown;
    }
}

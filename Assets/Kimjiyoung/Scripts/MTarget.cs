using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MTarget : MonoBehaviour
{
    public float targetRadius = 1f;
    //public MonsterHPbar hpBar;  // HP�� ���� ��ũ��Ʈ
    public GameObject PlayerRay;
    public Image aim;
    private Transform targetMonster;
    private Transform targetPoition;


    public Color rayColor = Color.red;
    public Color sphereColor = Color.green;

    private void Start()
    {
        aim.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // �ֺ� ���͸� Ž���Ͽ� �ڵ����� Ÿ������ �����մϴ�.
            UpdateTarget();
        }
        // Ÿ���� ������ ��� ���� �̹����� ȭ�鿡 ǥ���մϴ�.
        if (targetPoition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPoition.position);

            aim.transform.position = screenPosition;
        }
        else
        {
            aim.gameObject.SetActive(false); // Ÿ���� ���� ��� ���� �̹����� ��Ȱ��ȭ�մϴ�.
        }
    }
    private void UpdateTarget()
    {
        // ������ ���� ��ġ�� ������ �����մϴ�.
        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);

        RaycastHit hit;
        float sphereCastRadius = 5f; // ������ �ݰ� (���� ����)

        // SphereCast ������ �ð������� ǥ���մϴ�.

        // SphereCast�� ����Ͽ� �ݰ� ���� ������Ʈ�� Ž���մϴ�.

        //if (Physics.SphereCast(ray, sphereCastRadius, out hit, targetRadius))
        //{
        //    //MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
        //    if (monster != null)
        //    {
        //        // Ÿ�� ���Ͱ� ������ ���
        //        if (targetMonster == null || targetMonster != monster.transform)
        //        {
        //            targetMonster = monster.transform;
        //            targetPoition = monster.targetPosition; // Ÿ�� ��ġ�� ����
        //            aim.gameObject.SetActive(true);
        //            hpBar.SetTargetmonster(monster); // Ŭ���� ���͸� Ÿ����
        //            // ü�� ��ȭ �̺�Ʈ�� �����Ͽ� ü�� 0�� �� UI ��Ȱ��ȭ
        //            monster.onHpChange.AddListener(OnMonsterHpChanged);
        //        }
        //        else
        //        {
        //            // ���Ͱ� �ƴ� ��� ���� �̹����� ��Ȱ��ȭ�մϴ�.
        //            aim.gameObject.SetActive(false);
        //            hpBar.hpBarUI.SetActive(false);
        //        }
        //    }
        //}
        //else
        //{
        //    // SphereCast�� ���Ͱ� ���� ���
        //    if (targetMonster != null)
        //    {
        //        // Ÿ�� ���Ͱ� ���� ���������� Ž������ �ʴ� ���
        //        targetMonster = null;
        //        targetPoition = null;
        //        hpBar.hpBarUI.SetActive(false);
        //        aim.gameObject.SetActive(false);
        //    }
        //}
    }
    //private void OnMonsterHpChanged(int currentHp, int maxHP)
    //{
    //    if (currentHp <= 0)
    //    {
    //        aim.gameObject.SetActive(false);
    //        hpBar.hpBarUI.SetActive(false);
    //    }
    //}
    private void OnDrawGizmos()
    {
        if (PlayerRay == null)
            return;

        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);
        float sphereCastRadius = 1f; // ������ �ݰ� (���� ����)

        // ������ ������ �ð������� ǥ���մϴ�.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(ray.origin, ray.direction * targetRadius);

        // ������ ������ �ð������� ǥ���մϴ�.
        Vector3 sphereCastEndPoint = ray.origin + ray.direction * targetRadius;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(sphereCastEndPoint, sphereCastRadius);
    }

}



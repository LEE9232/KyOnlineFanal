using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieCheck : MonoBehaviour
{
    public MonsterStatus monsterStatus;
    public GameObject potalObj;
    private bool isPortalActivated = false; // ��Ż�� Ȱ��ȭ�� ���� �ִ��� üũ
    private bool isPortalDeactivated = false; // ��Ż�� ��Ȱ��ȭ�� ���� �ִ��� üũ
    private void Update()
    {
        // ���Ͱ� ��� ���� �� �� ���� ��Ż ��Ȱ��ȭ
        if (monsterStatus.currentHp > 0 && !isPortalDeactivated)
        {
            potalObj.SetActive(false);
            isPortalDeactivated = true; // ���� �� ������ �ٽ� �������� ����
            isPortalActivated = false;  // Ȱ��ȭ �÷��� �ʱ�ȭ
        }

        // ���Ͱ� �׾��� �� �� ���� ��Ż Ȱ��ȭ
        if (monsterStatus.currentHp <= 0 && !isPortalActivated)
        {
            potalObj.SetActive(true);
            isPortalActivated = true; // ���� �� ������ �ٽ� �������� ����
            isPortalDeactivated = false; // ��Ȱ��ȭ �÷��� �ʱ�ȭ
        }
    }
}

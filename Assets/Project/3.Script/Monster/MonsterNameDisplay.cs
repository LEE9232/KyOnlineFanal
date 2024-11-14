using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonsterNameDisplay : MonoBehaviour
{
    public MonsterStatus mons_Stat;
    public Transform nameTagPosition; // �̸��� ǥ���� ��ġ (������ �Ӹ� ���� ��ġ ����)
    public Camera mainCamera; // �÷��̾��� ���� ī�޶�
    public TextMeshProUGUI nameText; // TextMeshPro �ؽ�Ʈ UI
    private void Start()
    {
        mainCamera = Camera.main; // ���� ī�޶� �ڵ����� �Ҵ�
        // ���� �̸� ����
        nameText.text = mons_Stat.monsName;
        SetTextColorByGrade(mons_Stat.monsterGrade);
        nameText.raycastTarget = false; // �ڵ�� �����ϴ� ���
    }

    private void LateUpdate()
    {
        // �̸��� �׻� ī�޶� ���ϵ��� ����
        if (mainCamera != null)
        {
            nameTagPosition.rotation = Quaternion.LookRotation(nameTagPosition.position - mainCamera.transform.position);
        }
    }
    private void SetTextColorByGrade(MonsterStatus.MonsterGrade grade)
    {
        switch (grade)
        {
            case MonsterStatus.MonsterGrade.Normal:
                nameText.color = Color.white; // �⺻ ����
                break;
            case MonsterStatus.MonsterGrade.Rare:
                nameText.color = Color.blue; // ��� ������ ����
                break;
            case MonsterStatus.MonsterGrade.Epic:
                nameText.color = Color.magenta; // ���� ������ ����
                break;
            case MonsterStatus.MonsterGrade.Legendary:
                nameText.color = Color.yellow; // ���� ������ ����
                break;
            default:
                nameText.color = Color.white;
                break;
        }
    }
}

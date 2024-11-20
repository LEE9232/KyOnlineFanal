using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public BossSkillParticle bossSkillParticle;
    public int skillIndex; // ��ų�� ������ ������ �ε���
    public int damagePerSecond = 0; // �ʴ� ������
    public int JumpAttackDamage = 350;
    public int areaSwdSkillEffect = 400;
    private bool hasCollided = false; // �浹 ���θ� �����ϴ� �÷���
    private float collisionCooldown; // �浹 ��ٿ� �ð�
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player") && !hasCollided) // �ʿ��� �±׷� ����
        {
            hasCollided = true; // �浹 �߻� �÷��� ����
            // ��ų�� ���� �ٸ� ó���� �� �� �ֵ��� �б�
            switch (skillIndex)
            {
                case 0: // JumpDashAttack
                    ApplyJumpDashAttackEffect(other);
                    collisionCooldown = 3.0f; // ���� �뽬 ������ �浹 �� 3�� ���
                    break;
                case 1: // AreaSwdSkill
                    ApplyAreaSwdSkillEffect(other);
                    collisionCooldown = 2.0f; // AreaSwdAttack�� �浹 �� 2�� ���
                    break;
                case 2: //SecondSkill
                    ApplyAreaMgSkillEffect(other);
                    collisionCooldown = 1f; // AreaMgAttack�� �浹 �� 1�� ���
                    break;
                // �ٸ� ��ų �߰� ����
                default:
                    //Debug.LogWarning("�ε���: " + skillIndex);
                    break;
            }
            // �浹 �� ���� �ð� �Ŀ� �ٽ� �浹 �����ϵ��� ����
            StartCoroutine(ResetCollisionCooldown());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && skillIndex == 2)
        {
            // ���� ������ ���� ��ų�� ���� �ʴ� ������ ����
            ApplyAreaMgSkillEffect(other);
        }
    }
    // �� ��ų�� ���� ó�� ������ �Լ��� ����
    private void ApplyJumpDashAttackEffect(Collider player)
    {
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
            playerManagement.TakeDamage(JumpAttackDamage);
            //GameManager.Instance.logUI.AddMessage($" {JumpAttackDamage} <color=red>Damage </color>�� �Ծ����ϴ�.");
        }
    }
    private void ApplyAreaSwdSkillEffect(Collider player)
    {
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
           
            playerManagement.TakeDamage(areaSwdSkillEffect);
            //GameManager.Instance.logUI.AddMessage($" {areaSwdSkillEffect} <color=red>Damage </color>�� �Ծ����ϴ�.");
        }
    }

    private void ApplyAreaMgSkillEffect(Collider player)
    {
           
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
            int toptalDamage = 10 + GameManager.Instance.PlayerData.Defensive;
            playerManagement.TakeDamage(toptalDamage);
            //GameManager.Instance.logUI.AddMessage($" {toptalDamage} <color=red>Damage </color>�� �Ծ����ϴ�.");
        }
    }
    // ���� �ð� �Ŀ� �浹�� �ٽ� Ȱ��ȭ
    private IEnumerator ResetCollisionCooldown()
    {
        yield return new WaitForSeconds(collisionCooldown);
        hasCollided = false;
    }
}

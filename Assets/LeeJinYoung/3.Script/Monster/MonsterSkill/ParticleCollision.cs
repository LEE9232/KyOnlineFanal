using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public BossSkillParticle bossSkillParticle;
    public int skillIndex; // 스킬의 종류를 구분할 인덱스
    public int damagePerSecond = 0; // 초당 데미지
    public int JumpAttackDamage = 350;
    public int areaSwdSkillEffect = 400;
    private bool hasCollided = false; // 충돌 여부를 저장하는 플래그
    private float collisionCooldown; // 충돌 쿨다운 시간
    private void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player") && !hasCollided) // 필요한 태그로 변경
        {
            hasCollided = true; // 충돌 발생 플래그 설정
            // 스킬에 따라 다른 처리를 할 수 있도록 분기
            switch (skillIndex)
            {
                case 0: // JumpDashAttack
                    ApplyJumpDashAttackEffect(other);
                    collisionCooldown = 3.0f; // 점프 대쉬 어택은 충돌 후 3초 대기
                    break;
                case 1: // AreaSwdSkill
                    ApplyAreaSwdSkillEffect(other);
                    collisionCooldown = 2.0f; // AreaSwdAttack는 충돌 후 2초 대기
                    break;
                case 2: //SecondSkill
                    ApplyAreaMgSkillEffect(other);
                    collisionCooldown = 1f; // AreaMgAttack는 충돌 후 1초 대기
                    break;
                // 다른 스킬 추가 가능
                default:
                    //Debug.LogWarning("인덱스: " + skillIndex);
                    break;
            }
            // 충돌 후 일정 시간 후에 다시 충돌 가능하도록 설정
            StartCoroutine(ResetCollisionCooldown());
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && skillIndex == 2)
        {
            // 매직 에리어 어택 스킬일 때만 초당 데미지 적용
            ApplyAreaMgSkillEffect(other);
        }
    }
    // 각 스킬에 따른 처리 로직을 함수로 나눔
    private void ApplyJumpDashAttackEffect(Collider player)
    {
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
            playerManagement.TakeDamage(JumpAttackDamage);
            //GameManager.Instance.logUI.AddMessage($" {JumpAttackDamage} <color=red>Damage </color>를 입었습니다.");
        }
    }
    private void ApplyAreaSwdSkillEffect(Collider player)
    {
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
           
            playerManagement.TakeDamage(areaSwdSkillEffect);
            //GameManager.Instance.logUI.AddMessage($" {areaSwdSkillEffect} <color=red>Damage </color>를 입었습니다.");
        }
    }

    private void ApplyAreaMgSkillEffect(Collider player)
    {
           
        PlayerManagement playerManagement = player.GetComponent<PlayerManagement>();
        if (playerManagement != null)
        {
            int toptalDamage = 10 + GameManager.Instance.PlayerData.Defensive;
            playerManagement.TakeDamage(toptalDamage);
            //GameManager.Instance.logUI.AddMessage($" {toptalDamage} <color=red>Damage </color>를 입었습니다.");
        }
    }
    // 일정 시간 후에 충돌을 다시 활성화
    private IEnumerator ResetCollisionCooldown()
    {
        yield return new WaitForSeconds(collisionCooldown);
        hasCollided = false;
    }
}

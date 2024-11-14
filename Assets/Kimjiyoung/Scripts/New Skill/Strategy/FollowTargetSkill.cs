using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FollowTargetSkill : IMagicSkills
{
    public string mSkillName { get; private set; }
    public float Cooldown { get; private set; }
    public int Damage { get; private set; }
    public int UseMP { get; private set; }
    //public int Level { get; private set; } = 1;
    public Sprite SkillImage { get; private set; }
    public Transform player { get; private set; }

    private GameObject skillPrefab;
    public bool isOnCooldown { get; private set; }


    private Transform shootPoint;
    private float fireballSpeed;

    private Transform targetMonster;
    public FollowTargetSkill(string mSkillName, float cooldown, int damage, int useMP, GameObject skillPrefab, bool isOnCooldown, float fireballSpeed, Transform shootPoint, Sprite skillImage, Transform player)
    {
        this.mSkillName = mSkillName;
        this.Cooldown = cooldown;
        this.Damage = damage;
        this.UseMP = useMP;
        this.skillPrefab = skillPrefab;
        //this.skillPrefab = skillPrefab;
        this.fireballSpeed = fireballSpeed;
        this.shootPoint = shootPoint;
        this.isOnCooldown = false;
        this.SkillImage = skillImage;
        this.player = player;
    }


    public void UseSkill(Transform target, Vector3 position, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        // 진영 추가 = 마나체크해서 스킬 발동여부 확인
        if (GameManager.Instance.PlayerData.MP < UseMP)
        {
            GameManager.Instance.logUI.AddMessage($"마나가 부족합니다.");
            return;
        }
        if (isOnCooldown)
        {
            Debug.Log($"{mSkillName} is on cooldown. Remaining time: {Cooldown}");
            return;
        }

        if (PlayerTargeting.Instance.targetMonster != null)
        {
            // 타겟 몬스터 방향을 바라봄
            targetMonster = PlayerTargeting.Instance.targetMonster;
            
            Transform skillTarget = PlayerTargeting.Instance.targetMonster.GetComponent<MonsterStatus>().targetPosition;
            
            Vector3 targetPosition = skillTarget.position;
            player.LookAt(targetPosition);

            // 진영 추가 =  스킬이 생성되면 마나가 줄어들어야함
            GameManager.Instance.PlayerData.MP -= UseMP;

            // 스킬 인스턴스 생성
            var skillInstance = GameObject.Instantiate(skillPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = skillInstance.GetComponent<Rigidbody>();

            // **SkillCollisionHandler에 데미지 설정**
            SkillCollisionHandler skillCollisionHandler = skillInstance.GetComponent<SkillCollisionHandler>();
            if (skillCollisionHandler != null)
            {
                // 진영 수정
                int damage = Damage + GameManager.Instance.PlayerData.Damage;
                skillCollisionHandler.SetDamage(damage);  // 스킬 데미지 설정
                skillCollisionHandler.SetTargetMonster(targetMonster);
            }

            // 타겟 방향으로 스킬 발사
            Vector3 direction = (targetPosition - shootPoint.position).normalized;
            rb.velocity = direction * fireballSpeed;

            // 타겟을 추적하는 로직 실행
            GameManager.Instance.StartCoroutine(UpdateSkillDirection(rb, skillTarget));

            // 스킬이 일정 시간이 지나면 파괴
            GameObject.Destroy(skillInstance, 4f);

            // 스킬 사용 후 쿨타임 시작
            //GameManager.Instance.StartCoroutine(StartCooldown(Cooldown, cooldownImage, cooldownText));

        }
        else
        {
            Debug.Log("No Target. Cannot use skill.");
        }
    }

    private IEnumerator UpdateSkillDirection(Rigidbody fireballRB, Transform target)
    {
        while (fireballRB != null)
        {
            if (target != null)
            {
                Vector3 targetDirection = (target.position - fireballRB.position).normalized;
                fireballRB.velocity = targetDirection * fireballSpeed;
            }
            else
            {
                break;
            }
            yield return null;
        }
    }

    private IEnumerator StartCooldown(float cooldown, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        if (isOnCooldown)
            yield break;
        // 레벨에 따른 쿨타임 감소 계산
        //float adjustedCooldown = cooldown - (skillLevel * CooldownReductionPerLevel);

        isOnCooldown = true;
        float remainingTime = cooldown;

        while (remainingTime >= 0)
        {
            remainingTime -= Time.deltaTime;
            if (cooldownImage != null)
                cooldownImage.fillAmount = remainingTime / cooldown;
            if (cooldownText != null)
                cooldownText.text = remainingTime.ToString("F1");
            yield return null;
        }

        // 쿨타임이 끝나면 UI 초기화
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
        if (cooldownText != null)
            cooldownText.text = "";
        isOnCooldown = false;
    }

    // 데미지 계산을 위한 메서드
    //public int CalculateDamage(int damage, int skillLevel)
    //{
    //    return damage + (skillLevel * DamageIncreasePerLevel);
    //}

    //private IEnumerator StartCooldown(float cooldown)
    //{
    //    isOnCooldown = true;
    //    yield return new WaitForSeconds(cooldown);
    //    isOnCooldown = false;
    //}
}

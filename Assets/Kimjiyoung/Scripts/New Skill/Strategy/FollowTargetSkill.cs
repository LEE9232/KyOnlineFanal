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
        // ���� �߰� = ����üũ�ؼ� ��ų �ߵ����� Ȯ��
        if (GameManager.Instance.PlayerData.MP < UseMP)
        {
            GameManager.Instance.logUI.AddMessage($"������ �����մϴ�.");
            return;
        }
        if (isOnCooldown)
        {
            Debug.Log($"{mSkillName} is on cooldown. Remaining time: {Cooldown}");
            return;
        }

        if (PlayerTargeting.Instance.targetMonster != null)
        {
            // Ÿ�� ���� ������ �ٶ�
            targetMonster = PlayerTargeting.Instance.targetMonster;
            
            Transform skillTarget = PlayerTargeting.Instance.targetMonster.GetComponent<MonsterStatus>().targetPosition;
            
            Vector3 targetPosition = skillTarget.position;
            player.LookAt(targetPosition);

            // ���� �߰� =  ��ų�� �����Ǹ� ������ �پ������
            GameManager.Instance.PlayerData.MP -= UseMP;

            // ��ų �ν��Ͻ� ����
            var skillInstance = GameObject.Instantiate(skillPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = skillInstance.GetComponent<Rigidbody>();

            // **SkillCollisionHandler�� ������ ����**
            SkillCollisionHandler skillCollisionHandler = skillInstance.GetComponent<SkillCollisionHandler>();
            if (skillCollisionHandler != null)
            {
                // ���� ����
                int damage = Damage + GameManager.Instance.PlayerData.Damage;
                skillCollisionHandler.SetDamage(damage);  // ��ų ������ ����
                skillCollisionHandler.SetTargetMonster(targetMonster);
            }

            // Ÿ�� �������� ��ų �߻�
            Vector3 direction = (targetPosition - shootPoint.position).normalized;
            rb.velocity = direction * fireballSpeed;

            // Ÿ���� �����ϴ� ���� ����
            GameManager.Instance.StartCoroutine(UpdateSkillDirection(rb, skillTarget));

            // ��ų�� ���� �ð��� ������ �ı�
            GameObject.Destroy(skillInstance, 4f);

            // ��ų ��� �� ��Ÿ�� ����
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
        // ������ ���� ��Ÿ�� ���� ���
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

        // ��Ÿ���� ������ UI �ʱ�ȭ
        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
        if (cooldownText != null)
            cooldownText.text = "";
        isOnCooldown = false;
    }

    // ������ ����� ���� �޼���
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

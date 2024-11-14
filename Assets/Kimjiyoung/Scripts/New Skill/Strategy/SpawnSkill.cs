using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnSkill : IMagicSkills
{


    public string mSkillName { get; private set; }

    public float Cooldown { get; private set; }

    public int Damage { get; private set; }

    public int UseMP { get; private set; }
    //public int Level { get; private set; } = 1;
    public GameObject skillPrefab { get; private set; }
    public bool isOnCooldown { get; private set; }
    public Transform player { get; private set; }
    public Sprite SkillImage { get; private set; }

    private Transform targetMonster;

    public SpawnSkill(string mSkillName, float cooldown, int damage, int useMP, GameObject skillPrefab, bool isOnCooldown, Transform player, Sprite skillImage)
    {
        this.mSkillName = mSkillName;
        this.Cooldown = cooldown;
        this.Damage = damage;
        this.UseMP = useMP;
        this.skillPrefab = skillPrefab;
        this.skillPrefab = skillPrefab;
        this.isOnCooldown = false;
        this.player = player;
        this.SkillImage = skillImage;
    }
    public void UseSkill(Transform target, Vector3 position, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        if (isOnCooldown)
        {
            Debug.Log($"{mSkillName} is on cooldown. Remaining time: {Cooldown}");
            return;
        }
        //if (target != null)
        if (PlayerTargeting.Instance.targetMonster != null)
        {
            targetMonster = PlayerTargeting.Instance.targetMonster; // 타겟 몬스터 저장
            //Vector3 targetPosition = target.position;
            //player.LookAt(PlayerTargeting.Instance.targetMonster);
            Transform skillTarget = PlayerTargeting.Instance.targetMonster;
            Vector3 targetPosition = skillTarget.position;
            player.LookAt(targetPosition);

            var skillInstance = GameObject.Instantiate(skillPrefab, targetPosition, Quaternion.identity);

            // SkillCollisionHandler에 데미지 설정 추가
            SkillCollisionHandler skillCollisionHandler = skillInstance.GetComponent<SkillCollisionHandler>();
            if (skillCollisionHandler != null)
            {
                skillCollisionHandler.SetDamage(Damage);  // 스킬의 데미지 설정
                skillCollisionHandler.SetTargetMonster(targetMonster);
                Debug.Log($"Monster hit by {mSkillName}, dealing {Damage} damage.");
            }

            GameObject.Destroy(skillInstance, 4f);

            //GameManager.Instance.StartCoroutine(StartCooldown(Cooldown, cooldownImage, cooldownText));
        }
        else
        {
            Debug.Log("No Target selected. Skill cannot be used.");
        }

    }

    private IEnumerator StartCooldown(float cooldown, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        if (isOnCooldown)
            yield break;

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

        if (cooldownImage != null)
            cooldownImage.fillAmount = 0;
        if (cooldownText != null)
            cooldownText.text = "";
        isOnCooldown = false;
    }

    //private IEnumerator StartCooldown(float cooldown)
    //{
    //    isOnCooldown = true;
    //    yield return new WaitForSeconds(cooldown);
    //    isOnCooldown = false;
    //}
}

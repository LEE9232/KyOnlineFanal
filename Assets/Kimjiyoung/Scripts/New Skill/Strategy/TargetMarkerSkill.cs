using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetMarkerSkill : IMagicSkills
{


    public string mSkillName { get; private set; }

    public float Cooldown { get; private set; }

    public int Damage { get; private set; }

    public int UseMP { get; private set; }
    //public int Level { get; private set; } = 1;

    public GameObject skillPrefab { get; private set; }
    public bool isOnCooldown { get; private set; }
    public float maxDistance { get; private set; }
    public float SpawnHight { get; private set; }
    public float destroyTime { get; private set; }
    public Sprite SkillImage { get; private set; }


    public bool isOnMarker { get; private set; }
    private GameObject TargetMarker;
    private Vector3 finalPosition;

    private float targetSkillRadius = 8f;

    public TargetMarkerSkill(string mSkillName, float cooldown, int damage, int useMP, GameObject skillPrefab, bool isOnCooldown, float maxDistance, float spawnHight, float destroyTime, GameObject TargetMarker, bool isOnMarker, Sprite skillImage)
    {
        this.mSkillName = mSkillName;
        this.Cooldown = cooldown;
        this.Damage = damage;
        this.UseMP = useMP;
        this.skillPrefab = skillPrefab;
        this.isOnCooldown = false;
        this.maxDistance = maxDistance;
        this.SpawnHight = spawnHight;
        this.destroyTime = destroyTime;
        this.TargetMarker = TargetMarker;
        this.SkillImage = skillImage;
        this.isOnMarker = isOnMarker;
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

        Debug.Log("타겟마커 스킬 발동");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = hit.point;
            //finalPosition.y = SpawnHight;
            finalPosition.y = hit.point.y + SpawnHight;
            //Debug.Log($"Hit point: {hit.point}, Final Position: {finalPosition}");
        }
        else
        {
            Vector3 direction = (Camera.main.transform.forward).normalized;
            finalPosition = Camera.main.transform.position + direction * maxDistance;
            //finalPosition.y += finalPosition.y + SpawnHight;
            //finalPosition.y = SpawnHight;
            finalPosition.y = Camera.main.transform.position.y + SpawnHight; // 기본 높이
            //Debug.Log($"Default position: {finalPosition}");
        }

        float distance = Vector3.Distance(Camera.main.transform.position, finalPosition);
        if (distance > maxDistance)
        {
            Debug.LogWarning("스킬 위치가 최대 거리 범위를 초과했습니다.");
            return;
        }

        if (target != null)
        {
            Vector3 lookDirection = (finalPosition - target.position).normalized;
            lookDirection.y = 0;
            target.rotation = Quaternion.LookRotation(lookDirection);
        }
        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);

        // 진영 추가 =  스킬이 생성되면 마나가 줄어들어야함
        GameManager.Instance.PlayerData.MP -= UseMP; 
        // 스킬 생성
        var skillInstance = GameObject.Instantiate(skillPrefab, finalPosition, Quaternion.identity);
        Collider[] hitColliders = Physics.OverlapSphere(finalPosition, targetSkillRadius);
        foreach (Collider hitCollider in hitColliders)
        {
            MonsterStatus monsterStatus = hitCollider.GetComponent<MonsterStatus>();
               
            if (monsterStatus != null)
            {
                int damage = Damage + GameManager.Instance.PlayerData.Damage;
                monsterStatus.TakeDamage(damage);
                Debug.Log($"Monster hit by {mSkillName}, dealing {damage} damage.");
            }
        }
        GameObject.Destroy(skillInstance, destroyTime);

        // 쿨다운 시작 및 타겟 마커 비활성화
        TargetMarker.SetActive(false);
        isOnMarker = false;
        GameManager.Instance.StartCoroutine(StartCooldown(Cooldown, cooldownImage, cooldownText));
        //}
    }

    public void ActivateTargetMarker()
    {
        if (TargetMarker != null)
        {
            Debug.Log("ActivateTargetMarker");
            isOnMarker = true;
            TargetMarker.SetActive(true);
            UpdateTargetMarker();
            GameManager.Instance.StartCoroutine(UpdateTargetMarkerPosition());
        }
    }

    public void CancelTargetMarker()
    {
        if (isOnMarker)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {

            }
        }
    }

    //private void Update()
    //{
    //    if (TargetMarker != null && TargetMarker.activeSelf)
    //    {
    //        UpdateTargetMarker(); // 타겟 마커가 활성화되는 동안 마우스를 따라다니도록 업데이트

    //        if (Input.GetKeyDown(KeyCode.Escape)) // Escape 키로 타겟 마커 비활성화
    //        {
    //            TargetMarker.SetActive(false);
    //        }
    //    }
    //}

    private IEnumerator UpdateTargetMarkerPosition()
    {
        while (isOnMarker)
        {
            //Debug.Log("Updating Target Marker Position");
            UpdateTargetMarker();
            yield return null;
        }
    }

    public void UpdateTargetMarker()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Vector3 finalPosition;

        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = hit.point;
            //finalPosition.y = 0.1f;
            finalPosition.y = hit.point.y + 0.2f;
            //Debug.Log($"Hit point: {hit.point}, Final Position: {finalPosition}");
        }
        else
        {
            Debug.LogWarning("Raycast failed");
            Vector3 direction = Camera.main.transform.forward.normalized;
            finalPosition = Camera.main.transform.position + direction * maxDistance;
            //finalPosition.y = 1f;
            finalPosition.y = Camera.main.transform.position.y + 1f; // 기본 높이 설정
            //Debug.Log($"Raycast failed, using default position: {finalPosition}");
        }

        float distance = Vector3.Distance(Camera.main.transform.position, finalPosition);
        // 최대 거리 체크
        if (distance > maxDistance)
        {
            // 최대 거리 내에서 위치 조정
            Vector3 direction = (finalPosition - Camera.main.transform.position).normalized;
            finalPosition = Camera.main.transform.position + direction * maxDistance;
            finalPosition.y = hit.point.y + 0.1f;
        }
        TargetMarker.transform.position = finalPosition;

    }

    private IEnumerator StartCooldown(float cooldown, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        if (isOnCooldown)
            yield break;

        isOnCooldown = true;
        float remainingTime = cooldown;
        // 쿨타임 동안 이미지와 텍스트 업데이트
        while (remainingTime >= 0)
        {
            //Debug.Log("시각 요소 쿨타임");
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

    //private IEnumerator StartCooldown(float cooldown)
    //{
    //    Debug.Log("타겟마커 스킬 쿨타임");
    //    isOnCooldown = true;
    //    yield return new WaitForSeconds(cooldown);
    //    isOnCooldown = false;
    //}
}

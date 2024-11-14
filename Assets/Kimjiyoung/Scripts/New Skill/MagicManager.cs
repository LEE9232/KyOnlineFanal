using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour
{
    private Dictionary<int, bool> skillCooldonws = new Dictionary<int, bool>();
    private Dictionary<int, MSkillSlot> skillSlots = new Dictionary<int, MSkillSlot>();
    private Dictionary<int, int> skillToSlotMapping = new Dictionary<int, int>();

    public List<IMagicSkills> mSkills;
    private IMagicSkills currentSkill;  // 현재 선택된 스킬
    private TargetMarkerSkill markerSkill;

    [SerializeField] private List<GameObject> skillPrefabs;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject TargetMarker;
    [SerializeField] public List<Sprite> skillImage;
    [SerializeField] public List<GameObject> skillImages; // 스킬 이미지들이 있는 GameObject 리스트

    private Animator skillAnim;

    private void Start()
    {
        mSkills = new List<IMagicSkills>
        {
            // 파이어볼(단일)
            new FollowTargetSkill
            ("FireBall", 3f, 20, 20, skillPrefabs[0], false, 30f, shootPoint, skillImage[0],transform),

            new FollowTargetSkill
            ("Meteor", 2.75f, 75, 40, skillPrefabs[1], false, 30f, shootPoint, skillImage[1], transform),

            new FollowTargetSkill
            ("Judgment of Flames", 2.15f, 215, 80, skillPrefabs[2], false, 30f, shootPoint, skillImage[2], transform),

            // 아이스 스피어(범위)
            new TargetMarkerSkill
            ("Ice Rain", 2.8f, 10, 15, skillPrefabs[3], false, 25f, 5f, 4.5f, TargetMarker, false, skillImage[3]),

            new TargetMarkerSkill
            ("Ice Spear Rain", 2.4f, 35, 20, skillPrefabs[4], false, 25f, 5f, 4f, TargetMarker, false, skillImage[4]),

            new TargetMarkerSkill
            ("Ice Pillar", 1.5f, 130, 40, skillPrefabs[5], false, 25f, 0.2f, 3f, TargetMarker, false, skillImage[5]),

            // 단일 극딜
            new SpawnSkill
            ("The Judge", 15f, 400, 300, skillPrefabs[6], false, transform, skillImage[6]),

            // 범위 극딜
            new TargetMarkerSkill
            ("Forbidden Tome", 10f, 250, 350, skillPrefabs[7], false, 25f, 4f, 4f, TargetMarker, false, skillImage[7])
        };

        for (int i = 0; i < mSkills.Count; i++)
        {
            skillCooldonws[i] = false;
        }
        skillAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        // 타겟 마커가 활성화된 경우 실시간으로 마우스를 따라가도록 업데이트
        if (markerSkill != null && markerSkill.isOnMarker)
        {
            int skillIndex = mSkills.IndexOf(markerSkill); // markerSkill의 인덱스를 가져옴
            markerSkill.UpdateTargetMarker();  // 마커가 활성화된 동안 마우스 위치를 계속 추적
                                               //Debug.Log("타겟마커 업데이트");
            if (Input.GetMouseButtonDown(0))
            {
                if (currentSkill != null && currentSkill is TargetMarkerSkill)
                {
                    if (skillToSlotMapping.TryGetValue(skillIndex, out int slotIndex) && skillSlots.TryGetValue(slotIndex, out MSkillSlot slot))
                    {
                        Vector3 markerPositon = GetMousePosition();
                        markerSkill.UseSkill(null, markerPositon, skillIndex, slot.cooldownImage, slot.cooldownText);
                        StartCoroutine(StartCooldown(skillIndex, markerSkill.Cooldown, slot.cooldownImage, slot.cooldownText));
                        //markerSkill.DeactivateTargetMarker();
                        this.markerSkill = null;
                    }
                }
            }
        }
    }

    public void UseSkill(int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        if (skillIndex >= 0 && skillIndex < mSkills.Count)
        {
            // 쿨타임 중이면 스킬을 사용할 수 없음
            if (skillCooldonws[skillIndex])
            {
                Debug.LogWarning($"{mSkills[skillIndex].mSkillName} 스킬은 아직 쿨타임 중입니다.");
                return;  // 쿨타임 중이므로 애니메이션도 실행하지 않음
            }

            currentSkill = mSkills[skillIndex];

            //// FollowTargetSkill일 경우의 애니메이션
            //if (currentSkill is FollowTargetSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseFollowSkill");  // Follow 스킬 애니메이션
            //    }
            //}
            //// SpawnSkill일 경우의 애니메이션
            //else if (currentSkill is SpawnSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseSpawnSkill");  // Spawn 스킬 애니메이션
            //    }
            //}
            //else if (currentSkill is TargetMarkerSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseTargetMarkerSkill"); // TargetMarker 스킬 애니메이션
            //    }
            //}

            // FollowTargetSkill 및 SpawnSkill 로직
            int damage = currentSkill.Damage;

            if (currentSkill is FollowTargetSkill || currentSkill is SpawnSkill)
            {
                //Transform target = FindObjectOfType<PlayerTargeting>().transform;
                Transform target = PlayerTargeting.Instance.targetMonster;
                if (skillToSlotMapping.TryGetValue(skillIndex, out int slotIndex) && skillSlots.TryGetValue(slotIndex, out MSkillSlot slot))
                {
                    currentSkill.UseSkill(target, target.position, skillIndex, cooldownImage, cooldownText);

                    StartCoroutine(StartCooldown(skillIndex, currentSkill.Cooldown, cooldownImage, cooldownText));
                    //StartCoroutine(StartCooldown(skillIndex, currentSkill.Cooldown, cooldownImage, cooldownText));
                }
                else
                {
                    Debug.LogWarning("타겟이 없습니다. 스킬을 사용할 수 없습니다.");
                }
            }
            // TargetMarkerSkill 로직
            else if (currentSkill is TargetMarkerSkill markerSkill)
            {
                this.markerSkill = markerSkill;
                markerSkill.ActivateTargetMarker();

                StartCoroutine(WaitForMarkerAndCast(markerSkill, skillIndex, cooldownImage, cooldownText));
            }
        }
    }

    public void RegisterSkillSlot(int slotIndex, MSkillSlot slot)
    {
        skillSlots[slotIndex] = slot;
    }

    public void UpdateSkillAssignment(int slotIndex, int skillIndex)
    {
        skillToSlotMapping[skillIndex] = slotIndex;
    }

    private IEnumerator StartCooldown(int skillIndex, float cooldown, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        skillCooldonws[skillIndex] = true;
        // 스킬별로 애니메이션 실행
        if (currentSkill is FollowTargetSkill)
        {
            skillAnim.SetTrigger("UseFollowSkill");  // Follow 스킬 애니메이션
        }
        else if (currentSkill is SpawnSkill)
        {
            skillAnim.SetTrigger("UseSpawnSkill");  // Spawn 스킬 애니메이션
        }
        else if (currentSkill is TargetMarkerSkill)
        {
            skillAnim.SetTrigger("UseTargetMarkerSkill"); // TargetMarker 스킬 애니메이션
        }
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
        skillCooldonws[skillIndex] = false;
        
    }

    // 타겟 마커 스킬의 경우 마우스 클릭 대기 후 발동
    private IEnumerator WaitForMarkerAndCast(TargetMarkerSkill markerSkill, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        // 마우스 클릭을 대기
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // 마우스 클릭 후 스킬 발동
        Vector3 markerPosition = GetMousePosition();
        markerSkill.UseSkill(null, markerPosition, skillIndex, cooldownImage, cooldownText);

        // 스킬 인스턴스 생성 및 SkillCollisionHandler에 데미지 전달
        //var skillInstance = GameObject.Instantiate(currentSkill.GetSkillPrefab(), markerPosition, Quaternion.identity);
        //var skillCollisionHandler = skillInstance.GetComponent<SkillCollisionHandler>();


        StartCoroutine(StartCooldown(skillIndex, currentSkill.Cooldown, cooldownImage, cooldownText));
    }

    // 마우스 위치를 통해 타겟 마커 위치 반환
    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 markerPosition = hit.point;
            markerPosition.y += 0.1f;
            return markerPosition;
        }
        else
        {
            Debug.LogWarning("Raycast 실패: 카메라 앞 방향으로 최대 거리 설정");
            Vector3 direction = Camera.main.transform.forward.normalized;
            return Camera.main.transform.position + direction * 10f;  // 임의 거리
        }
    }

}
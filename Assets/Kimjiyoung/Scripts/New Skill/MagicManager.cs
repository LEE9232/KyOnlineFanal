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
    private IMagicSkills currentSkill;  // ���� ���õ� ��ų
    private TargetMarkerSkill markerSkill;

    [SerializeField] private List<GameObject> skillPrefabs;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject TargetMarker;
    [SerializeField] public List<Sprite> skillImage;
    [SerializeField] public List<GameObject> skillImages; // ��ų �̹������� �ִ� GameObject ����Ʈ

    private Animator skillAnim;

    private void Start()
    {
        mSkills = new List<IMagicSkills>
        {
            // ���̾(����)
            new FollowTargetSkill
            ("FireBall", 3f, 20, 20, skillPrefabs[0], false, 30f, shootPoint, skillImage[0],transform),

            new FollowTargetSkill
            ("Meteor", 2.75f, 75, 40, skillPrefabs[1], false, 30f, shootPoint, skillImage[1], transform),

            new FollowTargetSkill
            ("Judgment of Flames", 2.15f, 215, 80, skillPrefabs[2], false, 30f, shootPoint, skillImage[2], transform),

            // ���̽� ���Ǿ�(����)
            new TargetMarkerSkill
            ("Ice Rain", 2.8f, 10, 15, skillPrefabs[3], false, 25f, 5f, 4.5f, TargetMarker, false, skillImage[3]),

            new TargetMarkerSkill
            ("Ice Spear Rain", 2.4f, 35, 20, skillPrefabs[4], false, 25f, 5f, 4f, TargetMarker, false, skillImage[4]),

            new TargetMarkerSkill
            ("Ice Pillar", 1.5f, 130, 40, skillPrefabs[5], false, 25f, 0.2f, 3f, TargetMarker, false, skillImage[5]),

            // ���� �ص�
            new SpawnSkill
            ("The Judge", 15f, 400, 300, skillPrefabs[6], false, transform, skillImage[6]),

            // ���� �ص�
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
        // Ÿ�� ��Ŀ�� Ȱ��ȭ�� ��� �ǽð����� ���콺�� ���󰡵��� ������Ʈ
        if (markerSkill != null && markerSkill.isOnMarker)
        {
            int skillIndex = mSkills.IndexOf(markerSkill); // markerSkill�� �ε����� ������
            markerSkill.UpdateTargetMarker();  // ��Ŀ�� Ȱ��ȭ�� ���� ���콺 ��ġ�� ��� ����
                                               //Debug.Log("Ÿ�ٸ�Ŀ ������Ʈ");
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
            // ��Ÿ�� ���̸� ��ų�� ����� �� ����
            if (skillCooldonws[skillIndex])
            {
                Debug.LogWarning($"{mSkills[skillIndex].mSkillName} ��ų�� ���� ��Ÿ�� ���Դϴ�.");
                return;  // ��Ÿ�� ���̹Ƿ� �ִϸ��̼ǵ� �������� ����
            }

            currentSkill = mSkills[skillIndex];

            //// FollowTargetSkill�� ����� �ִϸ��̼�
            //if (currentSkill is FollowTargetSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseFollowSkill");  // Follow ��ų �ִϸ��̼�
            //    }
            //}
            //// SpawnSkill�� ����� �ִϸ��̼�
            //else if (currentSkill is SpawnSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseSpawnSkill");  // Spawn ��ų �ִϸ��̼�
            //    }
            //}
            //else if (currentSkill is TargetMarkerSkill)
            //{
            //    if (skillAnim != null)
            //    {
            //        skillAnim.SetTrigger("UseTargetMarkerSkill"); // TargetMarker ��ų �ִϸ��̼�
            //    }
            //}

            // FollowTargetSkill �� SpawnSkill ����
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
                    Debug.LogWarning("Ÿ���� �����ϴ�. ��ų�� ����� �� �����ϴ�.");
                }
            }
            // TargetMarkerSkill ����
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
        // ��ų���� �ִϸ��̼� ����
        if (currentSkill is FollowTargetSkill)
        {
            skillAnim.SetTrigger("UseFollowSkill");  // Follow ��ų �ִϸ��̼�
        }
        else if (currentSkill is SpawnSkill)
        {
            skillAnim.SetTrigger("UseSpawnSkill");  // Spawn ��ų �ִϸ��̼�
        }
        else if (currentSkill is TargetMarkerSkill)
        {
            skillAnim.SetTrigger("UseTargetMarkerSkill"); // TargetMarker ��ų �ִϸ��̼�
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

    // Ÿ�� ��Ŀ ��ų�� ��� ���콺 Ŭ�� ��� �� �ߵ�
    private IEnumerator WaitForMarkerAndCast(TargetMarkerSkill markerSkill, int skillIndex, Image cooldownImage, TextMeshProUGUI cooldownText)
    {
        // ���콺 Ŭ���� ���
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        // ���콺 Ŭ�� �� ��ų �ߵ�
        Vector3 markerPosition = GetMousePosition();
        markerSkill.UseSkill(null, markerPosition, skillIndex, cooldownImage, cooldownText);

        // ��ų �ν��Ͻ� ���� �� SkillCollisionHandler�� ������ ����
        //var skillInstance = GameObject.Instantiate(currentSkill.GetSkillPrefab(), markerPosition, Quaternion.identity);
        //var skillCollisionHandler = skillInstance.GetComponent<SkillCollisionHandler>();


        StartCoroutine(StartCooldown(skillIndex, currentSkill.Cooldown, cooldownImage, cooldownText));
    }

    // ���콺 ��ġ�� ���� Ÿ�� ��Ŀ ��ġ ��ȯ
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
            Debug.LogWarning("Raycast ����: ī�޶� �� �������� �ִ� �Ÿ� ����");
            Vector3 direction = Camera.main.transform.forward.normalized;
            return Camera.main.transform.position + direction * 10f;  // ���� �Ÿ�
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    //private PlayerTargeting playerTargeting;
    public MagicManager magicManager;
    public KeyCode key1 = KeyCode.Alpha1;
    public KeyCode key2 = KeyCode.Alpha2;
    public KeyCode key3 = KeyCode.Alpha3;
    public KeyCode key4 = KeyCode.Alpha4;

    private List<bool> isSkill = new List<bool> { false, false, false, false, false, false, false, false };
    public List<GameObject> SkillEffect = new List<GameObject>();

    #region FireBall
    public Transform shootPoint;
    public float fireballSpeed = 20f;
    #endregion

    #region Ice Spear
    public GameObject TargetMarker;
    private bool isTargetMarker = false;
    private Vector3 finalPosition;
    public float maxDistance = 15f; //�÷��̾�� ���콺Ŀ���� �ִ� �Ÿ�

    #endregion


    //public Cursor mousMarker;

    private void Awake()
    {
        //playerTargeting = GetComponent<PlayerTargeting>();
        magicManager = GetComponent<MagicManager>();
    }

    private void Start()
    {
        if (TargetMarker != null)
        {
            TargetMarker.SetActive(false);
        }
    }

    public void Update()
    {
        //if (Input.GetKeyDown(key1))
        //{
        //    Debug.Log("ȣ��");
        //    //magicManager.
        //    //SingleSkill(6);
        //    magicManager.UseFollowSkill(2);
        //}
        //if (Input.GetKeyDown(key2))
        //{
        //    Debug.Log("ȣ��");
        //    //ActivateTargetMarker();
        //    //WideSkill(7);
        //    magicManager.UseSpawnSkill(6);
        //}
        //if (Input.GetKeyDown(key3))
        //{
        //    Debug.Log("ȣ��");
        //    //ActivateTargetMarker();
        //    //UseiceSpear1(2);
        //    magicManager.UseTargetMarkerSkill(4);

        //}
        //if (Input.GetKeyDown(key4))
        //{
        //    Debug.Log("ȣ��");
        //    ActivateTargetMarker();
        //}
        if (isTargetMarker)
        {
            UpdateTargetMarker();

            if (Input.GetMouseButtonDown(0))
            {
                LookMarker();

                TargetMarker.SetActive(false);
                isTargetMarker = false;

                //WideSkill(7);
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TargetMarker.SetActive(false);
                isTargetMarker = false;
                Cursor.lockState = CursorLockMode.None; // Ŀ�� ��� ����
            }
            //if (Input.GetKeyDown(KeyCode.Escape))
            //{
            //    TargetMarker.SetActive(false);
            //    isTargetMarker = false;
            //}
        }
    }

    private void UseFireballSkill(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {
            if (PlayerTargeting.Instance.targetMonster != null)
            {
                isSkill[skillIndex] = true;

                // ��ü ���� Ÿ�� ���͸� �ٶ󺸵��� ����
                Vector3 targetPosition = PlayerTargeting.Instance.targetMonster.position;
                transform.LookAt(targetPosition); // ���⼭ �� ��ü�� ���͸� �ٶ�

                var fireball1Instance = Instantiate(SkillEffect[0], shootPoint.position, shootPoint.rotation);
                Rigidbody rb = fireball1Instance.GetComponent<Rigidbody>();

                Vector3 direction = (targetPosition - shootPoint.position).normalized;
                rb.velocity = direction * fireballSpeed;

                StartCoroutine(UpdateFireballDirection(rb));

                Destroy(fireball1Instance, 4f);
                StartCoroutine(SkillCoolTime(3f, skillIndex));
            }
            else
            {
                Debug.Log("No Target. You Can't Use Skill");
            }
            //if (PlayerTargeting.Instance.targetMonster != null)
            //{
            //    isSkill[skillIndex] = true;

            //    Vector3 targetPosition = PlayerTargeting.Instance.targetMonster.position;
            //    var fireball1Instance = Instantiate(SkillEffect[0], shootPoint.position, shootPoint.rotation);
            //    Rigidbody rb = fireball1Instance.GetComponent<Rigidbody>();

            //    Vector3 direction = (targetPosition - shootPoint.position).normalized;
            //    rb.velocity = direction * fireballSpeed;

            //    StartCoroutine(UpdateFireballDirection(rb));

            //    Destroy(fireball1Instance, 4f);
            //    StartCoroutine(SkillCoolTime(3f, skillIndex));
            //}
            //else
            //{
            //    Debug.Log("No Target. You Can't Use Skill");
            //}
        }
    }

    // ���̾�� ������ ���͸� ���� ������Ʈ
    private IEnumerator UpdateFireballDirection(Rigidbody fireballRb)
    {
        while (fireballRb != null && PlayerTargeting.Instance.targetMonster != null)
        {
            Vector3 targetDirection = (PlayerTargeting.Instance.targetMonster.position - fireballRb.position).normalized;
            fireballRb.velocity = targetDirection * fireballSpeed;

            yield return null; // ���� �����ӱ��� ���
        }
    }

    private void UseFireballSkill2(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {
            if (PlayerTargeting.Instance.targetMonster != null)
            {
                isSkill[skillIndex] = true;

                // ��ü ���� Ÿ�� ���͸� �ٶ󺸵��� ����
                Vector3 targetPosition = PlayerTargeting.Instance.targetMonster.position;
                transform.LookAt(targetPosition); // ���⼭ �� ��ü�� ���͸� �ٶ�

                var fireball1Instance = Instantiate(SkillEffect[1], shootPoint.position, shootPoint.rotation);
                Rigidbody rb = fireball1Instance.GetComponent<Rigidbody>();

                Vector3 direction = (targetPosition - shootPoint.position).normalized;
                rb.velocity = direction * fireballSpeed;

                StartCoroutine(UpdateFireballDirection(rb));

                Destroy(fireball1Instance, 4f);
                StartCoroutine(SkillCoolTime(3f, skillIndex));
            }
            else
            {
                Debug.Log("No Target. You Can't Use Skill");
            }
        }
    }

    private void UseFireballSkill3(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {
            if (PlayerTargeting.Instance.targetMonster != null)
            {
                isSkill[skillIndex] = true;

                // ��ü ���� Ÿ�� ���͸� �ٶ󺸵��� ����
                Vector3 targetPosition = PlayerTargeting.Instance.targetMonster.position;
                transform.LookAt(targetPosition); // ���⼭ �� ��ü�� ���͸� �ٶ�

                var fireball1Instance = Instantiate(SkillEffect[2], shootPoint.position, shootPoint.rotation);
                Rigidbody rb = fireball1Instance.GetComponent<Rigidbody>();

                Vector3 direction = (targetPosition - shootPoint.position).normalized;
                rb.velocity = direction * fireballSpeed;

                StartCoroutine(UpdateFireballDirection(rb));

                Destroy(fireball1Instance, 4f);
                StartCoroutine(SkillCoolTime(3f, skillIndex));
            }
            else
            {
                Debug.Log("No Target. You Can't Use Skill");
            }
        }
    }

    private void UseiceSpear1(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
            if (Physics.Raycast(ray, out hit))
            {
                finalPosition = hit.point; // ����ĳ��Ʈ�� ���� ������ ���� ��ǥ
                finalPosition.y = 5f; // �ʿ��� ���̷� ����
            }
            else
            {
                Vector3 direction = (Camera.main.transform.forward).normalized;
                finalPosition = transform.position + direction * maxDistance;
            }
            //��ų �Ÿ��� �ִ���̸� ����
            float distance = Vector3.Distance(transform.position, finalPosition);
            if (distance > maxDistance)
            {
                Debug.LogWarning("��ų ��ġ�� �ִ� �Ÿ� ������ �ʰ��߽��ϴ�.");
                return;
            }

            isSkill[skillIndex] = true;
            var fireball1Instance = Instantiate(SkillEffect[3], finalPosition, Quaternion.identity);
            Destroy(fireball1Instance, 4f);
            StartCoroutine(SkillCoolTime(3f, skillIndex));
        }
    }

    private void UseiceSpear2(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
            if (Physics.Raycast(ray, out hit))
            {
                finalPosition = hit.point; // ����ĳ��Ʈ�� ���� ������ ���� ��ǥ
                finalPosition.y = 5f; // �ʿ��� ���̷� ����
            }
            else
            {
                Vector3 direction = (Camera.main.transform.forward).normalized;
                finalPosition = transform.position + direction * maxDistance;
            }
            //��ų �Ÿ��� �ִ���̸� ����
            float distance = Vector3.Distance(transform.position, finalPosition);
            if (distance > maxDistance)
            {
                Debug.LogWarning("��ų ��ġ�� �ִ� �Ÿ� ������ �ʰ��߽��ϴ�.");
                return;
            }

            isSkill[skillIndex] = true;
            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            var fireball1Instance = Instantiate(SkillEffect[4], finalPosition, Quaternion.identity);
            Destroy(fireball1Instance, 4f);
            StartCoroutine(SkillCoolTime(3f, skillIndex));
        }
    }

    private void UseiceSpear3(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // ����ĳ��Ʈ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
            if (Physics.Raycast(ray, out hit))
            {
                finalPosition = hit.point; // ����ĳ��Ʈ�� ���� ������ ���� ��ǥ
                finalPosition.y = 0.2f; // �ʿ��� ���̷� ����
            }
            else
            {
                Vector3 direction = (Camera.main.transform.forward).normalized;
                finalPosition = transform.position + direction * maxDistance;
            }
            //��ų �Ÿ��� �ִ���̸� ����
            float distance = Vector3.Distance(transform.position, finalPosition);
            if (distance > maxDistance)
            {
                Debug.LogWarning("��ų ��ġ�� �ִ� �Ÿ� ������ �ʰ��߽��ϴ�.");
                return;
            }

            isSkill[skillIndex] = true;

            // ī�޶� �������� ȸ�� ����
            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            var fireball1Instance = Instantiate(SkillEffect[5], finalPosition, Quaternion.identity);
            Destroy(fireball1Instance, 4f);
            StartCoroutine(SkillCoolTime(3f, skillIndex));
        }
    }


    private void SingleSkill(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {
            if (PlayerTargeting.Instance.targetMonster != null)
            {
                isSkill[skillIndex] = true;

                Vector3 targetPosition = PlayerTargeting.Instance.targetPoition.position;
                transform.LookAt(targetPosition);

                var singleSkill = Instantiate(SkillEffect[6], targetPosition, Quaternion.identity);

                Destroy(singleSkill, 4.5f);
                StartCoroutine(SkillCoolTime(3f, skillIndex));
            }
            else
            {
                Debug.Log("No Target. You Can't Use Skill");
            }

        }
    }

    private void WideSkill(int skillIndex)
    {
        if (!isSkill[skillIndex])
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                finalPosition = hit.point; // ����ĳ��Ʈ�� ���� ������ ���� ��ǥ
                finalPosition.y = 4f; // �ʿ��� ���̷� ����
            }
            else
            {
                Vector3 direction = (Camera.main.transform.forward).normalized;
                finalPosition = transform.position + direction * maxDistance;
            }
            float distance = Vector3.Distance(transform.position, finalPosition);
            if (distance > maxDistance)
            {
                Debug.LogWarning("��ų ��ġ�� �ִ� �Ÿ� ������ �ʰ��߽��ϴ�.");
                return;
            }

            isSkill[skillIndex] = true;

            // ī�޶� �������� ȸ�� ����
            Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            var fireball1Instance = Instantiate(SkillEffect[7], finalPosition, Quaternion.identity);
            Destroy(fireball1Instance, 4f);
            StartCoroutine(SkillCoolTime(3f, skillIndex));
        }
    }


    public IEnumerator SkillCoolTime(float cooltime1, int skillIndex)
    {
        yield return new WaitForSeconds(cooltime1);
        print("��Ÿ�� ��");
        isSkill[skillIndex] = false;
    }

    private void ActivateTargetMarker()
    {
        if (TargetMarker != null)
        {
            isTargetMarker = true;
            UpdateTargetMarker();
            TargetMarker.SetActive(true);
        }
    }


    public void UpdateTargetMarker()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // ����ĳ��Ʈ�� ȭ�鿡�� ���� ��ǥ�� ��ȯ
        if (Physics.Raycast(ray, out hit))
        {
            finalPosition = hit.point; // ����ĳ��Ʈ�� ���� ������ ���� ��ǥ
            finalPosition.y = 0.1f; // �ʿ��� ���̷� ����
        }
        else
        {
            Vector3 direction = (Camera.main.transform.forward).normalized;
            finalPosition = transform.position + direction * maxDistance;
        }

        float distance = Vector3.Distance(transform.position, finalPosition);

        // �ִ� �Ÿ� üũ
        if (distance > maxDistance)
        {
            // �ִ� �Ÿ� ������ ��ġ ����
            Vector3 direction = (finalPosition - transform.position).normalized;
            finalPosition = transform.position + direction * maxDistance;
        }
        TargetMarker.transform.position = finalPosition;
    }

    private void LookMarker()
    {
        Vector3 targetDirection = (finalPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = lookRotation;
    }

}

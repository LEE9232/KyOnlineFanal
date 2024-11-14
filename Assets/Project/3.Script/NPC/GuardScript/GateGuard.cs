using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GateGuard : MonoBehaviour
{
    public Animator Anim;
    public Transform guardPosition; // ����� ��ų ��ġ
    public float detectionRange = 10f; // ����� ������ ����
    public float attackRange = 3f; // ����� ������ ����
    public float attackInterval = 1.5f; // ���� ����
    public int guardDamage = 200; // ����� ���ݷ�
    private NavMeshAgent agent;
    private GameObject currentTarget; // ���� ���� ���� ����
    private float attackCooldown = 0f; // ���� ��� �ð�
    private bool isReturningToGuard = false; // ����� ���� ��ġ�� ���� ������ ����
    private MinimapIconManager minimapIconManager;  // MinimapIconManager ����
    public TextMeshProUGUI popupText;
    private List<string> popchat = new List<string>();
    public GameObject popUI;
    private bool hasDisplayedPopup = false; // �˾��� ǥ�õǾ����� ����

    private void Start()
    {
        Anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        ReturnToGuardPosition(); // ó������ ������ ��ġ�� �̵�
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.NPCNoQuest);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }

        // �˾�
        popchat.Add("���Ͱ� �����ؿԴ�!!!\n��� �����ض�!");
        popchat.Add("������ ���Ѷ�!!!\n���͸� ���ƶ�!!");
        popchat.Add("���⼭���� ���ƾ��Ѵ�!!\n��θ� ���Ѷ�!!");
        //popchat.Add("���� �ڸ��� ���� �Ѵ�!!");
    }

    private void Update()
    {
        if (currentTarget == null)
        {
            // ���� ���� �� ���� Ž��
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange, LayerMask.GetMask("Monster"));
            if (hits.Length > 0)
            {
                currentTarget = hits[0].gameObject; // ù ��°�� ������ ���� ����
                agent.SetDestination(currentTarget.transform.position);
                Anim.SetBool("Running", true);
                if (!hasDisplayedPopup) // �˾��� ǥ�õ��� �ʾҴٸ�
                {
                    popUI.SetActive(true);
                    int randomIndex = Random.Range(0, popchat.Count);
                    popupText.text = popchat[randomIndex]; // ���� �ؽ�Ʈ ���
                    hasDisplayedPopup = true; // �˾��� �� ���� ǥ�õǵ��� ����
                }
            }
            else if (!isReturningToGuard)
            {
                // ���Ͱ� ������ ���� ��ġ�� ����
                ReturnToGuardPosition();
                Anim.SetBool("Walking", true);
            }
        }
        else
        {
            // ����� ���͸� ���� ���� ��
            float distanceToTarget = Vector3.Distance(transform.position, currentTarget.transform.position);
            MonsterStatus monsterStatus = currentTarget.GetComponent<MonsterStatus>();

            if (distanceToTarget <= attackRange)
            {
                AttackTarget();
            }
            else
            {
                // ���� �������� ����� �ʾҴٸ� ��� ����
                agent.SetDestination(currentTarget.transform.position);
                Anim.SetBool("Walking", false);
                Anim.SetBool("Running", true);
                Anim.ResetTrigger("Attacking2");


            }
            if (monsterStatus != null && monsterStatus.currentHp <= 0)
            {
                currentTarget = null; // ���Ͱ� �׾����� ���� ����
                Anim.SetBool("Walking", true);     
                Anim.SetBool("Running", false);
                Anim.ResetTrigger("Attacking2");
                ReturnToGuardPosition(); // ���� ��ġ�� ����
                popupText.text = "���� �ڸ��� ���� �Ѵ�!!";

                //Debug.Log("����� 2");
            }
        }
        if (attackCooldown > 0f)
        {
            attackCooldown -= Time.deltaTime;
        }
        // ���� �ڸ� ���� ���� Ȯ��
        if (isReturningToGuard && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isReturningToGuard = false;
                // ����� ���� �ڸ����� ���߰� �ϱ� ���� NavMeshAgent ��Ȱ��ȭ
                agent.isStopped = true;
                agent.ResetPath(); // ��� �ʱ�ȭ
                Anim.ResetTrigger("Attacking2");
                Anim.SetBool("Running", false);
                Anim.SetBool("Walking", false);
                popupText.text = ""; // ���� �ؽ�Ʈ ���
                popUI.SetActive(false);
                hasDisplayedPopup = false; // ���� ���͸� ���� �˾� �ٽ� ǥ�� �����ϰ� ����
            }
        }
    }

    private void AttackTarget()
    {
        if (attackCooldown <= 0f && currentTarget != null)
        {
            //Debug.Log("���: ���͸� �����մϴ�!");
            Anim.SetTrigger("Attacking2");
            Anim.SetBool("Running", false);
            Anim.SetBool("Walking", false);
            // ���� ���� �߰�
            MonsterStatus monsterStatus = currentTarget.GetComponent<MonsterStatus>();
            if (monsterStatus != null)
            {
                monsterStatus.TakeDamage(guardDamage); // ���Ϳ��� ������ ����
            }
            attackCooldown = attackInterval; // ���� ���ݱ����� ��� �ð� ����
        }
    }

    private void ReturnToGuardPosition()
    {
        Anim.SetBool("Walking", true);
        Anim.SetBool("Running", false);
        isReturningToGuard = true;
        agent.isStopped = false; // NavMeshAgent �ٽ� Ȱ��ȭ
        agent.SetDestination(guardPosition.position);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //
    //    if (other.CompareTag("Monster"))
    //    {
    //        currentTarget = other.gameObject;
    //        agent.isStopped = false; // ����� ���͸� ������ �� �ֵ��� NavMeshAgent Ȱ��ȭ
    //        agent.SetDestination(currentTarget.transform.position); // ���� ���� ����
    //        isReturningToGuard = false;
    //    }
    //}
    //
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Monster") && currentTarget == other.gameObject)
    //    {
    //        popUI.SetActive(true);
    //        currentTarget = null; // ���Ͱ� ������ ����� ���� ����
    //        ReturnToGuardPosition(); // ���� ��ġ�� ����
    //    }
    //}
}

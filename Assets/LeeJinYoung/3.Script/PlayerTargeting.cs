using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTargeting : MonoBehaviour
{
    public static PlayerTargeting Instance { get; private set; }
    private List<Transform> monsters = new List<Transform>();
    public MonsterHPbar hpBar;  // HP�� ���� ��ũ��Ʈ
    public GameObject PlayerRay;
    public Image aim;
    public Transform targetMonster;  //  ���� ��ü
    public Transform targetPoition;  // ���� ��ġ

    public float targetRadius = 1f;
    public float sphereCastRadius = 5f;

    public Color rayColor = Color.red;
    public Color sphereColor = Color.green;

    private void Awake()
    {
        AimScript AimUI = FindObjectOfType<AimScript>();
        aim = AimUI.Aim;
        Instance = this;
        if (PlayerRay == null)
        {
            PlayerRay = GameObject.FindWithTag("Player");  // "Player" �±׸� ���� ������Ʈ�� ã�Ƽ� �Ҵ�
            if (PlayerRay == null)
            {
                Debug.LogError("PlayerRay�� �Ҵ���� �ʾҽ��ϴ�. 'Player' �±׸� Ȯ���ϼ���.");
            }

        }
        if(aim == null)
        {
            GameObject aimObj = GameObject.FindWithTag("AimUI");
            if(aimObj != null )
            { 
                aim = aimObj.GetComponentInChildren<Image>();    
            }
            if (aim == null)
            {
                Debug.LogError("Aim �Ҵ� �ȉ�");
            }
        }
    }

    private void Start()
    {
        aim.gameObject.SetActive(false);
    }


    public float playerTargetRange;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ���� ��ư Ŭ�� ��
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
                if (monster != null)
                {
                    //targetMonster = monster.transform;
                    //hpBar.SetTargetmonster(monster); // Ŭ���� ���͸� Ÿ����
                    //aim.gameObject.SetActive(true);
                    SetTarget(monster);
                }
                else
                {
                    ClearTarget();
                }
            }
            else
            {
                ClearTarget();
            }
        }
        //UpdateTarget();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //aim.gameObject.SetActive(false);
            //hpBar.hpBarUI.SetActive(false);
            //targetMonster = null;
            ClearTarget();
        }

        if (targetMonster != null && targetPoition != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetPoition.position);
            aim.transform.position = screenPosition;
        }
        else
        {
            aim.gameObject.SetActive(false);
        }

    }
    // ���͸� Ÿ���� �ý��ۿ� ���
    public void RegisterMonster(Transform monster)
    {
        if (!monsters.Contains(monster))
        {
            monsters.Add(monster);
            Debug.Log("Monster registered to targeting system: " + monster.name);
        }
    }

    private void SetTarget(MonsterStatus monster)
    {
        targetMonster = monster.transform; //���� ��ü�� Transform���� ����
        targetPoition = monster.targetPosition != null ? monster.targetPosition : monster.transform;

        aim.gameObject.SetActive(true);
        hpBar.SetTargetmonster(monster); // HP �� Ÿ�� ����
        monster.onHpChange.AddListener(OnMonsterHpChanged);
    }
    //private void SetTarget(MonsterStatus monster)
    //{
    //
    //    //targetMonster = monster.transform; //���� ��ü�� Transform���� ����
    //    //targetPoition = monster.targetPosition != null ? monster.targetPosition : monster.transform;
    //    //
    //    //aim.gameObject.SetActive(true);
    //    //hpBar.SetTargetmonster(monster); // HP �� Ÿ�� ����
    //    //monster.onHpChange.AddListener(OnMonsterHpChanged);
    //    //
    //    //
    //    //targetMonster = monster.targetPosition;
    //
    //    // targetPosition�� null�� ��� �⺻���� �Ҵ��ϰų� �α� ���
    //    if (monster.targetPosition != null)
    //    {
    //        targetPoition = monster.targetPosition;
    //    }
    //    else
    //    {
    //        Debug.LogError("Target position is null for the selected monster.");
    //        targetPoition = targetMonster; // targetPosition�� null�� ���, ���� ��ü�� Transform���� ��ü
    //    }
    //    
    //    aim.gameObject.SetActive(true);
    //    hpBar.SetTargetmonster(monster);
    //    monster.onHpChange.AddListener(OnMonsterHpChanged);
    //
    //    // TODO: ������...
    //    //targetMonster = monster.transform;
    //    //targetPoition = monster.targetPosition;
    //    //aim.gameObject.SetActive(true);
    //    //hpBar.SetTargetmonster(monster);
    //    //monster.onHpChange.AddListener(OnMonsterHpChanged);
    //}

    private void ClearTarget()
    {
        aim.gameObject.SetActive(false);
        hpBar.hpBarUI.SetActive(false);
        targetMonster = null;
    }



    private void UpdateTarget()
    {
        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.forward);
        RaycastHit hit;
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereCastRadius);
        if (Physics.SphereCast(ray, 20f, out hit, 20f))
        {
            MonsterStatus monster = hit.collider.GetComponent<MonsterStatus>();
            if (monster != null)
            {
                //Ÿ�� ���Ͱ� ������ ���
                if (targetMonster == null || targetMonster != monster.transform)
                {
                    targetMonster = monster.transform;
                    targetPoition = monster.targetPosition; // Ÿ�� ��ġ�� ����
                    aim.gameObject.SetActive(true);
                    hpBar.SetTargetmonster(monster); // Ŭ���� ���͸� Ÿ����
                                                     //ü�� ��ȭ �̺�Ʈ�� �����Ͽ� ü�� 0�� �� UI ��Ȱ��ȭ
                    monster.onHpChange.AddListener(OnMonsterHpChanged);
                }
                else
                {
                    //���Ͱ� �ƴ� ��� ���� �̹����� ��Ȱ��ȭ�մϴ�.
                    aim.gameObject.SetActive(false);
                    hpBar.hpBarUI.SetActive(false);
                }
            }
        }
        else
        {
            // SphereCast�� ���Ͱ� ���� ���
            if (targetMonster != null)
            {
                // Ÿ�� ���Ͱ� ���� ���������� Ž������ �ʴ� ���
                targetMonster = null;
                targetPoition = null;
                hpBar.hpBarUI.SetActive(false);
                aim.gameObject.SetActive(false);
            }
        }



    }

    private void OnMonsterHpChanged(int currentHp, int maxHP)
    {
        if (currentHp <= 0)
        {
            aim.gameObject.SetActive(false);
            hpBar.hpBarUI.SetActive(false);
            targetMonster = null; // Ÿ�� �ʱ�ȭ
            //targetPoition = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (PlayerRay == null)
            return;

        Ray ray = new Ray(PlayerRay.transform.position, PlayerRay.transform.position);
        //float sphereCastRadius = 1f; // ������ �ݰ� (���� ����)

        // ������ ������ �ð������� ǥ���մϴ�.
        Gizmos.color = rayColor;
        Gizmos.DrawRay(ray.origin, ray.direction * targetRadius);

        // ������ ������ �ð������� ǥ���մϴ�.targetRadius
        Vector3 sphereCastEndPoint = ray.origin + ray.direction * targetRadius;
        Gizmos.color = sphereColor;
        Gizmos.DrawWireSphere(sphereCastEndPoint, sphereCastRadius);
    }
}
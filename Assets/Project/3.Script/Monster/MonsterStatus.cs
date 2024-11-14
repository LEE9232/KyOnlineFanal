using UnityEngine;
using UnityEngine.Events;

public class MonsterStatus : MonoBehaviour
{
    //��� 
    public enum MonsterGrade { Normal, Rare, Epic, Legendary }
    public MonsterGrade monsterGrade;   //���
    [TextArea(3, 10)]
    public string monsName; // ���� �̸�
    public int monsLevel;  // ���� ����
    public int monsmaxHp = 1000;  // ���� ü��
    public float monsSpeed;   // ������ ���ǵ�
    public float monsWalkSpeed = 1f;
    public int monsDamage;  // ������ ���ݷ�
    public int monsExp;  // ���� ����ġ
    public int currentHp = 0;  // ���� ���� ü��
    public Transform targetPosition;
    public MonsterITemDrop monsDrop;
    private MinimapIconManager minimapIconManager;  // MinimapIconManager ����
    private bool killedByPlayer = false;  // �÷��̾ ���͸� �׿����� ����
    public UnityEvent onDeath; // ���Ͱ� �׾��� �� �߻��ϴ� �̺�Ʈ
    public MonsterTextPool damageTextPool; // ������ �ؽ�Ʈ Ǯ ����
    public Transform damageTextPosition; // ������ �ؽ�Ʈ�� ǥ�õ� ��ġ
    public string monsterType;  // �� ������ Ÿ�� (����Ʈ�� ��ġ�ϴ��� Ȯ��)
    public int testdamage = 0;

    public UnityEvent<int, int> onHpChange;
    // ����Ƽ �̺�Ʈ ȣ��� ü�¹ٿ� ������ ü���� ����ȭ��.

    private bool isDie = false;


    private void Awake()
    {
        currentHp = monsmaxHp;
        if (onDeath == null)
            onDeath = new UnityEvent();
        if (targetPosition == null)
        {
            targetPosition = this.transform;
        }
    }
    private void Start()
    {
        minimapIconManager = FindObjectOfType<MinimapIconManager>();
        if (minimapIconManager != null)
        {
            minimapIconManager.RegisterIcon(transform, EntityType.Monster);
        }
        else
        {
            Debug.LogError("minimapIconManager �� ���Դϴ�.");
        }
        damageTextPool = FindObjectOfType<MonsterTextPool>();
        if (damageTextPool == null)
        {
            Debug.LogError("MonsterTextPool�� ã�� �� �����ϴ�.");
        }
    }

    private void OnDestroy()
    {
        // ���Ͱ� ����� �� �̴ϸ� �������� ����
        if (minimapIconManager != null)
        {
            minimapIconManager.UnregisterIcon(transform);
        }
    }

    //�޴� ������ �޼���
    public void TakeDamage(int damage)
    {
        ShowDamageText(damage);
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, monsmaxHp);
        onHpChange?.Invoke(currentHp, monsmaxHp);
    }
    // ������ �ؽ�Ʈ ǥ��
    private void ShowDamageText(int damage)
    {
        GameObject damageTextObject = damageTextPool.GetDamageText(damageTextPosition, new Vector3(0, 2, 0));
        MonsterDagageText damageText = damageTextObject.GetComponent<MonsterDagageText>();
        damageText.SetDamageText(damage, damageTextPool); // Ǯ�� ������ ��ġ ����
    }
    // ���� ü�¹ٿ� �����Ұ�
    public void SetHP(int monsHp)
    {
        currentHp = Mathf.Clamp(monsHp, 0, monsmaxHp);
        onHpChange?.Invoke(currentHp, monsmaxHp);
    }
    public void Die(GameObject attacker)
    {
        if (isDie || currentHp > 0)
        {
            return;
        }
        PlayerManagement player = FindObjectOfType<PlayerManagement>();
        if (player != null)
        {
            // �����ڰ� �÷��̾��� ��쿡�� ����Ʈ ī��Ʈ ������Ʈ
            if (attacker.CompareTag("Player"))
            {   
                // �����ڰ� �÷��̾��� ��쿡�� ����ġ�� �ֵ��� �÷��� ����
                killedByPlayer = true;
                player.GainEXP(monsExp);
                PlayerQuestManager.Instance.UpdateQuestProgress();
            }
        }
        // �÷��̾ ���͸� ���� ��쿡�� ����ġ ����
        isDie = true;
        onDeath?.Invoke();
        monsDrop.DropSystem();
        Destroy(gameObject, 3f);
    }
}

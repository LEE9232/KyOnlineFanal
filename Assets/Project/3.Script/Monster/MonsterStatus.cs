using UnityEngine;
using UnityEngine.Events;

public class MonsterStatus : MonoBehaviour
{
    //레어도 
    public enum MonsterGrade { Normal, Rare, Epic, Legendary }
    public MonsterGrade monsterGrade;   //레어도
    [TextArea(3, 10)]
    public string monsName; // 몬스터 이름
    public int monsLevel;  // 몬스터 레벨
    public int monsmaxHp = 1000;  // 몬스터 체력
    public float monsSpeed;   // 몬스터의 스피드
    public float monsWalkSpeed = 1f;
    public int monsDamage;  // 몬스터의 공격력
    public int monsExp;  // 몬스터 경험치
    public int currentHp = 0;  // 몬스터 현재 체력
    public Transform targetPosition;
    public MonsterITemDrop monsDrop;
    private MinimapIconManager minimapIconManager;  // MinimapIconManager 참조
    private bool killedByPlayer = false;  // 플레이어가 몬스터를 죽였는지 여부
    public UnityEvent onDeath; // 몬스터가 죽었을 때 발생하는 이벤트
    public MonsterTextPool damageTextPool; // 데미지 텍스트 풀 참조
    public Transform damageTextPosition; // 데미지 텍스트가 표시될 위치
    public string monsterType;  // 이 몬스터의 타입 (퀘스트와 일치하는지 확인)
    public int testdamage = 0;

    public UnityEvent<int, int> onHpChange;
    // 유니티 이벤트 호출로 체력바와 몬스터의 체력을 동기화함.

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
            Debug.LogError("minimapIconManager 가 널입니다.");
        }
        damageTextPool = FindObjectOfType<MonsterTextPool>();
        if (damageTextPool == null)
        {
            Debug.LogError("MonsterTextPool을 찾을 수 없습니다.");
        }
    }

    private void OnDestroy()
    {
        // 몬스터가 사망할 때 미니맵 아이콘을 제거
        if (minimapIconManager != null)
        {
            minimapIconManager.UnregisterIcon(transform);
        }
    }

    //받는 데미지 메서드
    public void TakeDamage(int damage)
    {
        ShowDamageText(damage);
        currentHp -= damage;
        currentHp = Mathf.Clamp(currentHp, 0, monsmaxHp);
        onHpChange?.Invoke(currentHp, monsmaxHp);
    }
    // 데미지 텍스트 표시
    private void ShowDamageText(int damage)
    {
        GameObject damageTextObject = damageTextPool.GetDamageText(damageTextPosition, new Vector3(0, 2, 0));
        MonsterDagageText damageText = damageTextObject.GetComponent<MonsterDagageText>();
        damageText.SetDamageText(damage, damageTextPool); // 풀과 데미지 수치 전달
    }
    // 몬스터 체력바와 연결할곳
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
            // 공격자가 플레이어일 경우에만 퀘스트 카운트 업데이트
            if (attacker.CompareTag("Player"))
            {   
                // 공격자가 플레이어인 경우에만 경험치를 주도록 플래그 설정
                killedByPlayer = true;
                player.GainEXP(monsExp);
                PlayerQuestManager.Instance.UpdateQuestProgress();
            }
        }
        // 플레이어가 몬스터를 죽인 경우에만 경험치 지급
        isDie = true;
        onDeath?.Invoke();
        monsDrop.DropSystem();
        Destroy(gameObject, 3f);
    }
}

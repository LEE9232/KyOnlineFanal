using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerManagement : MonoBehaviour
{
    [Header("Player Stats")]
    public Animator pmAnim;
    public bool diePlayer = false;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI mpText;
    public TextMeshProUGUI expText;
    public TextMeshProUGUI skillpointText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI defensiveText;
    public TextMeshProUGUI hpregenText;
    public TextMeshProUGUI mpregenText;
    public Slider hpSlider;  // HP 슬라이더
    public Slider mpSlider;  // MP 슬라이더
    public Slider expSlider; // 경험치 슬라이더
    private float regenTimer = 0f;
    private float regenInterval = 1f; // 1초마다 재생
    public GameObject levelUpParticle;
    public Transform respawnPoint; // 부활 지점
    public GameObject deathPopup;  // 사망 팝업 UI
    public Button respawnButton;   // 부활 버튼
    private bool isRespawning = false; // 부활 중인지 여부

    public Button TestBtn;
    public int testint = 100;
    private void Awake()
    {
        pmAnim = GetComponent<Animator>();
        TestBtn.onClick.AddListener(testBtnClick);
        UpdateStatsUI();
    }
    private void Start()
    {        // 파티클을 처음엔 비활성화
        if (levelUpParticle != null)
        {
            levelUpParticle.SetActive(false);
        }
    }
    private void Update()
    {
        if (!diePlayer)
        {   
            UpdateStatsUI();
        }
        regenTimer += Time.deltaTime;
        if (regenTimer >= regenInterval)
        {
            RegenPerSecond();
            regenTimer = 0f;
        }
    }
    public void testBtnClick()
    {
        GainEXP(300);
    }
    public void TakeDamage(int damage)
    {
        if (!isRespawning && !diePlayer)
        {
            // 방어력이 데미지보다 크면 데미지를 0으로 설정
            int finalDamage = Mathf.Max(0, damage - GameManager.Instance.PlayerData.Defensive);
            GameManager.Instance.PlayerData.HP -= finalDamage;
            GameManager.Instance.logUI.AddMessage($" {finalDamage} <color=red>Damage </color>를 입었습니다.");
            if (GameManager.Instance.PlayerData.HP <= 0)
            {
                PlayerDie();
            }
            GameManager.Instance.PlayerData.MarkDataAsChanged(); // 데이터 변경
            Debug.Log(damage);
        }
    }
    public void GainEXP(int expAmount)
    {
        var playerData = GameManager.Instance.PlayerData;
        playerData.EXP += expAmount;
        Debug.Log($"경험치 {expAmount} 획득. 현재 경험치: {playerData.EXP}");
        while (playerData.EXP >= playerData.MaxEXP)
        {
            LevelUp();
        }
        GameManager.Instance.PlayerData.MarkDataAsChanged();
    }
    public void LevelUp()
    {
        var playerData = GameManager.Instance.PlayerData;
        playerData.Level++;
        playerData.SkillPoint += 1;
        playerData.EXP -= playerData.MaxEXP;

        // 레벨업 시 기본 스탯 증가
        playerData.BaseMaxHP += 10; // BaseMaxHP 증가
        playerData.BaseMaxMP += 5;  // BaseMaxMP 증가
        playerData.BaseDamage += 2; // BaseDamage 증가
        playerData.BaseDefensive += 1; // BaseDefensive 증가
        playerData.MaxEXP = playerData.Level * 100;
        playerData.ResetWeaponData();
        playerData.ResetArmorData();
        playerData.ResetMPData();
        playerData.HP = playerData.MaxHP;
        playerData.MP = playerData.MaxMP;
        // 장착된 아이템 스탯 적용
        playerData.ApplyEquippedItemsStats(playerData, playerData.EquippedItemsManager);
        // UI 업데이트
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        GameManager.Instance.PlayerData.MarkDataAsChanged();
        UpdateStatsUI();
        PlayLevelUpParticle();
    }
    private void PlayLevelUpParticle()
    {
        if (levelUpParticle != null)
        {
            levelUpParticle.SetActive(true);  // 파티클 활성화
            StartCoroutine(DisableParticleAfterSeconds(2f));  // 3초 후 비활성화
        }
    }

    private IEnumerator DisableParticleAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (levelUpParticle != null)
        {
            levelUpParticle.SetActive(false);  // 파티클 비활성화
        }
    }
    IEnumerator Testcotoutine()
    {
        while (true) // 무한 루프
        {
            yield return new WaitForSeconds(1f); // 1초마다 실행
            TakeDamage(5);
            Debug.Log($"현재 체력: {GameManager.Instance.PlayerData.HP}"); // 체력 로그 확인
        }
    }
    public void UseMP(int usemp)
    {
        var playerData = GameManager.Instance.PlayerData;
        playerData.MP -= usemp;
        if (playerData.MP <= 0)
        {
            playerData.MP = 0;
        }
        GameManager.Instance.PlayerData.MarkDataAsChanged();
    }
    public void PlayerDie()
    {
        if (!diePlayer && !isRespawning)
        {
            //isRespawning = true;
            diePlayer = true;
            pmAnim.SetBool("IsDie", true);
            SController sController = GetComponent<SController>();
            sController.Movement(false);
            deathPopup.SetActive(true);
            Debug.LogWarning("다이 호출");
            GameManager.Instance.PlayerData.HP = 0;
            GameManager.Instance.PlayerData.MP = 0;
            UpdateStatsUI();
        }
    }
    // 부활 버튼 클릭 시 호출되는 메서드
    public void RespawnPlayer()
    {
        if (!isRespawning)
        {
            StartCoroutine(RespawnInTown());
        }
    }
    private IEnumerator RespawnInTown()
    {
        isRespawning = true;
        deathPopup.SetActive(false);  // 팝업 비활성화

        // 현재 씬이 마을 씬인지 확인
        if (SceneManager.GetActiveScene().name == "Stage1")
        {
            transform.position = respawnPoint.position;
            yield return new WaitForSeconds(2f);
            GameManager.Instance.PlayerData.HP = GameManager.Instance.PlayerData.MaxHP / 2;
            GameManager.Instance.PlayerData.MP = GameManager.Instance.PlayerData.MaxMP / 2;
            GameManager.Instance.PlayerData.EXP -= 300;
            diePlayer = false;
            pmAnim.SetBool("IsDie", false);
            SController sController = GetComponent<SController>();
            sController.Movement(diePlayer);
        }
        else
        {
            // 다른 씬일 경우 마을 씬으로 씬 전환
            //yield return Changescenemaneger.Instance.StageOneScene(); //LoadSceneAsync("Stage1");
            Changescenemaneger.Instance.StageOneScene();
            yield return new WaitForSeconds(2f);
            // 씬이 로드된 후 부활 위치로 플레이어 이동
            PlayerRespawnInTown();
        }
        isRespawning = false;
    }
    // 마을 씬에서 플레이어 부활 처리
    private void PlayerRespawnInTown()
    {
        //Transform respawnPoint = GameObject.FindWithTag("RespawnPoint")?.transform;  // 부활 위치를 찾아 이동
        //transform.position = respawnPoint.position;
        GameManager.Instance.PlayerData.HP = GameManager.Instance.PlayerData.MaxHP / 2;
        GameManager.Instance.PlayerData.MP = GameManager.Instance.PlayerData.MaxMP / 2;
        GameManager.Instance.PlayerData.EXP -= 300;
        diePlayer = false;
        pmAnim.SetBool("IsDie", false);
        SController sController = GetComponent<SController>();
        sController.Movement(diePlayer);
    }
    public void UpdateStatsUI()
    {
        var playerData = GameManager.Instance.PlayerData;
        //GameManager.Instance.PlayerData.ApplyEquippedItemsStats(playerData, playerData.EquippedItemsManager);
        nameText.text = playerData.NickName;
        levelText.text = playerData.Level.ToString();
        hpText.text = $"{playerData.HP} / {playerData.MaxHP}";
        mpText.text = $"{playerData.MP} / {playerData.MaxMP}";
        expText.text = $"{playerData.EXP} / {playerData.MaxEXP}";
        skillpointText.text = playerData.SkillPoint.ToString();
        damageText.text = playerData.Damage.ToString();
        defensiveText.text = playerData.Defensive.ToString();
        hpregenText.text = playerData.HpRegenPerSecond.ToString();
        mpregenText.text = playerData.MpRegenPerSecond.ToString();
        // 슬라이더 UI 업데이트
        hpSlider.maxValue = playerData.MaxHP;
        hpSlider.value = playerData.HP;

        mpSlider.maxValue = playerData.MaxMP;
        mpSlider.value = playerData.MP;

        expSlider.maxValue = playerData.MaxEXP;
        expSlider.value = playerData.EXP;
    }
    public void RegenPerSecond()
    {
        var playerData = GameManager.Instance.PlayerData;
        if (diePlayer)  // 사망 상태이면 체력 재생 중단
        {
            return;
        }
        if (playerData.HP < playerData.MaxHP)
        {
            playerData.HP = Mathf.Clamp(playerData.HP + playerData.HpRegenPerSecond, 0, playerData.MaxHP);
        }

        if (playerData.MP < playerData.MaxMP)
        {
            playerData.MP = Mathf.Clamp(playerData.MP + playerData.MpRegenPerSecond, 0, playerData.MaxMP);
        }
    }
}

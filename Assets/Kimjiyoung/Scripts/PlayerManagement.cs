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
    public Slider hpSlider;  // HP �����̴�
    public Slider mpSlider;  // MP �����̴�
    public Slider expSlider; // ����ġ �����̴�
    private float regenTimer = 0f;
    private float regenInterval = 1f; // 1�ʸ��� ���
    public GameObject levelUpParticle;
    public Transform respawnPoint; // ��Ȱ ����
    public GameObject deathPopup;  // ��� �˾� UI
    public Button respawnButton;   // ��Ȱ ��ư
    private bool isRespawning = false; // ��Ȱ ������ ����

    public Button TestBtn;
    public int testint = 100;
    private void Awake()
    {
        pmAnim = GetComponent<Animator>();
        TestBtn.onClick.AddListener(testBtnClick);
        UpdateStatsUI();
    }
    private void Start()
    {        // ��ƼŬ�� ó���� ��Ȱ��ȭ
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
            // ������ ���������� ũ�� �������� 0���� ����
            int finalDamage = Mathf.Max(0, damage - GameManager.Instance.PlayerData.Defensive);
            GameManager.Instance.PlayerData.HP -= finalDamage;
            GameManager.Instance.logUI.AddMessage($" {finalDamage} <color=red>Damage </color>�� �Ծ����ϴ�.");
            if (GameManager.Instance.PlayerData.HP <= 0)
            {
                PlayerDie();
            }
            GameManager.Instance.PlayerData.MarkDataAsChanged(); // ������ ����
            Debug.Log(damage);
        }
    }
    public void GainEXP(int expAmount)
    {
        var playerData = GameManager.Instance.PlayerData;
        playerData.EXP += expAmount;
        Debug.Log($"����ġ {expAmount} ȹ��. ���� ����ġ: {playerData.EXP}");
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

        // ������ �� �⺻ ���� ����
        playerData.BaseMaxHP += 10; // BaseMaxHP ����
        playerData.BaseMaxMP += 5;  // BaseMaxMP ����
        playerData.BaseDamage += 2; // BaseDamage ����
        playerData.BaseDefensive += 1; // BaseDefensive ����
        playerData.MaxEXP = playerData.Level * 100;
        playerData.ResetWeaponData();
        playerData.ResetArmorData();
        playerData.ResetMPData();
        playerData.HP = playerData.MaxHP;
        playerData.MP = playerData.MaxMP;
        // ������ ������ ���� ����
        playerData.ApplyEquippedItemsStats(playerData, playerData.EquippedItemsManager);
        // UI ������Ʈ
        GameManager.Instance.inventoryUI.UpdateInventoryUI();
        GameManager.Instance.PlayerData.MarkDataAsChanged();
        UpdateStatsUI();
        PlayLevelUpParticle();
    }
    private void PlayLevelUpParticle()
    {
        if (levelUpParticle != null)
        {
            levelUpParticle.SetActive(true);  // ��ƼŬ Ȱ��ȭ
            StartCoroutine(DisableParticleAfterSeconds(2f));  // 3�� �� ��Ȱ��ȭ
        }
    }

    private IEnumerator DisableParticleAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (levelUpParticle != null)
        {
            levelUpParticle.SetActive(false);  // ��ƼŬ ��Ȱ��ȭ
        }
    }
    IEnumerator Testcotoutine()
    {
        while (true) // ���� ����
        {
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
            TakeDamage(5);
            Debug.Log($"���� ü��: {GameManager.Instance.PlayerData.HP}"); // ü�� �α� Ȯ��
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
            Debug.LogWarning("���� ȣ��");
            GameManager.Instance.PlayerData.HP = 0;
            GameManager.Instance.PlayerData.MP = 0;
            UpdateStatsUI();
        }
    }
    // ��Ȱ ��ư Ŭ�� �� ȣ��Ǵ� �޼���
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
        deathPopup.SetActive(false);  // �˾� ��Ȱ��ȭ

        // ���� ���� ���� ������ Ȯ��
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
            // �ٸ� ���� ��� ���� ������ �� ��ȯ
            //yield return Changescenemaneger.Instance.StageOneScene(); //LoadSceneAsync("Stage1");
            Changescenemaneger.Instance.StageOneScene();
            yield return new WaitForSeconds(2f);
            // ���� �ε�� �� ��Ȱ ��ġ�� �÷��̾� �̵�
            PlayerRespawnInTown();
        }
        isRespawning = false;
    }
    // ���� ������ �÷��̾� ��Ȱ ó��
    private void PlayerRespawnInTown()
    {
        //Transform respawnPoint = GameObject.FindWithTag("RespawnPoint")?.transform;  // ��Ȱ ��ġ�� ã�� �̵�
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
        // �����̴� UI ������Ʈ
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
        if (diePlayer)  // ��� �����̸� ü�� ��� �ߴ�
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

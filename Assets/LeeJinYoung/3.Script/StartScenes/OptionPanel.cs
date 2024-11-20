using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    //public UIPanelManager panelManager;
    public GameObject optionPanel;
    public Slider sensitivitySlider;  // ���콺 ���� �����̴�
    public Slider musicVolumeSlider;  // ����� ���� �����̴�
    public Slider effectVolumeSlider; // ȿ���� ���� �����̴�
    public AudioSource[] musicSources;  // ����� ����� �ҽ���
    public AudioSource[] effectSources; // ȿ���� ����� �ҽ���
    public AudioSource buttonClickSound; // ��ư Ŭ�� ����
    private const string sensitivityPrefKey = "MouseSensitivity";
    private const string musicVolumePrefKey = "MusicVolume";
    private const string effectVolumePrefKey = "EffectVolume";
    public Button optionsavebutton;
    public Button optionexitButton;
    public Button optioncBtn;
    //private bool isSoundEnabled = false;  // ù ��� ������ ���� �÷���
    //private bool hasStarted = false; // ù ���� üũ�� �÷���
    private void Awake()
    {
        optionPanel.SetActive(false);
        optioncBtn.onClick.AddListener(OptionBtnClick);
        optionsavebutton.onClick.AddListener(OptionSaveBtClick);
        optionexitButton.onClick.AddListener(OptionExitBtClick);
        sensitivitySlider.minValue = 1.0f;  // �ּ� ���� ��
        sensitivitySlider.maxValue = 100.0f; // �ִ� ���� ��
        float savedSensitivity = PlayerPrefs.GetFloat(sensitivityPrefKey, 100.0f); // �⺻�� 100
        sensitivitySlider.value = savedSensitivity;
    }

    private void OnEnable()
    {
        // ���� �ִ� ��� ��ư�� Ŭ�� ���� �߰�
        AddSoundToAllButtons();

    }

    private void OnDisable()
    {
        // ���� ��Ȱ��ȭ�� �� �̺�Ʈ ������ ���� (�ߺ� ����)
        RemoveSoundFromAllButtons();
    }
    // �� �� ��� ��ư���� Ŭ�� ���� ������ ����
    private void RemoveSoundFromAllButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(PlayButtonClickSound);
        }
    }
    // �� �� ��� ��ư�� Ŭ�� ���� �߰�
    private void AddSoundToAllButtons()
    {
        // ��� ��ư�� ��Ȱ��ȭ�� ���±��� �����Ͽ� �˻�
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.AddListener(PlayButtonClickSound);
        }
    }
    public void OptionBtnClick()
    {
        optionPanel.SetActive(true);
        // �����̴� �ʱ�ȭ - PlayerPrefs���� �� �ҷ�����
        sensitivitySlider.value = PlayerPrefs.GetFloat(sensitivityPrefKey, 1.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumePrefKey, 1.0f);
        effectVolumeSlider.value = PlayerPrefs.GetFloat(effectVolumePrefKey, 1.0f);

        // �����̴� �̺�Ʈ ������ �߰�
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        effectVolumeSlider.onValueChanged.AddListener(UpdateEffectVolume);
    }
    // ����� ����
    public void UpdateMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(musicVolumePrefKey, newVolume);
        foreach (var source in musicSources)
        {
            source.volume = newVolume;
        }
    }
    // ȿ���� ����
    public void UpdateEffectVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(effectVolumePrefKey, newVolume);
        foreach (var source in effectSources)
        {
            source.volume = newVolume;
        }
        // ��ư Ŭ�� ������ ������ ȿ���� ������ �����ϰ� ����
        if (buttonClickSound != null)
        {
            buttonClickSound.volume = newVolume;
        }
    }
    // ���콺 ����
    public void UpdateSensitivity(float newSensitivity)
    {
        MyCamera cameraScript = FindObjectOfType<MyCamera>(); // MyCamera ��ũ��Ʈ�� ã�� ���� ������Ʈ
        if (cameraScript != null)
        {
            cameraScript.SetMouseSensitivity(newSensitivity); // ���� ������Ʈ
        }
        PlayerPrefs.SetFloat(sensitivityPrefKey, newSensitivity);
    }
    public void OptionSaveBtClick()
    {
        optionPanel.SetActive(false);
        Debug.Log("�ɼ� ���̺� ��ư Ŭ��");
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectVolumeSlider.onValueChanged.RemoveAllListeners();
    }
    public void OptionExitBtClick()
    {
        optionPanel.SetActive(false);
        Debug.Log("�ɼ� ������ ��ư Ŭ��");
        // �����̴� �̺�Ʈ ������ ���� (�޸� ���� ����)
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectVolumeSlider.onValueChanged.RemoveAllListeners();
    }
    // ��ư Ŭ�� �� ���� ���
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();  // ��ư Ŭ�� ���� ���
        }
    }

}

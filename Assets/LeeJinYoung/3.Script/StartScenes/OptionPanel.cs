using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionPanel : MonoBehaviour
{
    //public UIPanelManager panelManager;
    public GameObject optionPanel;
    public Slider sensitivitySlider;  // 마우스 감도 슬라이더
    public Slider musicVolumeSlider;  // 배경음 볼륨 슬라이더
    public Slider effectVolumeSlider; // 효과음 볼륨 슬라이더
    public AudioSource[] musicSources;  // 배경음 오디오 소스들
    public AudioSource[] effectSources; // 효과음 오디오 소스들
    public AudioSource buttonClickSound; // 버튼 클릭 사운드
    private const string sensitivityPrefKey = "MouseSensitivity";
    private const string musicVolumePrefKey = "MusicVolume";
    private const string effectVolumePrefKey = "EffectVolume";
    public Button optionsavebutton;
    public Button optionexitButton;
    public Button optioncBtn;
    //private bool isSoundEnabled = false;  // 첫 재생 방지를 위한 플래그
    //private bool hasStarted = false; // 첫 실행 체크용 플래그
    private void Awake()
    {
        optionPanel.SetActive(false);
        optioncBtn.onClick.AddListener(OptionBtnClick);
        optionsavebutton.onClick.AddListener(OptionSaveBtClick);
        optionexitButton.onClick.AddListener(OptionExitBtClick);
        sensitivitySlider.minValue = 1.0f;  // 최소 감도 값
        sensitivitySlider.maxValue = 100.0f; // 최대 감도 값
        float savedSensitivity = PlayerPrefs.GetFloat(sensitivityPrefKey, 100.0f); // 기본값 100
        sensitivitySlider.value = savedSensitivity;
    }

    private void OnEnable()
    {
        // 씬에 있는 모든 버튼에 클릭 사운드 추가
        AddSoundToAllButtons();

    }

    private void OnDisable()
    {
        // 씬이 비활성화될 때 이벤트 리스너 제거 (중복 방지)
        RemoveSoundFromAllButtons();
    }
    // 씬 내 모든 버튼에서 클릭 사운드 리스너 제거
    private void RemoveSoundFromAllButtons()
    {
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.RemoveListener(PlayButtonClickSound);
        }
    }
    // 씬 내 모든 버튼에 클릭 사운드 추가
    private void AddSoundToAllButtons()
    {
        // 모든 버튼을 비활성화된 상태까지 포함하여 검색
        Button[] allButtons = FindObjectsOfType<Button>(true);

        foreach (Button btn in allButtons)
        {
            btn.onClick.AddListener(PlayButtonClickSound);
        }
    }
    public void OptionBtnClick()
    {
        optionPanel.SetActive(true);
        // 슬라이더 초기화 - PlayerPrefs에서 값 불러오기
        sensitivitySlider.value = PlayerPrefs.GetFloat(sensitivityPrefKey, 1.0f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumePrefKey, 1.0f);
        effectVolumeSlider.value = PlayerPrefs.GetFloat(effectVolumePrefKey, 1.0f);

        // 슬라이더 이벤트 리스너 추가
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
        effectVolumeSlider.onValueChanged.AddListener(UpdateEffectVolume);
    }
    // 배경음 볼륨
    public void UpdateMusicVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(musicVolumePrefKey, newVolume);
        foreach (var source in musicSources)
        {
            source.volume = newVolume;
        }
    }
    // 효과음 볼륨
    public void UpdateEffectVolume(float newVolume)
    {
        PlayerPrefs.SetFloat(effectVolumePrefKey, newVolume);
        foreach (var source in effectSources)
        {
            source.volume = newVolume;
        }
        // 버튼 클릭 사운드의 볼륨도 효과음 볼륨과 동일하게 조절
        if (buttonClickSound != null)
        {
            buttonClickSound.volume = newVolume;
        }
    }
    // 마우스 감도
    public void UpdateSensitivity(float newSensitivity)
    {
        MyCamera cameraScript = FindObjectOfType<MyCamera>(); // MyCamera 스크립트를 찾아 감도 업데이트
        if (cameraScript != null)
        {
            cameraScript.SetMouseSensitivity(newSensitivity); // 감도 업데이트
        }
        PlayerPrefs.SetFloat(sensitivityPrefKey, newSensitivity);
    }
    public void OptionSaveBtClick()
    {
        optionPanel.SetActive(false);
        Debug.Log("옵션 세이브 버튼 클릭");
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectVolumeSlider.onValueChanged.RemoveAllListeners();
    }
    public void OptionExitBtClick()
    {
        optionPanel.SetActive(false);
        Debug.Log("옵션 나가기 버튼 클릭");
        // 슬라이더 이벤트 리스너 제거 (메모리 누수 방지)
        sensitivitySlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        effectVolumeSlider.onValueChanged.RemoveAllListeners();
    }
    // 버튼 클릭 시 사운드 재생
    public void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            buttonClickSound.Play();  // 버튼 클릭 사운드 재생
        }
    }

}

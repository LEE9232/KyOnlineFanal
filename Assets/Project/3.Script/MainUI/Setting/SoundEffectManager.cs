using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public AudioSource buttonClickSource;  // 버튼 클릭 소리용
    public AudioSource otherEffectSource;  // 나머지 효과음용
    public Slider effectVolumeSlider;
    private const string effectVolumePrefKey = "EffectVolume";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        LoadEffectVolume();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬에서 효과음 볼륨 슬라이더를 찾고 값 설정
        effectVolumeSlider = GameObject.Find("EffectVolumeSlider")?.GetComponent<Slider>();
        if (effectVolumeSlider != null)
        {
            effectVolumeSlider.value = buttonClickSource.volume;
            effectVolumeSlider.onValueChanged.AddListener(SetEffectVolume);
        }
    }

    private void LoadEffectVolume()
    {
        if (PlayerPrefs.HasKey(effectVolumePrefKey))
        {
            float volume = PlayerPrefs.GetFloat(effectVolumePrefKey);
            buttonClickSource.volume = volume;
            otherEffectSource.volume = volume;
        }
        else
        {
            PlayerPrefs.SetFloat(effectVolumePrefKey, buttonClickSource.volume);
        }
    }

    private void SetEffectVolume(float volume)
    {
        buttonClickSource.volume = volume;
        otherEffectSource.volume = volume;
        PlayerPrefs.SetFloat(effectVolumePrefKey, volume);
    }

    public void PlayButtonClickSound()
    {
        buttonClickSource.Play();
    }

    public void PlayOtherEffectSound()
    {
        otherEffectSource.Play();
    }
}

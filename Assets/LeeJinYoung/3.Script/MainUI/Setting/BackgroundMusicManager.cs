using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource[] backgroundMusicSources;  // 배경음 3개
    public Slider musicVolumeSlider;
    private const string musicVolumePrefKey = "MusicVolume";

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
        musicVolumeSlider = GetComponent<Slider>();
        // 첫 씬에서 슬라이더 즉시 할당
        //musicVolumeSlider = GameObject.Find("MusicVolumeSlider")?.GetComponent<Slider>();
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumePrefKey, backgroundMusicSources[0].volume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
        else
        {
            Debug.LogError("MusicVolumeSlider not found in the first scene.");
        }

        LoadMusicVolume();
        foreach (var source in backgroundMusicSources)
        {
            source.Play();  // 각 배경음 재생 시작
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬에서 배경음 볼륨 슬라이더를 찾고 값 설정
        musicVolumeSlider = GameObject.Find("StartUI/PanelList/OptionPanel/Sound/MusicVolumeSlider")?.GetComponent<Slider>();
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = backgroundMusicSources[0].volume;
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    private void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey(musicVolumePrefKey))
        {
            float volume = PlayerPrefs.GetFloat(musicVolumePrefKey);
            foreach (var source in backgroundMusicSources)
            {
                source.volume = volume;
            }
        }
        else
        {
            PlayerPrefs.SetFloat(musicVolumePrefKey, backgroundMusicSources[0].volume);
        }
    }

    private void SetMusicVolume(float volume)
    {
        foreach (var source in backgroundMusicSources)
        {
            source.volume = volume;
        }
        PlayerPrefs.SetFloat(musicVolumePrefKey, volume);
    }
}

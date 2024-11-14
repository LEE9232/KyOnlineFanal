using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioSource[] backgroundMusicSources;  // ����� 3��
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
        // ù ������ �����̴� ��� �Ҵ�
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
            source.Play();  // �� ����� ��� ����
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �� ������ ����� ���� �����̴��� ã�� �� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    public Slider sensitivitySlider;
    public float sensitivity = 1.0f;
    private const string sensitivityPrefKey = "MouseSensitivity";

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
        LoadSensitivity();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 새 씬에서 마우스 감도 슬라이더 값 설정
        sensitivitySlider = GameObject.Find("Mouse")?.GetComponent<Slider>();
        if (sensitivitySlider != null)
        {
            sensitivitySlider.value = sensitivity;
            sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        }
    }

    private void LoadSensitivity()
    {
        if (PlayerPrefs.HasKey(sensitivityPrefKey))
        {
            sensitivity = PlayerPrefs.GetFloat(sensitivityPrefKey);
        }
        else
        {
            PlayerPrefs.SetFloat(sensitivityPrefKey, sensitivity);
        }
    }

    private void UpdateSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
        PlayerPrefs.SetFloat(sensitivityPrefKey, newSensitivity);
    }
}

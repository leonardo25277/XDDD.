using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject[] panels;

    [Header("Botones del menu")]
    public Button[] menuButtons;

    [Header("Botones Volver")]
    public Button[] backButtons;

    [Header("Ajustes")]
    public TMP_InputField playerNameField;
    public TMP_Dropdown qualityDropdown;
    public Slider brightnessSlider;
    public Toggle fullscreenToggle;
    public Image brightnessOverlay;

    [Header("Escena de juego")]
    public string gameSceneName = "Joystick";

    void Start()
    {
        if (menuButtons != null && menuButtons.Length >= 5)
        {
            menuButtons[0].onClick.AddListener(PlayGame);
            menuButtons[1].onClick.AddListener(() => ShowPanel(1));
            menuButtons[2].onClick.AddListener(() => ShowPanel(2));
            menuButtons[3].onClick.AddListener(() => ShowPanel(3));
            menuButtons[4].onClick.AddListener(QuitGame);
        }

        if (backButtons != null)
        {
            foreach (Button button in backButtons)
                if (button != null)
                    button.onClick.AddListener(() => ShowPanel(0));
        }

        if (playerNameField != null)
        {
            playerNameField.text = PlayerPrefs.GetString("playerName", "");
            playerNameField.onEndEdit.AddListener(value => PlayerPrefs.SetString("playerName", value));
        }

        if (qualityDropdown != null)
        {
            int quality = PlayerPrefs.GetInt("quality", 2);
            quality = Mathf.Clamp(quality, 0, qualityDropdown.options.Count - 1);
            qualityDropdown.SetValueWithoutNotify(quality);
            ApplyQuality(quality);
            qualityDropdown.onValueChanged.AddListener(ApplyQuality);
        }

        if (brightnessSlider != null)
        {
            brightnessSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("brightness", 0.8f));
            ApplyBrightness(brightnessSlider.value);
            brightnessSlider.onValueChanged.AddListener(value =>
            {
                ApplyBrightness(value);
                PlayerPrefs.SetFloat("brightness", value);
            });
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetIsOnWithoutNotify(PlayerPrefs.GetInt("fullscreen", 1) == 1);
            ApplyFullscreen(fullscreenToggle.isOn);
            fullscreenToggle.onValueChanged.AddListener(value =>
            {
                ApplyFullscreen(value);
                PlayerPrefs.SetInt("fullscreen", value ? 1 : 0);
            });
        }

        ShowPanel(0);
    }

    void ShowPanel(int index)
    {
        if (panels == null) return;
        for (int i = 0; i < panels.Length; i++)
            if (panels[i] != null)
                panels[i].SetActive(i == index);
    }

    void ApplyQuality(int index)
    {
        int lastQuality = Mathf.Max(0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(Mathf.Clamp(index, 0, lastQuality), true);
        PlayerPrefs.SetInt("quality", index);
    }

    void ApplyBrightness(float value)
    {
        if (brightnessOverlay == null) return;
        Color color = brightnessOverlay.color;
        color.a = 1f - Mathf.Clamp01(value);
        brightnessOverlay.color = color;
    }

    void ApplyFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }

    void PlayGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
            SceneManager.LoadScene(gameSceneName);
    }

    void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

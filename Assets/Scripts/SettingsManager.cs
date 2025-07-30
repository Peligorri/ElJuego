using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Botones de configuración")]
    public Button musicToggleButton;
    public TextMeshProUGUI musicButtonText;

    public Button soundToggleButton;
    public TextMeshProUGUI soundButtonText;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource[] soundSources;

    [Header("Texto High Score")]
    public TextMeshProUGUI highScoreText;

    public GameObject settingsPanel;

    private bool musicOn;
    private bool soundOn;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Evita duplicados
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Hace persistente el objeto
    }

    void Start()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);

        // Cargar preferencias
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        soundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;

        UpdateMusicState();
        UpdateSoundState();

        int highScore = PlayerPrefs.GetInt("BestScore", 0);
        if (highScoreText != null)
            highScoreText.text = highScore.ToString("0000");

        // Listeners solo si los botones están activos (en el menú)
        if (musicToggleButton != null)
            musicToggleButton.onClick.AddListener(ToggleMusic);
        if (soundToggleButton != null)
            soundToggleButton.onClick.AddListener(ToggleSound);
    }

    public void ToggleMusic()
    {
        musicOn = !musicOn;
        PlayerPrefs.SetInt("MusicOn", musicOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateMusicState();
    }

    public void ToggleSound()
    {
        soundOn = !soundOn;
        PlayerPrefs.SetInt("SoundOn", soundOn ? 1 : 0);
        PlayerPrefs.Save();
        UpdateSoundState();

        foreach (AudioSource source in Resources.FindObjectsOfTypeAll<AudioSource>())
        {
            if (source.CompareTag("SFX"))
                source.mute = !soundOn;
        }
    }

    public void UpdateMusicState()
    {
        if (musicButtonText != null)
            musicButtonText.text = musicOn ? "On" : "Off";
        if (musicSource != null)
            musicSource.mute = !musicOn;
    }

    public void UpdateSoundState()
    {
        if (soundButtonText != null)
            soundButtonText.text = soundOn ? "On" : "Off";
        foreach (var source in soundSources)
        {
            if (source != null)
                source.mute = !soundOn;
        }
    }

    public static bool AreSoundsEnabled()
    {
        return PlayerPrefs.GetInt("SoundOn", 1) == 1;
    }

    public void CloseSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void ShowSettingsPanel()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(true);
    }
}
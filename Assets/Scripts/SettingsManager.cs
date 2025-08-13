using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    [Header("Botones de configuración")]
    public Button musicToggleButton;
    public TextMeshProUGUI musicButtonText;
    public GameObject settingsPanel; 
    public GameObject botonesPanel;

    public Button soundToggleButton;
    public TextMeshProUGUI soundButtonText;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioSource[] soundSources;

    [Header("Texto High Score")]
    public TextMeshProUGUI highScoreText;

    private bool musicOn;
    private bool soundOn;


    void Start()
    {


        int highScore = PlayerPrefs.GetInt("BestScore", 0);
        if (highScoreText != null)
            highScoreText.text = highScore.ToString("0000");

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
        // Lee el estado actual
        int musicBool = PlayerPrefs.GetInt("Musica", 1);

        if (musicBool == 0)
        {
            // Apagar música
            musicButtonText.text = "Off";
            musicSource.mute = true;
            PlayerPrefs.SetInt("Musica", 1);
        }
        else if (musicBool == 1)
        {
            // Encender música
            musicButtonText.text = "On";
            musicSource.mute = false;
            PlayerPrefs.SetInt("Musica", 0);
        }

        PlayerPrefs.Save();
    }

    public void UpdateSoundState()
    {

        // Lee el estado actual
        int soundBool = PlayerPrefs.GetInt("Sonido", 1);

        if (soundBool == 0)
        {
            // Apagar música
            soundButtonText.text = "Off";
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = true; // Mutear
            }
            PlayerPrefs.SetInt("Sonido", 1);
        }
        else if (soundBool == 1)
        {
            // Encender música
            soundButtonText.text = "On";
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = false; // Desmutear
            }
            PlayerPrefs.SetInt("Sonido", 0);
        }

        PlayerPrefs.Save();
    }

    public static bool AreSoundsEnabled()
    {
        return PlayerPrefs.GetInt("SoundOn", 1) == 1;
    }

    public void OnCerrarBtnPressed(){
        settingsPanel.SetActive(false);
    }

  
}
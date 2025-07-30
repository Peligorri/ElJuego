using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    public static SettingsController Instance;

    public GameObject settingsPanelPrefab;
    private GameObject currentPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel); // Asegura que no persista mal entre escenas
        }

        currentPanel = Instantiate(settingsPanelPrefab);
        currentPanel.SetActive(false);
    }

    public void ShowSettings()
    {
        if (currentPanel != null)
            currentPanel.SetActive(true);
    }

    public void HideSettings()
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);
    }
}
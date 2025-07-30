using UnityEngine;

public class PersistentIniitializer : MonoBehaviour
{
    public GameObject persistentPrefab;

    void Awake()
    {
        if (SettingsController.Instance == null)
        {
            GameObject persistentInstance = Instantiate(persistentPrefab);
            DontDestroyOnLoad(persistentInstance);
        }
    }
}
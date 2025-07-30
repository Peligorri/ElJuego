using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public GameObject instruccionesPanel; 
    public GameObject botonesPanel; 
  

    public void OnPlayPressed() {
    	SceneManager.LoadScene("GameScene");
    }

    public void OnInstructionsPressed(){
    	instruccionesPanel.SetActive(true);
        botonesPanel.SetActive(false);
    }

    public void OnCerrarBtnPressed(){
        instruccionesPanel.SetActive(false);
        botonesPanel.SetActive(true);
    }

    public void OnSettingsPressed(){
    	if (SettingsController.Instance != null)
            SettingsController.Instance.ShowSettings();
        else
            Debug.LogWarning("No se encontró el SettingsManager.");
    }

    public void OnQuitPressed(){
    	Application.Quit();
    }

}

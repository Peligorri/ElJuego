using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

    public GameObject instruccionesPanel; 
    public GameObject dificultadPanel; 
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
        dificultadPanel.SetActive(false);
        botonesPanel.SetActive(true);
    }

    public void OnSettingsPressed(){
    	if (SettingsController.Instance != null)
            SettingsController.Instance.ShowSettings();
        else
            Debug.LogWarning("No se encontró el SettingsManager.");
    }

    public void OnDifficultPressed(){
        dificultadPanel.SetActive(true);
        botonesPanel.SetActive(false);
    }

    public void OnQuitPressed(){
    	Application.Quit();
    }

}

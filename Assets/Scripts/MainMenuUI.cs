using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;

public class MainMenuUI : MonoBehaviour {

    public GameObject instruccionesPanel; 
    public GameObject dificultadPanel; 
    public GameObject botonesPanel; 
    public TextMeshProUGUI dificultadTxt;
  
    void Awake(){

        int dificultadActual = PlayerPrefs.GetInt("Dificultad", 0);

        if(!PlayerPrefs.HasKey("Dificultad")){
            PlayerPrefs.SetInt("Dificultad", 0);
            PlayerPrefs.Save();
        }else{
            if (dificultadActual == 0){
            dificultadTxt.text = "Fácil";
            } 
            else if (dificultadActual == 1){
                dificultadTxt.text = "Normal";
            } else if (dificultadActual == 2){
                dificultadTxt.text = "Difícil";
            }
        }
        
    }

    
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

    public void CambioDificultadNext(){

        int dificultadActual = PlayerPrefs.GetInt("Dificultad", 0);
        
        if (dificultadActual == 0){
            dificultadTxt.text = "Normal";
            PlayerPrefs.SetInt("Dificultad", 1);
            PlayerPrefs.Save(); 
        } 
        else if(dificultadActual == 1){
            dificultadTxt.text = "Difícil";
            PlayerPrefs.SetInt("Dificultad", 2);
            PlayerPrefs.Save();
        } else if (dificultadActual == 2){
            dificultadTxt.text = "Fácil";
            PlayerPrefs.SetInt("Dificultad", 0);
            PlayerPrefs.Save();
        }
    }

    public void CambioDificultadBack(){

        int dificultadActual = PlayerPrefs.GetInt("Dificultad", 0);
        
        if (dificultadActual == 0){
            dificultadTxt.text = "Díficil";
            PlayerPrefs.SetInt("Dificultad", 2);
            PlayerPrefs.Save(); 
        } 
        else if(dificultadActual == 1){
            dificultadTxt.text = "Fácil";
            PlayerPrefs.SetInt("Dificultad", 0);
            PlayerPrefs.Save();
        } else if (dificultadActual == 2){
            dificultadTxt.text = "Normal";
            PlayerPrefs.SetInt("Dificultad", 1);
            PlayerPrefs.Save();
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using TMPro;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public GameObject instruccionesPanel; 
    public GameObject settingsPanel; 
    public GameObject dificultadPanel; 
    public GameObject botonesPanel; 
    public TextMeshProUGUI dificultadTxt;
    public Button musicToggleButton;
    public TextMeshProUGUI musicButtonText;
    public Button soundToggleButton;
    public TextMeshProUGUI soundButtonText;
    public AudioSource musicSource;
    public AudioSource[] soundSources;
  
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

        int musicBool = PlayerPrefs.GetInt("Musica", 1); // 1 = encendida por defecto

        if (musicBool == 0)
        {
            // Música apagada
            musicButtonText.text = "Off";
            musicSource.mute = true;
        }
        else if (musicBool == 1)
        {
            // Música encendida
            musicButtonText.text = "On";
            musicSource.mute = false;
        }


        int soundBool = PlayerPrefs.GetInt("Sonido", 1); // 1 = encendido por defecto

        if (soundBool == 0)
        {
            // Sonido apagado
            soundButtonText.text = "Off";
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = true;
            }
        }
        else if (soundBool == 1)
        {
            // Sonido encendido
            soundButtonText.text = "On";
            foreach (var source in soundSources)
            {
                if (source != null)
                    source.mute = false;
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
        settingsPanel.SetActive(true);
        //botonesPanel.SetActive(false);
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

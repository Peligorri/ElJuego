using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Collections;
using System.Globalization;

public class GameManager : MonoBehaviour{
	 [Header("UI References")]
    public TextMeshProUGUI[] chipTexts;
    public TextMeshProUGUI definitionText;
    public TMP_InputField answerInput;
    public Button checkButton;
    public Button passButton;
    public TextMeshProUGUI feedbackText;
    public TextAsset jsonFile;
    public GameObject summaryPanel;
    public GameObject gameCanvas;
    public Transform summaryContent;
    public GameObject summaryItemPrefab;

    [Header("Data")]
    public List<WordEntry> allWords; // Puedes rellenarlo a mano en el inspector por ahora

    private List<WordEntry> gameWords = new List<WordEntry>();
    private WordEntry currentWord;

    private List<WordEntry> orderedPending = new List<WordEntry>();
    private int pendingIndex = 0;

    public Sprite correctIcon;
    public Sprite incorrectIcon;
    bool seRindio = false;
    int palabrasAcertadas = 0;
    public TextMeshProUGUI scoreText; 
    private bool shuffledPending = false;

    public GameObject definitionPanel;       // Panel que se muestra con la definición
    public TextMeshProUGUI palabraText2; 
    public TextMeshProUGUI definitionText2; 
    public GameObject tablaPanel;

    public GameObject starFull1;
    public GameObject starFull2;
    public GameObject starFull3;

    public AudioSource audioSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public AudioClip coinClip;
    public AudioClip btnClip;

    public GameObject persistentObjectPrefab;
    


    int lastGuessedIndex = -1;

    float startTime;

    void Awake(){
        if (SettingsController.Instance == null)
        {
            GameObject obj = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(obj);
        }
    }

    void Start(){

    	LoadWordsFromJson();
    	StartGame();
    	checkButton.onClick.AddListener(CheckAnswer);
    	passButton.onClick.AddListener(NextDefinition);
        startTime = Time.time;
    }

    void StartGame(){

    	gameWords = GetRandomWords(7);
    	UpdateChips();
    	NextDefinition();
        answerInput.ActivateInputField();

    }


    public void OpenSettingsPanel()
    {
        if (SettingsController.Instance != null)
            SettingsController.Instance.ShowSettings();
        else
            Debug.LogWarning("No se encontró el SettingsManager.");
    }


    List<WordEntry> GetRandomWords(int count)
    {
        // Leemos la dificultad actual guardada
        int dificultadActual = PlayerPrefs.GetInt("Dificultad", 0);

        string filtroDificultad = "";

        // Asignamos el texto según el número
        if (dificultadActual == 0) filtroDificultad = "facil";
        else if (dificultadActual == 1) filtroDificultad = "normal";
        else if (dificultadActual == 2) filtroDificultad = "dificil";

        // Filtramos la lista original según dificultad
        List<WordEntry> palabrasFiltradas = allWords
            .Where(w => w.dificultad == filtroDificultad)
            .ToList();

        // Copia para sacar palabras aleatorias sin repetición
        List<WordEntry> copy = new List<WordEntry>(palabrasFiltradas);
        List<WordEntry> result = new List<WordEntry>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }

    void UpdateChips(int lastGuessedIndex = -1){

        for (int i = 0; i < chipTexts.Length; i++)
        {
            if (i < gameWords.Count)
            {
                string prefix = gameWords[i].GetPrefix();

                if (gameWords[i].guessed)
                {
                    string tached = $"<s>{prefix}</s>";
                    if (chipTexts[i].text != tached)
                        chipTexts[i].text = tached;

                    chipTexts[i].color = Color.gray;

                    GameObject chipParent = chipTexts[i].transform.parent.gameObject;

                    if (i == lastGuessedIndex)
                    {
                        Animation anim = chipParent.GetComponent<Animation>();
                        anim.Stop();
                        anim.Play("ChipAnimation");
                    }
                }
                else
                {
                    if (chipTexts[i].text != prefix)
                        chipTexts[i].text = prefix;

                    chipTexts[i].color = Color.white;

                    if (!chipTexts[i].gameObject.activeSelf)
                        chipTexts[i].gameObject.SetActive(true);
                }
            }
            else
            {
                chipTexts[i].text = "";
                chipTexts[i].color = Color.white;

                if (!chipTexts[i].gameObject.activeSelf)
                    chipTexts[i].gameObject.SetActive(true);
            }
        }
    }

    void NextDefinition(){

           
        // Cada vez actualizamos la lista con las palabras no acertadas
        orderedPending = gameWords.FindAll(w => !w.guessed);

        if (orderedPending.Count == 0) {
            GameOver();
            return;
        }

        // Barajar la lista solo la primera vez que quedan palabras pendientes
        if (!shuffledPending) {
            for (int i = 0; i < orderedPending.Count; i++) {
                var temp = orderedPending[i];
                int rand = Random.Range(i, orderedPending.Count);
                orderedPending[i] = orderedPending[rand];
                orderedPending[rand] = temp;
            }
            shuffledPending = true;
            pendingIndex = 0;
        }

        // Saltar palabras acertadas (por si alguna se marcó entre llamadas)
        while (pendingIndex < orderedPending.Count && orderedPending[pendingIndex].guessed) {
            pendingIndex++;
        }

        if (pendingIndex >= orderedPending.Count) {
            pendingIndex = 0;
        }

        currentWord = orderedPending[pendingIndex];
        pendingIndex++;

        definitionText.text = currentWord.definition;
        answerInput.text = "";
        feedbackText.text = "";
        answerInput.ActivateInputField();
    }

    void CheckAnswer() {
        string userInput = RemoveAccents(answerInput.text.Trim().ToLower());
        string correctAnswer = RemoveAccents(currentWord.lemma.ToLower());

        if (userInput == correctAnswer)
        {
            currentWord.guessed = true;
            feedbackText.text = "¡Correcto!";

            int index = gameWords.IndexOf(currentWord);
            audioSource.PlayOneShot(correctClip);
            UpdateChips(index);   

            Invoke(nameof(NextDefinition), 1.5f);
        }
        else
        {
            feedbackText.text = "Incorrecto";
            audioSource.PlayOneShot(wrongClip);
        }
    }

    string RemoveAccents(string input) {
        string normalized = input.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char c in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }

    void GameOver(){

        definitionText.text = "¡Has adivinado todas las palabras!";
        answerInput.interactable = false;
        checkButton.interactable = false;
        passButton.interactable = false;

        ShowSummary();
    }
   
    void LoadWordsFromJson() {

	  if (jsonFile != null) {
            WordEntryList list = JsonUtility.FromJson<WordEntryList>(jsonFile.text);
            allWords = new List<WordEntry>(list.words);  // Si allWords es una List<WordEntry>
        } else {
            Debug.LogError("No se asignó el archivo JSON.");
        }
	}

   void ShowSummary() {
        float elapsedTime = Time.time - startTime;
        palabrasAcertadas = gameWords.Count(w => w.guessed);
        int estrellas;
        int finalScore = CalculateFinalScore(elapsedTime, palabrasAcertadas, seRindio, out estrellas);

        summaryPanel.SetActive(true);
        gameCanvas.SetActive(false);
        StartCoroutine(AnimateScore(finalScore));

        // Asegúrate de ocultar las estrellas al principio
        starFull1.SetActive(false);
        starFull2.SetActive(false);
        starFull3.SetActive(false);

        // Mostrar estrellas animadas
        StartCoroutine(ShowStars(estrellas));

        foreach (Transform child in summaryContent)
        {
            Destroy(child.gameObject);
        }
        audioSource.PlayOneShot(coinClip);

        foreach (var word in gameWords)
        {
            GameObject item = Instantiate(summaryItemPrefab, summaryContent);
            TextMeshProUGUI[] texts = item.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 2)
            {
                texts[0].text = word.GetPrefix();
                texts[1].text = Capitalize(word.lemma);
            }

            Image resultIcon = item.transform.Find("Canvas/ResultIcon").GetComponent<Image>();
            resultIcon.sprite = word.guessed ? correctIcon : incorrectIcon;

            SummaryItemButton sib = item.GetComponent<SummaryItemButton>();
            if (sib != null)
            {
                sib.Initialize(word.lemma, word.definition, (pal, def) => ShowDefinition(pal, def));
            }
        }
    }

    void ShowDefinition(string palabra, string def){

        audioSource.PlayOneShot(btnClip);
        palabraText2.text = palabra;
        definitionText2.text = def;
        definitionPanel.SetActive(true);
        
        tablaPanel.SetActive(false);
    }

    public void CloseDefinitionPanel(){
        definitionPanel.SetActive(false);
        tablaPanel.SetActive(true);
    }

    public void Surrender(){

        seRindio = true;

        definitionText.text = "Has decidido rendirte.";
        answerInput.interactable = false;
        checkButton.interactable = false;
        passButton.interactable = false;

        ShowSummary(); // Muestra la pantalla final con el resumen
    }

    public void BackToMenu(){
        SceneManager.LoadScene("MainMenu"); // Asegúrate de que el nombre coincide
    }
    
    public void RestartGame(){
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    [System.Serializable]
    public class TimeScoreEntry {

        public float timeLimitSeconds;
        public int pointsAllCorrect;
        public int pointsSurrendered;
    }

   int CalculateFinalScore(float elapsedTimeSeconds, int wordsGuessed, bool surrendered, out int stars) {
        stars = 0;

        if (wordsGuessed <= 0)
            return 0;

        int baseScore = wordsGuessed * 100;
        bool allCorrect = (wordsGuessed == 7 && !surrendered);
        int bonusAllCorrect = allCorrect ? 500 : 0;

        int timeBonus = Mathf.Max(0, 300 - (int)elapsedTimeSeconds) * 2;
        if (!allCorrect)
            timeBonus /= 3;

        int totalScore = baseScore + bonusAllCorrect + timeBonus;

        // Asignar estrellas
        if (totalScore >= 1500) stars = 3;
        else if (totalScore >= 700) stars = 2;
        else if (totalScore > 100) stars = 1;

        if (totalScore > PlayerPrefs.GetInt("BestScore", 0)){
            PlayerPrefs.SetInt("BestScore", totalScore);
            PlayerPrefs.Save(); 
        }

        return totalScore;
    }

    IEnumerator ShowStars(int count) {
        yield return new WaitForSeconds(1f); // Espera inicial antes de empezar

        if (count >= 1)
        {
            starFull1.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        if (count >= 2)
        {
            starFull2.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        if (count == 3)
        {
            starFull3.SetActive(true);
        }
    }

    IEnumerator AnimateScore(int finalScore) {
        int currentScore = 0;
        float duration = 0.75f; // Duración total de la animación en segundos
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            currentScore = Mathf.RoundToInt(Mathf.Lerp(0, finalScore, progress));
            scoreText.text = currentScore.ToString();
            yield return null;
        }

        scoreText.text = finalScore.ToString(); // Asegura que quede exacto al final
    }

    string Capitalize(string input) {
        if (string.IsNullOrEmpty(input)) return input;
        input = input.ToLower();
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input);
    }
}
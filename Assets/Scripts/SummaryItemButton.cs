using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SummaryItemButton : MonoBehaviour
{
    public TextMeshProUGUI wordText;    // Texto que mostrará la palabra
    public Button infoButton;            // Botón que abrirá la definición

    private string palabra;
    private string definition;
    private Action<string, string> onShowDefinition;  // Acción que recibe palabra y definición

    // Inicializa el item con la palabra, definición y acción para mostrar definición
    public void Initialize(string palabra, string def, Action<string, string> onClickAction)
    {
        this.palabra = palabra;
        this.definition = def;
        this.onShowDefinition = onClickAction;

        if (wordText != null)
        {
            wordText.text = palabra;
        }

        if (infoButton != null)
        {
            infoButton.onClick.RemoveAllListeners();
            // Al pulsar, llama la acción pasando palabra y definición
            infoButton.onClick.AddListener(() => onShowDefinition?.Invoke(this.palabra, this.definition));
        }
    }
}
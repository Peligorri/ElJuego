using UnityEngine;
using System.Globalization;
using System.Text;

[System.Serializable]
public class WordEntry{

    public WordEntry[] words;
    public string lemma;
    public string definition;
    public bool guessed;
    public string dificultad;

    public string GetPrefix() {

         string normalized = lemma.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char c in normalized) {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark) {
                sb.Append(c);
            }
        }

        string sinTildes = sb.ToString().Normalize(NormalizationForm.FormC);
        return sinTildes.Substring(0, 3).ToUpper();
    }
}
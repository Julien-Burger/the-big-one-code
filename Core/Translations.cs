using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Translations : MonoBehaviour
{
    Dictionary<string, Dictionary<int, string>> translations = new();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadTranslations();
    }

    private void LoadTranslations()
    {
        string path = ""; // .csv path
        string[] lines = System.IO.File.ReadAllLines(path);
        string[] translationName = new string[2];
        int indexToAdd = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string[] columns = lines[i].Split(',');

            for (int y = 0; y < columns.Length; y++)
            {
                if (string.IsNullOrEmpty(columns[1]))
                {
                    indexToAdd++;

                    break;
                }

                if (i == 0)
                {
                    for (int u = 0; u < translationName.Length; u++)
                    {
                        if (string.IsNullOrEmpty(translationName[u]))
                        {
                            translationName[u] = columns[y];

                            break;
                        }
                    }

                    translations.Add(columns[y], new Dictionary<int, string>());
                }
                else
                {
                    Dictionary<int, string> value = translations[translationName[y]];

                    value.Add(value.Count + 2 + indexToAdd, columns[y]);
                    translations.Remove(translationName[y]);
                    translations.Add(translationName[y], value);
                }
            }
        }
    }

    public string PickText(SystemLanguage language, int index)
    {
        foreach (KeyValuePair<string, Dictionary<int, string>> item in translations)
        {
            if (language.ToString() == item.Key)
            {
                foreach (KeyValuePair<int, string> text in item.Value)
                {
                    if (text.Key == index)
                    {
                        return text.Value;
                    }
                }
            }
        }

        return "ERROR";
    }
}

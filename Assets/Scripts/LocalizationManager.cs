using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager
{
    private const string LocalizationFilePath = "localization";

    private class LanguageDis
    {
        public string English;
        public string Russian;
    }

    public enum Language
    {
        English,
        Russian,
    }

    public static LocalizationManager Instnance { get; } = new LocalizationManager();

    private static Language currentLanguage = Language.English;

    private static Dictionary<string, Dictionary<Language, string>> langStrings = new Dictionary<string, Dictionary<Language, string>>();

    public event Action<Language> OnGameLocalizationChanged = l => { };

    public Language CurrentLanguage => currentLanguage;

    public void Init()
    {
        ReadJsonLocalizationStrings();
    }

    public static string GetString(string key)
    {
        return langStrings[key][currentLanguage];
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;
        OnGameLocalizationChanged(currentLanguage);
    }

    private void ReadJsonLocalizationStrings()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(LocalizationFilePath);
        string t = textAsset.text;
        var diserializedStrings = JsonConvert.DeserializeObject<Dictionary<string, LanguageDis>>(t);

        foreach(var diserializedString in diserializedStrings)
        {
            var dict = new Dictionary<Language, string>();
            dict.Add(Language.Russian, diserializedString.Value.Russian);
            dict.Add(Language.English, diserializedString.Value.English);
            langStrings.Add(diserializedString.Key, dict);
        }
    }
}

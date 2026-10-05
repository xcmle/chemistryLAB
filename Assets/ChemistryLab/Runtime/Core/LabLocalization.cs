using UnityEngine;

namespace ChemistryLab.Desktop
{
    public enum LabLanguage
    {
        Vietnamese = 0,
        English = 1
    }

    public static class LabLocalization
    {
        private const string LanguagePreferenceKey = "chemistryLab.desktop.language";
        private const string MobileEnglishMigrationKey = "chemistryLab.mobile.englishMigration.v2";

        public static LabLanguage Current
        {
            get
            {
                if (Application.isMobilePlatform
                    && PlayerPrefs.GetInt(MobileEnglishMigrationKey, 0) == 0)
                {
                    PlayerPrefs.SetInt(LanguagePreferenceKey, (int)LabLanguage.English);
                    PlayerPrefs.SetInt(MobileEnglishMigrationKey, 1);
                    PlayerPrefs.Save();
                }

                var defaultLanguage = Application.isMobilePlatform
                    ? LabLanguage.English
                    : LabLanguage.Vietnamese;
                return PlayerPrefs.GetInt(
                    LanguagePreferenceKey,
                    (int)defaultLanguage) == (int)LabLanguage.English
                    ? LabLanguage.English
                    : LabLanguage.Vietnamese;
            }
            set
            {
                PlayerPrefs.SetInt(LanguagePreferenceKey, (int)value);
                PlayerPrefs.Save();
            }
        }

        public static bool IsEnglish
        {
            get { return Current == LabLanguage.English; }
        }

        public static string Text(string vietnamese, string english)
        {
            return IsEnglish ? english : vietnamese;
        }

        public static void Toggle()
        {
            Current = IsEnglish ? LabLanguage.Vietnamese : LabLanguage.English;
        }
    }
}

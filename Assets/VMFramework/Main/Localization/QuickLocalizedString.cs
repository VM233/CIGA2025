using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

namespace VMFramework.Localization
{
    public struct QuickLocalizedString
    {
        private readonly TableReference tableReference;
        private readonly Locale locale;
        private readonly string key;
        private readonly string defaultValue;
        
        private StringTableEntry entry;

        public QuickLocalizedString(TableReference tableReference, Locale locale, string key,
            string defaultValue)
        {
            this.tableReference = tableReference;
            this.locale = locale;
            this.key = key;
            this.defaultValue = defaultValue;
            entry = null;
        }

        public QuickLocalizedString(TableReference tableReference, string key, string defaultValue)
        {
            this.tableReference = tableReference;
            locale = null;
            this.key = key;
            this.defaultValue = defaultValue;
            entry = null;
        }

        public QuickLocalizedString(string key, string defaultValue)
        {
            tableReference = LocalizationTableNames.EDITOR;
            locale = null;
            this.key = key;
            this.defaultValue = defaultValue;
            entry = null;
        }
        
        public QuickLocalizedString(string key)
        {
            tableReference = LocalizationTableNames.EDITOR;
            locale = null;
            this.key = key;
            defaultValue = key;
            entry = null;
        }

        public string GetLocalizedValue()
        {
            if (entry == null)
            {
                StringTable table;
                if (locale == null)
                {
                    table = LocalizationSettings.StringDatabase
                        .GetTable(tableReference, LocalizationSettings.ProjectLocale);
                }
                else
                {
                    table = LocalizationSettings.StringDatabase.GetTable(tableReference, locale);
                }

                if (table == null)
                {
                    return defaultValue;
                }
                
                entry = table.GetEntry(key);
            }
            
            if (entry == null)
            {
                return defaultValue;
            }

            return entry.GetLocalizedString();
        }
        
        public static implicit operator string(QuickLocalizedString localizedString)
        {
            return localizedString.GetLocalizedValue();
        }
    }
}
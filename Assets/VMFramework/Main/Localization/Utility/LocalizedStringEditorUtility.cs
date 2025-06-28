#if UNITY_EDITOR
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Localization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMFramework.Localization
{
    public static class LocalizedStringEditorUtility
    {
        public static string GetLocalizedStringInEditor(this LocalizedString localizedString)
        {
            StringTable table;
            var tableReference = localizedString.TableReference;

            if (tableReference.ReferenceType == TableReference.Type.Empty)
            {
                return null;
            }
            
            if (LocalizationSettings.ProjectLocale == null)
            {
                var collection =
                    LocalizationEditorSettings.GetStringTableCollection(tableReference);

                if (collection == null)
                {
                    table = null;
                }
                else
                {
                    table = collection.StringTables.First();
                }
            }
            else
            {
                table = LocalizationSettings.StringDatabase.GetTable(tableReference,
                    LocalizationSettings.ProjectLocale);
            }

            if (table == null)
            {
                return null;
            }

            StringTableEntry entry = null;
            var key = localizedString.TableEntryReference.Key;

            if (string.IsNullOrEmpty(key) == false)
            {
                entry = table.GetEntry(key);
            }

            if (entry == null)
            {
                var keyID = localizedString.TableEntryReference.KeyId;
                entry = table.GetEntry(keyID);
            }

            return entry?.GetLocalizedString();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(ref LocalizedString localizedString, TableReference tableReference, string key,
            string content, bool replace)
        {
            if (tableReference.ReferenceType == TableReference.Type.Empty)
            {
                return;
            }
            
            localizedString ??= new LocalizedString()
            {
                WaitForCompletion = true
            };
            localizedString.TableReference = tableReference;
            localizedString.SetDefaultKey(key, content, replace);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(this LocalizedString localizedString, string key, string content, bool replace)
        {
            localizedString.SetDefaultKey(localizedString.TableReference, key, content, replace);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetDefaultKey(this LocalizedString localizedString, TableReference reference, string key,
            string content, bool replace)
        {
            if (StringTableCollectionUtility.TryGetStringTableCollection(reference, out var stringTableCollection) ==
                false)
            {
                Debugger.LogWarning($"Could not find string table collection with reference: {reference}");
                return;
            }

            if (key.IsNullOrWhiteSpace())
            {
                Debugger.LogWarning("Key cannot be empty.");
                return;
            }

            foreach (var stringTable in stringTableCollection.StringTables)
            {
                var entry = stringTable.GetEntry(key);

                if (entry != null)
                {
                    if (entry.GetLocalizedString().IsNullOrWhiteSpace() == false && replace == false)
                    {
                        continue;
                    }
                }

                stringTable.AddEntry(key, content);

                stringTable.SetEditorDirty();
            }

            stringTableCollection.SetEditorDirty();
            stringTableCollection.SharedData.SetEditorDirty();

            localizedString.TableEntryReference = key;
        }
    }
}
#endif
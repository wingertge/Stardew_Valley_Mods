using System;
using System.Collections.Generic;
using System.IO;
using StardewValley;

namespace Vocalization.Framework
{
    /// <summary>A class which deals with handling different translations for Vocalization should other voice teams ever wish to voice act for that language.</summary>
    public class TranslationInfo
    {
        /// <summary>The list of all supported translations by this mod.</summary>
        public List<string> translations;

        /// <summary>The current translation mode for the mod, so that it knows what files to load at the beginning of the game.</summary>
        public string currentTranslation;

        /// <summary>Holds the info for what translation has what file extension.</summary>
        public Dictionary<string, string> translationFileInfo;


        public Dictionary<string, LocalizedContentManager.LanguageCode> translationCodes;
        /// <summary>Default constructor.</summary>
        public TranslationInfo()
        {
            this.translations = new List<string>();

            this.translationFileInfo = new Dictionary<string, string>();
            this.translationCodes = new Dictionary<string, LocalizedContentManager.LanguageCode>();
            this.translations.Add("English");
            this.translations.Add("Spanish");
            this.translations.Add("Chinese");
            this.translations.Add("Japanese");
            this.translations.Add("Russian");
            this.translations.Add("German");
            this.translations.Add("Brazillian Portuguese");

            this.currentTranslation = "English";

            this.translationFileInfo.Add("English", ".xnb");
            this.translationFileInfo.Add("Spanish", ".es-ES.xnb");
            this.translationFileInfo.Add("Chinese", ".zh-CN.xnb");
            this.translationFileInfo.Add("Japanese", ".ja-JP.xnb");
            this.translationFileInfo.Add("Russian", ".ru-RU.xnb");
            this.translationFileInfo.Add("German", ".de-DE.xnb");
            this.translationFileInfo.Add("Brazillian Portuguese", ".pt-BR.xnb");
            
            this.translationCodes.Add("English", LocalizedContentManager.LanguageCode.en);
            this.translationCodes.Add("Spanish", LocalizedContentManager.LanguageCode.es);
            this.translationCodes.Add("Chinese", LocalizedContentManager.LanguageCode.zh);
            this.translationCodes.Add("Japanese", LocalizedContentManager.LanguageCode.ja);
            this.translationCodes.Add("Russian", LocalizedContentManager.LanguageCode.ru);
            this.translationCodes.Add("German", LocalizedContentManager.LanguageCode.de);
            this.translationCodes.Add("Brazillian Portuguese", LocalizedContentManager.LanguageCode.pt);
        }

        public string getTranslationNameFromPath(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }

        public void changeLocalizedContentManagerFromTranslation(string translation)
        {
            string tra = this.getTranslationNameFromPath(translation);
            LocalizedContentManager.CurrentLanguageCode = !this.translationCodes.TryGetValue(tra, out LocalizedContentManager.LanguageCode code)
                ? LocalizedContentManager.LanguageCode.en
                : code;
            return;
        }

        public void resetLocalizationCode()
        {
            LocalizedContentManager.CurrentLanguageCode = LocalizedContentManager.LanguageCode.en;
        }

        /// <summary>Gets the proper file extension for the current translation.</summary>
        public string getFileExtentionForTranslation(string path)
        {
            /*
            bool f = translationFileInfo.TryGetValue(translation, out string value);
            if (!f) return ".xnb";
            else return value;
            */
            string translation = Path.GetFileName(path);
            try
            {
                return this.translationFileInfo[translation];
            }
            catch (Exception err)
            {

                Vocalization.ModMonitor.Log(err.ToString());
                Vocalization.ModMonitor.Log("Attempted to get translation: " + translation);
                return ".xnb";
            }
        }

        /// <summary>Gets the proper XNB for Buildings (aka Blueprints) from the data folder.</summary>
        public string getBuildingXNBForTranslation(string translation)
        {
            string buildings = "Blueprints";
            return buildings + this.getFileExtentionForTranslation(translation);
        }

        /// <summary>Gets the proper XNB file for the name passed in. Combines the file name with it's proper translation extension.</summary>
        public string getXNBForTranslation(string xnbFileName, string translation)
        {
            return xnbFileName + this.getFileExtentionForTranslation(translation);
        }

        /// <summary>Loads an XNB file from StardewValley/Content</summary>
        public string LoadXNBFile(string xnbFileName, string key, string translation)
        {
            string xnb = xnbFileName + this.getFileExtentionForTranslation(translation);
            Dictionary<string, string> loadedDict = Game1.content.Load<Dictionary<string, string>>(xnb);

            if (!loadedDict.TryGetValue(key, out string loaded))
            {
                Vocalization.ModMonitor.Log("Big issue: Key not found in file:" + xnb + " " + key);
                return "";
            }
            return loaded;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2, object sub3)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2, sub3);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1, object sub2)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1, sub2);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation, object sub1)
        {
            string format = this.LoadString(path, translation);
            try
            {
                return string.Format(format, sub1);
            }
            catch { }

            return format;
        }

        public virtual string LoadString(string path, string translation)
        {
            this.parseStringPath(path, out string assetName, out string key);

            return this.LoadXNBFile(assetName, key, translation);
        }

        public virtual string LoadString(string path, string translation, params object[] substitutions)
        {
            string format = this.LoadString(path, translation);
            if (substitutions.Length != 0)
            {
                try
                {
                    return string.Format(format, substitutions);
                }
                catch { }
            }
            return format;
        }
        
        private void parseStringPath(string path, out string assetName, out string key)
        {
            int length = path.IndexOf(':');
            assetName = path.Substring(0, length);
            key = path.Substring(length + 1, path.Length - length - 1);
        }
    }
}

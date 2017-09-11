using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace KaptainsLogNamespace
{
    public static class GlobalSettings
    {
        private static ConfigNode settingsFile = null;
        private static ConfigNode allSettings = null;

        private static string PLUGINDATA = KaptainsLog.MOD_FOLDER + "PluginData/GlobalSettings.cfg";
        
        public static string NODENAME = "KaptainsLog";

        

#if false
        public GlobalSettings()
        {
            Log.Info("GlobalSettings constructor");
            LoadSettings();
        }
#endif
        public static bool UpdateNode(string name, ConfigNode node, bool addIfNotFound = true, bool saveAfterUpdate = true)
        {
            bool b = false;
            if (settingsFile != null && allSettings != null)
            {
                b = allSettings.SetNode(name, node, addIfNotFound);
                if (b && saveAfterUpdate)
                    SaveSettings();
            }
            
            return b;
        }

        public static ConfigNode FetchNode(string name, bool loadIfNeeded = true)
        {
            Log.Info("FetchNode: " + name);

            ConfigNode settings = null;
            
            if ((settingsFile == null || allSettings == null) && loadIfNeeded)
                LoadSettings();
            if (settingsFile != null && allSettings != null)
                settings = allSettings.GetNode(name);

            return settings;
        }

        public static void SaveSettings()
        {
            Log.Info("SaveSettings");
            if (settingsFile == null)
                Log.Info("settignsFile is null");
            if (allSettings == null)
                Log.Info("allSettings is null");
            if (settingsFile != null && allSettings != null)
            {
                settingsFile.Save(PLUGINDATA);
                ScreenMessages.PostScreenMessage("Global settings saved");
            }
        }

        public static void LoadSettings()
        {
            Log.Info("Global settings loaded: " + PLUGINDATA);
            ScreenMessages.PostScreenMessage("Global settings loaded");
            settingsFile = ConfigNode.Load(PLUGINDATA);

            if (settingsFile == null)
            {
                settingsFile = new ConfigNode();
                allSettings = new ConfigNode();
                settingsFile.SetNode(KaptainsLog.Instance.NODENAME, allSettings, true);
            }
            else
            {
                allSettings = settingsFile.GetNode(KaptainsLog.Instance.NODENAME);
                if (allSettings == null)
                    Log.Info("allSetting is null after loading");
            }
        }


        public static void SaveGlobalSettings()
        {
            Log.Info("SaveGlobalSettings");
            //allSettings.AddValue("screenshotCnt", screenshotCnt);
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4General)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().SaveGlobalSettingsNode();
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_21>().useGlobalSettings4EventPause)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_21>().SaveGlobalSettingsNode();
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_22>().useGlobalSettings4EventCapture)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_22>().SaveGlobalSettingsNode();
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().useGlobalSettings4EventScreenshots)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().SaveGlobalSettingsNode();
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_12>().useGlobalSettings4InitialDisplay)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_12>().SaveGlobalSettingsNode();
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().useGlobalSettings4Misc)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().SaveGlobalSettingsNode();
            SaveSettings();
        }

        public static void LoadGlobalSettings()
        {
            Log.Info("LoadGlobalSettings");
            LoadSettings();
            //screenshotCnt = Int32.Parse(Utils.SafeLoad(allSettings.GetValue("screenshotCnt"), "0"));
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4General)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().LoadGlobalSettingsNode();

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_21>().useGlobalSettings4EventPause)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_21>().LoadGlobalSettingsNode();

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_22>().useGlobalSettings4EventCapture)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_22>().LoadGlobalSettingsNode();

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().useGlobalSettings4EventScreenshots)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().LoadGlobalSettingsNode();

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_12>().useGlobalSettings4InitialDisplay)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_12>().LoadGlobalSettingsNode();

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useGlobalSettings4All || HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().useGlobalSettings4Misc)
                HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().LoadGlobalSettingsNode();
        }
    }
}

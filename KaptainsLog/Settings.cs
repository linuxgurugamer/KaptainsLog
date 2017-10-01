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
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class MMCheck : MonoBehaviour
    {
        public static bool atMainMenu = false;

        public static bool EnabledForSave;
        public static bool useGlobalSettings4InitialDisplay;
        public static bool useGlobalSettings4Misc;
        public static bool useGlobalSettings4General;
        public static bool useGlobalSettings4All;
        public static bool useGlobalSettings4EventPause;
        public static bool useGlobalSettings4EventCapture;
        public static bool useGlobalSettings4EventScreenshots;

        void Start()
        {
            Log.Info("MMCheck.Start");
            atMainMenu = true;

            EnabledForSave = true;
            useGlobalSettings4All = true;
            useGlobalSettings4InitialDisplay = true;
            useGlobalSettings4Misc = true;
            useGlobalSettings4General = true;
            useGlobalSettings4EventPause = true;
            useGlobalSettings4EventCapture = true;
            useGlobalSettings4EventScreenshots = true;
        }

        void OnDestroy()
        {
            atMainMenu = false;
            Log.Info("MMCheck.OnDestroy");
        }

    }
    // http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
    // search for "Mod integration into Stock Settings

    public class KL_11 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "General Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Mod Enabled")]
        public bool EnabledForSave = true;      // is enabled for this save file

        [GameParameters.CustomParameterUI("Use global settings for ALL settings")]
        public bool useGlobalSettings4All = true;

        [GameParameters.CustomParameterUI("Use global settings for general settings")]
        public bool useGlobalSettings4General = true;


        [GameParameters.CustomParameterUI("Show intro window at Startup")]
        public bool showIntroAtStartup = true;

        [GameParameters.CustomParameterUI("Use Blizzy toolbar if installed")]
        public bool useBlizzy = false;

        [GameParameters.CustomParameterUI("Keep windows on screen",
            toolTip = "Prevents windows from being dragged to be off-screen, even partially")]
        public bool keepOnScreen = true;

        //[GameParameters.CustomParameterUI("Override Pause menu in Flight scene")]
        //public bool overridePause = true;

        //[GameParameters.CustomParameterUI("Screenshot at log entry",
        //    toolTip = "See the Event Screenshot Settings to set screenshot options for specific events (in Kaptain's Log 2 section)")]
        //public bool screenshot = true;

        [GameParameters.CustomParameterUI("Save images in save folder",
            toolTip = "The screenshots and thumbnails will be saved in a folder called 'KaptainsLogScreenshots' in the save folder")]
        public bool saveScreenshotsInSaveFolder = true;

        [GameParameters.CustomParameterUI("Save thumbnails in sub-folder",
            toolTip = "The screenshots and thumbnails will be saved in a sub-folder inside the directory where the screenshots are saved")]
        public bool saveThumbnailsInSubFolder = true;


        //[GameParameters.CustomParameterUI("Hide UI for screenshot",
        //    toolTip = "The screen will flicker when the screenshot is taken if this is enabled")]
        //public bool hideUIforScreenshot = false;

        [GameParameters.CustomParameterUI("Save screenshot as JPG",
            toolTip = "Screenshots are saved as PNG files, select this to convert it to JPG")]
        public bool saveAsJPEG = false;

        [GameParameters.CustomParameterUI("Save PNG screenshot",
            toolTip = "If saving as JPG, this will keep the original PNG file.  The PNG file has\nbetter color detail, there is some minor loss converting to JPG")]
        public bool keepPNG = true;

        [GameParameters.CustomIntParameterUI("Thumbnail size", minValue = 24, maxValue = 200)]
        public int thumbnailSize = 120;

        [GameParameters.CustomParameterUI("Use Bilinear filtering for thumbnail",
                    toolTip = "If false, will use point filtering")]
        public bool useBilinear = true;


        [GameParameters.CustomFloatParameterUI("Delay before screenshot & notes", minValue = 0.0f, maxValue = 5.0f)]
        public double delayBeforePause = 2.5f;

        [GameParameters.CustomFloatParameterUI("Minimum time between log entries", minValue = 0.0f, maxValue = 300f)]
        public double minTime = 60f;

        //[GameParameters.CustomParameterUI("Save through reverts")]
        //public bool saveThroughReverts = true;

        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_11 SaveGlobalSettingsNode");
            ConfigNode settings = new ConfigNode("KL_11");
            settings.AddValue("showIntroAtStartup", showIntroAtStartup);
            settings.AddValue("useBlizzy", useBlizzy);
            settings.AddValue("keepOnScreen", keepOnScreen);

            //settings.AddValue("screenshot", screenshot);
            settings.AddValue("saveScreenshotsInSaveFolder", saveScreenshotsInSaveFolder);
            settings.AddValue("saveThumbnailsInSubFolder", saveThumbnailsInSubFolder);
            //settings.AddValue("hideUIforScreenshot", hideUIforScreenshot);
            settings.AddValue("saveAsJPEG", saveAsJPEG);
            settings.AddValue("keepPNG", keepPNG);
            settings.AddValue("thumbnailSize", thumbnailSize);
            settings.AddValue("useBilinear", useBilinear);
            settings.AddValue("delayBeforePause", delayBeforePause);
            settings.AddValue("minTime", minTime);

            GlobalSettings.UpdateNode("KL_11", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_11.LoadGlobalSettingsNode");
            var settings = GlobalSettings.FetchNode("KL_11");
            if (settings == null)
            {
                Log.Info("Unable to load KL_11 settings");
                return;
            }

            showIntroAtStartup = Boolean.Parse(Utils.SafeLoad(settings.GetValue("showIntroAtStartup"), "true"));
            useBlizzy = Boolean.Parse(Utils.SafeLoad(settings.GetValue("useBlizzy"), "false"));
            keepOnScreen = Boolean.Parse(Utils.SafeLoad(settings.GetValue("keepOnScreen"), "true"));
            //screenshot = Boolean.Parse(Utils.SafeLoad(settings.GetValue("screenshot"), "true"));
            saveScreenshotsInSaveFolder = Boolean.Parse(Utils.SafeLoad(settings.GetValue("saveScreenshotsInSaveFolder"), "true"));
            saveThumbnailsInSubFolder = Boolean.Parse(Utils.SafeLoad(settings.GetValue("saveThumbnailsInSubFolder"), "true"));
//            hideUIforScreenshot = Boolean.Parse(Utils.SafeLoad(settings.GetValue("hideUIforScreenshot"), "true"));
            saveAsJPEG = Boolean.Parse(Utils.SafeLoad(settings.GetValue("saveAsJPEG"), "true"));
            keepPNG = Boolean.Parse(Utils.SafeLoad(settings.GetValue("keepPNG"), "true"));
            thumbnailSize = Int32.Parse(Utils.SafeLoad(settings.GetValue("thumbnailSize"), "120"));
            useBilinear = Boolean.Parse(Utils.SafeLoad(settings.GetValue("useBilinear"), "true"));

            delayBeforePause = Double.Parse(Utils.SafeLoad(settings.GetValue("delayBeforePause"), "120"));
            minTime = Double.Parse(Utils.SafeLoad(settings.GetValue("minTime"), "60"));
        }

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            useGlobalSettings4All = true;
            showIntroAtStartup = true;
            keepOnScreen = true;
            EnabledForSave = true;      // is enabled for this save file
            useBlizzy = false;
            //screenshot = true;
            //hideUIforScreenshot = false;
            saveAsJPEG = false;
            keepPNG = true;
            thumbnailSize = 120;
            useBilinear = true;
            delayBeforePause = 2.5f;
            minTime = 60f;
            saveScreenshotsInSaveFolder = true;
            saveThumbnailsInSubFolder = true;
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4All && !MMCheck.useGlobalSettings4All)
            {
                GlobalSettings.LoadGlobalSettings();
                useGlobalSettings4All = true;
                useGlobalSettings4General = true;
                MMCheck.useGlobalSettings4EventCapture = true;
                MMCheck.useGlobalSettings4EventPause = true;
                MMCheck.useGlobalSettings4EventScreenshots = true;
                MMCheck.useGlobalSettings4InitialDisplay = true;
                MMCheck.useGlobalSettings4Misc = true;
            }
                
            if (useGlobalSettings4General && !MMCheck.useGlobalSettings4General )
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4General = true;
            }
            MMCheck.EnabledForSave = EnabledForSave;
            MMCheck.useGlobalSettings4General = useGlobalSettings4General;
            MMCheck.useGlobalSettings4All = useGlobalSettings4All;

            if (member.Name == "EnabledForSave")
                return true;

            return MMCheck.EnabledForSave; //otherwise return true
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (member.Name == "useGlobalSettings4General" || member.Name == "EnabledForSave")
                return true;

            if (MMCheck.EnabledForSave)
            {
                if (member.Name == "useGlobalSettings4All" || member.Name == "useGlobalSettings4General")
                    return true;
                if (MMCheck.atMainMenu && (MMCheck.useGlobalSettings4All || MMCheck.useGlobalSettings4General))
                        return false;
            }

            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


    public class KL_21 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return " Event Pause Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Use global settings for event pause")]
        public bool useGlobalSettings4EventPause = true;



        [GameParameters.CustomParameterUI("Pause on all events")]
        public bool pauseOnAllEvents = false;

        [GameParameters.CustomParameterUI("Unset all pause settings")]
        public bool unsetSettings = false;



        [GameParameters.CustomParameterUI("Pause on Vessel Rollout")]
        public bool pauseOnVesselRollout = false;

        [GameParameters.CustomParameterUI("Pause on landing")]
        public bool pauseOnVesselLanded = false;

        [GameParameters.CustomParameterUI("Pause on splashdown")]
        public bool pauseOnVesselSplashdown = false;

        [GameParameters.CustomParameterUI("Pause on crash or splashdown")]
        public bool pauseOnCrashSplashdown = false;

        [GameParameters.CustomParameterUI("Pause on vessel recovered")]
        public bool pauseOnVesselRecovered = false;

        [GameParameters.CustomParameterUI("Pause on launch")]
        public bool pauseOnLaunch = false;

        [GameParameters.CustomParameterUI("Pause on stage separation")]
        public bool pauseOnStageSeparation = false;

        [GameParameters.CustomParameterUI("Pause on stage activate")]
        public bool pauseOnStageActivate = false;

        [GameParameters.CustomParameterUI("Pause on part dying")]
        public bool pauseOnPartDie = false;

        [GameParameters.CustomParameterUI("Pause on disconnected part dying")]
        public bool pauseOnDisconnectedPartDie = false;

        [GameParameters.CustomParameterUI("Pause on part couple (docking)")]
        public bool pauseOnPartCouple = false;

        [GameParameters.CustomParameterUI("Pause on vessel was modified")]
        public bool pauseOnVesselWasModified = false;

        [GameParameters.CustomParameterUI("Pause on crew modified (EVA)")]
        public bool pauseOnVesselCrewWasModified = false;

        [GameParameters.CustomParameterUI("Pause on closing into orbit")]
        public bool pauseOnVesselOrbitClosed = false;

        [GameParameters.CustomParameterUI("Pause on escaping orbit")]
        public bool pauseOnVesselOrbitEscaped = false;

        [GameParameters.CustomParameterUI("Pause on crew killed")]
        public bool pauseOnCrewKilled = false;

        [GameParameters.CustomParameterUI("Pause on crew transferred")]
        public bool pauseOnCrewTransferred = false;

        [GameParameters.CustomParameterUI("Pause on SOI change")]
        public bool pauseOnDominantBodyChange = false;

        [GameParameters.CustomParameterUI("Pause on flag plant")]
        public bool pauseOnFlagPlant = false;

        [GameParameters.CustomParameterUI("Pause on Crew EVA")]
        public bool pauseOnCrewOnEVA = false;


        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_21 SaveGlobalSettingsNode");

            ConfigNode settings = new ConfigNode("KL_21");

            
            settings.AddValue("pauseOnVesselRollout", pauseOnVesselRollout);
            settings.AddValue("pauseOnVesselLanded", pauseOnVesselLanded);
            settings.AddValue("pauseOnVesselSplashdown", pauseOnVesselSplashdown);
            settings.AddValue("pauseOnCrashSplashdown", pauseOnCrashSplashdown);
            settings.AddValue("pauseOnVesselRecovered", pauseOnVesselRecovered);
            settings.AddValue("pauseOnLaunch", pauseOnLaunch);
            settings.AddValue("pauseOnStageSeparation", pauseOnStageSeparation);
            settings.AddValue("pauseOnStageActivate", pauseOnStageActivate);
            settings.AddValue("pauseOnPartDie", pauseOnPartDie);
            settings.AddValue("pauseOnDisconnectedPartDie", pauseOnDisconnectedPartDie);
            settings.AddValue("pauseOnPartCouple", pauseOnPartCouple);
            settings.AddValue("pauseOnVesselWasModified", pauseOnVesselWasModified);
            settings.AddValue("pauseOnVesselCrewWasModified", pauseOnVesselCrewWasModified);
            settings.AddValue("pauseOnVesselOrbitClosed", pauseOnVesselOrbitClosed);
            settings.AddValue("pauseOnVesselOrbitEscaped", pauseOnVesselOrbitEscaped);
            settings.AddValue("pauseOnCrewKilled", pauseOnCrewKilled);
            settings.AddValue("pauseOnCrewTransferred", pauseOnCrewTransferred);
            settings.AddValue("pauseOnDominantBodyChange", pauseOnDominantBodyChange);
            settings.AddValue("pauseOnFlagPlant", pauseOnFlagPlant);
            settings.AddValue("pauseOnCrewOnEVA", pauseOnCrewOnEVA);
            settings.AddValue("pauseOnVesselOrbitEscaped", pauseOnVesselOrbitEscaped);

            GlobalSettings.UpdateNode("KL_21", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_21.LoadGlobalSettingsNode");
            var settings = GlobalSettings.FetchNode("KL_21");
            if (settings == null)
            {
                Log.Info("Unable to load KL_21 settings");
                return;
            }

            pauseOnVesselRollout = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselRollout"), "false"));
            pauseOnVesselLanded = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselLanded"), "false"));
            pauseOnVesselSplashdown = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselSplashdown"), "false"));
            pauseOnCrashSplashdown = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnCrashSplashdown"), "false"));
            pauseOnVesselRecovered = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselRecovered"), "false"));
            pauseOnLaunch = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnLaunch"), "false"));
            pauseOnStageSeparation = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnStageSeparation"), "false"));
            pauseOnStageActivate = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnStageActivate"), "false"));
            pauseOnPartDie = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnPartDie"), "false"));
            pauseOnDisconnectedPartDie = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnDisconnectedPartDie"), "false"));
            pauseOnPartCouple = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnPartCouple"), " false"));
            pauseOnVesselWasModified = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselWasModified"), "false"));
            pauseOnVesselCrewWasModified = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselCrewWasModified"), "false"));
            pauseOnVesselOrbitClosed = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselOrbitClosed"), "false"));
            pauseOnVesselOrbitEscaped = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselOrbitEscaped"), "true"));
            pauseOnCrewKilled = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnCrewKilled"), "false"));
            pauseOnCrewTransferred = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnCrewTransferred"), " false"));
            pauseOnDominantBodyChange = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnDominantBodyChange"), " false"));
            pauseOnFlagPlant = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnFlagPlant"), "false"));
            pauseOnCrewOnEVA = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnCrewOnEVA"), "false"));
            pauseOnVesselOrbitEscaped = Boolean.Parse(Utils.SafeLoad(settings.GetValue("pauseOnVesselOrbitEscaped"), "false"));
        }

        void SetAllPauseOptions(bool b)
        {
            pauseOnAllEvents = false;
            unsetSettings = false;
            pauseOnVesselRollout = b;
            pauseOnVesselLanded = b;
            pauseOnVesselSplashdown = b;
            pauseOnCrashSplashdown = b;
            pauseOnVesselRecovered = b;
            pauseOnLaunch = b;
            pauseOnStageSeparation = b;
            pauseOnStageActivate = b;
            pauseOnPartDie = b;
            pauseOnDisconnectedPartDie = b;
            pauseOnPartCouple = b;
            pauseOnVesselWasModified = b;
            pauseOnVesselCrewWasModified = b;
            pauseOnVesselOrbitClosed = b;
            pauseOnVesselOrbitEscaped = b;
            pauseOnCrewKilled = b;
            pauseOnCrewTransferred = b;
            pauseOnDominantBodyChange = b;
            pauseOnFlagPlant = b;
            pauseOnCrewOnEVA = b;
            pauseOnVesselOrbitEscaped = b;
        }
        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            SetAllPauseOptions(false);
        }
        void enableAllPause()
        {
            SetAllPauseOptions(true);
        }
        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4EventPause && !MMCheck.useGlobalSettings4EventPause)
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4EventPause = true;
            }
            MMCheck.useGlobalSettings4EventPause = useGlobalSettings4EventPause;
            return MMCheck.EnabledForSave;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4All)
                return false;

            if (member.Name == "useGlobalSettings4EventPause")
                return true;
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4EventPause)
                    return false;
            
            if (member.Name == "pauseOnAllEvents")
                return true;
            if (pauseOnAllEvents)
                enableAllPause();
            if (unsetSettings)
            {
                SetAllPauseOptions(false);
            }
            return !pauseOnAllEvents;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


    public class KL_22 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Event Capture Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Use global settings for event capture")]
        public bool useGlobalSettings4EventCapture = true;


        [GameParameters.CustomParameterUI("Log all events")]
        public bool logAllEvents = true;
        [GameParameters.CustomParameterUI("Unset all event log settings")]
        public bool unsetSettings = false;


        [GameParameters.CustomParameterUI("Log on vessel rollout")]
        public bool logOnVesselRollout = true;

        [GameParameters.CustomParameterUI("Log on landing")]
        public bool logOnVesselLanded = true;

        [GameParameters.CustomParameterUI("Log on crash or splashdown")]
        public bool logOnCrashSplashdown = true;

        [GameParameters.CustomParameterUI("Log on vessel recovered")]
        public bool logOnVesselRecovered = true;

        [GameParameters.CustomParameterUI("Log on launch")]
        public bool logOnLaunch = true;

        [GameParameters.CustomParameterUI("Log on stage separation")]
        public bool logOnStageSeparation = true;

        [GameParameters.CustomParameterUI("Log on stage activate")]
        public bool logOnStageActivate = true;

        [GameParameters.CustomParameterUI("Log on part dying")]
        public bool logOnPartDie = true;

        [GameParameters.CustomParameterUI("Log on disconnected part dying")]
        public bool logOnDisconnectedPartDie = true;

        [GameParameters.CustomParameterUI("Log on part couple (docking)")]
        public bool logOnPartCouple = true;

        [GameParameters.CustomParameterUI("Log on vessel was modified")]
        public bool logOnVesselWasModified = true;

        [GameParameters.CustomParameterUI("Log on crew modified (EVA)")]
        public bool logOnVesselCrewWasModified = true;

        [GameParameters.CustomParameterUI("Log on closing into orbit")]
        public bool logOnVesselOrbitClosed = true;

        [GameParameters.CustomParameterUI("Log on escaping orbit")]
        public bool logOnVesselOrbitEscaped = true;

        [GameParameters.CustomParameterUI("Log on crew killed")]
        public bool logOnCrewKilled = true;

        [GameParameters.CustomParameterUI("Log on crew transferred")]
        public bool logOnCrewTransferred = true;

        [GameParameters.CustomParameterUI("Log on SOI change")]
        public bool logOnDominantBodyChange = true;

        [GameParameters.CustomParameterUI("Log on flag plant")]
        public bool logOnFlagPlant = true;

        [GameParameters.CustomParameterUI("Log on Crew EVA")]
        public bool logOnCrewOnEVA = true;

        [GameParameters.CustomStringParameterUI("Record-only Events Below", autoPersistance = true, lines = 2, title = "Record-only Events Below", toolTip = "")]
        public string UIstring = "";

        /* Following don't have equivilent Pause options */
        [GameParameters.CustomParameterUI("Log on Kerbal passed out from G-force")]
        public bool logOnKerbalPassedOutFromGeeForce = true;

        [GameParameters.CustomParameterUI("Log on Flight Log Recorded")]
        public bool logOnFlightLogRecorded = true;

        [GameParameters.CustomParameterUI("Log on Progress Achievement")]
        public bool logOnProgressAchieve = true;

        [GameParameters.CustomParameterUI("Log on Progress Completion")]
        public bool logOnProgressComplete = true;

        [GameParameters.CustomParameterUI("Log on Science Changed")]
        public bool logOnScienceChanged = true;

        [GameParameters.CustomParameterUI("Log on Science Received")]
        public bool logOnScienceReceived = true;

        [GameParameters.CustomParameterUI("Log on Orbital Survey Completed")]
        public bool logOnOrbitalSurveyCompleted = true;

        [GameParameters.CustomParameterUI("Log on reputation changed")]
        public bool logOnReputationChanged = true;

        [GameParameters.CustomParameterUI("Log on triggered data transmission")]
        public bool logOnTriggeredDataTransmission = true;

        [GameParameters.CustomParameterUI("Log on part upgrade purchased")]
        public bool logOnPartUpgradePurchased = true;

        [GameParameters.CustomParameterUI("Log on part purchase")]
        public bool logOnPartPurchased = true;

        [GameParameters.CustomParameterUI("Log on funds changed")]
        public bool logOnFundsChanged = true;


        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_22 SaveGlobalSettingsNode");

            ConfigNode settings = new ConfigNode("KL_22");

            //settings.AddValue("logAllEvents", logAllEvents);
            settings.AddValue("logOnVesselLanded", logOnVesselLanded);
            settings.AddValue("logOnCrashSplashdown", logOnCrashSplashdown);
            settings.AddValue("logOnVesselRecovered", logOnVesselRecovered);
            settings.AddValue("logOnLaunch", logOnLaunch);
            settings.AddValue("logOnStageSeparation", logOnStageSeparation);
            settings.AddValue("logOnStageActivate", logOnStageActivate);
            settings.AddValue("logOnPartDie", logOnPartDie);
            settings.AddValue("logOnDisconnectedPartDie", logOnDisconnectedPartDie);
            settings.AddValue("logOnPartCouple", logOnPartCouple);
            settings.AddValue("logOnVesselWasModified", logOnVesselWasModified);
            settings.AddValue("logOnVesselCrewWasModified", logOnVesselCrewWasModified);
            settings.AddValue("logOnVesselOrbitClosed", logOnVesselOrbitClosed);
            settings.AddValue("logOnVesselOrbitEscaped", logOnVesselOrbitEscaped);
            settings.AddValue("logOnCrewKilled", logOnCrewKilled);
            settings.AddValue("logOnCrewTransferred", logOnCrewTransferred);
            settings.AddValue("logOnDominantBodyChange", logOnDominantBodyChange);
            settings.AddValue("logOnFlagPlant", logOnFlagPlant);
            settings.AddValue("logOnCrewOnEVA", logOnCrewOnEVA);
            settings.AddValue("logOnScienceChanged", logOnScienceChanged);
            settings.AddValue("logOnScienceReceived", logOnScienceReceived);
            settings.AddValue("logOnOrbitalSurveyCompleted", logOnOrbitalSurveyCompleted);
            settings.AddValue("logOnReputationChanged", logOnReputationChanged);
            settings.AddValue("logOnTriggeredDataTransmission", logOnTriggeredDataTransmission);
            settings.AddValue("logOnVesselRollout", logOnVesselRollout);
            settings.AddValue("logOnPartUpgradePurchased", logOnPartUpgradePurchased);
            settings.AddValue("logOnPartPurchased", logOnPartPurchased);
            settings.AddValue("logOnFundsChanged", logOnFundsChanged);
            settings.AddValue("logOnKerbalPassedOutFromGeeForce", logOnKerbalPassedOutFromGeeForce);
            settings.AddValue("logOnFlightLogRecorded", logOnFlightLogRecorded);
            settings.AddValue("logOnProgressAchieve", logOnProgressAchieve);
            settings.AddValue("logOnProgressComplete", logOnProgressComplete);

            GlobalSettings.UpdateNode("KL_22", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_22.LoadGlobalSettingsNode");

            var settings = GlobalSettings.FetchNode("KL_22");
            if (settings == null)
            {
                Log.Info("Unable to load KL_22 settings");
                return;
            }
            
            //logAllEvents = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logAllEvents"), "true"));
            logOnVesselLanded = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselLanded"), "true"));
            logOnCrashSplashdown = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnCrashSplashdown"), "true"));
            logOnVesselRecovered = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselRecovered"), "true"));
            logOnLaunch = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnLaunch"), "true"));
            logOnStageSeparation = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnStageSeparation"), "true"));
            logOnStageActivate = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnStageActivate"), "true"));
            logOnPartDie = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnPartDie"), "true"));
            logOnDisconnectedPartDie = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnDisconnectedPartDie"), "true"));
            logOnPartCouple = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnPartCouple"), "true"));
            logOnVesselWasModified = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselWasModified"), "true"));
            logOnVesselCrewWasModified = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselCrewWasModified"), "true"));
            logOnVesselOrbitClosed = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselOrbitClosed"), "true"));
            logOnVesselOrbitEscaped = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselOrbitEscaped"), "true"));
            logOnCrewKilled = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnCrewKilled"), "true"));
            logOnCrewTransferred = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnCrewTransferred"), "true"));
            logOnDominantBodyChange = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnDominantBodyChange"), "true"));
            logOnFlagPlant = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnFlagPlant"), "true"));
            logOnCrewOnEVA = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnCrewOnEVA"), "true"));
            logOnScienceChanged = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnScienceChanged"), "true"));
            logOnScienceReceived = Boolean.Parse(Utils.SafeLoad(settings.GetValue("showIntroAtStartup"), "true"));
            logOnOrbitalSurveyCompleted = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnOrbitalSurveyCompleted"), "true"));
            logOnReputationChanged = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnReputationChanged"), "true"));
            logOnTriggeredDataTransmission = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnTriggeredDataTransmission"), "true"));
            logOnVesselRollout = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnVesselRollout"), "true"));
            logOnPartUpgradePurchased = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnPartUpgradePurchased"), "true"));
            logOnPartPurchased = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnPartPurchased"), "true"));
            logOnFundsChanged = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnFundsChanged"), "true"));
            logOnKerbalPassedOutFromGeeForce = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnKerbalPassedOutFromGeeForce"), "true"));
            logOnFlightLogRecorded = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnFlightLogRecorded"), "true"));
            logOnProgressAchieve = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnProgressAchieve"), "true"));
            logOnProgressComplete = Boolean.Parse(Utils.SafeLoad(settings.GetValue("logOnProgressComplete"), "true"));
        }

        void SetAllLogOptions(bool b)
        {
            logAllEvents = false;
            unsetSettings = false;
            
            logOnVesselLanded = b;
            logOnCrashSplashdown = b;
            logOnVesselRecovered = b;
            logOnLaunch = b;
            logOnStageSeparation = b;
            logOnStageActivate = b;
            logOnPartDie = b;
            logOnDisconnectedPartDie = b;
            logOnPartCouple = b;
            logOnVesselWasModified = b;
            logOnVesselCrewWasModified = b;
            logOnVesselOrbitClosed = b;
            logOnVesselOrbitEscaped = b;
            logOnCrewKilled = b;
            logOnCrewTransferred = b;
            logOnDominantBodyChange = b;
            logOnFlagPlant = b;
            logOnCrewOnEVA = b;
            logOnScienceChanged = b;
            logOnScienceReceived = b;
            logOnOrbitalSurveyCompleted = b;
            logOnReputationChanged = b;
            logOnTriggeredDataTransmission = b;
            logOnVesselRollout = b;
            logOnPartUpgradePurchased = b;
            logOnPartPurchased = b;
            logOnFundsChanged = b;
            logOnKerbalPassedOutFromGeeForce = b;
            logOnFlightLogRecorded = b;
            logOnProgressAchieve = b;
            logOnProgressComplete = b;
        }

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            SetAllLogOptions(true);
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4EventCapture && !MMCheck.useGlobalSettings4EventCapture)
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4EventCapture = true;
            }
            MMCheck.useGlobalSettings4EventCapture = useGlobalSettings4EventCapture;
            return MMCheck.EnabledForSave;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4All)
                return false;
            if (member.Name == "useGlobalSettings4EventCapture")
                return true;
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4EventCapture)
                    return false;
            if (member.Name == "logAllEvents")
                return true;
            if (logAllEvents)
                SetDifficultyPreset(GameParameters.Preset.Normal);
            if (unsetSettings)
            {
                SetAllLogOptions(false);
            }
            return !logAllEvents;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }

    /////////////////////////////
    public enum ScreenshotOptions { No_Screenshot, With_Gui, Without_Gui };
    public class KL_23 : GameParameters.CustomParameterNode
    {
        // following to be used if I add the ability to specify the gui status for each specific screenshow
       
        // Example code using an enum
        //[GameParameters.CustomParameterUI("Screenshot Option")]
        //public ScreenshotOptions MyEnum = ScreenshotOptions.With_Gui;


        public override string Title { get { return "Event Screenshot Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return false; } }

        [GameParameters.CustomParameterUI("Use global settings for event screenshots")]
        public bool useGlobalSettings4EventScreenshots = true;



        [GameParameters.CustomParameterUI("Screenshot on all events (with GUI)",
            toolTip = "All screenshot settings will be set to With_GUI.")]
        public bool screenshotOnAllEventsWithGUI = false;

        [GameParameters.CustomParameterUI("Screenshot on all events (without GUI)",
            toolTip = "Please be warned that the screen will flicker every time a screenshot is taken with these settings.")]
        public bool screenshotWithoutGUIOnAllEvents = false;

        [GameParameters.CustomParameterUI("Disable all screenshots")]
        public bool disableAllScreenshots = false;

        [GameParameters.CustomParameterUI("<color=yellow><b>Vessel Rollout (EVA)</b></color>")]
        public ScreenshotOptions screenshotOnVesselRollout = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Land</b></color>")]
        public ScreenshotOptions screenshotOnLanded = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Splashdown</b></color>")]
        public ScreenshotOptions screenshotOnSplashdown = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Crash</b></color>")]
        public ScreenshotOptions screenshotOnCrashSplashdown = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Vessel recovered</b></color>")]
        public ScreenshotOptions screenshotOnVesselRecovered = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Launch</b></color>")]
        public ScreenshotOptions screenshotOnLaunch = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Stage separation</b></color>")]
        public ScreenshotOptions screenshotOnStageSeparation = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Stage activate</b></color>")]
        public ScreenshotOptions screenshotOnStageActivate = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Part dying</b></color>",
            toolTip = "This applies to both connected and disconnected parts")]
        public ScreenshotOptions screenshotOnPartDie = ScreenshotOptions.With_Gui;

        // [GameParameters.CustomParameterUI("Screenshot on disconnected part dying")]
        // public bool screenshotOnDisconnectedPartDie = true;

        [GameParameters.CustomParameterUI("<color=yellow><b>Part couple (docking)</b></color>")]
        public ScreenshotOptions screenshotOnPartCouple = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Vessel was modified</b></color>")]
        public ScreenshotOptions screenshotOnVesselWasModified = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Crew modified (EVA)</b></color>")]
        public ScreenshotOptions screenshotOnVesselCrewWasModified = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Closing into orbit</b></color>")]
        public ScreenshotOptions screenshotOnVesselOrbitClosed = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Escaping orbit</b></color>")]
        public ScreenshotOptions screenshotOnVesselOrbitEscaped = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Crew killed</b></color>")]
        public ScreenshotOptions screenshotOnCrewKilled = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Crew transferred</b></color>")]
        public ScreenshotOptions screenshotOnCrewTransferred = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>SOI change</b></color>")]
        public ScreenshotOptions screenshotOnDominantBodyChange = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Flag plant</b></color>")]
        public ScreenshotOptions screenshotOnFlagPlant = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Crew EVA</b></color>")]
        public ScreenshotOptions screenshotOnCrewOnEVA = ScreenshotOptions.With_Gui;

        /* Following don't have equivilent Pause options */
        [GameParameters.CustomParameterUI("<color=yellow><b>Kerbal passed out (G-force)</b></color>")]
        public ScreenshotOptions screenshotOnKerbalPassedOutFromGeeForce = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Progress Achievement</b></color>")]
        public ScreenshotOptions screenshotOnProgressAchieve = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Progress Completion</b></color>")]
        public ScreenshotOptions screenshotOnProgressComplete = ScreenshotOptions.With_Gui;

        [GameParameters.CustomParameterUI("<color=yellow><b>Manual Entry</b></color>")]
        public ScreenshotOptions screenshotOnManualEntry;

        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_23 SaveGlobalSettingsNode");

            ConfigNode settings = new ConfigNode("KL_23");

            //settings.AddValue("screenshotOnAllEvents", screenshotOnAllEvents);
            settings.AddValue("screenshotOnLanded", screenshotOnLanded);
            settings.AddValue("screenshotOnSplashdown", screenshotOnSplashdown);
            settings.AddValue("screenshotOnCrashSplashdown", screenshotOnCrashSplashdown);
            settings.AddValue("screenshotOnVesselRecovered", screenshotOnVesselRecovered);
            settings.AddValue("screenshotOnLaunch", screenshotOnLaunch);
            settings.AddValue("screenshotOnStageSeparation", screenshotOnStageSeparation);
            settings.AddValue("screenshotOnStageActivate", screenshotOnStageActivate);
            settings.AddValue("screenshotOnPartDie", screenshotOnPartDie);
            settings.AddValue("screenshotOnPartCouple", screenshotOnPartCouple);
            settings.AddValue("screenshotOnVesselWasModified", screenshotOnVesselWasModified);
            settings.AddValue("screenshotOnVesselCrewWasModified", screenshotOnVesselCrewWasModified);
            settings.AddValue("screenshotOnVesselOrbitClosed", screenshotOnVesselOrbitClosed);
            settings.AddValue("screenshotOnVesselOrbitEscaped", screenshotOnVesselOrbitEscaped);
            settings.AddValue("screenshotOnCrewKilled", screenshotOnCrewKilled);
            settings.AddValue("screenshotOnCrewTransferred", screenshotOnCrewTransferred);
            settings.AddValue("screenshotOnDominantBodyChange", screenshotOnDominantBodyChange);
            settings.AddValue("screenshotOnFlagPlant", screenshotOnFlagPlant);
            settings.AddValue("screenshotOnCrewOnEVA", screenshotOnCrewOnEVA);
            settings.AddValue("screenshotOnKerbalPassedOutFromGeeForce", screenshotOnKerbalPassedOutFromGeeForce);
            settings.AddValue("screenshotOnProgressAchieve", screenshotOnProgressAchieve);
            settings.AddValue("screenshotOnProgressComplete", screenshotOnProgressComplete);
            settings.AddValue("screenshotOnVesselRollout", screenshotOnVesselRollout);
            settings.AddValue("screenshotOnVesselRollout", screenshotOnManualEntry);

            GlobalSettings.UpdateNode("KL_23", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_23.LoadGlobalSettingsNode");

            var settings = GlobalSettings.FetchNode("KL_23");
            if (settings == null)
            {
                Log.Info("Unable to load KL_23 settings");
                return;
            }


            //screenshotOnAllEvents = Boolean.Parse(Utils.SafeLoad(settings.GetValue("screenshotOnAllEvents"), "true"));
            screenshotOnLanded = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions), Utils.SafeLoad(settings.GetValue("screenshotOnLanded"), "With_Gui"));
            screenshotOnSplashdown = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions), Utils.SafeLoad(settings.GetValue("screenshotOnSplashdown"), "With_Gui"));
            screenshotOnCrashSplashdown = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnCrashSplashdown"), "With_Gui"));
            screenshotOnVesselRecovered = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnVesselRecovered"), "With_Gui"));
            screenshotOnLaunch = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnLaunch"), "With_Gui"));
            screenshotOnStageSeparation = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnStageSeparation"), "With_Gui"));
            screenshotOnStageActivate = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnStageActivate"), "With_Gui"));
            screenshotOnPartDie = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnPartDie"), "With_Gui"));
            screenshotOnPartCouple = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnPartCouple"), "With_Gui"));
            screenshotOnVesselWasModified = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnVesselWasModified"), "With_Gui"));
            screenshotOnVesselCrewWasModified = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnVesselCrewWasModified"), "With_Gui"));
            screenshotOnVesselOrbitClosed = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnVesselOrbitClosed"), "With_Gui"));
            screenshotOnVesselOrbitEscaped = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnVesselOrbitEscaped"), "With_Gui"));
            screenshotOnCrewKilled = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnCrewKilled"), "With_Gui"));
            screenshotOnCrewTransferred = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnCrewTransferred"), "With_Gui"));
            screenshotOnDominantBodyChange = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnDominantBodyChange"), "With_Gui"));
            screenshotOnFlagPlant = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnFlagPlant"), "With_Gui"));
            screenshotOnCrewOnEVA = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnCrewOnEVA"), "With_Gui"));
            screenshotOnKerbalPassedOutFromGeeForce = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnKerbalPassedOutFromGeeForce"), "With_Gui"));
            screenshotOnProgressAchieve = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions),Utils.SafeLoad(settings.GetValue("screenshotOnProgressAchieve"), "With_Gui"));
            screenshotOnProgressComplete = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions), Utils.SafeLoad(settings.GetValue("screenshotOnProgressComplete"), "With_Gui"));
            screenshotOnVesselRollout = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions), Utils.SafeLoad(settings.GetValue("screenshotOnVesselRollout"), "With_Gui"));
            screenshotOnManualEntry = (ScreenshotOptions)Enum.Parse(typeof(ScreenshotOptions), Utils.SafeLoad(settings.GetValue("screenshotOnManualEntry"), "With_Gui"));
        }


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        { }
        void SetAllScreenshotOptions(ScreenshotOptions s)
        {
            //screenshotOnAllEvents = true;
            screenshotOnLanded = s;
            screenshotOnSplashdown = s;
            screenshotOnCrashSplashdown = s;
            screenshotOnVesselRecovered = s;
            screenshotOnLaunch = s;
            screenshotOnStageSeparation = s;
            screenshotOnStageActivate = s;
            screenshotOnPartDie = s;
            //screenshotOnDisconnectedPartDie = true; // need to implement
            screenshotOnPartCouple = s;
            screenshotOnVesselWasModified = s;
            screenshotOnVesselCrewWasModified = s;
            screenshotOnVesselOrbitClosed = s;
            screenshotOnVesselOrbitEscaped = s;
            screenshotOnCrewKilled = s;
            screenshotOnCrewTransferred = s;
            screenshotOnDominantBodyChange = s;
            screenshotOnFlagPlant = s;
            screenshotOnCrewOnEVA = s;

            screenshotOnKerbalPassedOutFromGeeForce = s;
            screenshotOnProgressAchieve = s;
            screenshotOnProgressComplete = s;
            screenshotOnVesselRollout = s;
            screenshotOnManualEntry = s;
        }


        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4EventScreenshots && !MMCheck.useGlobalSettings4EventScreenshots)
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4EventScreenshots = true;
            }
            MMCheck.useGlobalSettings4EventScreenshots = useGlobalSettings4EventScreenshots;
            return MMCheck.EnabledForSave;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4All)
                return false;
            
            if (member.Name == "useGlobalSettings4EventScreenshots")
                return true;

            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4EventScreenshots)
                return false;

            

            if (screenshotOnAllEventsWithGUI)
            {
                SetAllScreenshotOptions(ScreenshotOptions.With_Gui);
                screenshotOnAllEventsWithGUI = false;
            }
            if (screenshotWithoutGUIOnAllEvents)
            {
                SetAllScreenshotOptions(ScreenshotOptions.Without_Gui);
                screenshotWithoutGUIOnAllEvents = false;
            }
            if (member.Name == "screenshotOnAllEvents" || member.Name == "screenshotWithutGUIOnAllEvents")
                return true;
            if (disableAllScreenshots)
            {
                disableAllScreenshots = false;
                SetAllScreenshotOptions(ScreenshotOptions.No_Screenshot); 
            }
            return !screenshotOnAllEventsWithGUI;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


}

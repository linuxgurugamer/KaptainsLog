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

    // http://forum.kerbalspaceprogram.com/index.php?/topic/147576-modders-notes-for-ksp-12/#comment-2754813
    // search for "Mod integration into Stock Settings

    public class KL_1 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "General Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Mod Enabled")]
        public bool EnabledForSave = true;      // is enabled for this save file

        [GameParameters.CustomParameterUI("Use Blizzy toolbar if installed")]
        public bool useBlizzy = false;

        [GameParameters.CustomParameterUI("Keep windows on screen",
            toolTip ="Prevents windows from being dragged to be off-screen, even partially")]
        public bool keepOnScreen = true;

        [GameParameters.CustomParameterUI("Override Pause menu in Flight scene")]
        public bool overridePause = true;

        [GameParameters.CustomParameterUI("Screenshot at log entry",
            toolTip ="See the Event Screenshot Settings to set screenshot options for specific events (in Kaptain's Log 2 section)")]
        public bool screenshot = true;

        [GameParameters.CustomParameterUI("Save images in save folder",
            toolTip = "The screenshots and thumbnails will be saved in a folder called 'KaptainsLogScreenshots' in the save folder")]
        public bool saveScreenshotsInSaveFolder = true;

        [GameParameters.CustomParameterUI("Save thumbnails in sub-folder",
            toolTip = "The screenshots and thumbnails will be saved in a sub-folder inside the directory where the screenshots are saved")]
        public bool saveThumbnailsInSubFolder = true;


        [GameParameters.CustomParameterUI("Hide UI for screenshot",
            toolTip = "The screen will flicker when the screenshot is taken if this is enabled")]
        public bool hideUIforScreenshot = true;

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



        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            EnabledForSave = true;      // is enabled for this save file
            useBlizzy = false;
            screenshot = true;
            hideUIforScreenshot = true;
            saveAsJPEG = true;
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
            if (member.Name == "EnabledForSave") //This Field must always be enabled.
                return true;

            return EnabledForSave; //otherwise return true
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


    public class KL_2 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return " Event Pause Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return true; } }


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




        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            pauseOnCrashSplashdown = false;
            pauseOnVesselRecovered = false;
            pauseOnLaunch = false;
            pauseOnStageSeparation = false;
            pauseOnStageActivate = false;
            pauseOnPartDie = false;
            pauseOnDisconnectedPartDie = false;
            pauseOnPartCouple = false;
            pauseOnVesselWasModified = false;
            pauseOnVesselCrewWasModified = false;
            pauseOnVesselOrbitClosed = false;
            pauseOnVesselOrbitEscaped = true;
            pauseOnCrewKilled = false;
            pauseOnCrewTransferred = false;
            pauseOnDominantBodyChange = false;
            pauseOnFlagPlant = false;
            pauseOnCrewOnEVA = false;

        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            try
            {
                return HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().EnabledForSave;
            } catch { return true; }
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


    public class KL_3 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Event Capture Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return true; } }


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

        /* Following don't have equivilent Pause options */
        [GameParameters.CustomParameterUI("Log on Kerbal passed out from G-force")]
        public bool logOnKerbalPassedOutFromGeeForce = true;

        [GameParameters.CustomParameterUI("Log on Flight Log Recorded")]
        public bool logOnFlightLogRecorded = true;

        [GameParameters.CustomParameterUI("Log on Progress Achievement")]
        public bool logOnProgressAchieve = true;

        [GameParameters.CustomParameterUI("Log on Progress Completion")]
        public bool logOnProgressComplete = true;




        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            logOnCrashSplashdown = true;
            logOnVesselRecovered = true;
            logOnLaunch = true;
            logOnStageSeparation = true;
            logOnStageActivate = true;
            logOnPartDie = true;
            logOnDisconnectedPartDie = true;
            logOnPartCouple = true;
            logOnVesselWasModified = true;
            logOnVesselCrewWasModified = true;
            logOnVesselOrbitClosed = true;
            logOnVesselOrbitEscaped = true;
            logOnCrewKilled = true;
            logOnCrewTransferred = true;
            logOnDominantBodyChange = true;
            logOnFlagPlant = true;
            logOnCrewOnEVA = true;

        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            try
            {
                return HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().EnabledForSave;
            }
            catch { return true; }
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }

    /////////////////////////////

    public class KL_4 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Event Screenshot Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return true; } }


        [GameParameters.CustomParameterUI("Screenshot on all events")]
        public bool screenshotOnAllEvents = true;

        [GameParameters.CustomParameterUI("Unset all screenshot settings")]
        public bool unsetSettings = false;


        [GameParameters.CustomParameterUI("Screenshot on landing, splashdown or crash")]
        public bool screenshotOnCrashSplashdown = true;

        [GameParameters.CustomParameterUI("Screenshot on vessel recovered")]
        public bool screenshotOnVesselRecovered = true;

        [GameParameters.CustomParameterUI("Screenshot on launch")]
        public bool screenshotOnLaunch = true;

        [GameParameters.CustomParameterUI("Screenshot on stage separation")]
        public bool screenshotOnStageSeparation = true;

        [GameParameters.CustomParameterUI("Screenshot on stage activate")]
        public bool screenshotOnStageActivate = true;

        [GameParameters.CustomParameterUI("Screenshot on part dying", 
            toolTip = "This applies to both connected and disconnected parts")]
        public bool screenshotOnPartDie = true;

       // [GameParameters.CustomParameterUI("Screenshot on disconnected part dying")]
       // public bool screenshotOnDisconnectedPartDie = true;

        [GameParameters.CustomParameterUI("Screenshot on part couple (docking)")]
        public bool screenshotOnPartCouple = true;

        [GameParameters.CustomParameterUI("Screenshot on vessel was modified")]
        public bool screenshotOnVesselWasModified = true;

        [GameParameters.CustomParameterUI("Screenshot on crew modified (EVA)")]
        public bool screenshotOnVesselCrewWasModified = true;

        [GameParameters.CustomParameterUI("Screenshot on closing into orbit")]
        public bool screenshotOnVesselOrbitClosed = true;

        [GameParameters.CustomParameterUI("Screenshot on escaping orbit")]
        public bool screenshotOnVesselOrbitEscaped = true;

        [GameParameters.CustomParameterUI("Screenshot on crew killed")]
        public bool screenshotOnCrewKilled = true;

        [GameParameters.CustomParameterUI("Screenshot on crew transferred")]
        public bool screenshotOnCrewTransferred = true;

        [GameParameters.CustomParameterUI("Screenshot on SOI change")]
        public bool screenshotOnDominantBodyChange = true;

        [GameParameters.CustomParameterUI("Screenshot on flag plant")]
        public bool screenshotOnFlagPlant = true;

        [GameParameters.CustomParameterUI("Screenshot on Crew EVA")]
        public bool screenshotOnCrewOnEVA = true;

        /* Following don't have equivilent Pause options */
        [GameParameters.CustomParameterUI("Screenshot on Kerbal passed out from G-force")]
        public bool screenshotOnKerbalPassedOutFromGeeForce = true;

        [GameParameters.CustomParameterUI("Screenshot on Flight Screenshot Recorded")]
        public bool screenshotOnFlightScreenshotRecorded = true;

        [GameParameters.CustomParameterUI("Screenshot on Progress Achievement")]
        public bool screenshotOnProgressAchieve = true;

        [GameParameters.CustomParameterUI("Screenshot on Progress Completion")]
        public bool screenshotOnProgressComplete = true;




        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            screenshotOnCrashSplashdown = true;
            screenshotOnVesselRecovered = true;
            screenshotOnLaunch = true;
            screenshotOnStageSeparation = true;
            screenshotOnStageActivate = true;
            screenshotOnPartDie = true;
            //screenshotOnDisconnectedPartDie = true; // need to implement
            screenshotOnPartCouple = true;
            screenshotOnVesselWasModified = true;
            screenshotOnVesselCrewWasModified = true;
            screenshotOnVesselOrbitClosed = true;
            screenshotOnVesselOrbitEscaped = true;
            screenshotOnCrewKilled = true;
            screenshotOnCrewTransferred = true;
            screenshotOnDominantBodyChange = true;
            screenshotOnFlagPlant = true;
            screenshotOnCrewOnEVA = true;

        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            try
            {
                return HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().EnabledForSave;
            }
            catch { return true; }
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (member.Name == "screenshotOnAllEvents") //This Field must always be Interactible.
                return true;
            if (screenshotOnAllEvents)
                SetDifficultyPreset(GameParameters.Preset.Normal);
            if (unsetSettings)
            {
                unsetSettings = false;
                screenshotOnCrashSplashdown = false;
                screenshotOnVesselRecovered = false;
                screenshotOnLaunch = false;
                screenshotOnStageSeparation = false;
                screenshotOnStageActivate = false;
                screenshotOnPartDie = false;
                //screenshotOnDisconnectedPartDie = false;
                screenshotOnPartCouple = false;
                screenshotOnVesselWasModified = false;
                screenshotOnVesselCrewWasModified = false;
                screenshotOnVesselOrbitClosed = false;
                screenshotOnVesselOrbitEscaped = false;
                screenshotOnCrewKilled = false;
                screenshotOnCrewTransferred = false;
                screenshotOnDominantBodyChange = false;
                screenshotOnFlagPlant = false;
                screenshotOnCrewOnEVA = false;
            }
            return !screenshotOnAllEvents;
            //            return true; //otherwise return true
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }


}

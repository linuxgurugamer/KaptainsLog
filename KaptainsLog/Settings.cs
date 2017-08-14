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

        [GameParameters.CustomParameterUI("Screenshot at log entry")]
        public bool screenshot = true;

        [GameParameters.CustomParameterUI("Save screenshots/thumbnails in save folder",
            toolTip ="The screenshots will be saved in a folder called 'KaptainsLogScreenshots' in the save folder")]
        public bool saveScreenshotsInSaveFolder = true;


        [GameParameters.CustomParameterUI("Hide UI for screenshot",
            toolTip = "The screen will flicker when the screenshot is taken if this is enabled")]
        public bool hideUIforScreenshot = true;

        [GameParameters.CustomParameterUI("Save screenshot as JPG",
            toolTip = "Screenshots are saved as PNG files, select this to convert it to JPG")]
        public bool saveAsJPEG = true;

        [GameParameters.CustomParameterUI("Save PNG screenshot",
            toolTip = "If saving as JPG, this will keep the original PNG file.  The PNG file has\nbetter color detail, there is some minor loss converting to JPG")]
        public bool keepPNG = true;

        [GameParameters.CustomIntParameterUI("Thumbnail size", minValue = 24, maxValue = 200)]
        public int thumbnailSize = 120;

        [GameParameters.CustomParameterUI("Use Bilinear filtering for thumbnail",
                    toolTip = "If false, will use point filtering")]
        public bool useBilinear = true;


        [GameParameters.CustomFloatParameterUI("Delay before pause for screenshot & notes", minValue = 0.0f, maxValue = 5.0f)]
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
        public override string Title { get { return "Event Settings"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
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

        [GameParameters.CustomParameterUI("Pause on flag plant")]
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
        public override string Title { get { return "Message Log"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomIntParameterUI("Max messages to display", minValue = 1, maxValue = 50)]
        public int maxMsgs = 20;

        [GameParameters.CustomIntParameterUI("Keep messages for (minutes):", minValue = 0, maxValue = 50)]
        public int expireMsgsAfter = 20;

        //[GameParameters.CustomParameterUI("Save messages to file")]
        //public bool saveMsgsToFile = false; 

            [GameParameters.CustomParameterUI("Hide when no messages",
            toolTip ="Hide the windw if it is open and the remaining messages are expired")]
        public bool hideWhenNoMsgs = false; 


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
                return true;
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

}

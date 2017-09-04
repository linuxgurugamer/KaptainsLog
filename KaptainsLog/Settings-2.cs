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
    public class KL_12 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Initial Display Columns"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return false; } }

        [GameParameters.CustomParameterUI("Use global settings for initial display columns")]
        public bool useGlobalSettings4InitialDisplay = true;


        [GameParameters.CustomParameterUI("Vessel name")]
        public bool vesselName = true;
        [GameParameters.CustomParameterUI("Universal time")]
        public bool universalTime = true;
        [GameParameters.CustomParameterUI("UTC time")]
        public bool utcTime = false;
        [GameParameters.CustomParameterUI("Mission time")]
        public bool missionTime = true;

        [GameParameters.CustomParameterUI("Vessel Situation")]
        public bool vesselSituation = true;
        [GameParameters.CustomParameterUI("Control Level")]
        public bool controlLevel = false;
        [GameParameters.CustomParameterUI("Main body")]
        public bool mainBody = true;
        [GameParameters.CustomParameterUI("Altitude")]
        public bool altitude = true;
        [GameParameters.CustomParameterUI("Speed")]
        public bool speed = true;

        [GameParameters.CustomParameterUI("Event Type")]
        public bool eventType = true;
        [GameParameters.CustomParameterUI("Notes")]
        public bool notes = false;
        [GameParameters.CustomParameterUI("Thumbnail")]
        public bool thumbnail = true;

        public bool screenshot = false;
        public bool lastItem = false;

        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_12 SaveGlobalSettingsNode");

            ConfigNode settings = new ConfigNode("KL_12");

            settings.AddValue("vesselName", vesselName);
            settings.AddValue("universalTime", universalTime);
            settings.AddValue("utcTime", utcTime);
            settings.AddValue("missionTime", missionTime);
            settings.AddValue("vesselSituation", vesselSituation);
            settings.AddValue("controlLevel", controlLevel);
            settings.AddValue("mainBody", mainBody);
            settings.AddValue("altitude", altitude);
            settings.AddValue("speed", speed);
            settings.AddValue("eventType", eventType);
            settings.AddValue("notes", notes);
            settings.AddValue("thumbnail", thumbnail);
            settings.AddValue("screenshot", screenshot);
            settings.AddValue("lastItem", lastItem);

            GlobalSettings.UpdateNode("KL_12", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_12.LoadGlobalSettingsNode");
            var settings = GlobalSettings.FetchNode("KL_12");
            if (settings == null)
            {
                Log.Info("Unable to load KL_12 settings");
                return;
            }


            vesselName = Boolean.Parse(Utils.SafeLoad(settings.GetValue("vesselName"), "true"));
            universalTime = Boolean.Parse(Utils.SafeLoad(settings.GetValue("universalTime"), "true"));
            utcTime = Boolean.Parse(Utils.SafeLoad(settings.GetValue("utcTime"), "true"));
            missionTime = Boolean.Parse(Utils.SafeLoad(settings.GetValue("missionTime"), "true"));
            vesselSituation = Boolean.Parse(Utils.SafeLoad(settings.GetValue("vesselSituation"), "true"));
            controlLevel = Boolean.Parse(Utils.SafeLoad(settings.GetValue("controlLevel"), "true"));
            mainBody = Boolean.Parse(Utils.SafeLoad(settings.GetValue("mainBody"), "true"));
            altitude = Boolean.Parse(Utils.SafeLoad(settings.GetValue("altitude"), "true"));
            speed = Boolean.Parse(Utils.SafeLoad(settings.GetValue("speed"), "true"));
            eventType = Boolean.Parse(Utils.SafeLoad(settings.GetValue("eventType"), "true"));
            notes = Boolean.Parse(Utils.SafeLoad(settings.GetValue("notes"), "true"));
            thumbnail = Boolean.Parse(Utils.SafeLoad(settings.GetValue("thumbnail"), "true"));
            screenshot = Boolean.Parse(Utils.SafeLoad(settings.GetValue("screenshot"), "true"));
            lastItem = Boolean.Parse(Utils.SafeLoad(settings.GetValue("lastItem"), "true"));
        }


        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4InitialDisplay && !MMCheck.useGlobalSettings4InitialDisplay)
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4InitialDisplay = true;
            }
            MMCheck.useGlobalSettings4InitialDisplay = useGlobalSettings4InitialDisplay;
            
            return MMCheck.EnabledForSave;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4All)
                return false;
            if (member.Name == "useGlobalSettings4InitialDisplay")
                return true;
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4InitialDisplay)
                    return false;
            
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }
    public class KL_13 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Misc"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log"; } }
        public override string DisplaySection { get { return "Kaptain's Log"; } }
        public override int SectionOrder { get { return 3; } }
        public override bool HasPresets { get { return false; } }


        [GameParameters.CustomParameterUI("Use global settings for misc settings")]
        public bool useGlobalSettings4Misc = true;


        [GameParameters.CustomStringParameterUI("Filter Limits", autoPersistance = true, lines = 2, title = "Filter Limits", toolTip = "Specifiy limits on some filters")]
        public string UIstring = "";

        [GameParameters.CustomFloatParameterUI("Upper limit on altitude filter", minValue = 100.0f, maxValue = 500000.0f)]
        public double altitudeFilterMax = 300000f;

        [GameParameters.CustomFloatParameterUI("Upper limit on speed filter", minValue = 100.0f, maxValue = 5000.0f)]
        public double speedFilterMax = 2500;


        [GameParameters.CustomStringParameterUI("Screen Message Log", autoPersistance = true, lines = 2, title = "")]
        public string UIstring2 = "";
        [GameParameters.CustomStringParameterUI("Screen Message Log", autoPersistance = true, lines = 2, title = "Screen Message Log", toolTip = "Options for the screen messages")]
        public string UIstring3 = "";
        [GameParameters.CustomStringParameterUI("Screen Message Log", autoPersistance = true, lines = 2, title = "")]
        public string UIstring4 = "";


        [GameParameters.CustomIntParameterUI("Max screen messages to display", minValue = 1, maxValue = 50,
            toolTip = "This refers to the messages which appear on screen")]
        public int maxMsgs = 20;

        [GameParameters.CustomIntParameterUI("Keep screen messages for (minutes)", minValue = 0, maxValue = 50,
            toolTip = "This refers to the messages which appear on screen")]
        public int expireMsgsAfter = 20;

        [GameParameters.CustomParameterUI("Hide when no screen messages",
            toolTip = "Hide the window if it is open and the remaining messages are expired")]
        public bool hideWhenNoMsgs = false;


        public void SaveGlobalSettingsNode()
        {
            Log.Info("KL_13 SaveGlobalSettingsNode");

            ConfigNode settings = new ConfigNode("KL_13");

            settings.AddValue("altitudeFilterMax", altitudeFilterMax);
            settings.AddValue("speedFilterMax", speedFilterMax);
            settings.AddValue("maxMsgs", maxMsgs);
            settings.AddValue("expireMsgsAfter", expireMsgsAfter);
            settings.AddValue("hideWhenNoMsgs", hideWhenNoMsgs);

            GlobalSettings.UpdateNode("KL_13", settings);

        }
        public void LoadGlobalSettingsNode()
        {
            Log.Info("KL_13.LoadGlobalSettingsNode");
            var settings = GlobalSettings.FetchNode("KL_13");
            if (settings == null)
            {
                Log.Info("Unable to load KL_13 settings");
                return;
            }

            altitudeFilterMax = Double.Parse(Utils.SafeLoad(settings.GetValue("altitudeFilterMax"), "300000"));
            speedFilterMax = Double.Parse(Utils.SafeLoad(settings.GetValue("speedFilterMax"), "2500"));
            maxMsgs = Int32.Parse(Utils.SafeLoad(settings.GetValue("maxMsgs"), "20"));
            expireMsgsAfter = Int32.Parse(Utils.SafeLoad(settings.GetValue("expireMsgsAfter"), "20"));
            hideWhenNoMsgs = Boolean.Parse(Utils.SafeLoad(settings.GetValue("hideWhenNoMsgs"), "false"));
        }

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            if (useGlobalSettings4Misc && !MMCheck.useGlobalSettings4Misc)
            {
                LoadGlobalSettingsNode();
                useGlobalSettings4Misc = true;
            }
            MMCheck.useGlobalSettings4Misc = useGlobalSettings4Misc;

            return MMCheck.EnabledForSave;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4All)
                return false;
            if (member.Name == "useGlobalSettings4Misc")
                return true;
            if (MMCheck.atMainMenu && MMCheck.useGlobalSettings4Misc)
                    return false;
            
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }
    
}

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using ClickThroughFix;


namespace KaptainsLogNamespace
{
    class ManualEntrySelectHotKey : MonoBehaviour
    {
        public static ManualEntrySelectHotKey Instance;

        public bool active = false;
        public bool completed = false;
        KeyCode _lastKeyPressed = KeyCode.None;
        public KeyCode hotkey = KeyCode.None;
        internal bool keyOK;
        public float lastTimeTic = 0;
        private Rect settingsRect = new Rect(200, 200, 350, 150);

        void Start()
        {
            Log.Info("ManualEntrySelectHotKey.Start");
            Instance = this;
        }
        public void EnableWindow(bool b = true)
        {
            if (b)
            {
                if (!active)
                {
                    _lastKeyPressed = KeyCode.None;
                    active = true;
                    completed = false;
                    keyOK = false;
                }
            }
            else
            {
                completed = false;
            }
        }
        void OnGUI()
        {
            if (!active)
                return;

            if (Time.realtimeSinceStartup - lastTimeTic > 0.25)            {                active = false;                return;            }

            // The settings are only available in the space center
            GUI.skin = HighLogic.Skin;
            settingsRect = ClickThruBlocker.GUILayoutWindow("HotKeySettings".GetHashCode(),                                            settingsRect,                                            SettingsWindowFcn,                                            "Manual Entry Hotkey",                                            GUILayout.ExpandWidth(true),
                                            GUILayout.ExpandHeight(true));
        }

        void Update()
        {
            if (Event.current.isKey)
            {
                _lastKeyPressed = Event.current.keyCode;
            }
        }

        void SettingsWindowFcn(int windowID)
        {            GUILayout.BeginHorizontal();            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();            GUILayout.Label("Enter desired hotkey: ");
            GUILayout.Label(hotkey.ToString(), GUI.skin.textField);

            if (_lastKeyPressed != KeyCode.None)
            {
                hotkey = _lastKeyPressed;
                _lastKeyPressed = KeyCode.None;
            }
            // look at EEX

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();            GUILayout.FlexibleSpace();            GUILayout.Label("(type the key you want to use,");
            GUILayout.FlexibleSpace();            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();            GUILayout.FlexibleSpace();            GUILayout.Label("press the OK button when complete)");            GUILayout.FlexibleSpace();            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();            if (GUILayout.Button("Cancel", GUILayout.Width(60)))            {
                active = false;
                completed = true;
                keyOK = false;
            }
            if (GUILayout.Button("OK", GUILayout.Width(60)))            {
                active = false;
                completed = true;
                keyOK = true;
            }
            GUILayout.FlexibleSpace();            GUILayout.EndHorizontal();            GUI.DragWindow();
        }

    }


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
        [GameParameters.CustomParameterUI("Tag")]
        public bool tag = true;

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
            settings.AddValue("tag", tag);

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
            tag = Boolean.Parse(Utils.SafeLoad(settings.GetValue("tag"), "false"));
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

        [GameParameters.CustomFloatParameterUI("Upper limit on altitude filter", minValue = 100.0f, maxValue = 500000.0f,
            toolTip = "Used in filter slider only")]
        public double altitudeFilterMax = 300000f;

        [GameParameters.CustomFloatParameterUI("Upper limit on speed filter", minValue = 100.0f, maxValue = 5000.0f,
            toolTip = "Used in filter slider only")]
        public double speedFilterMax = 2500;


        [GameParameters.CustomStringParameterUI("", autoPersistance = true, lines = 2, title = "")]
        public string UIstring1 = "";

        [GameParameters.CustomFloatParameterUI("Landed stability time", minValue = 0.5f, maxValue = 5.0f,
            toolTip = "How long a vessel needs to be stable after landing before being considered <b>Landed</b>")]
        public double landedStabilityTime = 1;

        [GameParameters.CustomFloatParameterUI("Min altitude to be flying", minValue = 25.0f, maxValue = 500.0f,
            toolTip = "Minimum altitude to be considered flying")]
        public double minFlyingAltitude = 50;
        [GameParameters.CustomFloatParameterUI("Min flying time ", minValue = 1.0f, maxValue = 50.0f,
            toolTip = "Minimum time not touching the ground to be considered flying")]
        public double minFlyingTime = 10;

        [GameParameters.CustomParameterUI("Specify Manual Log Entry Key",
            toolTip ="initial default is letter 'O'")]
        public bool getLogEntryKey = false;

        public KeyCode ManualEntryKeycode = KeyCode.O;


        [GameParameters.CustomStringParameterUI("", autoPersistance = true, lines = 2, title = "")]
        public string UIstring2 = "";
        [GameParameters.CustomStringParameterUI("", autoPersistance = true, lines = 2, title = "<b>Screen Message Log</b>")]
        public string UIstring3 = "";


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
            settings.AddValue("ManualEntryKeycode", ManualEntryKeycode);
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
            string s = settings.GetValue("ManualEntryKeycode");
            if (s != null)
                ManualEntryKeycode = (KeyCode)Enum.Parse(typeof(KeyCode), s);
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

            if (getLogEntryKey)            {                // Use MapView.MapCamera to get a gameObject                ManualEntrySelectHotKey gui = MapView.MapCamera.gameObject.GetComponent<ManualEntrySelectHotKey>();
                if (gui == null)
                {
                    Log.Info("Adding ManualEntrySelectHotKey");
                    gui = MapView.MapCamera.gameObject.AddComponent<ManualEntrySelectHotKey>();
                }
                if (ManualEntrySelectHotKey.Instance != null)
                {
                    if (ManualEntrySelectHotKey.Instance.completed)
                    {
                        getLogEntryKey = false;
                        ManualEntrySelectHotKey.Instance.EnableWindow(false);
                        if (ManualEntrySelectHotKey.Instance.keyOK)
                            ManualEntryKeycode = ManualEntrySelectHotKey.Instance.hotkey;
                        UnityEngine.Object.Destroy(gui);
                        gui = null;
                        ManualEntrySelectHotKey.Instance = null;
                    }
                    else
                    {
                        ManualEntrySelectHotKey.Instance.lastTimeTic = Time.realtimeSinceStartup;
                        if (!ManualEntrySelectHotKey.Instance.active)
                        {
                            ManualEntrySelectHotKey.Instance.EnableWindow();
                            ManualEntrySelectHotKey.Instance.hotkey = ManualEntryKeycode;
                        }
                    }
                }
                return true;
            }
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }
    
}

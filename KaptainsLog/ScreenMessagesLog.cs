using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;


namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public class ScreenMessagesLog : MonoBehaviour
    {
        static public ScreenMessagesLog Instance;

        private IEnumerator coroutine;

        Queue<ScreenMessage> scrnMsgLog = new Queue<ScreenMessage>();
        bool visible = false;
        public Rect filterListWindow;
        int logentryWindowId = GUIUtility.GetControlID(FocusType.Native);
        Vector2 displayScrollVector;

        const int NARROW_WIDTH = 400;
        const int MED_WIDTH = 700;
        const int WIDE_WIDTH = 1000;
        GUIStyle msgTextStyle;
        GUIStyle buttonOnStyle, buttonOffStyle;
        Texture2D greenButtonTexture;

        List<string> filterList = new List<string>();

        const string LCOffIcon = "KaptainsLog/Icons/LC_Off";
        const string LCOnIcon = "KaptainsLog/Icons/LC_On";

        const string UCOffIcon = "KaptainsLog/Icons/UC_Off";
        const string UCOnIcon = "KaptainsLog/Icons/UC_On";

        const string ULOffIcon = "KaptainsLog/Icons/UL_Off";
        const string ULOnIcon = "KaptainsLog/Icons/UL_On";

        const string UROffIcon = "KaptainsLog/Icons/UR_Off";
        const string UROnIcon = "KaptainsLog/Icons/UR_On";


        private void Awake()
        {
            UnityEngine.Debug.Log("ScreenMessagesLog.Awake");
            Instance = this;
            DontDestroyOnLoad(this);
            filterListWindow = new Rect((Screen.width - 400) / 2, (Screen.height - 400) / 2, 400, 400);
        }

        void Start()
        {
            coroutine = MonitorScreenMessages(0.25f);
            LoadFilterList();
            StartCoroutine(coroutine);

            greenButtonTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            greenButtonTexture.SetPixel(0, 0, Color.green);
            greenButtonTexture.SetPixel(0, 1, Color.green);
            greenButtonTexture.SetPixel(1, 0, Color.green);
            greenButtonTexture.SetPixel(1, 1, Color.green);
        }
        private void OnDestroy()
        {
            this.StopAllCoroutines();
        }


        IEnumerator MonitorScreenMessages(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
               
                int numActiveMsgs = ScreenMessages.Instance.ActiveMessages.Count();
                int numToCheck = Math.Min(numActiveMsgs, scrnMsgLog.Count);
                if (numActiveMsgs > 0)
                {
                    for (int cnt = 0; cnt < numActiveMsgs; cnt++)
                    {
                        bool copied = false;
                        if (numToCheck > 0)
                        {
                            var idx = scrnMsgLog.Count - 1;
                            if (filterList.Count > 0 && filterList.Contains(ScreenMessages.Instance.ActiveMessages[cnt].message))
                            {
                                Log.Info("filtered: " + ScreenMessages.Instance.ActiveMessages[cnt].message);
                                copied = true;
                            }
                            else
                                for (int i = 0; i < numToCheck; i++)
                                {
                                   
                                    if (scrnMsgLog.ElementAtOrDefault(idx - i) == ScreenMessages.Instance.ActiveMessages[cnt])
                                        copied = true;
                                }

                        }

                        if (!copied)
                            scrnMsgLog.Enqueue(ScreenMessages.Instance.ActiveMessages[cnt]);
                    }

                    // Remove expired messages here
                    while (scrnMsgLog.Count > HighLogic.CurrentGame.Parameters.CustomParams<KL_6>().maxMsgs)
                    {
                        scrnMsgLog.Dequeue();
                    }
                    while (scrnMsgLog.Count > 0 && scrnMsgLog.LastOrDefault().startTime + 60 * HighLogic.CurrentGame.Parameters.CustomParams<KL_6>().expireMsgsAfter < Time.realtimeSinceStartup)
                    {
                        scrnMsgLog.Dequeue();
                        if (HighLogic.CurrentGame.Parameters.CustomParams<KL_6>().hideWhenNoMsgs && scrnMsgLog.Count == 0)
                            ShowWin(false);

                    }
                }
            }
        }


        public void ShowWin(bool b)
        {
            visible = b;
        }
        private void OnGUI()
        {
            if (visible)
            {
                GUI.color = Color.grey;

                msgTextStyle = new GUIStyle(GUI.skin.textField);
                msgTextStyle.normal.textColor = Color.yellow;

                buttonOffStyle = new GUIStyle(GUI.skin.button);
                buttonOnStyle = new GUIStyle(GUI.skin.button);
                //buttonOnStyle.normal.background = greenButtonTexture;
                buttonOnStyle.normal.textColor = Color.green;
                buttonOnStyle.active.textColor = Color.green;
                buttonOnStyle.focused.textColor = Color.green;
                buttonOnStyle.hover.textColor = Color.green;
                //buttonOnStyle.active.background = greenButtonTexture;
                //buttonOnStyle.focused.background = greenButtonTexture;

                KaptainsLog.Instance.logEntryWindow = GUILayout.Window(logentryWindowId, filterListWindow, DisplayLogEntryWindow, "Kaptain's Log Entry"); //, KaptainsLog.windowStyle);
            }
        }

        bool lowerCenterFilter = false;
        bool upperCenterFilter = true;
        bool upperLeftFilter = true;
        bool upperRightFilter = true;

        void DisplayLogEntryWindow(int id)
        {
            
#if false
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(lowerCenterFilter ? LCOnIcon : LCOffIcon, false), GUIStyle.none, GUILayout.Height(32), GUILayout.Width(32)))
                lowerCenterFilter = !lowerCenterFilter;
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperCenterFilter ? UCOnIcon : UCOffIcon, false), GUIStyle.none, GUILayout.Height(32), GUILayout.Width(32)))
                upperCenterFilter = !upperCenterFilter;
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperLeftFilter ? ULOnIcon : ULOffIcon, false), GUIStyle.none, GUILayout.Height(32), GUILayout.Width(32)))
                upperLeftFilter = !upperLeftFilter;
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperRightFilter ? UROnIcon : UROffIcon, false), GUIStyle.none, GUILayout.Height(32), GUILayout.Width(32)))
                upperRightFilter = !upperRightFilter;
#endif   
            GUILayout.BeginHorizontal();
            string newfilter = "";
            displayScrollVector = GUILayout.BeginScrollView(displayScrollVector);
            for (int i = 0; i < scrnMsgLog.Count; i++)
            {

                var scrnMsg = scrnMsgLog.ElementAt(i);
                bool notFiltered = false;

                notFiltered = notFiltered |(scrnMsg.style == ScreenMessageStyle.LOWER_CENTER & lowerCenterFilter) ;
                notFiltered = notFiltered | (scrnMsg.style == ScreenMessageStyle.UPPER_CENTER & upperRightFilter);
                notFiltered = notFiltered | (scrnMsg.style == ScreenMessageStyle.UPPER_LEFT & upperLeftFilter);
                notFiltered = notFiltered | (scrnMsg.style == ScreenMessageStyle.UPPER_RIGHT & upperRightFilter);
                
                if (notFiltered)
                {

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("F", GUILayout.Width(21)))
                    {
                        Log.Info("Adding to filter: " + scrnMsg.message);
                        filterList.Add(scrnMsg.message);
                        newfilter = scrnMsg.message;
                        SaveFilterList();

                    }

                    if (GUILayout.Button("A", GUILayout.Width(21)))
                    {
                        KaptainsLog.utils.CreateLogEntry(Events.ScreenMsgRecord, false, scrnMsg.message);
                    }
                    Texture2D icon = null;
                    switch (scrnMsg.style)
                    {
                        case ScreenMessageStyle.LOWER_CENTER:
                            icon = GameDatabase.Instance.GetTexture(LCOnIcon, false);
                            break;
                        case ScreenMessageStyle.UPPER_CENTER:
                            icon = GameDatabase.Instance.GetTexture(UCOnIcon, false);
                            break;

                        case ScreenMessageStyle.UPPER_LEFT:
                            icon = GameDatabase.Instance.GetTexture(ULOnIcon, false);
                            break;
                        case ScreenMessageStyle.UPPER_RIGHT:
                            icon = GameDatabase.Instance.GetTexture(UROnIcon, false);
                            break;

                    }
                    GUILayout.Button(icon, GUIStyle.none, GUILayout.Height(32), GUILayout.Width(32));
                    GUILayout.TextField(scrnMsg.message, msgTextStyle, GUILayout.ExpandWidth(true));
                    
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            if (newfilter != "")
            {
                Queue<ScreenMessage> newQueue = new Queue<ScreenMessage>();
                for (int i = 0; i < scrnMsgLog.Count; i++)
                {
                    var scrnMsg = scrnMsgLog.ElementAt(i);
                    if (scrnMsg.message != "" && scrnMsg.message != newfilter)
                        newQueue.Enqueue(scrnMsg);
                }
                scrnMsgLog.Clear();
                scrnMsgLog = newQueue;
            }
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(lowerCenterFilter ? LCOnIcon : LCOffIcon, false),  GUILayout.Height(32), GUILayout.Width(32)))
                lowerCenterFilter = !lowerCenterFilter;
            //if (GUILayout.Button("LC", lowerCenterFilter? buttonOnStyle:buttonOffStyle, GUILayout.Width(30)))
            //                lowerCenterFilter = !lowerCenterFilter;

            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperCenterFilter ? UCOnIcon : UCOffIcon, false),  GUILayout.Height(32), GUILayout.Width(32)))
                upperCenterFilter = !upperCenterFilter;
            //if (GUILayout.Button("UC", upperCenterFilter ? buttonOnStyle : buttonOffStyle, GUILayout.Width(30)))
            //    upperCenterFilter = !upperCenterFilter;

            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperLeftFilter ? ULOnIcon : ULOffIcon, false),  GUILayout.Height(32), GUILayout.Width(32)))
                upperLeftFilter = !upperLeftFilter;
            //if (GUILayout.Button("UL", upperLeftFilter ? buttonOnStyle : buttonOffStyle, GUILayout.Width(30)))
            //    upperLeftFilter = !upperLeftFilter;

            if (GUILayout.Button(GameDatabase.Instance.GetTexture(upperRightFilter ? UROnIcon : UROffIcon, false),  GUILayout.Height(32), GUILayout.Width(32)))
                upperRightFilter = !upperRightFilter;
            //if (GUILayout.Button("UR", upperRightFilter ? buttonOnStyle : buttonOffStyle, GUILayout.Width(30)))
            //    upperRightFilter = !upperRightFilter;


            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Clear All"))
            {
                scrnMsgLog.Clear();
            }
            if (GUILayout.Button("Clear Filters"))
            {
                filterList.Clear();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close"))
            {
                ShowWin(false);
            }
            GUILayout.FlexibleSpace();

            if (filterListWindow.width != NARROW_WIDTH)
            {
                if (GUILayout.Button("Narrow"))
                    filterListWindow.width = NARROW_WIDTH;
            }
            if (filterListWindow.width != MED_WIDTH)
            {
                if (GUILayout.Button("Medium"))
                    filterListWindow.width = MED_WIDTH;
            }
            if (filterListWindow.width != WIDE_WIDTH)
            {
                if (GUILayout.Button("Wide"))
                    filterListWindow.width = WIDE_WIDTH;
            }
            GUILayout.EndHorizontal();

            GUI.DragWindow();
        }

        public string NODENAME = "Filters";
        string SETTINGSNAME = "FilterString";
        string VALUENAME = "filter";
        string FILTERDATA = KaptainsLog.MOD_FOLDER + "PluginData/FilterList.cfg";

        void SaveFilterList()
        {
            ConfigNode settingsFile = new ConfigNode();
            ConfigNode settings = new ConfigNode();

            settingsFile.SetNode(SETTINGSNAME, settings, true);

            foreach (var s in filterList)
                settings.SetValue(VALUENAME, s, true);

            settingsFile.Save(FILTERDATA);
        }

        void LoadFilterList()
        {
            ConfigNode settingsFile;
            ConfigNode settings;
          
            Log.Info("Loading settings file: " + FILTERDATA);
            settingsFile = ConfigNode.Load(FILTERDATA);
            if (settingsFile != null)
            {
                settings = settingsFile.GetNode(SETTINGSNAME);
                if (settings == null)
                    return;
                var s = settings.GetValues(VALUENAME);

                filterList = new List<string>(s);
            }
        }
    }
}

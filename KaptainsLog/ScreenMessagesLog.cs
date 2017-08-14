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

        List<string> filterList = new List<string>();

        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            filterListWindow = new Rect((Screen.width - 400) / 2, (Screen.height - 400) / 2, 400, 400);
        }

        void Start()
        {
           
            coroutine = MonitorScreenMessages(0.25f);
            LoadFilterList();
            StartCoroutine(coroutine);
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
                                //Log.Info("filtered: " + ScreenMessages.Instance.ActiveMessages[cnt].message);
                                copied = true;
                            }
                            else
                                for (int i = 0; i < numToCheck; i++)
                                {
                                    //Log.Info("MonitorScreenMessages 1, idx: " + idx.ToString());
                                    if (scrnMsgLog.ElementAtOrDefault(idx - i) == ScreenMessages.Instance.ActiveMessages[cnt])
                                        copied = true;
                                }

                        }
                        //Log.Info("MonitorScreenMessages 2");

                        if (!copied)
                            scrnMsgLog.Enqueue(ScreenMessages.Instance.ActiveMessages[cnt]);
                    }

                    // Remove expired messages here
                    while (scrnMsgLog.Count > HighLogic.CurrentGame.Parameters.CustomParams<KL_3>().maxMsgs)
                    {
                        scrnMsgLog.Dequeue();
                    }
                    while (scrnMsgLog.Count > 0 && scrnMsgLog.LastOrDefault().startTime + 60 * HighLogic.CurrentGame.Parameters.CustomParams<KL_3>().expireMsgsAfter < Time.realtimeSinceStartup)
                    {
                        scrnMsgLog.Dequeue();
                        if (HighLogic.CurrentGame.Parameters.CustomParams<KL_3>().hideWhenNoMsgs && scrnMsgLog.Count == 0)
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
                filterListWindow = GUILayout.Window(logentryWindowId, filterListWindow, DisplayLogEntryWindow, "Kaptain's Log Entry"); //, KaptainsLog.windowStyle);
            }
        }

        void DisplayLogEntryWindow(int id)
        {
            GUILayout.BeginHorizontal();
            string newfilter = "";
            displayScrollVector = GUILayout.BeginScrollView(displayScrollVector);
            for (int i = 0; i < scrnMsgLog.Count; i++)
            {

                var scrnMsg = scrnMsgLog.ElementAt(i);
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

                GUILayout.TextField(scrnMsg.message);
                GUILayout.EndHorizontal();
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

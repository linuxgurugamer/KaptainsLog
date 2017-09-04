using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KSP.UI.Screens;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KaptainsLogNamespace
{
    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] {
        GameScenes.SPACECENTER,
        GameScenes.EDITOR,
        GameScenes.FLIGHT,
        GameScenes.TRACKSTATION,
        GameScenes.SPACECENTER
    })]
    class Shelter : ScenarioModule
    {
        public static Shelter Instance;
        public static bool inited = false;

        static internal List<LogEntry> kaptainsLogList = new List<LogEntry>();
        static internal Queue<ScreenMessage> scrnMsgLog = new Queue<ScreenMessage>();
        static internal bool dirtyFilter = true;
        static internal bool dirtyColSel = true;
        static internal bool logsLoaded = false;
        static internal bool imgCacheFilled = false;

        [Persistent]
        public int logIdx = 0;

        public void DoInit()
        {
            dirtyFilter = true;
            dirtyColSel = true;
            logsLoaded = false;
            imgCacheFilled = false;
        }
        override public void OnAwake()
        {
            Log.Info("ShelterPersistent.Awake");
            //Shelter.persistent = this;
            inited = true;
            Instance = this;
            DoInit();
        }

    }

}

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
    class ShelterPersistent : ScenarioModule
    {
        public static bool inited = false;

        override public void OnAwake()
        {
            Log.Info("ShelterPersistent.Awake");
            Shelter.persistent = this;
            inited = true;           
        }

    }



    // This class is a place to stick data that will not be destroyed by KRASH.Deactivate(). 
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class Shelter : MonoBehaviour
    {
        public static ShelterPersistent persistent;

        static internal List<LogEntry> kaptainsLogList = new List<LogEntry>();
        static internal Queue<ScreenMessage> scrnMsgLog = new Queue<ScreenMessage>();
        static internal bool dirtyFilter = true;
        static internal bool dirtyColSel = true;
        static internal bool logsLoaded = false;
        static internal bool imgCacheFilled = false;

        void Start()
        {
            Log.Info("Shelter.Start");

            //GameEvents.onLevelWasLoaded.Add(CallbackLevelWasLoaded);
            GameEvents.onGameStateCreated.Add(onGameStateCreated);
            DontDestroyOnLoad(this);
        }
        private void OnDestroy()
        {
            GameEvents.onGameStateCreated.Remove(onGameStateCreated);
        }
        void OnEnable()
        {
            //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            SceneManager.sceneLoaded += CallbackLevelWasLoaded;
        }

        void OnDisable()
        {
            //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            SceneManager.sceneLoaded -= CallbackLevelWasLoaded;
        }

        void onGameStateCreated(Game g)
        {
            Log.Info("onGameStateCreated");
            kaptainsLogList.Clear();
            scrnMsgLog.Clear();
            dirtyFilter = true;
            dirtyColSel = true;
            logsLoaded = false;
            imgCacheFilled = false;
        }

        void CallbackLevelWasLoaded(Scene scene, LoadSceneMode mode)
        {
            Log.Info("CallbackLevelWasLoaded");
            //[KSPScenario(ScenarioCreationOptions.AddToNewGames, new[] { GameScenes.FLIGHT, GameScenes.TRACKSTATION, GameScenes.SPACECENTER })]
            if (HighLogic.LoadedScene == GameScenes.FLIGHT || HighLogic.LoadedScene == GameScenes.TRACKSTATION || HighLogic.LoadedScene == GameScenes.SPACECENTER)
            {
               
                Log.Info("CallbackLevelWasLoaded loaded for " + scene.ToString() + "   " + HighLogic.LoadedScene.ToString());
            }
            else
            {
                Log.Info("No call at CallbackLevelWasLoaded for " + scene.ToString() + "   " + HighLogic.LoadedScene.ToString());
            }
        }

    }

}

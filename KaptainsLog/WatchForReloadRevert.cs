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

// The GameEvents:
//    onGameStateCreated
//    onGameSTateLoad
//    onGameStatePostLoad
//
// Don't always fire for the same events.  For example, loading a game the 1st time, nothing fires, loading it a 2nd time, you get 2 onGameStateCreated and one OnGameSTateLoad
//
// So I monitor the UniversalTime here  for any changes
//


namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    public class WatchForReloadRevert : MonoBehaviour
    {
        //static WatchForReloadRevert Instance;

#if false
        private IEnumerator coroutine;

        string lastGameTitle;
        double lastUT;

        const float WAITTIME = 2f;
#endif
        private void Awake()
        {
            UnityEngine.Debug.Log("WatchForReloadRevert.Awake");
            //Instance = this;
            DontDestroyOnLoad(this);

            GameEvents.onGameStateCreated.Add(onGameStateCreated);
            GameEvents.onGameStateLoad.Add(onGameStateLoad);
            GameEvents.onGameStatePostLoad.Add(onGameStatePostLoad);
        }

        void onGameStateCreated(Game g)
        {
            Log.Info("onGameStateCreated");
        }
        void onGameStateLoad(ConfigNode n)
        {
            Log.Info("onGameStateLoad");
            GlobalSettings.LoadGlobalSettings();
        }
        void onGameStatePostLoad(ConfigNode n)
        {
            Log.Info("onGameStatePostLoad");
            if (KaptainsLog.Instance.disableTagOnRevert)
                KaptainsLog.Instance.activeTag = "";
            KaptainsLog.utils.CreateLogEntry(Events.Revert, false, "Game reverted to " + KaptainsLog.utils.GetKerbinTime(Planetarium.GetUniversalTime(), false).ToString(), "");
        }

#if false
        void Start()
        {
            lastGameTitle = HighLogic.CurrentGame.Title;
            lastUT = Planetarium.GetUniversalTime();
            coroutine = MonitorGameTime(WAITTIME);
            StartCoroutine(coroutine);
        }
        private void OnDestroy()
        {
            this.StopAllCoroutines();
        }

        IEnumerator MonitorGameTime(float waitTime)
        {
            float w = 2 * waitTime;
            while (true)
            {
                yield return new WaitForSeconds(waitTime);

                if (HighLogic.LoadedScene == GameScenes.MAINMENU)
                {
                    lastGameTitle = "";
                    lastUT = 0;
                }
                else
                {
                    //Log.Info("MonitorGameTime, lastGameTitle: " + lastGameTitle + ", currentGameTitle: " + HighLogic.CurrentGame.Title + ",  UT: " + Planetarium.GetUniversalTime().ToString() + ", " + lastUT.ToString());
                    if (lastGameTitle == HighLogic.CurrentGame.Title)
                    {
                        // Use abs here in case a game is loaded which is LATER than the current time
                        // Both cases are considered to be "reverted"
                        if (Math.Abs(Planetarium.GetUniversalTime() - lastUT) > w)
                        {
                            KaptainsLog.utils.CreateLogEntry(Events.Revert, false, "Game reverted to " + KaptainsLog.utils.GetKerbinTime(Planetarium.GetUniversalTime(), false).ToString(), "");
                        }
                    }
                    else
                        lastGameTitle = HighLogic.CurrentGame.Title;
                    lastUT = Planetarium.GetUniversalTime();
                }
            }

        }
#endif
    }
}

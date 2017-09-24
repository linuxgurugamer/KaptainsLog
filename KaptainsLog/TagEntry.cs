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

    class TagEntry : MonoBehaviour
    {
        internal static bool ready = false;
        private Rect settingsRect = new Rect(200, 200, 350, 150);

        internal bool completed = false;

        
        internal static bool disableTagOnRevert = false;
        internal static bool disableTagOnLanding = false;
        internal static bool disableTagOnRecovery = false;
        internal static string newtag = "";

        void Start()
        {
            Log.Info("TagEntry.Start");
            ready = true;
        }


        void OnGUI()
        {

            GUI.skin = HighLogic.Skin;
            settingsRect = GUILayout.Window("Tag Entry".GetHashCode(),                                            settingsRect,                                            SettingsWindowFcn,                                            "Tag Entry",                                            GUILayout.ExpandWidth(true),
                                            GUILayout.ExpandHeight(true));
        }

 
   
        void SettingsWindowFcn(int windowID)
        {            GUILayout.BeginHorizontal();            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();            GUILayout.Label("Enter desired tag: ");
            if (newtag != null)
                newtag = GUILayout.TextField(newtag, GUILayout.Width(90));
            GUILayout.EndHorizontal();            GUILayout.BeginHorizontal();
            disableTagOnRevert = GUILayout.Toggle(disableTagOnRevert, "Disable tag on revert");
            GUILayout.EndHorizontal();            GUILayout.BeginHorizontal();
            disableTagOnLanding = GUILayout.Toggle(disableTagOnLanding, "Disable tag on landing, splashdown or crash");
            GUILayout.EndHorizontal();            GUILayout.BeginHorizontal();
            disableTagOnRecovery = GUILayout.Toggle(disableTagOnRecovery, "Disable tag on vessel recovery");
            GUILayout.EndHorizontal();                        GUILayout.FlexibleSpace();            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();            if (GUILayout.Button("Clear", GUILayout.Width(60)))            {
                newtag = "";
            }
            if (GUILayout.Button("OK", GUILayout.Width(60)))            {
                completed = true;
                Log.Info("tagentry OK completed");
            }
            if (GUILayout.Button("Cancel", GUILayout.Width(60)))            {
                newtag = null;
                completed = true;
               
            }
            GUILayout.FlexibleSpace();            GUILayout.EndHorizontal();            GUI.DragWindow();
        }

    }
}

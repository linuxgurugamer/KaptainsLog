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
    partial class KaptainsLog
    {
        int lastSelectedHtmlTemplate = 0;
        void DisplayHtmlTemplateSelectionWindow(int id)
        {
            // Log.Info("DisplayHtmlTemplateSelectionWindow");
            GUIStyle toggleStyle = new GUIStyle(HighLogic.Skin.label);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Select HTML Template:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            htmlFileSelScrollVector = GUILayout.BeginScrollView(htmlFileSelScrollVector);
            int cnt = 0;
            foreach (string fileName in dirEntries)
            {
                GUILayout.BeginHorizontal();
                if (lastSelectedHtmlTemplate != cnt)
                    toggleStyle.normal.textColor = Color.red;
                else
                    toggleStyle.normal.textColor = Color.green;
                if (GUILayout.Button(fileName.Substring(fileName.LastIndexOf('/') + 1), toggleStyle))
                {
                    lastSelectedHtmlTemplate = cnt;
                }
                cnt++;
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(90)))
            {

                htmlTemplate = dirEntries[lastSelectedHtmlTemplate];
                displayHTMLTemplate = false;
                loadTemplateConfig(htmlTemplate);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel", GUILayout.Width(90)))
            {
                displayHTMLTemplate = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

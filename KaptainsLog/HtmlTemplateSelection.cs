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
        int lastSelectedQuickHtmlTemplate = 0;
        void DisplayHtmlTemplateSelectionWindow(int id)
        {
            // Log.Info("DisplayHtmlTemplateSelectionWindow");
            GUIStyle toggleStyle = new GUIStyle(HighLogic.Skin.label);

            GUILayout.BeginHorizontal();
            if (displayQuickHTMLTemplate)
                GUILayout.Label("Select Quick HTML Template:");
            else
                GUILayout.Label("Select HTML Template:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            htmlFileSelScrollVector = GUILayout.BeginScrollView(htmlFileSelScrollVector);
            int cnt = 0;
            foreach (string fileName in dirEntries)
            {
                GUILayout.BeginHorizontal();
                if ((exportSingle? lastSelectedQuickHtmlTemplate: lastSelectedHtmlTemplate) != cnt)
                    toggleStyle.normal.textColor = Color.red;
                else
                    toggleStyle.normal.textColor = Color.green;
                if (GUILayout.Button(fileName.Substring(fileName.LastIndexOf('/') + 1), toggleStyle))
                {
                    if (exportSingle)
                        lastSelectedQuickHtmlTemplate = cnt;
                    else
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
                if (exportSingle)
                    activeExportSettings.htmlTemplate = dirEntries[lastSelectedQuickHtmlTemplate];
                else
                    activeExportSettings.htmlTemplate = dirEntries[lastSelectedHtmlTemplate];
                displayHTMLTemplate = false;
                displayQuickHTMLTemplate = false;
                loadTemplateConfig(activeExportSettings.htmlTemplate);
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel", GUILayout.Width(90)))
            {
                displayHTMLTemplate = false;
                displayQuickHTMLTemplate = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

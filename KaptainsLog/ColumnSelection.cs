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
        void DisplayColSelWindow(int id)
        {
            GUIStyle toggleStyle = new GUIStyle(HighLogic.Skin.label);
            bool moveLeft = false, moveRight = false;
            int item = 0;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Green text = column will be displayed");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Red text = column will not be displayed");
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            colSelScrollVector = GUILayout.BeginScrollView(colSelScrollVector);
            GUILayout.BeginHorizontal();
            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none)
                {
                    GUILayout.BeginHorizontal();
                    if (d > 0)
                    {
                        GUILayout.Space(5);
                        if (GUILayout.Button("<", GUILayout.Width(20)))
                        {
                            moveLeft = true;
                            item = d;
                        }
                    }

                    if (!displayFields[d].visible)
                        toggleStyle.normal.textColor = Color.red;
                    else
                        toggleStyle.normal.textColor = Color.green;

                    string f = LogEntry.displayFieldName(displayFields[d].f);
                    float w = GUI.skin.label.CalcSize(new GUIContent(f)).x;

                    //toggleStyle.fixedWidth = colWidth[d];
                    bool b = displayFields[d].visible;
                    displayFields[d].visible = GUILayout.Toggle(displayFields[d].visible, LogEntry.displayFieldName(displayFields[d].f), toggleStyle, GUILayout.Width(w));
                    if (b != displayFields[d].visible)
                        KLScenario.dirtyColSel = true;
                    if (d < displayFields.Count)
                    {
                        if (GUILayout.Button(">", GUILayout.Width(20)))
                        {
                            moveRight = true;
                            item = d;
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(90)))
            {
                displayColSelectWindow = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
            if (moveLeft)
            {
                DisplayField t = displayFields[item];
                displayFields[item] = displayFields[item - 1];
                displayFields[item - 1] = t;

                float w = colWidth[item];
                colWidth[item] = colWidth[item - 1];
                colWidth[item - 1] = w;
            }
            if (moveRight)
            {
                DisplayField t = displayFields[item];
                displayFields[item] = displayFields[item + 1];
                displayFields[item + 1] = t;

                float w = colWidth[item];
                colWidth[item] = colWidth[item + 1];
                colWidth[item + 1] = w;
            }

        }
    }
}

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
    // Following class gotten from this page:
    // https://stackoverflow.com/questions/12172162/how-to-insert-item-into-list-in-order
    //
    public static class ListExt
    {
        public static void AddSorted<T>(this List<T> @this, T item) where T : IComparable<T>
        {
            if (@this.Count == 0)
            {
                @this.Add(item);
                return;
            }
            if (@this[@this.Count - 1].CompareTo(item) <= 0)
            {
                @this.Add(item);
                return;
            }
            if (@this[0].CompareTo(item) >= 0)
            {
                @this.Insert(0, item);
                return;
            }
            int idx = @this.BinarySearch(item);
            if (idx < 0)
                idx = ~idx;
            @this.Insert(idx, item);
        }
    }
    partial class KaptainsLog
    {
        GUIStyle toggleStyle = new GUIStyle(HighLogic.Skin.label);



        void SortLog()
        {
            kaptainsLogList.Sort();
        }
        bool sortImmediate = false;
        
        void DisplaySortSelWindow(int id)
        {
            GUILayout.BeginHorizontal();
            sortImmediate = GUILayout.Toggle(sortImmediate, "Sort Immediately");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            LogEntry.sortReverse = GUILayout.Toggle(LogEntry.sortReverse, "Reverse Sort");
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            htmlFileSelScrollVector = GUILayout.BeginScrollView(htmlFileSelScrollVector);

            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none && displayFields[d].f != Fields.thumbnail && displayFields[d].f != Fields.notes)
                {
                    var f = displayFields[d].f;

                    if (LogEntry.sortField == displayFields[d].f)
                        toggleStyle.normal.textColor = Color.red;
                    else
                        toggleStyle.normal.textColor = Color.green;
                    if (GUILayout.Button(LogEntry.displayFieldName(f), toggleStyle))
                    {
                        LogEntry.sortField = displayFields[d].f;
                        if (sortImmediate)
                            SortLog();
                    }
                }
            }
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(90)))
            {
                displaySortWindow = false;
                SortLog();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            GUI.DragWindow();
        }
    }
}

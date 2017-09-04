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
            int index = @this.BinarySearch(item);
            if (index < 0)
                index = ~index;
            @this.Insert(index, item);
        }
    }
    partial class KaptainsLog
    {
        GUIStyle toggleStyle = new GUIStyle(HighLogic.Skin.label);



        void SortLog()
        {
#if false
            // This uses Linq
            List<LogEntry> newList = null;

            switch (sortField)
            {
                case Fields.vesselName:
                    newList = kaptainsLogList.OrderBy(l => l.vesselName).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.universalTime:
                    newList = kaptainsLogList.OrderBy(l => l.universalTime).ThenBy(l => l.vesselName).ToList(); break;
                case Fields.utcTime:
                    newList = kaptainsLogList.OrderBy(l => l.utcTime).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.missionTime:
                    newList = kaptainsLogList.OrderBy(l => l.missionTime).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.vesselSituation:
                    newList = kaptainsLogList.OrderBy(l => l.vesselSituation).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.controlLevel:
                    newList = kaptainsLogList.OrderBy(l => l.controlLevel).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.mainBody:
                    newList = kaptainsLogList.OrderBy(l => l.vesselMainBody).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.altitude:
                    newList = kaptainsLogList.OrderBy(l => l.altitude).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.speed:
                    newList = kaptainsLogList.OrderBy(l => l.speed).ThenBy(l => l.universalTime).ToList(); break;
                case Fields.eventType:
                    newList = kaptainsLogList.OrderBy(l => l.eventType).ThenBy(l => l.universalTime).ToList(); break;
            }
            if (newList != null)
                kaptainsLogList = newList;
#else
            
            kaptainsLogList.Sort();
#if false
            kaptainsLogList.Sort(delegate (LogEntry x, LogEntry y)
            {
                int rc = 0;
                switch (sortField)
                {
                    case Fields.index:
                        rc = x.index - y.index;
                        break;

                    case Fields.vesselId:
                        rc = String.Compare(x.vesselId, y.vesselId);
                        break;
                    case Fields.vesselName:
                        rc = String.Compare(x.vesselName, y.vesselName);
                        break;
                    case Fields.universalTime:
                        if (x.universalTime < y.universalTime) rc = -1;
                        else
                            if (x.universalTime > y.universalTime) rc = 1;
                        break;
                    case Fields.utcTime:
                        if (x.utcTime < y.utcTime) rc = -1;
                        else
                           if (x.utcTime > y.utcTime) rc = 1;
                        break;
                    case Fields.missionTime:
                        if (x.missionTime < y.missionTime) rc = -1;
                        else
                          if (x.missionTime > y.missionTime) rc = 1;
                        break;
                    case Fields.vesselSituation:
                        if (x.vesselSituation < y.vesselSituation) rc = -1;
                        else
                         if (x.vesselSituation > y.vesselSituation) rc = 1;
                        break;
                    case Fields.controlLevel:
                        if (x.controlLevel < y.controlLevel) rc = -1;
                        else
                         if (x.missionTime > y.missionTime) rc = 1;
                        break;
                    case Fields.mainBody:
                        rc = String.Compare(x.vesselMainBody, y.vesselMainBody);
                        break;
                    case Fields.altitude:
                        if (x.altitude < y.altitude) rc = -1;
                        else
                         if (x.altitude > y.altitude) rc = 1;
                        break;
                    case Fields.speed:
                        if (x.speed < y.speed) rc = -1;
                        else
                         if (x.speed > y.speed) rc = 1;
                        break;
                    case Fields.eventType:
                        if (x.eventType < y.eventType) rc = -1;
                        else
                         if (x.eventType > y.eventType) rc = 1;
                        break;
                }
                if (rc != 0)
                    return rc;
                if (sortField != Fields.universalTime)
                {
                    if (x.universalTime < y.universalTime) rc = -1;
                    else
                            if (x.universalTime > y.universalTime) rc = 1;
                } else
                {
                    rc = String.Compare(x.vesselId, y.vesselId);
                }
                return rc;
            });
#endif
#endif
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

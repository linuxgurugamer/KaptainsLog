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
        void DisplayFilterSelWindow(int id)
        {
            // vesselName
            // vesselSituation
            // mainBody
            // eventType

            GUILayout.BeginHorizontal();
            GUILayout.Label(" ");
            GUILayout.EndHorizontal();
            if (entryField != Fields.none)
                GUI.enabled = false;
            else
                GUI.enabled = true;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Active");
            GUILayout.EndHorizontal();

            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none && displayFields[d].f != Fields.thumbnail && displayFields[d].visible)
                {
                    var f = displayFields[d].f;
                    if (f == Fields.universalTime)
                        continue;
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("  ");
                    bool oldFilter = displayFields[d].filter;
                    displayFields[d].filter = GUILayout.Toggle(displayFields[d].filter, "");
                    GUI.enabled = displayFields[d].filter;
                    if (oldFilter != displayFields[d].filter)
                        KLScenario.dirtyFilter = true;

                    if (f == Fields.vesselName || f == Fields.vesselSituation || f == Fields.mainBody || f == Fields.eventType || f == Fields.controlLevel)
                    {
                        if (GUILayout.Button(LogEntry.displayFieldName(displayFields[d].f), GUILayout.Width(90)))
                        {
                            entryField = f;
                        }
                        string str = "";
                        switch (displayFields[d].f)
                        {
                            case Fields.vesselName:
                                foreach (KeyValuePair<string, vesselId> vid in allVessels)
                                    if (vid.Value.selected)
                                        str += ", " + vid.Value.vesselName;
                                break;
                            case Fields.vesselSituation:
                                foreach (var s in situations)
                                    if (s.Value.selected)
                                        str += ", " + LogEntry.displayVesselSituation(s.Value.situation);
                                break;
                            case Fields.controlLevel:
                                foreach (var vcl in controlLevels)
                                    if (vcl.Value.selected)
                                        str += ", " + LogEntry.displayControlLevel(vcl.Value.controlLevel);
                                break;
                            case Fields.mainBody:
                                foreach (var body in bodiesList)
                                    if (body.Value.selected)
                                        str += ", " + body.Value.bodyName;
                                break;
                            case Fields.eventType:
                                foreach (var evt in eventTypes)
                                    if (evt.Value.selected)
                                        str += ", " + LogEntry.displayEventString(evt.Value.evnt);
                                break;
                        }
                        if (str != "")
                            GUILayout.Label(str.Substring(2, str.Length - 2));
                        GUILayout.FlexibleSpace();
                    }
                    else
                    {
                        float old;
                        GUILayout.BeginVertical();
                        GUILayout.Label(LogEntry.displayFieldName(displayFields[d].f) + ":", GUILayout.Width(90));
                        GUILayout.EndVertical();
                        GUILayout.BeginVertical();
                        GUILayout.BeginHorizontal();
                        switch (f)
                        {
                            case Fields.altitude:

                                GUILayout.Label(((int)minAlt).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                                old = minAlt;
                                minAlt = GUILayout.HorizontalSlider(minAlt, 0, (float)highestAltitude, GUILayout.Width(SLIDER_WIDTH));
                                if (old != minAlt)
                                    KLScenario.dirtyFilter = true;
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();
                                GUILayout.Label(((int)maxAlt).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                                old = maxAlt;
                                maxAlt = GUILayout.HorizontalSlider(maxAlt, 0, (float)highestAltitude, GUILayout.Width(SLIDER_WIDTH));
                                if (old != maxAlt)
                                    KLScenario.dirtyFilter = true;
                                if (minAlt > maxAlt)
                                    maxAlt = minAlt;
                                break;
                            case Fields.missionTime:
                                GUILayout.Label(utils.GetKerbinTime(misTimStart) + ":", GUILayout.Width(LABEL_WIDTH));
                                old = misTimStart;
                                misTimStart = GUILayout.HorizontalSlider(misTimStart, 0, (float)largestTime, GUILayout.Width(SLIDER_WIDTH));
                                if (old != misTimStart)
                                    KLScenario.dirtyFilter = true;
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();

                                GUILayout.Label(utils.GetKerbinTime(misTimEnd) + ":", GUILayout.Width(LABEL_WIDTH));
                                old = misTimEnd;
                                
                                misTimEnd = GUILayout.HorizontalSlider(misTimEnd, 0, (float)Planetarium.GetUniversalTime() + 3600 * 2, GUILayout.Width(SLIDER_WIDTH));
                                if (old != misTimEnd)
                                    KLScenario.dirtyFilter = true;
                                if (misTimStart > misTimEnd)
                                    misTimEnd = misTimStart;
                                break;
                            case Fields.speed:
                                GUILayout.Label(((int)spdLow).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                                old = spdLow;
                                spdLow = GUILayout.HorizontalSlider(spdLow, 0, (float)highestSpeed, GUILayout.Width(SLIDER_WIDTH));
                                if (old != spdLow)
                                    KLScenario.dirtyFilter = true;
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();

                                GUILayout.Label(((int)spdHigh).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                                old = spdHigh;
                                spdHigh = GUILayout.HorizontalSlider(spdHigh, 0, (float)highestSpeed, GUILayout.Width(SLIDER_WIDTH));
                                if (old != spdHigh)
                                    KLScenario.dirtyFilter = true;
                                if (spdLow > spdHigh)
                                    spdHigh = spdLow;
                                break;
                            case Fields.universalTime:
                                GUILayout.Label(utils.GetKerbinTime(uniTimStart) + ":", GUILayout.Width(LABEL_WIDTH));
                                old = uniTimStart;
                                uniTimStart = GUILayout.HorizontalSlider(uniTimStart, 0, (float)largestUniTime, GUILayout.Width(SLIDER_WIDTH));
                                if (old != uniTimStart)
                                    KLScenario.dirtyFilter = true;
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal();

                                GUILayout.Label(utils.GetKerbinTime(uniTimEnd) + ":", GUILayout.Width(LABEL_WIDTH));
                                old = uniTimEnd;
                                uniTimEnd = GUILayout.HorizontalSlider(uniTimEnd, 0, (float)largestUniTime, GUILayout.Width(SLIDER_WIDTH));
                                if (old != uniTimEnd)
                                    KLScenario.dirtyFilter = true;
                                if (uniTimStart > uniTimEnd)
                                    uniTimEnd = uniTimStart;
                                break;
                            case Fields.utcTime:
                                break;

                        }
                        GUILayout.EndHorizontal();
                        GUILayout.EndVertical();
                    }
                    GUILayout.EndHorizontal();
                }

                GUI.enabled = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(90)))
            {
                displayFilterWindow = false;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            GUI.DragWindow();
        }

    }
}

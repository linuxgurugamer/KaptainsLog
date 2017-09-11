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
        void DisplayEntryFieldWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Specify " + LogEntry.displayFieldName(entryField) + " to filter on:");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (entryField == Fields.altitude || entryField == Fields.missionTime || entryField == Fields.speed ||
                entryField == Fields.universalTime || entryField == Fields.utcTime)
            {
                float oldFloat;
                switch (entryField)
                {
                    case Fields.altitude:
                    case Fields.speed:
                        GUILayout.Label(((int)minAlt).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = minAlt;
                        minAlt = GUILayout.HorizontalSlider(minAlt, 0, (float)highestAltitude, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != minAlt)
                            KLScenario.dirtyFilter = true;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(((int)maxAlt).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = maxAlt;
                        maxAlt = GUILayout.HorizontalSlider(maxAlt, 0, (float)highestAltitude, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != maxAlt)
                            KLScenario.dirtyFilter = true;
                        if (minAlt > maxAlt)
                            maxAlt = minAlt;
                        //break;
                        GUILayout.EndHorizontal();
                        GUILayout.Space(20);
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Specify " + LogEntry.displayFieldName(Fields.speed) + " to filter on:");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(((int)spdLow).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = spdLow;
                        spdLow = GUILayout.HorizontalSlider(spdLow, 0, (float)highestSpeed, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != spdLow)
                            KLScenario.dirtyFilter = true;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(((int)spdHigh).ToString() + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = spdHigh;
                        spdHigh = GUILayout.HorizontalSlider(spdHigh, 0, (float)highestSpeed, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != spdHigh)
                            KLScenario.dirtyFilter = true;
                        if (spdLow > spdHigh)
                            spdHigh = spdLow;
                        break;
                    case Fields.missionTime:
                        GUILayout.Label(utils.GetKerbinTime(misTimStart) + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = misTimStart;
                        misTimStart = GUILayout.HorizontalSlider(misTimStart, 0, (float)largestTime, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != misTimStart)
                            KLScenario.dirtyFilter = true;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(utils.GetKerbinTime(misTimEnd) + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = misTimEnd;

                        misTimEnd = GUILayout.HorizontalSlider(misTimEnd, 0, (float)Planetarium.GetUniversalTime() + 3600 * 2, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != misTimEnd)
                            KLScenario.dirtyFilter = true;
                        if (misTimStart > misTimEnd)
                            misTimEnd = misTimStart;
                        break;

                    case Fields.universalTime:
                        GUILayout.Label(utils.GetKerbinTime(uniTimStart) + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = uniTimStart;
                        uniTimStart = GUILayout.HorizontalSlider(uniTimStart, 0, (float)largestUniTime, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != uniTimStart)
                            KLScenario.dirtyFilter = true;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();

                        GUILayout.Label(utils.GetKerbinTime(uniTimEnd) + ":", GUILayout.Width(LABEL_WIDTH));
                        oldFloat = uniTimEnd;
                        uniTimEnd = GUILayout.HorizontalSlider(uniTimEnd, 0, (float)largestUniTime, GUILayout.Width(SLIDER_WIDTH));
                        if (oldFloat != uniTimEnd)
                            KLScenario.dirtyFilter = true;
                        if (uniTimStart > uniTimEnd)
                            uniTimEnd = uniTimStart;
                        break;
                    case Fields.utcTime:
                        break;

                }
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

            }
            else
            {
                entryFieldScrollVector = GUILayout.BeginScrollView(entryFieldScrollVector);
                bool old;

                switch (entryField)
                {
                    case Fields.vesselName:
                        foreach (KeyValuePair<string, vesselId> vid in allVessels)
                        {
                            GUILayout.BeginHorizontal();
                            old = vid.Value.selected;
                            vid.Value.selected = GUILayout.Toggle(vid.Value.selected, vid.Value.vesselName);
                            if (old != vid.Value.selected)
                                KLScenario.dirtyFilter = true;
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case Fields.vesselSituation:
                        foreach (var s in situations)
                        {
                            GUILayout.BeginHorizontal();
                            old = s.Value.selected;
                            s.Value.selected = GUILayout.Toggle(s.Value.selected, LogEntry.displayVesselSituation(s.Value.situation));
                            if (old != s.Value.selected)
                                KLScenario.dirtyFilter = true;
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case Fields.controlLevel:
                        foreach (var vcl in controlLevels)
                        {
                            GUILayout.BeginHorizontal();
                            old = vcl.Value.selected;
                            vcl.Value.selected = GUILayout.Toggle(vcl.Value.selected, LogEntry.displayControlLevel(vcl.Value.controlLevel));
                            if (old != vcl.Value.selected)
                                KLScenario.dirtyFilter = true;
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case Fields.mainBody:
                        foreach (var body in bodiesList)
                        {
                            GUILayout.BeginHorizontal();
                            old = body.Value.selected;
                            body.Value.selected = GUILayout.Toggle(body.Value.selected, body.Value.bodyName);
                            if (old != body.Value.selected)
                                KLScenario.dirtyFilter = true;
                            GUILayout.EndHorizontal();
                        }
                        break;
                    case Fields.eventType:
                        foreach (var evt in eventTypes)
                        {
                            GUILayout.BeginHorizontal();
                            old = evt.Value.selected;
                            evt.Value.selected = GUILayout.Toggle(evt.Value.selected, LogEntry.displayEventString(evt.Value.evnt));
                            if (old != evt.Value.selected)
                                KLScenario.dirtyFilter = true;
                            GUILayout.EndHorizontal();
                        }
                        break;
                }


                GUILayout.EndScrollView();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();


                if (GUILayout.Button("Select All", GUILayout.Width(90)))
                {
                    KLScenario.dirtyFilter = true;
                    switch (entryField)
                    {
                        case Fields.vesselName:
                            foreach (KeyValuePair<string, vesselId> vid in allVessels)
                                vid.Value.selected = true;
                            break;
                        case Fields.vesselSituation:
                            foreach (var s in situations)
                                s.Value.selected = true;
                            break;
                        case Fields.controlLevel:
                            foreach (var vcl in controlLevels)
                                vcl.Value.selected = true;
                            break;
                        case Fields.mainBody:
                            foreach (var body in bodiesList)
                                body.Value.selected = true;
                            break;
                        case Fields.eventType:
                            foreach (var evt in eventTypes)
                                evt.Value.selected = true;
                            break;
                    }
                }
                if (GUILayout.Button("Deselect All", GUILayout.Width(90)))
                {
                    KLScenario.dirtyFilter = true;
                    switch (entryField)
                    {
                        case Fields.vesselName:
                            foreach (KeyValuePair<string, vesselId> vid in allVessels)
                                vid.Value.selected = false;
                            break;
                        case Fields.vesselSituation:
                            foreach (var s in situations)
                                s.Value.selected = false;
                            break;
                        case Fields.controlLevel:
                            foreach (var vcl in controlLevels)
                                vcl.Value.selected = false;
                            break;
                        case Fields.mainBody:
                            foreach (var body in bodiesList)
                                body.Value.selected = false;
                            break;
                        case Fields.eventType:
                            foreach (var evt in eventTypes)
                                evt.Value.selected = false;
                            break;
                    }
                }
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Width(90)))
            {
                entryField = Fields.none;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

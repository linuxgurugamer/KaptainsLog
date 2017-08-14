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
                            Shelter.dirtyFilter = true;
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
                            Shelter.dirtyFilter = true;
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
                            Shelter.dirtyFilter = true;
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
                            Shelter.dirtyFilter = true;
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
                            Shelter.dirtyFilter = true;
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
                Shelter.dirtyFilter = true;
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
                Shelter.dirtyFilter = true;
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

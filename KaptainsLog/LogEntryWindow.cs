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

        void DisplayLogEntryExitCleanup()
        {
            notesText = "";
            notesEntry = false;
            manualEntry = false;
           // escapePressed = false;
            notesEntryComplete = true;

            editItem = null;
            
            FlightDriver.SetPause(false);
        }

        internal void SaveEditItem()
        {
            Log.Info("Saving editItem: " + editItem.Value.ToString());
            //if (!escapePressed || notesText != "")
            if (notesText != "")
                    utils.le.notes = notesText;
            kaptainsLogList[editItem.Value] = utils.le;
            DisplayLogEntryExitCleanup();

        }
        void DisplayLogEntryWindow(int id)
        {
            //Log.Info("DisplayLogEntryWindow");
            if (utils.le == null )
                return;

            //
            // Check escapePressed here because otherwise the event is swallowed by Unity
            //

//if (Event.current.type == UnityEngine.EventType.KeyDown)
//    escapePressed = (Event.current.keyCode == KeyCode.Escape);

#if false
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            labelStyle.normal.textColor = Color.red;
            GUILayout.Label("Hit Escape to close", labelStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
#endif
            GUILayout.BeginHorizontal();
            GUILayout.Label("Vessel: ");
            GUILayout.TextField(utils.le.vesselName);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Situation: ");
            GUILayout.TextField(utils.le.displayVesselSituation());
            GUILayout.FlexibleSpace();
            GUILayout.Label("Event: ");
            GUILayout.TextField(utils.le.displayEventString());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            notesScrollVector = GUILayout.BeginScrollView(notesScrollVector);
            // Configurable font size, independent from the skin.

            notesText = GUILayout.TextArea(notesText, GUILayout.ExpandHeight(true));
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (manualEntry)
            {
                if (GUILayout.Button("Add Image", GUILayout.Width(90)))
                {
                    EnableImageSelectionFor(ImageSelectionFor.LogEntry);
                }
                GUILayout.FlexibleSpace();
            }
            if (GUILayout.Button("Add Crew", GUILayout.Width(90)))
            {
                notesText += utils.getCurrentCrew();
            }
            GUILayout.FlexibleSpace();
            var b1 = GUILayout.Button("Cancel", GUILayout.Width(90));
            //if (b1 || (escapePressed && notesText == ""))
            if (b1 || notesText == "")
            {
               // Log.Info("LogEntryWindow b1, escapePressed: " + escapePressed.ToString());
                pms = PauseMenuState.hidden;
#if false
                if (escapePressed)
                {
                    Log.Info("escapePressed, setting pms = PauseMenuState.KLEntry");
                    pms = PauseMenuState.KLEntry;
                    cancelManualEntry = true;
                }
#endif
                DisplayLogEntryExitCleanup();
               
                //if (visibleByToolbar)
                {
                    ToggleToolbarButton();
                  //  kaptainsLogStockButton.SetFalse();
                }
                kaptainsLogStockButton.SetFalse();
                notesText = "";
                //escapePressed = false;
            }
            if (editItem == null)
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Clear", GUILayout.Width(90)))
                    notesText = "";
            }
            GUILayout.FlexibleSpace();
            
            var b = GUILayout.Button("OK", GUILayout.Width(120));
            //if (escapePressed || b)
            if (b)
            {

                pms = PauseMenuState.hidden;
#if false
                if (escapePressed)
                {
                    Log.Info("LogEntryWindow b, escapePressed: " + escapePressed.ToString());
                    pms = PauseMenuState.KLEntry;
       }
#endif

                if (editItem != null)
                {
                    SaveEditItem();
                }
                else
                {
                    //if (!escapePressed || notesText != "")
                    if (notesText != "")
                    {
                        utils.le.notes = notesText;
                        //if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().screenshot)
                        if (eventScreenshot(utils.le) != ScreenshotOptions.No_Screenshot)
                        {
                            if (utils.le.screenshotName == null || utils.le.screenshotName == "")
                            {
                                Log.Info("queueScreenshot 1");
                                utils.queueScreenshot(ref utils.le);

                            }
                        }

                        lastPauseTime = Planetarium.GetUniversalTime();
                        lastNoteTime = Planetarium.GetUniversalTime() + HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().minTime;
                    }
                }

                DisplayLogEntryExitCleanup();

                if (visibleByToolbar)
                {
                    ToggleToolbarButton();
                    kaptainsLogStockButton.SetFalse();
                }
            }
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

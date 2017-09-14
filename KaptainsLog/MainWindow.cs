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
        void SelectAllExport(PrintType export)
        {
            for (int i = 0; i < kaptainsLogList.Count; i++)
                kaptainsLogList[i].printFlag = export;
        }


        // bool imgCacheFilled = false;
        List<GUIContent> imgCacheList = new List<GUIContent>();

        void FreeImgCache()
        {
            foreach (var i in imgSelList)
            {
                Destroy(i.image);
            }
            imgCacheList.Clear();
            KLScenario.imgCacheFilled = false;
        }

        void InitImageCache()
        {
            FreeImgCache();
            for (int i = 0; i < kaptainsLogList.Count; i++)
            {
                var le1 = kaptainsLogList[i];

                var fileName = le1.jpgThumbnailName;
                GUIContent content = new GUIContent();
                if (System.IO.File.Exists(fileName))
                {

                    string _imageurl = "file://" + fileName;
                    var imagetex = new WWW(_imageurl + _imageurl);
                    content.image = imagetex.texture;
                }
                imgCacheList.Add(content);
            }
            KLScenario.imgCacheFilled = true;
        }

        void UpdateImageCache()
        {
            for (int i = imgCacheList.Count; i < kaptainsLogList.Count; i++)
            {
                var le1 = kaptainsLogList[i];

                var fileName = le1.jpgThumbnailName;
                GUIContent content = new GUIContent();
                if (System.IO.File.Exists(fileName))
                {

                    string _imageurl = "file://" + fileName;
                    var imagetex = new WWW(_imageurl + _imageurl);
                    content.image = imagetex.texture;
                }
                imgCacheList.Add(content);
            }
            KLScenario.imgCacheFilled = true;
        }


        int? deleteItem, editItem, editNewItem;
        string lastSortField = "";
        string selectedSortField = "none";
        const string N_A = "N/A";

        int mainWinId;

        void DisplayMainWindow(int id)
        {
            mainWinId = id;

            if (KLScenario.dirtyColSel)
                getDataColLimits(displayFields);
            if (!KLScenario.imgCacheFilled)
                InitImageCache();

            GUIStyle bStyle = new GUIStyle(HighLogic.Skin.button);
            bStyle.normal.textColor = Color.red;

            lastSortField = "";
            GUILayout.BeginArea(new Rect(0, 0, 26, 26));
            if (GUILayout.Button(GameDatabase.Instance.GetTexture(QuestionMarkIcon, false), GUIStyle.none, GUILayout.Height(26), GUILayout.Width(26)))
            {
                IntroWindowClass.showHelp = true;
            }
            GUILayout.EndArea();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Vessel"))
                if (entryField == Fields.none)
                    entryField = Fields.vesselName;
                else
                    entryField = Fields.none;
            if (GUILayout.Button("Mission Time"))
                if (entryField == Fields.none)
                    entryField = Fields.missionTime;
                else
                    entryField = Fields.none;
            if (GUILayout.Button("Situation"))
                if (entryField == Fields.none)
                    entryField = Fields.vesselSituation;
                else
                    entryField = Fields.none;
            if (GUILayout.Button("Body"))
                if (entryField == Fields.none)
                    entryField = Fields.mainBody;
                else
                    entryField = Fields.none;
            if (GUILayout.Button("Altitude/Speed"))
                if (entryField == Fields.none)
                    entryField = Fields.altitude;
                else
                    entryField = Fields.none;
#if false
            if (GUILayout.Button("Speed"))
                if (entryField == Fields.none)
                    entryField = Fields.speed;
                else
                    entryField = Fields.none;
#endif
            if (GUILayout.Button("Event"))
                if (entryField == Fields.none)
                    entryField = Fields.eventType;
                else
                    entryField = Fields.none;
            if (entryField != Fields.none)
            {
                for (int d = 0; d < displayFields.Count; d++)
                    if (displayFields[d].f == entryField)
                        displayFields[d].filter = true;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label(" Sel", GUILayout.Width(30));
            GUILayout.Space(45);
            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none && displayFields[d].visible)
                {
                    GUILayout.Label(LogEntry.displayFieldName(displayFields[d].f), GUILayout.Width(colWidth[d]));
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            displayScrollVector = GUILayout.BeginScrollView(displayScrollVector);

            bool treeView = false;
            if (LogEntry.sortField == Fields.vesselId || LogEntry.sortField == Fields.vesselId || LogEntry.sortField == Fields.vesselName)
            {
                treeView = true;
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("N/A", toggleStyle))
                {
                    if (selectedSortField == N_A)
                        selectedSortField = "none";
                    else
                        selectedSortField = N_A;
                }
                GUILayout.EndHorizontal();
            }
            for (int i = 0; i < kaptainsLogList.Count; i++)
            {
                var le1 = kaptainsLogList[i];
                if (KLScenario.dirtyFilter)
                    ApplyFilters(le1);
                if (!le1.leSelected)
                    continue;

                string sortedField = "all";
                if (treeView)
                {
                    switch (LogEntry.sortField)
                    {
                        case Fields.vesselId:
                            sortedField = le1.vesselId + ": " + le1.vesselName;
                            break;
                        case Fields.vesselName:
                            sortedField = le1.vesselName;
                            break;
                        case Fields.mainBody:
                            sortedField = le1.vesselMainBody;
                            break;
                    }

                    toggleStyle.normal.textColor = Color.red;


                    if (sortedField != "")
                    {
                        if (lastSortField != sortedField)
                        {
                            if (sortedField == selectedSortField)
                                toggleStyle.normal.textColor = Color.green;
                            lastSortField = sortedField;

                            GUILayout.BeginHorizontal();
                            if (GUILayout.Button(sortedField, toggleStyle))
                            {
                                if (selectedSortField == sortedField)
                                    selectedSortField = "none";
                                else
                                    selectedSortField = sortedField;
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                }
                if (sortedField == "all" || (sortedField == "" && selectedSortField == N_A) || sortedField == selectedSortField)
                {
                    GUILayout.BeginHorizontal();

                    GUIStyle printStyle = new GUIStyle(GUI.skin.button);
                    printStyle.fixedWidth = 20;

                    string prDisplay = " ";

                    if (le1.printFlag != PrintType.noPrint)
                    {
                        printStyle.normal.background = greenButtonTexture;
                        printStyle.hover.background = greenButtonTexture;
                        printStyle.active.background = greenButtonTexture;

                        printStyle.normal.textColor = Color.black;
                        printStyle.hover.textColor = Color.black;
                        printStyle.active.textColor = Color.black;

                    }
                    if (le1.printFlag == PrintType.screenshotPrint)
                        prDisplay = "+";
                    if (GUILayout.Button(prDisplay, printStyle))
                    {
                        le1.printFlag = (PrintType)((int)le1.printFlag + 1);
                        if (le1.printFlag > PrintType.screenshotPrint)
                            le1.printFlag = PrintType.noPrint;
                    }

                    if (GUILayout.Button("x", bStyle, GUILayout.Height(21), GUILayout.Width(21)))
                    {
                        deleteItem = i;
                    }
                    if (GUILayout.Button(GameDatabase.Instance.GetTexture(PencilIcon, false), GUILayout.Height(21), GUILayout.Width(21)))
                    {
                        editNewItem = i;
                    }
                    for (int d = 0; d < displayFields.Count; d++)
                    {
                        if (displayFields[d].f != Fields.none && displayFields[d].visible)
                        {

                            string f = utils.getDisplayString(le1, displayFields[d].f);
                            if (displayFields[d].f != Fields.thumbnail)
                            {
                                GUILayout.TextField(f, GUILayout.Width(colWidth[d]));
                            }
                            else
                            {
                                if (i <= imgCacheList.Count - 1 && imgCacheList[i].image != null)
                                {

                                    if (GUILayout.Button(imgCacheList[i].image, GUIStyle.none, GUILayout.Width(imgCacheList[i].image.width), GUILayout.Height(imgCacheList[i].image.height)))
                                    {
                                        displayScreenshot = true;
                                        leToDisplay = le1;
                                        newLeToDisplay = true;
                                    }
                                }
                                else
                                {
                                    Log.Info("i: " + i.ToString() + "  imgCacheList.Count: " + imgCacheList.Count.ToString());
                                    GUILayout.Box("n/a", GUILayout.Width(colWidth[d]));
                                }
                            }
                        }

                    }
                    GUILayout.EndHorizontal();
                    GUILayout.Space(5);
                }
            }
            KLScenario.dirtyFilter = false;
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (/* displayFilterWindow ||*/ displayColSelectWindow)
                GUI.enabled = false;
            if (GUILayout.Button("Select All"))
            {
                SelectAllExport(PrintType.print);
            }
            if (GUILayout.Button("Select None"))
            {
                SelectAllExport(PrintType.noPrint);
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Sort", GUILayout.Width(90)))
            {
                displaySortWindow = true;
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Delete", GUILayout.Width(90)))
            {
                // Delete selected, if any
                ConfirmDelete();
                //DeleteSelected();
            }
            if (deleteConfirmed)
            {
                DeleteSelected();
                deleteConfirmed = false;
            }
#if false
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Filters", GUILayout.Width(90)))
            {
                displayFilterWindow = true;
                getDataColLimits(displayFields);
            }
#endif
            GUILayout.FlexibleSpace();

            if (displayFilterWindow || displayColSelectWindow)
                GUI.enabled = false;
            else GUI.enabled = true;

            if (GUILayout.Button("Select Fields", GUILayout.Width(90)))
            {
                KLScenario.dirtyColSel = true;
                displayColSelectWindow = true;
                getDataColLimits(displayFields, false);
            }
            GUILayout.FlexibleSpace();
            if (displayFilterWindow || displayColSelectWindow)
                GUI.enabled = false;
            else GUI.enabled = true;
            if (GUILayout.Button("Export", GUILayout.Width(90)))
            {
                EnableExportWindow();
            }
            GUILayout.FlexibleSpace();

            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close", GUILayout.Width(90)))
            {
                ToggleToolbarButton();
            }
            GUILayout.FlexibleSpace();
            if (FlightGlobals.ActiveVessel != null)
            {
                if (GUILayout.Button("Make Log Entry"))
                {
                    onManualEntry();

                }
            }
            if (GUILayout.Button("Screen Messages"))
            {
                ScreenMessagesLog.Instance.ShowWin(true);
                //visibleByToolbar = false;
                ToggleToolbarButton();
            }

            GUILayout.EndHorizontal();
            GUI.DragWindow();

            if (deleteItem != null)
            {
                DeleteSingleItem(deleteItem.Value);
                deleteItem = null;
            }
            if (editNewItem != null)
            {
                if (editItem != null)
                    SaveEditItem();

                utils.le = kaptainsLogList[editNewItem.Value];
                notesEntry = true;
                notesText = utils.le.notes;
                editItem = editNewItem;
                editNewItem = null;
            }
        }

        void DeleteSelected()
        {
            Log.Info("DeleteSelected");
            for (int i = kaptainsLogList.Count - 1; i >= 0; i--)
            {
                var le = kaptainsLogList[i];
                if (le.printFlag != PrintType.noPrint)
                {
                    DeleteSingleItem(i, false);
                }
            }
            utils.SaveLogs();
        }
        void DeleteFile(string s)
        {
            if (s.Trim() != "")
            {
                Log.Info("Deleting file: " + s);
                try
                {
                    System.IO.File.Delete(s);
                } catch (Exception ex)
                {
                    Log.Error("File delete failed, " + ex.Message);
                }
            }
        }
        void DeleteSingleItem(int idx, bool single = true)
        {
            var le = kaptainsLogList[idx];
            kaptainsLogList.RemoveAt(idx);

            if (single)
                utils.SaveLogs();
            // Check to see if the associated images are being used by another log record
            // If not, then they can be deleted.
            foreach (var l in kaptainsLogList)
            {
                if (le.screenshotName == l.screenshotName)
                    le.screenshotName = "";
                if (le.pngThumbnailName == l.pngThumbnailName)
                    le.pngThumbnailName = "";
                if (le.jpgThumbnailName == l.jpgThumbnailName)
                    le.jpgThumbnailName = "";
            }

            DeleteFile(le.screenshotName);
            DeleteFile(le.pngThumbnailName);
            DeleteFile(le.jpgThumbnailName);
            
        }

        private PopupDialog confirmDeletePopup = null;
        bool deleteConfirmed = false;
        public void ConfirmDelete()
        {
            deleteConfirmed = false;
            InputLockManager.SetControlLock(ControlTypes.All, "ConfirmDelete");

            string dialogMsg = "<color=white>Are you sure that you want to <color=red>permanently remove</color> the selected rows?";
            string windowTitle = "WARNING";

            DialogGUIBase[] options = { new DialogGUIButton("Cancel", DeleteCanceled), new DialogGUIButton("<color=orange>Confirm</color>", DeleteConfirmed) };

            MultiOptionDialog confirmationBox = new MultiOptionDialog("confirmDelete", dialogMsg, windowTitle, HighLogic.UISkin, options);

            confirmDeletePopup = PopupDialog.SpawnPopupDialog(confirmationBox, false, HighLogic.UISkin);
            GUI.BringWindowToFront(confirmDeletePopup.dialogToDisplay.id);
        }

        public void DeleteCanceled()
        {
            Invoke("Unlock", 0.5f);
        }

        public void DeleteConfirmed()
        {
            Invoke("Unlock", 0.5f);
            deleteConfirmed = true;
        }

        private void Unlock()
        {
            InputLockManager.RemoveControlLock("ConfirmDelete");
            confirmDeletePopup = null;
        }
    }
}

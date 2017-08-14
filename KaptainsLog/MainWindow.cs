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
            Shelter.imgCacheFilled = false;
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
            Shelter.imgCacheFilled = true;
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
            Shelter.imgCacheFilled = true;
        }


        int? deleteItem, editItem;

        void DisplayMainWindow(int id)
        {
            if (Shelter.dirtyColSel)
                getDataColLimits(displayFields);
            if (!Shelter.imgCacheFilled)
                InitImageCache();

            GUIStyle bStyle = new GUIStyle(HighLogic.Skin.button);
            bStyle.normal.textColor = Color.red;

            GUILayout.BeginHorizontal();
            GUILayout.Label("Prt", GUILayout.Width(30));
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
            for (int i = 0; i < kaptainsLogList.Count; i++)
            {
                var le1 = kaptainsLogList[i];
                if (Shelter.dirtyFilter)
                    ApplyFilters(le1);
                if (!le1.leSelected)
                {
                    continue;
                }
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
                    editItem = i;
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
                            if (i < imgCacheList.Count - 1 &&  imgCacheList[i].image != null)
                            {
                                    GUILayout.Box(imgCacheList[i].image, GUILayout.ExpandWidth(false));
                            }
                            else
                                GUILayout.Box("n/a", GUILayout.Width(8+colWidth[d]));
                        }
                    }

                }
                GUILayout.EndHorizontal();
            }
            Shelter.dirtyFilter = false;
            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (displayFilterWindow || displayColSelectWindow)
                GUI.enabled = false;
            if (GUILayout.Button("Export All"))
            {
                SelectAllExport(PrintType.print);
            }
            if (GUILayout.Button("Export None"))
            {
                SelectAllExport(PrintType.noPrint);
            }

            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Filters", GUILayout.Width(90)))
            {
                displayFilterWindow = true;
                getDataColLimits(displayFields);
            }
            GUILayout.FlexibleSpace();

            if (displayFilterWindow || displayColSelectWindow)
                GUI.enabled = false;
            else GUI.enabled = true;

            if (GUILayout.Button("Select Fields", GUILayout.Width(90)))
            {
                Shelter.dirtyColSel = true;
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
                visibleByToolbar = false;
                ToggleToolbarButton();
                kaptainsLogStockButton.SetFalse();
                // FreeImgCache(); // Done in the ToggleToolbarButton method
            }
            GUILayout.FlexibleSpace();
            if (FlightGlobals.ActiveVessel != null)
            {
                if (GUILayout.Button("Make Log Entry"))
                {
                    onManualEntry();

                }
            }
            if (GUILayout.Button("ScreenMessages"))
            {
                ScreenMessagesLog.Instance.ShowWin(true);
                visibleByToolbar = false;
                ToggleToolbarButton();
                kaptainsLogStockButton.SetFalse();
            }

            GUILayout.EndHorizontal();
            GUI.DragWindow();

            if (deleteItem != null)
            {
                kaptainsLogList.RemoveAt(deleteItem.Value);
                deleteItem = null;
                utils.SaveLogs();
            }
            if (editItem != null && !notesEntry)
            {
                utils.le = kaptainsLogList[editItem.Value];
                notesEntry = true;
                notesText = utils.le.notes;
            }
        }
    }
}

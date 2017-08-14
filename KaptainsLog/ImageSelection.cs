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
        public enum ImageSelectionFor { LogEntry, ExportHeader, ExportFooter };

        ImageSelectionFor imageSelectionFor;

        void EnableImageSelectionFor(ImageSelectionFor isf)
        {
            imageSelectionFor = isf;
            imageSelection = true;
            fileEntries = Directory.GetFiles(screenshotPath);
        }

        void DisableImageSelection()
        {
            imageSelection = false;
            FreeImgSelList();
        }

        bool imgSelListFilled = false;
        List<GUIContent> imgSelList = new List<GUIContent>();

        void FreeImgSelList()
        {
            foreach (var i in imgSelList)
            {
                Destroy(i.image);
            }
            imgSelList.Clear();
            imgSelListFilled = false;
        }

        void InitImageSelectionList()
        {
            FreeImgSelList();
            foreach (string fileName in fileEntries)
            {
                if (screenshotPrefix == fileName.Substring(0, screenshotPrefix.Length))
                    continue;
                GUIContent content = new GUIContent();
                content.image = MakeThumbnailFrom(fileName, HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().thumbnailSize);
                content.text = fileName.Substring(fileName.LastIndexOf('/') + 1);
                imgSelList.Add(content);
            }
            imgSelListFilled = true;
        }

        
        void DisplayImageSelectionWindow(int id)
        {
            if (!imgSelListFilled)
                InitImageSelectionList();
            GUILayout.BeginHorizontal();
            switch (imageSelectionFor)
            {
                case ImageSelectionFor.LogEntry:
                    GUILayout.Label("Select screenshot to attach to log:");
                    break;
                case ImageSelectionFor.ExportHeader:
                    GUILayout.Label("Select screenshot for export header:");
                    break;
                case ImageSelectionFor.ExportFooter:
                    GUILayout.Label("Select screenshot for export footer:");
                    break;
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            imageFileSelScrollVector = GUILayout.BeginScrollView(imageFileSelScrollVector);
            int cnt = 0;
            GUIStyle bStyle = new GUIStyle(HighLogic.Skin.label);
            bStyle.stretchHeight = false;
            bStyle.stretchWidth = false;

            foreach (GUIContent content in imgSelList)
            {
                GUILayout.BeginHorizontal();
                if (lastSelectedImage != cnt)
                    bStyle.normal.textColor = Color.red;
                else
                    bStyle.normal.textColor = Color.green;

                if (GUILayout.Button(content, bStyle))
                {
                    lastSelectedImage = cnt;
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
                switch (imageSelectionFor)
                {
                    case ImageSelectionFor.LogEntry:
                        utils.le.screenshotName = fileEntries[lastSelectedImage];
                        break;
                    case ImageSelectionFor.ExportHeader:
                        exportHeaderImage = fileEntries[lastSelectedImage];
                        break;
                    case ImageSelectionFor.ExportFooter:
                        exportFooterImage = fileEntries[lastSelectedImage];
                        break;
                }
                DisableImageSelection();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel", GUILayout.Width(90)))
                DisableImageSelection();
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }

    }
}

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
        bool csv = false;
        bool html = false;
        bool openInBrowser = true;
        string saveFile = "";
        string htmlTemplate = "";

        string exportHeaderTitle, exportFooterTitle;
        string exportHeaderImage, exportFooterImage;
        int pagenum = 0;

        void EnableExportWindow()
        {
            displayExportWindow = true;
            exportHeaderImage = "";
            exportFooterImage = "";
            exportHeaderTitle = "";
            exportFooterTitle = "";
            pagenum = 1;
        }

        void SaveAsCSV(string fname)
        {
            Log.Info("SaveAsCSV, saving to: " + SAVE_PATH + "/" + fname + ".csv");
            // Open the stream and write to it.
            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(SAVE_PATH + "/" + fname + ".csv"))
            {
                for (int i = 0; i < kaptainsLogList.Count; i++)
                {
                    var le1 = kaptainsLogList[i];
                    if (Shelter.dirtyFilter)
                        ApplyFilters(le1);
                    if (!le1.leSelected)
                        continue;
                    string line = "\"";
                    for (int d = 0; d < displayFields.Count; d++)
                    {
                        if (displayFields[d].f != Fields.none && displayFields[d].visible)
                        {
                            string f = utils.getDisplayString(le1, displayFields[d].f);
                            if (line.Length > 1)
                                line += ",";
                            line += f + "\"";
                        }

                    }
                    fs.WriteLine(line);
                }
            }
        }

        string LoadTemplate(string template)
        {
            Log.Info("LoadTemplate, name: " + template);
            string s = File.ReadAllText(template);
            return s;
        }

        string TemplateField(Fields f)
        {
            switch (f)
            {
                case Fields.vesselName:
                    return "VesselName";
                case Fields.universalTime:
                    return "UniversalTime";
                case Fields.utcTime:
                    return "UTCtime";
                case Fields.missionTime:
                    return "MissionTime";
                case Fields.vesselSituation:
                    return "Situation";
                case Fields.controlLevel:
                    return "ControlLevel";
                case Fields.mainBody:
                    return "MainBody";
                case Fields.altitude:
                    return "Altitude";
                case Fields.speed:
                    return "Speed";
                case Fields.eventType:
                    return "EventType";
                case Fields.notes:
                    return "Notes";
                case Fields.thumbnail:
                    return "Thumbnail";
                case Fields.screenshot:
                    return "Screenshot";
                case Fields.lastItem:
                    return "none";
            }

            return "";
        }
        
        string formatImgHtml(string imgHtml, string fname)
        {
            string s = imgHtml.Replace("[url]", fname);
            return s;
        }
        void processData(ref string template, string tag, string data)
        {
            template = template.Replace(tag, data);
        }

        string getFlagURL(string flag)
        {
            string s = GAMEDATA_FOLDER + "/" + flag;
            if (System.IO.File.Exists(s + ".png"))
                return s + ".png";
            if (System.IO.File.Exists(s + ".jpg"))
                return s + ".jpg";

            return s;
        }
        string ProcessTemplate(string fname, LogEntry le)
        {
            string template = LoadTemplate(fname);
            
            string data, s;
            for (int d = 0; d < (int)Fields.lastItem; d++)
            {
                var fld = (Fields)d;
                s = "[" + TemplateField(fld) + "]";
                
                data = utils.getDisplayString(le, fld, true);
                Log.Info("d: " + d.ToString() + ",   fld: " + fld.ToString() + ",   s: " + s + ",   data: " + data);
                if (fld == Fields.thumbnail || fld == Fields.screenshot)
                {
                    if (fld == Fields.screenshot && le.printFlag != PrintType.screenshotPrint)
                    {
                        data = "";
                    }
                    else
                    {
                        switch (fld)
                        {
                            case Fields.screenshot:
                                if (isHtml)
                                    data = formatImgHtml(screenshotHtml, data); break;
                            case Fields.thumbnail:
                                if (isHtml)
                                    data = formatImgHtml(thumbnailHtml, data); break;
                             
                        }
                    }
                }
                processData(ref template, s, data);
            }

            // do process for each of the following:
            string[] lines = le.notes.Split('\n');
            if (lines.Length > 0)
            {
                s = "[Notes-0]"; data = lines[0];
                processData(ref template, s, data);
            }
            s = "[Notes-1]";
            if (lines.Length > 1)
                data = lines[1];
            else
                data = "";
            processData(ref template, s, data);
            s = "[VesselFlag]"; data = getFlagURL(le.vesselFlagURL);
            if (isHtml)
                data = formatImgHtml(flagHtml, data);
            processData(ref template, s, data);
            return template;
        }

        string ProcessTemplate(string fname)
        {
            Log.Info("ProcessTemplate, fname: " + fname);
            string template = LoadTemplate(fname);
            string data, s;

            s = "[HeaderImg]"; data = exportHeaderImage;
            processData(ref template, s, data);
            if (isHtml)
                data = formatImgHtml(headerImgHtml, data);
            s = "[FooterImg]"; data = exportFooterImage;
            if (isHtml)
                data = formatImgHtml(footerImgHtml, data);
            processData(ref template, s, data);
            s = "[Page]"; data = pagenum.ToString();
            processData(ref template, s, data);
            s = "[HeaderTitle]"; data = exportHeaderTitle;
            processData(ref template, s, data);
            s = "[FooterTitle]"; data = exportFooterTitle;
            processData(ref template, s, data);
            s = "[Flag]"; data = getFlagURL(HighLogic.CurrentGame.flagURL);
            if (isHtml)
                data = formatImgHtml(flagHtml, data);
            processData(ref template, s, data);
            return template;
        }

        bool isHtml;
        int entriesPerPage;
        string fileSuffix;
        string headerTmpl, detailTmpl, footerTmpl;

        string flagHtml, screenshotHtml, thumbnailHtml, headerImgHtml, footerImgHtml;

        const string templateConfig = "/TemplateConfig.cfg";

        void loadTemplateConfig(string htmlTemplate)
        {
            ConfigNode templateCfg = null;
            ConfigNode templateCfgFile = ConfigNode.Load(htmlTemplate + templateConfig);

            // Set the defaults here
            isHtml = false;
            entriesPerPage = -1;
            fileSuffix = "html";
            headerTmpl = "header.tmpl";
            detailTmpl = "detail.tmpl";
            footerTmpl = "footer.tmpl";

            flagHtml = "<img src=\"[url]\">";
            screenshotHtml= "<img src=\"[url]\">";
            thumbnailHtml = "<img src=\"[url]\">";
            headerImgHtml = "<img src=\"[url]\">";
            footerImgHtml = "<img src=\"[url]\">";

            if (templateCfgFile != null)
                templateCfg = templateCfgFile.GetNode("TemplateConfig");
            if (templateCfg != null)
            {
                var isHtmlStr = templateCfg.GetValue("isHtml");

                if (isHtmlStr == "true" || isHtmlStr == "True" || isHtmlStr == "TRUE")
                    isHtml = true;
                entriesPerPage = Int32.Parse(Utils.SafeLoad(templateCfg.GetValue("entriesPerPage"), entriesPerPage));
                fileSuffix = Utils.SafeLoad(templateCfg.GetValue("fileSuffix"), fileSuffix);
                headerTmpl = Utils.SafeLoad(templateCfg.GetValue("header"), headerTmpl);
                detailTmpl = Utils.SafeLoad(templateCfg.GetValue("detail"), detailTmpl);
                footerTmpl = Utils.SafeLoad(templateCfg.GetValue("footer"), footerTmpl);

                flagHtml = Utils.SafeLoad(templateCfg.GetValue("flagHtml"), flagHtml);
                screenshotHtml = Utils.SafeLoad(templateCfg.GetValue("screenshotHtml"), screenshotHtml);
                thumbnailHtml = Utils.SafeLoad(templateCfg.GetValue("thumbnailHtml"), thumbnailHtml);

            }

            Log.Info("isHtml: " + isHtml.ToString());
        }

        void SaveAsHtml(string htmlTemplate, string saveFile)
        {
            Log.Info("SaveAsHtml, config file: " + htmlTemplate + templateConfig);
            
            loadTemplateConfig(htmlTemplate);
            
            string header = ProcessTemplate(htmlTemplate + "/" + headerTmpl);
            string detail = "";

            for (int i = 0; i < kaptainsLogList.Count; i++)
            {

                var le = kaptainsLogList[i];
                if (!le.leSelected || le.printFlag == PrintType.noPrint)
                    continue;

                detail += ProcessTemplate(htmlTemplate + "/" + detailTmpl, le);
            }
            string footer = ProcessTemplate(htmlTemplate + "/" + footerTmpl);

            string fullOutput = header + detail + footer;
            Log.Info("SaveAsHtml, output file: " + saveFile + "." + fileSuffix);

            File.WriteAllText(saveFile + "." + fileSuffix, fullOutput);
            Application.OpenURL("file://" + saveFile + "." + fileSuffix);         
        }

        bool deleteExistingFile = false;
        void DisplaySaveWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(" ");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Space(200);
            csv = GUILayout.Toggle(csv, "Save all fields in CSV file");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Space(200);
            html = GUILayout.Toggle(html, "Save as HTML file");
            GUILayout.EndHorizontal();
            if (html)
                GUI.enabled = true;
            else
                GUI.enabled = false;

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Select HTML Template", GUILayout.Width(150)))
            {
                dirEntries = Directory.GetDirectories(htmlTemplatePath);
                displayHTMLTemplate = true;
            }
            GUILayout.Space(50);
            GUILayout.TextField(htmlTemplate.Substring(htmlTemplate.LastIndexOf('/') + 1));
            GUILayout.EndHorizontal();
            GUILayout.Box(GUIContent.none, MyGUIStyles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Header Config");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Optional title:", GUILayout.Width(150));
            GUILayout.Space(50);
            exportHeaderTitle = GUILayout.TextField(exportHeaderTitle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Optional header image", GUILayout.Width(150)))
            {
                EnableImageSelectionFor(ImageSelectionFor.ExportHeader);
            }
            GUILayout.Space(50);
            GUILayout.TextField(exportHeaderImage.Substring(exportHeaderImage.LastIndexOf('/') + 1));
            GUILayout.EndHorizontal();
            GUILayout.Box(GUIContent.none, MyGUIStyles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(1f));
            GUILayout.BeginHorizontal();
            GUILayout.Label("Footer Config");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Optional title:", GUILayout.Width(150));
            GUILayout.Space(50);
            exportFooterTitle = GUILayout.TextField(exportFooterTitle);
            GUILayout.EndHorizontal();
            GUILayout.Box(GUIContent.none, MyGUIStyles.EditorLine, GUILayout.ExpandWidth(true), GUILayout.Height(1f));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Optional footer image", GUILayout.Width(150)))
            {
                EnableImageSelectionFor(ImageSelectionFor.ExportFooter);
            }
            GUILayout.Space(50);
            GUILayout.TextField(exportFooterImage.Substring(exportFooterImage.LastIndexOf('/') + 1));
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            GUILayout.Space(200);
            openInBrowser = GUILayout.Toggle(openInBrowser, "Open in browser");
            GUILayout.EndHorizontal();
            GUI.enabled = true;
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Files will be saved in the save directory");
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Enter file name (no suffix): ");
            saveFile = GUILayout.TextField(saveFile, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            if (System.IO.File.Exists(SAVE_PATH + "/" + saveFile + "." + fileSuffix))
            {
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                labelStyle.normal.textColor = Color.red;
                GUIStyle toggleStyle = new GUIStyle(GUI.skin.toggle);
                toggleStyle.normal.textColor = Color.red;
                deleteExistingFile = GUILayout.Toggle(deleteExistingFile, "Delete existing file");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            else
                deleteExistingFile = false;
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel", GUILayout.Width(60)))
            {
                displayExportWindow = false;
            }
            GUILayout.FlexibleSpace();
            if (saveFile == "" || (!csv && !html))
                GUI.enabled = false;
            else
                GUI.enabled = true;
            if (GUILayout.Button("Export", GUILayout.Width(60)))
            {
                displayExportWindow = false;
                //SAVE_PATH = ROOT_PATH + "saves/" + HighLogic.SaveFolder;
                if (csv)
                {
                    SaveAsCSV(saveFile);
                }
                if (html)
                {
                    SaveAsHtml(htmlTemplate, SAVE_PATH + "/" + saveFile);
                }
            }
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            GUI.enabled = true;

            GUI.DragWindow();
        }

        //Class to hold custom gui styles
        public static class MyGUIStyles
        {
            private static GUIStyle m_line = null;

            //constructor
            static MyGUIStyles()
            {

                m_line = new GUIStyle("box");
                m_line.border.top = m_line.border.bottom = 1;
                m_line.margin.top = m_line.margin.bottom = 1;
                m_line.padding.top = m_line.padding.bottom = 1;
            }

            public static GUIStyle EditorLine
            {
                get { return m_line; }
            }
        }
    }
}

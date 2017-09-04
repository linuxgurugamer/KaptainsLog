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
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public partial class IntroWindowClass : MonoBehaviour
    {
        static bool shown = false;
        static string lastGameShown = "";

        Rect introWindow;
        int introWindowId = GUIUtility.GetControlID(FocusType.Native);
        int MAIN_WIDTH = Screen.height *3/4;
        const int MAIN_HEIGHT = 400;
        int automoved = 0;

        GUIStyle areaStyle;

        private void Start()
        {
            MAIN_WIDTH = Screen.height * 3 / 4;
            introWindow = new Rect((Screen.width - MAIN_WIDTH) / 2, (Screen.height - MAIN_HEIGHT) / 2, MAIN_WIDTH, MAIN_HEIGHT);
            areaStyle = new GUIStyle(HighLogic.Skin.textArea);
            areaStyle.richText = true;
            msg = msgOverview;
            LoadImage("Settings.png");
        }
        public void OnGUI()
        {
            if ((shown && lastGameShown == HighLogic.SaveFolder) || !HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().EnabledForSave || !HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().showIntroAtStartup)
                return;
            introWindow = GUILayout.Window(introWindowId, introWindow, IntroWindow, "Kaptain's Log Intro", KaptainsLog.windowStyle);
            if (automoved < 2)
            {
                introWindow.x = (Screen.width - introWindow.width) / 2;
                introWindow.y = (Screen.height - introWindow.height) / 2;
                automoved++;
            }
        }

        const string imagePath = "GameData/KaptainsLog/Images/";
        string msg = null;
        Texture2D image;
        string msgOverview =
            "\nBe sure to check the various settings for The Kaptain's Log.\n" +
            "This window will be shown at the beginning of each game until you disable it\n" +
            "in the settings.\n\n" +                      
            "There are two settings pages, shown on the right.  The first page contains general\n" + 
            "options, the second page contains options specific to the various events that can\n" + 
            "be recorded\n" +
            "\n" +
            "First page:\n" +
            "     <color=yellow><B>General Settings</B></color> contains general options related to the mod as a whole\n" +
            "     <color=yellow><B>Initial Display Columns</B></color> specifies which columns should be displayed in the\n" +
            "        main window when it is first shown\n" +
            "     <color=yellow><B>Misc</B></color> contains limits on filters, and options for the Screen Messages\n" +
            "\n" +
            "Second page:\n" +
            "     <color=yellow><B>Event Capture Settings</B></color> specifies which events will be captured and logged.\n" +
            "     <color=yellow><B>Event Pause Settings</B></color> specifies which events which, when captured, will pause\n" +
            "         the game and open a window to add notes\n" +
            "     <color=yellow><B>Event Screenshot Settings</B></color> specifies which events which, when captured, will have\n" +
            "        a screenshot take.\n" +
            "\n" +
            "You can disable this window from showing up by toggling the option on the first settings page, under General Settings";

        string globalSettings = 
            "There is a setting on the General Settings which says to use global settings forall the settings.  Additionally,\n" + 
            "each settings section also has an option to use global settings for that section.\n\n" +
            "This gives you the ability to have some of the options used for all the games, and to\n" +
            "have customized settings for specific games\n\n" +
            "Global settings can only be set when in a game; if this is a new install, global settings\n" +
            "will be the initial defaults.  If a new game, the global settings will be loaded once\n" +
            "the game is started";

        string generalSettings = "The <color=yellow><B>General Settings</B></color> column in the first settings page contains general options related to the mod as a whole";
        string initialDisplay = "The <color=yellow><B>Initial Display Columns</B></color> column in the first settings page specifies which columns should be displayed in the main window when it is first shown";
        string misc = "The <color=yellow><B>Misc</B></color> column in the first settings page contains limits on filters, and options for the Screen Messages";
        string captureSettings = "The <color=yellow><B>Event Capture Settings</B></color> column in the second settings page specifies which events will be captured and logged";
        string pauseSettings = "The <color=yellow><B>Event Pause Settings</B></color> column in the second settings page specifies which events which, when captured, will pause the game and open a window to add notes";
        string screenshotSettings = "The <color=yellow><B>Event Screenshot Settings</B></color> column in the second settings page specifies which events which, when captured, will have a screenshot taken";

        void LoadImage(string s)
        {
            image = ImageViewer.LoadImage(KaptainsLog.ROOT_PATH + imagePath + s, (int)introWindow.width / 2, (int)introWindow.height / 2);
        }
        void IntroWindow(int id)
        {
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Overview"))
            {
                msg = msgOverview;
                LoadImage("Settings.png");
            }
            if (GUILayout.Button("Global"))
            {
                msg = globalSettings;
                LoadImage("GlobalSettings.png");
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("General Settings"))
            {
                msg = generalSettings;
                LoadImage("GeneralSettings.png");
            }
            if (GUILayout.Button("Initial Display Columns"))
            {
                msg = initialDisplay;
                LoadImage("InitialDisplay.png");
            }
            if (GUILayout.Button("Misc"))
            {
                msg = misc;
                LoadImage("Misc.png");
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Event Capture Settings"))
            {
                msg = captureSettings;
                LoadImage("CaptureSettings.png");
            }
            if (GUILayout.Button("Event Pause Settings"))
            {
                msg = pauseSettings;
                LoadImage("PauseSettings.png");
            }
            if (GUILayout.Button("Event Screenshot Settings"))
            {
                msg = screenshotSettings;
                LoadImage("ScreenshotSettings.png");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            
            if (image != null)
            {
                GUILayout.BeginVertical(GUILayout.Width(introWindow.width / 2));
                GUILayout.TextArea(msg, areaStyle);
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
                GUILayout.Box(image);
                GUILayout.FlexibleSpace();
            }
            else
            {
                GUILayout.BeginVertical();
                GUILayout.TextArea(msg, areaStyle);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("OK"))
            {
                shown = true;
                lastGameShown = HighLogic.SaveFolder;
            }
            
            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

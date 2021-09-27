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
using ClickThroughFix;

namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, true)]
    public partial class IntroWindowClass : MonoBehaviour
    {
        public static bool showHelp = false;
        static  bool shown = false;
        static string lastGameShown = "";

        Rect introWindow;
        int introWindowId;
        int MAIN_WIDTH = Screen.height *3/4;
        int MAIN_HEIGHT = 400;
        int automoved = 0;

        GUIStyle areaStyle;

        void Awake()
        {
            introWindowId = GUIUtility.GetControlID(FocusType.Passive);
        }

        private void Start()
        {
            MAIN_WIDTH = Screen.width * 3 / 4;
            MAIN_HEIGHT = Screen.height *3/4;

            introWindow = new Rect((Screen.width - MAIN_WIDTH) / 2, (Screen.height - MAIN_HEIGHT) / 2, MAIN_WIDTH, MAIN_HEIGHT);
            areaStyle = new GUIStyle(HighLogic.Skin.textArea);
            areaStyle.richText = true;
            msg = msgOverview;
            LoadImage("Settings.png");
            DontDestroyOnLoad(this);
        }

        public void OnGUI()
        {
            if (HighLogic.CurrentGame == null)
                return;
            if (!showHelp && ((shown && lastGameShown == HighLogic.SaveFolder) || !HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().EnabledForSave || !HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().showIntroAtStartup))
                return;
            introWindow = ClickThruBlocker.GUILayoutWindow(introWindowId, introWindow, IntroWindow, "Kaptain's Log Intro", KaptainsLog.windowStyle);
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
            "The Kaptain's Log is designed to record events and screenshots of your missions.  It can record \n" +
            "more than 40 different events.\n" +
            "\n" +
            "<color=yellow><B>Tags</B></color>\n" +
            "You can specify a tag to be applied to events as they occur, you can enter your own\n" +
            "notes for each event, and you can create an entry at any time by using the hotkey (default 'O')\n" +
            "\n" +
            "<color=yellow><B>Resizing Windows</B></color>\n" +
            "Two of the windows can be resized by dragging the bottom edge, right edge or lower-right corner:\n" +
            "     the Main Window\n" +
            "     the Screen Messages window\n\n" +
            "<color=yellow><B>Settings</B></color>\n" +
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
            "There is a setting on the General Settings which says to use global settings for all the settings.  Additionally, " + 
            "each settings section also has an option to use global settings for that section.\n\n" +
            "This gives you the ability to have some of the options used for all the games, and to " +
            "have customized settings for specific games\n\n" +
            "Global settings can only be set when in a game; if this is a new install, global settings " +
            "will be the initial defaults.  If a new game, the global settings will be loaded once " +
            "the game is started";

        string generalSettings = "The <color=yellow><B>General Settings</B></color> column in the first settings page contains general options related to the mod as a whole.\n\n" +
            "This is where you can disable this Intro/Help window, as well as select using the Blizzy toolbar if it is available.\n\n" +
            "You also specify the general settings about the screenshots, " +
            "such as where they should be saved, where the thumbnails should be saved, size of thumbnail, delay before taking the screenshot, etc.\n\n" +
            "The option <B>Use Bilinear filtering for thumbnail</b> defaults to on, if your thumbnails aren't clear, try turning it off.\n\n" ;

        string initialDisplay = "The <color=yellow><B>Initial Display Columns</B></color> column in the first settings page specifies which columns should be displayed in the main window when it is first shown.\n\n" +
            "You can easily change which columns are shown during the game, but these are the initial columns shown when starting the game.  You can change which columns are shown at the start on this window.";

        string misc = "The <color=yellow><B>Misc</B></color> column in the first settings page contains limits on filters, and options for the Screen Messages.\n\n" +
            "The <B>Filter Limits</B> are used as an upper limit when specifying an altitude or speed filter.\n\n" +
            "The <b>Screen Message Log</b> options control the maximum messages which are displayed.  The <B>Keep screen message for</b> specifies how long they are kept before being removed.";

        string hotkeyEntry = "The hotkey is used when you want to create a manual entry in the log.\n\n"+
            "Press the key you want to use to bring up the Manual Entry window, the key you press will be shown.\n\n" + 
            "The default is current the letter 'O'";

        string captureSettings = "The <color=yellow><B>Event Capture Settings</B></color> column in the second settings page specifies which events will be captured and logged.\n\n" +
            "You can specify <b>Log all events</b> which will enable logging on all the available events.  Selecting the <b>Unset all event log settings</b> will disable all of them.  Note that this button does not stay enabled; once all the events are disabled, this option is also disabled";


        string pauseSettings = "The <color=yellow><B>Event Pause Settings</B></color> column in the second settings page specifies which events which, when captured, will pause the game and open a window to add notes.\n\n" +
            "You can always go in and edit a log entry after it's been logged.  This section has the same options to enable all the pauses, and unset all the pauses as the Capture settings window.  Unless you wish to be constanly interrupt, you should only enable pausing for those events which are especially important.";

        string screenshotSettings = "The <color=yellow><B>Event Screenshot Settings</B></color> column in the second settings page specifies which events which, " + 
            "when captured, will have a screenshot taken.\n\n" +
            "This section has the same options to enable all the screencaptures, and unset all the screencaptures as the Capture settings window.\n\n" + 
            "There are three possible values for each setting: <color=yellow><B>No_Screenshot</B></color>, <color=yellow><B>With_GUI</B></color>, and <color=yellow><B>Without_GUI</B></color>\n\n" +
            "At the top are three options to set all the screenshot options, these three will set all the screenshot settings to the same value, and you can then " +
            " change individual settings.\n\n" +
            "When a screenshot is taken without the GUI, the screen flickers while the game hides the GUI, takes the screenshot and then restores the GUI.  This can both be " +
            " a distraction and for some people, can cause problems due to the flickering.  These options have been provided to allow you to tailor the screenshots to what you want/need.\n\n";

        string mainWindow = "This is the main window where all the logs are shown.  There are three major sections, each circled by a different color in this picture.\n\n" +
            "Top-left is a black circle with a yellow questionmark, clicking that will bring up this help screen.\n\n" +
            "At the top, circled in <color=yellow><B>Yellow</b></color> are the filter buttons.  Selecting any of these buttons brings up a window where you can specify filtering contraints against all the log files.\n\n" +
            "In the center, circled in <color=green><B>Green</b></color> are the actual log entries, with whatever columns have been selected to display.\n\n" +
            "At the bottom, circled in <color=red><b>Red</b></color> are the various things you can do.\n\n\n" +
            "Things to do are:\n\n" +
            "<B><color=yellow>Select All</color></B> - Select all rows for action.  Clicking on the Sel button will select single rows, and clicking again will add a <B>+</b> which indicates that when printed, the screenshow will be printed\n" +
            "<B><color=yellow>Select None</color></B> - Removes the selection from all\n" +
            "<B><color=yellow>Sort</color></B> - Open a sort window to be able to sort the list\n" +
            "<B><color=yellow>Delete</color></B> - Delete all selected rows\n" +
            "<B><color=yellow>Select Fields</color></B> - Select which columns to display\n" +
            "<B><color=yellow>Export</color></B> - Start the export/print process\n" +
            "<B><color=yellow>Close</color></b> - Close the window\n" +
            "<B><color=yellow>Specify Tag</color></B> - Click to specify a tag.  Current tag is shown in parentheses\n" +
            "<B><color=yellow>Screen Messages</color></b> - Open the Screen Messages window";

        string tagEntry = "The tag is a way to mark log entries for a specific period of time.  For example,\n" +
            "if  you are doing a test flight you could set a tag so you could see only the test flight tags (once you set up the filters)\n\n" +
            "Enter the tag you want applied to all new entries.  There are three options which can be toggled to automatically disable the tag at specific events:\n\n" +
            "   Disable tag on revert\n" +
            "   Disable tag on landing, splashdown or crash\n" +
            "   disable tag on vessel recovery";


        string sortSelection = "Select which field to use for sorting on this screen.  The selected field is <b><color=green>Green</color></b>, and all other fields are <B><color=red>Red</color></b>";

        string displayFieldSelection = "Select columns to display";

        string screenMessages = "The Screen message window shows all the messages which pop up in <b><color=yellow>Yellow</color></b> at the four locations on the screen, upper left, upper right, top middle and lower middle.\n\n" +
            "The maximum number of messages displayed is configured in the settings, as is the amount of time a message will stay around until it is expired.";

        string displayExportSave = "Export or save window";

        string displayHTMLSelection = "The HTML template window is where you select the HTML template which you want to use.  As is normal, the selected one is <b><color=green>Green</color></b>, and all other templates are <B><color=red>Red</color></b>";

        string displayImageSelection = "The Image Selection window is where you can select any image in the Screenimages folder";

        string imageViewer = "The Image viewer window shows the selected screencapture, and has the ability to view the image either larger or smaller.  Additionally, you can do a quick export, to print just the single record associated with the image being displayed.";

        void LoadImage(string s)
        {
            image = ImageViewer.LoadImage(KaptainsLog.ROOT_PATH + imagePath + s, (int)introWindow.width / 2, (int)introWindow.height / 2);
        }

        void IntroWindow(int id)
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(150));

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
            if (GUILayout.Button("Hotkey"))
            {
                msg = hotkeyEntry;
                LoadImage("HotkeyEntry.png");
            }

            if (GUILayout.Button("Evt Capture Settings"))
            {
                msg = captureSettings;
                LoadImage("CaptureSettings.png");
            }
            if (GUILayout.Button("Evt Pause Settings"))
            {
                msg = pauseSettings;
                LoadImage("PauseSettings.png");
            }
            if (GUILayout.Button("Evt Screenshot Settings"))
            {
                msg = screenshotSettings;
                LoadImage("ScreenshotSettings.png");
            }
            GUILayout.Space(20);
            if (GUILayout.Button("Main Window"))
            {
                msg = mainWindow;
                LoadImage("Mainwindow.png");
            }
            if (GUILayout.Button("Tag Entry"))
            {
                msg = tagEntry;
                LoadImage("TagEntry.png");
            }
            if (GUILayout.Button("Sort Selection Window"))
            {
                msg = sortSelection;
                LoadImage("SortSelection.png");
            }
            if (GUILayout.Button("Display Field Selection Window"))
            {
                msg = displayFieldSelection;
                LoadImage("FieldDisplaySelection.png");
            }
            if (GUILayout.Button("Display Screen Messages Window"))
            {
                msg = screenMessages;
                LoadImage("ScreenMessages.png");
            }
            if (GUILayout.Button("Display Export/Save Window"))
            {
                msg = displayExportSave;
                LoadImage("Save.png");
            }
            if (GUILayout.Button("Display HTML Template Selection Window"))
            {
                msg = displayHTMLSelection;
                LoadImage("HTMLTemplateSelection.png");
            }
            if (GUILayout.Button("Display Image Selection Window"))
            {
                msg = displayImageSelection;
                LoadImage("ImageSelection.png");
            }
            if (GUILayout.Button("Display Image Viewer Window"))
            {
                msg = imageViewer;
                LoadImage("ImageViewer.png");
            }
            GUILayout.EndVertical();

            if (image != null)
            {
                GUILayout.BeginVertical(GUILayout.Width((MAIN_WIDTH - 160) / 2));
                GUILayout.TextArea(msg, areaStyle);
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width((MAIN_WIDTH - 160) / 2));
                GUILayout.Box(image, GUILayout.Width((MAIN_WIDTH - 160) / 2));
                //GUILayout.FlexibleSpace();
            }
            else
            {
                GUILayout.BeginVertical(GUILayout.Width(MAIN_WIDTH - 160));
                GUILayout.TextArea(msg, areaStyle);
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("OK"))
            {
                shown = true;
                showHelp = false;
                lastGameShown = HighLogic.SaveFolder;
            }            

            GUILayout.EndHorizontal();
            GUI.DragWindow();
        }
    }
}

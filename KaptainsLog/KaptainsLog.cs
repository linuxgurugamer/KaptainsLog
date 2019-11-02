using System;
using System.Collections.Generic;
using KSP.UI.Screens;
using UnityEngine;
using ClickThroughFix;
using ToolbarControl_NS;

//using UnityEngine.Networking;

namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    public partial class KaptainsLog : MonoBehaviour
    {
        public static KaptainsLog Instance;
        static bool dataLoaded = false;
        static public List<LogEntry> kaptainsLogList = new List<LogEntry>();
        static Dictionary<Vessel.Situations, Situation> situations = new Dictionary<Vessel.Situations, Situation>();
        static Dictionary<Vessel.ControlLevel, ControlLevel> controlLevels = new Dictionary<Vessel.ControlLevel, ControlLevel>();
        static Dictionary<Events, EventType> eventTypes = new Dictionary<Events, EventType>();
        static Dictionary<string, BodyName> bodiesList = new Dictionary<string, BodyName>();
        static internal Dictionary<string, Tag> tagList = new Dictionary<string, Tag>();
        internal string activeTag = "";
        internal bool disableTagOnRevert = false;
        internal bool disableTagOnLanding = false;
        internal bool disableTagOnRecovery = false;
        bool noTagfilter = false;
        TagEntry tagEntry = null;
        public class BodyName
        {
            public string bodyName;
            public bool selected = true;

            public BodyName(string b)
            {
                bodyName = b;
            }
        }
        // Define the controls to block.
        private const ControlTypes _blockAllControls =
            ControlTypes.ALL_SHIP_CONTROLS | ControlTypes.ACTIONS_ALL | ControlTypes.EVA_INPUT | ControlTypes.TIMEWARP |
            ControlTypes.MISC | ControlTypes.GROUPS_ALL | ControlTypes.CUSTOM_ACTION_GROUPS;


        ToolbarControl toolbarControl;

        internal const string MODID = "KaptainsLog_NS";
        internal const string MODNAME = "Kaptain's Log";

        // The tooltip text for the toolbar icon if the plugin is off.
        private const string _tooltipOff = "Show Kaptains Log";

        // The tooltip text for the toolbar icon if the plugin is on.
        private const string _tooltipOn = "Hide Kaptains Log";


        private const string BlizzyToolbarIconActive = "KaptainsLog/Icons/kaptainslog_active_toolbar_24_icon";
        private const string BlizzyToolbarIconInActive = "KaptainsLog/Icons/kaptainslog_inactive_toolbar_24_icon";
        private const string StockToolbarIconActive = "KaptainsLog/Icons/kaptainslog_active_toolbar_38_icon";
        private const string StockToolbarIconInactive = "KaptainsLog/Icons/kaptainslog_inactive_toolbar_38_icon";
        private const string PencilIcon = "KaptainsLog/Icons/pencil";
        public const string screenshotPrefix = "KL_";





        // true lock input, false to unlock.
        //private bool _toggleInput;

        bool visibleByToolbar = false;
        bool notesEntry = false;
        bool manualEntry = false;
        bool imageSelection = false;
        bool displayFilterWindow = false;
        bool displaySortWindow = false;
        bool displayColSelectWindow = false;
        bool displayExportWindow = false;
        bool displayQuickHTMLTemplate = false;
        bool displayHTMLTemplate = false;
        public bool displayScreenshot = false;
        ImageViewer iv = null;
        //string imageToDisplay;
        LogEntry leToDisplay;
        bool newLeToDisplay = false;

        bool filterPosInitted = false;
        Vector2 filterSelOffset;

        bool colselPosInitted = false;
        Vector2 colselSelOffset;

        bool entryFieldPosInitted = false;
        Vector2 entryFieldOffset;

        public enum PauseStatus { none, requested, complete };
        public PauseStatus pauseActivated = PauseStatus.none;
        public double lastPauseTime = 0;
        public double lastNoteTime = 0;

        Rect mainWindow;
        public Rect logEntryWindow;
        Rect filterSelectionWindow;
        Rect colSelectWindow;
        Rect saveWindow;
        Rect imageSelectionWindow;
        Rect sortSelectionWindow;
        Rect entryFieldWindow;
        Rect htmlTemplateSelectWindow;


        int mainWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int logentryWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int filterselWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int colSelWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int saveWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int imgSelWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int entryFieldWindowId = GUIUtility.GetControlID(FocusType.Passive);
        int htmlTemplateWindowId = GUIUtility.GetControlID(FocusType.Passive);

        const int MAIN_WIDTH = 1000;
        const int MAIN_HEIGHT = 600;

        const int LOGENTRY_WIDTH = 400;
        const int LOGENTRY_HEIGHT = 300;

        const int FILTERSEL_WIDTH = 700;
        const int FILTERSEL_HEIGHT = 300;

        const int COLSEL_WIDTH = 1000;
        const int COLSEL_HEIGHT = 200;

        const int ENTRY_FIELD_WIDTH = 300;
        const int ENTRY_FIELD_HEIGHT = 400;

        const int IMAGE_SEL_WIDTH = 300;
        const int IMAGE_SEL_HEIGHT = 600;

        const int SAVE_WIDTH = 400;
        const int SAVE_HEIGHT = 200;

        const int HTML_TEMPLATE_SEL_WIDTH = 300;
        const int HTML_TEMPLATE_SEL_HEIGHT = 600;


        public string notesText = "";
        Vector2 notesScrollVector;
        Vector2 displayScrollVector;
        Vector2 windowResizeScrollVector;
        Vector2 colSelScrollVector;
        Vector2 entryFieldScrollVector;
        Vector2 imageFileSelScrollVector;
        Vector2 htmlFileSelScrollVector;

        GameObject aGameObject = new GameObject();


        public TacAtomicClockSettings settings = new TacAtomicClockSettings();
        public static Utils utils = null;

        public bool logEntryComplete = false;
        public bool notesEntryComplete = false;

        public string logFileName = "KaptainsLog.cfg";

        public static string ROOT_PATH = KSPUtil.ApplicationRootPath;
        private static string GAMEDATA_FOLDER = ROOT_PATH + "GameData/";
        public static String MOD_FOLDER = GAMEDATA_FOLDER + "KaptainsLog/";
        public string SAVE_PATH = ROOT_PATH + "saves/" + HighLogic.SaveFolder;
        string PLUGINDATA = MOD_FOLDER + "PluginData/KaptainsLogSettings.cfg";

        //bool blizzyButtonActive;

        const string QuestionMarkIcon = "KaptainsLog/Icons/questionMark";

        public string NODENAME = "KaptainsLog";
        // string SETTINGSNAME = "KaptainsLogSettings";
        const string WINDOWPOS = "WindowPositions";
        public string CREWNODE = "CrewMember";

        float[] colWidth = new float[(int)Fields.lastItem] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        const int SLIDER_WIDTH = 350;
        const int LABEL_WIDTH = 200;
        float minAlt = 0;
        float maxAlt = 0;
        float misTimStart = 0;
        float misTimEnd = 0;
        float spdLow = 0;
        float spdHigh = 0;
        float uniTimStart = 0;
        float uniTimEnd = 0;

        double highestAltitude = 0;
        double largestTime = 0;
        double highestSpeed = 0;
        double largestUniTime = 0;

        string screenshotPath = ROOT_PATH + "Screenshots/";
        string htmlTemplatePath = MOD_FOLDER + "PluginData/HTMLTemplates/";
        string quickHtmlTemplatePath = MOD_FOLDER + "PluginData/QuickHTMLTemplates/";
        string[] fileEntries;
        string[] dirEntries;
        Fields entryField = Fields.none;

        int lastSelectedImage = 0;
        GUIStyle labelStyle = new GUIStyle(HighLogic.Skin.label);

        public class vesselId
        {
            public string vesselName;
            public string id;
            public bool selected = true;

            public vesselId(string n, string i)
            {
                vesselName = n;
                id = i;
            }
        }
        Dictionary<string, vesselId> allVessels = new Dictionary<string, vesselId>();

        public class Situation
        {
            public Vessel.Situations situation;
            public bool selected = true;

            public Situation(Vessel.Situations s)
            {
                situation = s;
            }
        }

        public class ControlLevel
        {
            public Vessel.ControlLevel controlLevel;
            public bool selected = true;

            public ControlLevel(Vessel.ControlLevel vcl)
            {
                controlLevel = vcl;
            }
        }


        public class EventType
        {
            public Events evnt;
            public bool selected = true;

            public EventType(Events evt)
            {
                evnt = evt;
            }
        }

        public class Body
        {
            public CelestialBody body;
            public bool selected = true;

            public Body(CelestialBody b)
            {
                body = b;
            }
        }

        public class Tag
        {
            public string tag;
            public bool selected = true;

            public Tag(string t)
            {
                tag = t;
            }
        }
        public static GUIStyle windowStyle;
        Texture2D greenButtonTexture;

        public void InitOnLoad()
        {
            Log.Info("InitOnLoad");
            pauseActivated = PauseStatus.none;
            lastPauseTime = 0;
            lastNoteTime = 0;
            screenshotAfter = 0;
            visibleByToolbar = false;
            notesEntry = false;
            manualEntry = false;
            imageSelection = false;
            //escapePressed = false;
            displayFilterWindow = false;
            displayColSelectWindow = false;
            displayExportWindow = false;
            displayHTMLTemplate = false;
            notesText = "";
            KLScenario.dirtyColSel = true;
            KLScenario.dirtyFilter = true;
            logEntryComplete = false;
            notesEntryComplete = false;
            kaptainsLogList.Clear();
            KaptainsLog.tagList.Clear();
            utils.snapshotInProgress = Utils.SnapshotProgress.none;

            screenshotAfter = 0;

            utils.snapshotTaken = 0;

            //pms = PauseMenuState.hidden;
            greenButtonTexture = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            greenButtonTexture.SetPixel(0, 0, Color.green);
            greenButtonTexture.SetPixel(0, 1, Color.green);
            greenButtonTexture.SetPixel(1, 0, Color.green);
            greenButtonTexture.SetPixel(1, 1, Color.green);
            greenButtonTexture.Apply();
            InitDisplayFields();
        }

        private void Awake()
        {
            Log.Warning("Awake");
            Instance = this;
        }
        List<DisplayField> displayFields;

        void onGameStateCreated(Game g)
        {
            Log.Info("onGameStateCreated");
            utils.LoadLogs();
            KLScenario.logsLoaded = true;
            // InitDisplayFields();

            visibleByToolbar = false;
            //showStdPauseMenu = false;
            //pauseCnt = 0;
            //pms = PauseMenuState.hidden;
            manualEntry = false;
            utils.le = null;
            logEntryComplete = false;
            utils.snapshotInProgress = Utils.SnapshotProgress.none;
            pauseActivated = PauseStatus.none;
            utils.leQ.Clear();
        }

        void InitDisplayFields()
        {
            Log.Info("InitDisplayFields");
            if (displayFields != null)
                displayFields.Clear();
            else
                displayFields = new List<DisplayField>();

            KL_12 kl4 = HighLogic.CurrentGame.Parameters.CustomParams<KL_12>();

            displayFields.Add(new DisplayField(false, Fields.index));
            displayFields.Add(new DisplayField(false, Fields.vesselId));
            displayFields.Add(new DisplayField(kl4.vesselName, Fields.vesselName));
            displayFields.Add(new DisplayField(kl4.universalTime, Fields.universalTime));
            displayFields.Add(new DisplayField(kl4.utcTime, Fields.utcTime));
            displayFields.Add(new DisplayField(kl4.missionTime, Fields.missionTime));
            displayFields.Add(new DisplayField(kl4.vesselSituation, Fields.vesselSituation));
            displayFields.Add(new DisplayField(kl4.mainBody, Fields.mainBody));
            displayFields.Add(new DisplayField(kl4.controlLevel, Fields.controlLevel));
            displayFields.Add(new DisplayField(kl4.altitude, Fields.altitude));
            displayFields.Add(new DisplayField(kl4.speed, Fields.speed));
            displayFields.Add(new DisplayField(kl4.eventType, Fields.eventType));
            displayFields.Add(new DisplayField(kl4.thumbnail, Fields.thumbnail));
            displayFields.Add(new DisplayField(kl4.notes, Fields.notes));
            displayFields.Add(new DisplayField(kl4.tag, Fields.tag));
        }


        internal void OnGameSettingsApplied()
        {
            GlobalSettings.SaveGlobalSettings();

        }


        private void Start()
        {
            Log.Warning("Start");
            DontDestroyOnLoad(this);

            if (!HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().EnabledForSave)
                return;

            ShowStockToolbarButton();



            mainWindow = new Rect((Screen.width - MAIN_WIDTH - 40), 50, MAIN_WIDTH, MAIN_HEIGHT);
            logEntryWindow = new Rect((Screen.width - LOGENTRY_WIDTH) / 2, (Screen.height - LOGENTRY_HEIGHT) / 2, LOGENTRY_WIDTH, LOGENTRY_HEIGHT);
            filterSelectionWindow = new Rect((Screen.width - FILTERSEL_WIDTH) / 2, (Screen.height - FILTERSEL_HEIGHT) / 2, FILTERSEL_WIDTH, FILTERSEL_HEIGHT);
            colSelectWindow = new Rect((Screen.width - COLSEL_WIDTH) / 2, (Screen.height - COLSEL_HEIGHT) / 2, COLSEL_WIDTH, COLSEL_HEIGHT);
            entryFieldWindow = new Rect((Screen.width - ENTRY_FIELD_WIDTH) / 2, (Screen.height - ENTRY_FIELD_HEIGHT) / 2, ENTRY_FIELD_WIDTH, ENTRY_FIELD_HEIGHT);
            imageSelectionWindow = new Rect((Screen.width - IMAGE_SEL_WIDTH) / 2, (Screen.height - IMAGE_SEL_HEIGHT) / 2, IMAGE_SEL_WIDTH, IMAGE_SEL_HEIGHT);
            sortSelectionWindow = new Rect((Screen.width - ENTRY_FIELD_WIDTH) / 2, (Screen.height - ENTRY_FIELD_HEIGHT) / 2, ENTRY_FIELD_WIDTH, ENTRY_FIELD_HEIGHT);
            saveWindow = new Rect((Screen.width - SAVE_WIDTH) / 2, (Screen.height - SAVE_HEIGHT) / 2, SAVE_WIDTH, SAVE_HEIGHT);
            htmlTemplateSelectWindow = new Rect((Screen.width - HTML_TEMPLATE_SEL_WIDTH) / 2, (Screen.height - HTML_TEMPLATE_SEL_HEIGHT) / 2, HTML_TEMPLATE_SEL_WIDTH, HTML_TEMPLATE_SEL_HEIGHT);


            GlobalSettings.LoadSettings();
            LoadWindowPositions();


            // Add hooks for showing/hiding on F2
            GameEvents.onShowUI.Add(showUI);
            GameEvents.onHideUI.Add(hideUI);
            GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
            GameEvents.onGameStateCreated.Add(onGameStateCreated);

            utils = new Utils(this);
            utils.initializeEvents();


            // The following are loaded into statics, so they only need to be loaded one time
            if (!dataLoaded)
            {
                dataLoaded = true;
                foreach (var s in Enum.GetValues(typeof(Vessel.Situations)))
                    situations.Add((Vessel.Situations)s, new Situation((Vessel.Situations)s));

                foreach (var vcl in Enum.GetValues(typeof(Vessel.ControlLevel)))
                    controlLevels.Add((Vessel.ControlLevel)vcl, new ControlLevel((Vessel.ControlLevel)vcl));

                foreach (var evt in Enum.GetValues(typeof(Events)))
                    eventTypes.Add((Events)evt, new EventType((Events)evt));

                foreach (CelestialBody body in GameObject.FindObjectsOfType(typeof(CelestialBody)))
                    bodiesList.Add(body.name, new BodyName(body.name));

                windowStyle = new GUIStyle(HighLogic.Skin.window);
                windowStyle.active.background = windowStyle.normal.background;

                Texture2D tex = windowStyle.normal.background;
                var pixels = tex.GetPixels32();

                for (int i = 0; i < pixels.Length; ++i)
                    pixels[i].a = 255;

                tex.SetPixels32(pixels); tex.Apply();

                windowStyle.active.background =
                windowStyle.focused.background =
                windowStyle.normal.background = tex;


                utils.LoadLogs();
            }
        }

        bool visibleUI = true;
        internal void showUI() // triggered on F2
        {
            visibleUI = true;
        }

        internal void hideUI() // triggered on F2
        {
            visibleUI = false;
        }

        void onManualEntry()
        {
            Log.Info("onManualEntry");
            if (utils != null)
                utils.CreateLogEntry(Events.ManualEntry, true);
        }

        public void OnDestroy()
        {
            Log.Info("OnDestroy");

            if (toolbarControl == null)
            {
                toolbarControl.OnDestroy();
                Destroy(toolbarControl);
            }
            if (utils != null)
                utils.initializeEvents(false);

        }

        private void ShowStockToolbarButton()
        {
            Log.Info("OnGUIAppLauncherReady");
            // Setup PW Stock Toolbar button
            // bool hidden = false;

            if (toolbarControl == null)
            {

                toolbarControl = gameObject.AddComponent<ToolbarControl>();
                toolbarControl.AddToAllToolbars(ToggleToolbarButton, ToggleToolbarButton,
                     ApplicationLauncher.AppScenes.SPACECENTER |
                     ApplicationLauncher.AppScenes.FLIGHT |
                     ApplicationLauncher.AppScenes.MAPVIEW |
                     ApplicationLauncher.AppScenes.TRACKSTATION |
                     ApplicationLauncher.AppScenes.VAB |
                     ApplicationLauncher.AppScenes.SPH,
                    MODID,
                    "kaptainsLogButton",
                    "KaptainsLog/Icons/kaptainslog_active_toolbar_38_icon",
                    "KaptainsLog/Icons/kaptainslog_inactive_toolbar_38_icon",
                    "KaptainsLog/Icons/kaptainslog_active_toolbar_24_icon",
                    "KaptainsLog/Icons/kaptainslog_inactive_toolbar_24_icon",
                    MODNAME
                );

            }
        }

        // Toggles plugin visibility.
        private void ToggleToolbarButton()
        {
            visibleByToolbar = !visibleByToolbar;
            if (!visibleByToolbar)
            {
                FreeImgCache();
                SaveWindowPositions();

                visibleByToolbar = false;
                notesEntry = false;
                manualEntry = false;
                imageSelection = false;
                displayFilterWindow = false;
                displaySortWindow = false;
                displayColSelectWindow = false;
                displayExportWindow = false;
                displayQuickHTMLTemplate = false;
                displayHTMLTemplate = false;
                displayScreenshot = false;
                entryField = Fields.none;
                // ScreenMessagesLog.Instance.ShowWin(false);
                if (TagEntry.ready)
                {
                    TagEntry.ready = false;
                    UnityEngine.Object.Destroy(tagEntry);
                    tagEntry = null;
                }

            }

            toolbarControl.SetFalse(false);
            if (visibleByToolbar)
            {
                toolbarControl.SetTexture(StockToolbarIconActive, BlizzyToolbarIconActive);
            }
            else
            {
                toolbarControl.SetTexture(StockToolbarIconInactive, BlizzyToolbarIconInActive);

            }
        }

        // bool escapePressed = false;
        bool cancelManualEntry = false;

        public void OnGUI()
        {
            if (!visibleUI)
                return;
            GUI.color = Color.grey;
            //  GUI.skin = HighLogic.Skin;
            //Event e = Event.current;

            if (confirmDeletePopup != null)
                return;

            if (visibleByToolbar)
            {
                if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().keepOnScreen)
                {
                    int buttonWidth = 40;
                    if (HighLogic.LoadedScene == GameScenes.SPACECENTER)
                        buttonWidth = 0;
                    if (mainWindow.x + mainWindow.width > Screen.width - buttonWidth)
                        mainWindow.x = Screen.width - mainWindow.width - buttonWidth;
                    if (mainWindow.x < 0)
                        mainWindow.x = 0;
                    if (mainWindow.y < 0)
                        mainWindow.y = 0;
                    if (mainWindow.y + mainWindow.height > Screen.height)
                        mainWindow.y = Screen.height - mainWindow.height;
                }
                mainWindow = ClickThruBlocker.GUILayoutWindow(mainWindowId, mainWindow, DisplayMainWindow, "Kaptain's Log Display", windowStyle);
            }

            if (notesEntry && !utils.recoveryRequested)
            {
                logEntryWindow = ClickThruBlocker.GUILayoutWindow(logentryWindowId, logEntryWindow, DisplayLogEntryWindow, "Kaptain's Log Entry", windowStyle);
                return;
            }
            if (imageSelection)
            {
                imageSelectionWindow = ClickThruBlocker.GUILayoutWindow(imgSelWindowId, imageSelectionWindow, DisplayImageSelectionWindow, "Kaptain's Log Image Selection", windowStyle);
            }
            if (displaySortWindow)
            {
                sortSelectionWindow = ClickThruBlocker.GUILayoutWindow(imgSelWindowId, sortSelectionWindow, DisplaySortSelWindow, "Kaptain's Log Sort Selection", windowStyle);
            }
            if (displayFilterWindow)
            {
                genericWindow(ref filterPosInitted, ref filterselWindowId, ref filterSelectionWindow, DisplayFilterSelWindow, "Kaptain's Log Filter Selection", mainWindow, ref filterSelOffset, windowStyle);
            }
            if (displayColSelectWindow)
            {
                genericWindow(ref colselPosInitted, ref colSelWindowId, ref colSelectWindow, DisplayColSelWindow, "Kaptain's Log Display Field Selection", mainWindow, ref colselSelOffset, windowStyle);
            }
            if (displayExportWindow)
            {
                saveWindow = ClickThruBlocker.GUILayoutWindow(saveWindowId, saveWindow, DisplaySaveWindow, "Kaptain's Log Export/Save", windowStyle);
            }
            if (entryField != Fields.none)
            {
                genericWindow(ref entryFieldPosInitted, ref entryFieldWindowId, ref entryFieldWindow, DisplayEntryFieldWindow, "Kaptain's Log " +
                    LogEntry.displayFieldName(entryField)
                     + " Filter Selection", filterSelectionWindow, ref entryFieldOffset, windowStyle);
            }
            if (displayHTMLTemplate || displayQuickHTMLTemplate)
            {
                htmlTemplateSelectWindow = ClickThruBlocker.GUILayoutWindow(saveWindowId, htmlTemplateSelectWindow, DisplayHtmlTemplateSelectionWindow, "Kaptain's Log HTML Template Selection", windowStyle);
            }
            if (TagEntry.ready)
            {
                if (tagEntry.completed)
                {
                    if (TagEntry.newtag != null)
                    {
                        activeTag = TagEntry.newtag;
                        disableTagOnRevert = TagEntry.disableTagOnRevert;
                        disableTagOnLanding = TagEntry.disableTagOnLanding;
                        disableTagOnRecovery = TagEntry.disableTagOnRecovery;
                        if (!tagList.ContainsKey(activeTag))
                        {
                            Tag t = new Tag(activeTag);
                            tagList.Add(activeTag, t);
                        }
                    }
                    TagEntry.ready = false;
                    UnityEngine.Object.Destroy(tagEntry);
                    tagEntry = null;
                }
            }
            if (displayScreenshot)
            {
                if (iv == null)
                    iv = new ImageViewer(utils.getDisplayString(leToDisplay, Fields.screenshot));
                else
                {
                    if (!ImageViewer.IsVisible)
                    {
                        ImageViewer.LoadImage(utils.getDisplayString(leToDisplay, Fields.screenshot));
                        GUI.BringWindowToFront(iv.winId);

                    }
                    else
                    {
                        if (newLeToDisplay)
                        {
                            ImageViewer.LoadImage(utils.getDisplayString(leToDisplay, Fields.screenshot));
                            GUI.BringWindowToFront(iv.winId);
                        }
                    }

                }
                newLeToDisplay = false;
                iv.OnGUI();
            }
        }

        void genericWindow(ref bool initted, ref int winId, ref Rect displayWin, GUI.WindowFunction func, string title, Rect parentWin, ref Vector2 offset, GUIStyle window)
        {
            if (!initted)
            {
                initted = true;
                displayWin.x = parentWin.x - displayWin.width;
                displayWin.y = parentWin.y;
            }
            else
            {
                // displayWin.x = parentWin.x - offset.x;
                // displayWin.y = parentWin.y - offset.y;
            }
            displayWin.x = Math.Max(0, displayWin.x);
            displayWin.y = Math.Max(0, displayWin.y);
            displayWin = ClickThruBlocker.GUILayoutWindow(winId, displayWin, func, title, window);
            offset.x = parentWin.x - displayWin.x;
            offset.y = parentWin.y - displayWin.y;
        }

        public class DisplayField
        {
            public bool visible;
            public Fields f;
            public bool filter = false;

            public DisplayField(bool v, Fields f1)
            {
                visible = v;
                f = f1;
            }
        }



        Texture2D MakeThumbnailFrom(string origImageFile, int thumbnailsize)
        {
            Log.Info("Converting screenshot to thumbnail New name: " + origImageFile);


            byte[] fileData = System.IO.File.ReadAllBytes(origImageFile);
            Texture2D screenshot = new Texture2D(2, 2);
            screenshot.LoadImage(fileData); //..this will auto-resize the texture dimensions.

            float h = (float)screenshot.height / (float)screenshot.width * thumbnailsize;

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().useBilinear)
                TextureScale.Bilinear(screenshot, (int)thumbnailsize, (int)h);
            else
                TextureScale.Point(screenshot, (int)thumbnailsize, (int)h);

            return screenshot;
        }



        void getDataColLimits(List<DisplayField> displayFields, bool allLogs = true)
        {
            // Get size of display fields
            Array.Clear(colWidth, 0, (int)Fields.lastItem);
            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none)
                {
                    string f = LogEntry.displayFieldName(displayFields[d].f);
                    colWidth[d] = Math.Max(colWidth[d], GUI.skin.label.CalcSize(new GUIContent(f)).x);
                }
            }
            if (!allLogs)
                return;

            // Initialiaze all values here
            KLScenario.dirtyColSel = false;
            highestAltitude = 0;
            largestTime = 0;
            highestSpeed = 0;
            largestUniTime = 0;
            allVessels.Clear();


            // Get size and other data from list of log entries.
            for (int i = 0; i < kaptainsLogList.Count; i++)
            {
                var le1 = kaptainsLogList[i];

                highestAltitude = Math.Min(Math.Max(highestAltitude, le1.altitude + 1), HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().altitudeFilterMax);
                largestTime = Math.Max(largestTime, le1.missionTime); // allow filter to be 2 hours later than newest mission time.
                highestSpeed = Math.Min(Math.Max(highestSpeed, le1.speed + 1), HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().speedFilterMax);
                largestUniTime = Math.Max(largestUniTime, le1.universalTime + 1);

                if (!allVessels.ContainsKey(le1.vesselId))
                {
                    vesselId vid = new vesselId(le1.vesselName, le1.vesselId);
                    allVessels.Add(le1.vesselId, vid);
                }
                for (int d = 0; d < displayFields.Count; d++)
                {
                    if (displayFields[d].f != Fields.none)
                    {
                        string f = utils.getDisplayString(le1, displayFields[d].f);
                        if (displayFields[d].f != Fields.thumbnail)
                        {
                            colWidth[d] = Math.Max(colWidth[d], GUI.skin.textField.CalcSize(new GUIContent(f)).x);
                        }
                        else
                        {
                            if (System.IO.File.Exists(f))
                            {
                                var _image = new Texture2D(2, 2);
                                ToolbarControl.LoadImageFromFile(ref _image, f);

                                colWidth[d] = Math.Max(colWidth[d], _image.width);
                            }
                        }
                    }
                }
            }

            maxAlt = (float)highestAltitude;
            misTimEnd = (float)Planetarium.GetUniversalTime() + 3600 * 2;
            spdHigh = (float)highestSpeed;
            uniTimEnd = (float)largestUniTime;

        }



        void ApplyFilters(LogEntry le)
        {
            le.leSelected = false;
            for (int d = 0; d < displayFields.Count; d++)
            {
                if (displayFields[d].f != Fields.none && displayFields[d].f != Fields.universalTime && displayFields[d].visible && displayFields[d].filter)
                {
                    var f = displayFields[d].f;

                    switch (f)
                    {
                        case Fields.vesselName:
                            if (allVessels.ContainsKey(le.vesselId) && !allVessels[le.vesselId].selected)
                            {
                                Log.Info("filtered vesselName");
                                return;
                            }
                            break;

                        case Fields.vesselSituation:
                            if (situations.ContainsKey(le.vesselSituation) && !situations[le.vesselSituation].selected)
                            {
                                Log.Info("filtered vesselSituation");
                                return;
                            }
                            break;
                        case Fields.controlLevel:
                            if (controlLevels.ContainsKey(le.controlLevel) && !controlLevels[le.controlLevel].selected)
                            {
                                Log.Info("filtered controlLevel");
                                return;
                            }
                            break;
                        case Fields.mainBody:
                            if (bodiesList.ContainsKey(le.vesselMainBody) && !bodiesList[le.vesselMainBody].selected)
                            {
                                Log.Info("filtered mainBody");
                                return;
                            }
                            break;
                        case Fields.eventType:
                            if (eventTypes.ContainsKey(le.eventType) && !eventTypes[le.eventType].selected)
                            {
                                Log.Info("filtered eventType");
                                return;
                            }
                            break;
                        case Fields.altitude:
                            if (le.altitude < minAlt || le.altitude > maxAlt)
                            {
                                Log.Info("filtered altitude");
                                return;
                            }
                            break;
                        case Fields.missionTime:
                            if (le.missionTime < misTimStart || le.missionTime > misTimEnd)
                            {
                                Log.Info("filtered missionTime");
                                return;
                            }
                            break;
                        case Fields.speed:
                            if (le.speed < spdLow || le.speed > spdHigh)
                            {
                                Log.Info("filtered speed");
                                return;
                            }
                            break;
                        case Fields.universalTime:
                            if (le.universalTime < uniTimStart || le.universalTime > uniTimEnd)
                            {
                                Log.Info("filtered universalTime");
                                return;
                            }
                            break;
                        case Fields.tag:
                            if (tagList.ContainsKey(le.tag) && !tagList[le.tag].selected)
                            {
                                Log.Info("filtered tag");
                                return;
                            }
                            if (!noTagfilter)
                                return;
                            break;
                    }

                }
            }
            le.leSelected = true;
            return;
        }





        public double screenshotAfter;
        bool guiHidden4Screenshot = false;


        // The following is done in the following:
        //
        // 1.  If manual entry cancelled, clear out queue of current manual entries
        // 1.1  Now check to see if anything is queued. if not, return
        // 2.  If snapshot is queued, check to see if it's time, if so, take the snapshow
        // 2.1 If a screenshot is scheduled with a manual entry (ie:  user selected Log Entry button), do 
        //     the screenshot immediately.
        // 3.  If a screenshot is in progress, check to see if it's done.  If it is done, do any requested conversions

        // 4.  If no screenshot is in progress and no screenshot is scheduled, and a manual entry (any kind) is requested, start the
        //     manual entry process

        // 5.  If no screenshot is in progress, no screenshot is scheduled, no manual entry in progress, complete the log entry
        //

        void checkForManualEntryCancel()
        {
            //
            // If manual entry cancelled, clear out the queue here
            //
            if (cancelManualEntry)
            {
                while (utils.le.eventType == Events.ManualEntry)
                {
                    utils.le = utils.leQ.Dequeue();
                    if (utils.leQ.Count == 0)
                        break;
                }
                if (utils.leQ.Count == 0)
                    cancelManualEntry = false;
            }
        }
        bool ProcessScreenshotImages()
        {
            utils.le.guiHidden = guiHidden4Screenshot;

            try
            {
                Texture2D screenshot = MakeThumbnailFrom(utils.le.pngName, HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().thumbnailSize);

                byte[] bytes = screenshot.EncodeToPNG();
                if (utils.le.pngThumbnailName != "")
                {
                    System.IO.File.WriteAllBytes(utils.le.pngThumbnailName, bytes);
                    utils.ConvertToJPG(utils.le.pngThumbnailName, utils.le.jpgThumbnailName);
                    System.IO.File.Delete(utils.le.pngThumbnailName);
                }
                Log.Info("logEntryComplete = true; 1");
                logEntryComplete = true;
                utils.le.pngThumbnailName = "";
                Destroy(screenshot);
            }
            catch
            {
                Log.Error("MakeThumbnailFrom generated exception");
                return false;
            }

            if (utils.wasUIVisible && guiHidden4Screenshot)
            {
                //&& HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().hideUIforScreenshot)
                GameEvents.onShowUI.Fire();
                guiHidden4Screenshot = false;
            }

            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().saveAsJPEG)
            {
                Log.Info("pngToConvert: " + utils.le.pngName);
                if (System.IO.File.Exists(utils.le.pngName))
                {
                    Log.Info("Converting screenshot to JPG. New name: " + utils.le.jpgName);
                    utils.ConvertToJPG(utils.le.pngName, utils.le.jpgName);

                    if (!HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().keepPNG)
                    {
                        System.IO.FileInfo file = new System.IO.FileInfo(utils.le.pngName);
                        Log.Info("Delete PNG file");
                        file.Delete();
                        utils.le.pngName = "";
                    }

                    utils.snapshotInProgress = Utils.SnapshotProgress.complete;
                }
            }
            return true;
        }


        public void LateUpdate()
        {
            //           if (FlightGlobals.ActiveVessel.LandedOrSplashed)
            //               utils.onVesselLanded(FlightGlobals.ActiveVessel);

            if (utils == null || utils.le == null || utils.leQ == null)
                return;

            //
            // Check for manual entry cancel
            //
            if (utils.leQ.Count > 0)
                Log.Info("LateUpdate, utils.leQ.Count: " + utils.leQ.Count.ToString() +
                    ", snapshotInProgress: " + utils.snapshotInProgress.ToString() +
                    ", utils.le.manualEntryRequired: " + utils.le.manualEntryRequired.ToString() +
                    ", notesEntry: " + notesEntry.ToString() + ", pauseActivated: " + pauseActivated.ToString());
            checkForManualEntryCancel();

            // Now check to see if anything is queued
            if (utils.leQ.Count == 0)
                return;

            //
            // if a screenshot is in progress, wait until it is done
            //

            if (utils.snapshotInProgress == Utils.SnapshotProgress.queued)
            {
                if (utils.le.eventScreenshot != ScreenshotOptions.No_Screenshot)
                {
                    if (utils.snapshotInProgress == Utils.SnapshotProgress.queued && (Planetarium.GetUniversalTime() >= screenshotAfter ||
                        (utils.le.eventType == Events.ManualEntry || utils.le.eventType == Events.Landed)))
                    {
                        if (utils.le.pngName != "")
                        {
                            if (utils.le.eventType == Events.ManualEntry)
                            {
                                utils.snapshotTaken = 0;
                                ScreenCapture.CaptureScreenshot(utils.le.pngName);
                                utils.snapshotInProgress = Utils.SnapshotProgress.inProgress;
                            }
                            if (utils.snapshotTaken > 0)
                            {

                                if (utils.le.eventScreenshot == ScreenshotOptions.Without_Gui)
                                {
                                    GameEvents.onHideUI.Fire();
                                    guiHidden4Screenshot = true;
                                    Log.Info("Hiding GUI");
                                }
                                //else
                                //    guiHidden4Screenshot = false;
                                utils.snapshotTaken--;
                                if (utils.snapshotTaken == 0)
                                {
                                    ScreenCapture.CaptureScreenshot(utils.le.pngName);
                                    utils.snapshotInProgress = Utils.SnapshotProgress.inProgress;
                                }
                                else
                                    return;
                            }
                        }
                        else
                            Log.Error("Snapshot scheduled, but no pngName set in queued event");
                    }
                }
                else
                {
                    Log.Error("Snapshot scheduled, but queued event: " + utils.le.displayEventString() + " doesn't allow snapshots");
                    // need to dequeue this one

                    ListExt.AddSorted<LogEntry>(kaptainsLogList, utils.le);
                    utils.leQ.Dequeue();
                    return;
                }
            }

            //
            // If snapshow is in progress, check to see if it's done
            //
            if (utils.snapshotInProgress == Utils.SnapshotProgress.inProgress)
            {
                if (System.IO.File.Exists(utils.le.pngName))
                {
                    if (ProcessScreenshotImages())
                        utils.snapshotInProgress = Utils.SnapshotProgress.complete;
                }
            }

            //
            // If no snapshot is scheduled or queued and a manual entry is requested, start that
            //
            if (pauseActivated == PauseStatus.none &&
                (utils.snapshotInProgress == Utils.SnapshotProgress.none || utils.snapshotInProgress == Utils.SnapshotProgress.complete) &&
                utils.le.manualEntryRequired)
            {
                Log.Info("Activating manual entry");
                pauseActivated = PauseStatus.requested;
                notesEntry = true;
                FlightDriver.SetPause(true);
                return;
            }

            //
            // If no screenshot is in progress, no screenshot is scheduled, no manual entry in progress, complete the log entry
            //
            if ((utils.snapshotInProgress == Utils.SnapshotProgress.none || utils.snapshotInProgress == Utils.SnapshotProgress.complete) &&
                (pauseActivated == PauseStatus.none || pauseActivated == PauseStatus.complete)
                )
            {
                if (utils.leQ.Count > 0)
                {
                    // add to list here

                    Log.Info("Adding log entry to list, queue count: " + utils.leQ.Count.ToString());

                    ListExt.AddSorted<LogEntry>(kaptainsLogList, utils.le);

                    utils.leQ.Dequeue();
                    KLScenario.dirtyFilter = true;

                    //
                    // If anything else queued, use the same screenshot (if any)
                    //
                    string screenshotName = utils.le.screenshotName;
                    string pngThumbnailName = utils.le.pngThumbnailName;
                    string jpgThumbnailName = utils.le.jpgThumbnailName;
                    while (utils.leQ.Count > 0)
                    {
                        utils.le = utils.leQ.Peek();
                        if (utils.le.eventScreenshot != ScreenshotOptions.No_Screenshot && screenshotName == "")
                        {
                            utils.wasUIVisible = utils.uiVisible;
                            utils.snapshotInProgress = Utils.SnapshotProgress.queued;
                            notesEntryComplete = false;

                            screenshotAfter = Planetarium.GetUniversalTime() + HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().delayBeforePause;
                            utils.snapshotTaken = 1;
                            break;
                        }
                        utils.le = utils.leQ.Dequeue();
                        utils.le.guiHidden = guiHidden4Screenshot;
                        utils.le.screenshotName = screenshotName;
                        utils.le.pngThumbnailName = pngThumbnailName;
                        utils.le.jpgThumbnailName = jpgThumbnailName;
                        ListExt.AddSorted<LogEntry>(kaptainsLogList, utils.le);
                    }
                    if (utils.leQ.Count == 0)
                        cancelManualEntry = false;

                    logEntryComplete = false;
                    notesEntryComplete = false;
                    utils.SaveLogs();
                    if (visibleByToolbar)
                        UpdateImageCache();
                }
                else
                {
                    Log.Error("Completing log entry, but no log entry queued");
                }

                logEntryComplete = false;
                notesEntryComplete = false;
                pauseActivated = PauseStatus.none;
                utils.snapshotInProgress = Utils.SnapshotProgress.none;
            }
        }

        void SaveWinPos(ConfigNode settings, string winName, Rect win)
        {
            settings.SetValue(winName + "X", win.x.ToString(), true);
            settings.SetValue(winName + "Y", win.y.ToString(), true);
        }
        void SaveWindowPositions()
        {
            ConfigNode settings = new ConfigNode();

            SaveWinPos(settings, "mainWindow", mainWindow);
            SaveWinPos(settings, "logEntryWindow", logEntryWindow);
            SaveWinPos(settings, "filterSelectionWindow", filterSelectionWindow);
            SaveWinPos(settings, "colSelectWindow", colSelectWindow);
            SaveWinPos(settings, "saveWindow", saveWindow);
            SaveWinPos(settings, "imageSelectionWindow", imageSelectionWindow);
            SaveWinPos(settings, "entryFieldWindow", entryFieldWindow);
            SaveWinPos(settings, "htmlTemplateSelectWindow", htmlTemplateSelectWindow);

            SaveWinPos(settings, "ScrnMsgsWindow", ScreenMessagesLog.Instance.ScrnMsgsWindow);

            //ScreenMessages.PostScreenMessage("Window Positions Saved");

            GlobalSettings.UpdateNode(WINDOWPOS, settings);
        }

        Rect GetWinPos(ConfigNode settings, string winName, float width, float height)
        {
            double x = (Screen.width - width) / 2;
            double y = (Screen.height - height) / 2;

            x = Double.Parse(Utils.SafeLoad(settings.GetValue(winName + "X"), x));
            y = Double.Parse(Utils.SafeLoad(settings.GetValue(winName + "Y"), y));
            Log.Info("GetWinPos, win: " + winName + ",    x,y: " + x.ToString("N0") + ", " + y.ToString("N0"));
            var r = new Rect((float)x, (float)y, width, height);
            return r;
        }

        void LoadWindowPositions()
        {
            ConfigNode settings;

            settings = GlobalSettings.FetchNode(WINDOWPOS);
            if (settings == null)
            {
                Log.Info("Unable to load window positions");
                return;
            }

            mainWindow = GetWinPos(settings, "mainWindow", MAIN_WIDTH, MAIN_HEIGHT);
            logEntryWindow = GetWinPos(settings, "logEntryWindow", LOGENTRY_WIDTH, LOGENTRY_HEIGHT);
            filterSelectionWindow = GetWinPos(settings, "filterSelectionWindow", FILTERSEL_WIDTH, FILTERSEL_HEIGHT);
            filterPosInitted = true;
            colSelectWindow = GetWinPos(settings, "colSelectWindow", COLSEL_WIDTH, COLSEL_HEIGHT);
            colselPosInitted = true;
            saveWindow = GetWinPos(settings, "saveWindow", SAVE_WIDTH, SAVE_HEIGHT);
            imageSelectionWindow = GetWinPos(settings, "imageSelectionWindow", IMAGE_SEL_WIDTH, IMAGE_SEL_HEIGHT);
            entryFieldWindow = GetWinPos(settings, "entryFieldWindow", ENTRY_FIELD_WIDTH, ENTRY_FIELD_HEIGHT);
            entryFieldPosInitted = true;
            htmlTemplateSelectWindow = GetWinPos(settings, "htmlTemplateSelectWindow", HTML_TEMPLATE_SEL_WIDTH, HTML_TEMPLATE_SEL_HEIGHT);

            if (ScreenMessagesLog.Instance != null)
                ScreenMessagesLog.Instance.ScrnMsgsWindow = GetWinPos(settings, "ScrnMsgsWindow", ENTRY_FIELD_WIDTH, ENTRY_FIELD_HEIGHT);


        }
    }
}

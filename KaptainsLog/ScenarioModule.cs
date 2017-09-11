using System;
using System.Text.RegularExpressions;

namespace KaptainsLogNamespace
{
    public class ExportSettings
    {
        public string settingName;

        public bool saveAsCSV = false;
        public bool saveAsHTML = true;
        public string htmlTemplate = "";
        public string exportHeaderTitle = "";
        public string exportHeaderImage = "";
        public string exportFooterTitle = "";
        public string exportFooterImage = "";
        public bool openInBrowser = true;

        /// <summary>
        /// Simple Constructor
        /// </summary>
        public ExportSettings()
        { }

        /// <summary>
        /// Constructor setting the name
        /// </summary>
        /// <param name="name">string</param>
        public ExportSettings(string name)
        {
            settingName = name;
        }

        /// <summary>
        /// Constructor setting values from ConfigNode passed in
        /// </summary>
        /// <param name="node">ConfigNode</param>
        public ExportSettings(ConfigNode node)
        {
            FromNode(node);
        }
        /// <summary>
        /// ToNode, converts data to a ConfigNode
        /// </summary>
        /// <returns>ConfigNode</returns>
        public ConfigNode ToNode()
        {
            ConfigNode node = new ConfigNode(settingName);

            node.AddValue("settingName", settingName);
            node.AddValue("saveAsCSV", saveAsCSV);
            node.AddValue("saveAsHTML", saveAsHTML);
            node.AddValue("htmlTemplate", htmlTemplate);
            node.AddValue("exportHeaderTitle", exportHeaderTitle);
            node.AddValue("exportHeaderImage", exportHeaderImage);
            node.AddValue("exportFooterTitle", exportFooterTitle);
            node.AddValue("exportFooterImage", exportFooterImage);
            node.AddValue("openInBrowser", openInBrowser);
            return node;
        }

       
        public void FromNode(ConfigNode node)
        { 
            settingName = Utils.SafeLoad(node.GetValue("settingName"), "");
            saveAsCSV = Boolean.Parse(Utils.SafeLoad(node.GetValue("saveAsCSV"), "false"));
            saveAsHTML = Boolean.Parse(Utils.SafeLoad(node.GetValue("saveAsHTML"), "false"));

            htmlTemplate = Utils.SafeLoad(node.GetValue("htmlTemplate"), "");
            exportHeaderTitle = Utils.SafeLoad(node.GetValue("exportHeaderTitle"), "");
            exportHeaderImage = Utils.SafeLoad(node.GetValue("exportHeaderImage"), "");
            exportFooterTitle = Utils.SafeLoad(node.GetValue("exportFooterTitle"), "");
            exportFooterImage = Utils.SafeLoad(node.GetValue("exportFooterImage"), "");

            openInBrowser = Boolean.Parse(Utils.SafeLoad(node.GetValue("openInBrowser"), "false"));
        }
    }

    [KSPScenario(ScenarioCreationOptions.AddToAllGames, new GameScenes[] {
        GameScenes.SPACECENTER,
        GameScenes.EDITOR,
        GameScenes.FLIGHT,
        GameScenes.TRACKSTATION,
        GameScenes.SPACECENTER
    })]
    public class KLScenario : ScenarioModule
    {
        [Persistent]
        int savedIdx;
        [Persistent]
        int savedScreenshotCnt;

        public static int logIdx;
        private static int screenshotCnt;

        public static KLScenario Instance; 
        static bool inited = false;
        static bool loaded = false;

        
        internal static ExportSettings exportSettings = new ExportSettings("Export");

        internal static ExportSettings quickExportSettings = new ExportSettings("QuickExport");



        //public static bool inited = false;

        //static internal List<LogEntry> kaptainsLogList = new List<LogEntry>();
        //static internal Queue<ScreenMessage> scrnMsgLog = new Queue<ScreenMessage>();
        static internal bool dirtyFilter = true;
        static internal bool dirtyColSel = true;
        static internal bool logsLoaded = false;
        static internal bool imgCacheFilled = false;

        public void DoInit()
        {
            dirtyFilter = true;
            dirtyColSel = true;
            logsLoaded = false;
            imgCacheFilled = false;
        }

        override public void OnAwake()
        {
            Log.Info("ShelterPersistent.Awake");
            //Shelter.persistent = this;
            inited = true;
            Instance = this;
            DoInit();
        }

        public static int GetNextCnt
        {
            get
            {
                screenshotCnt++;
                return screenshotCnt;
            }
        }
        void Start()
        {
            Instance = this;
        }


        public override void OnSave(ConfigNode node)
        {
            Log.Info("KLScenario.OnSave");
            

            base.OnSave(node);
            node.AddNode(exportSettings.ToNode());
            node.AddNode(quickExportSettings.ToNode());

            savedIdx = logIdx;
            savedScreenshotCnt = screenshotCnt;
            node.AddNode(ConfigNode.CreateConfigFromObject(this));
        }

        public override void OnLoad(ConfigNode node)
        {
            if (loaded)
                return;
            loaded = true;
            Log.Info("KLScenario.OnLoad");

            base.OnLoad(node);
            try
            {
                ConfigNode.LoadObjectFromConfig(this, node.GetNode(GetType().FullName));
                logIdx = savedIdx;
                screenshotCnt = savedScreenshotCnt;

                if (node.HasNode("Export"))
                {
                    exportSettings = new ExportSettings(node.GetNode("Export"));
                    Log.Info("Export settings loaded");
                }
                if (node.HasNode("QuickExport"))
                {
                    quickExportSettings = new ExportSettings(node.GetNode("QuickExport"));
                    Log.Info("QuickExport settings loaded");
                }
            }
            catch
            {
            }

        }


    }
}

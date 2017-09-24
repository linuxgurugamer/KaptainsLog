using System;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using KSP.UI.Screens;
using UnityEngine;

namespace KaptainsLogNamespace
{

    public class CrewMember
    {
        public string name;
        public ProtoCrewMember.KerbalType type;
        public int experienceLevel;

        public CrewMember(string n, ProtoCrewMember.KerbalType t, int e)
        {
            name = n;
            type = t;
            experienceLevel = e;
        }
        public CrewMember()
        { }
    }

    public enum Events
    {
        CrewKilled,
        CrewModified,
        CrewOnEVA,
        CrewTransferred,
        DominantBodyChange,
        FinalFrontier,
        FlagPlant,
        FlightLogRecorded,
        KerbalPassedOutFromGeeForce,
        Landed,
        Splashdown,
        CrashOrSplashdown,
        Launch,
        ManualEntry,
        MiscExternal,
        OnFundsChanged,
        OnOrbitalSurveyCompleted,
        OnPartPurchased,
        OnPartUpgradePurchased,
        OnReputationChanged,
        OnScienceChanged,
        OnScienceReceived,
        OnTechnologyResearched,
        OnTriggeredDataTransmission,
        OnVesselRollout,
        OrbitClosed,
        OrbitEscaped,
        PartCouple,
        PartDied,
        ProgressRecord,
        Revert,
        ScreenMsgRecord,
        StageActivate,
        StageSeparation,
        VesselModified,
        VesselRecovered

    };
    public enum Fields
    {
        none, index, vesselId, vesselName, universalTime, utcTime, missionTime,
        vesselSituation, controlLevel, mainBody, altitude, speed,
        eventType, notes, thumbnail, screenshot, tag, lastItem
    };

    public enum PrintType { noPrint, print, screenshotPrint };
    public class LogEntry : IComparable<LogEntry>
    {
        public int index = 0;
        public string vesselName = "";
        public string vesselId = "";
        public double universalTime;
        public DateTime utcTime;
        public double missionTime;
        public Vessel.Situations vesselSituation;
        public string vesselMainBody = "";
        public Vessel.ControlLevel controlLevel;
        public double altitude;
        public double speed;
        public Events eventType;
        public string notes;
        public string vesselFlagURL = "";
        public string tag = "";

        public bool guiHidden = false;
        public string screenshotName = "";
        public string pngThumbnailName = "";
        public string jpgThumbnailName = "";
        public List<CrewMember> crewList = new List<CrewMember>();

        // Following are not saved, only used internally
        public string pngName;
        public string jpgName;
        public bool leSelected;
        //public bool print = true;
        public PrintType printFlag;
        public bool manualEntryRequired;

        public static string displayFieldName(Fields f)
        {
            switch (f)
            {
                case Fields.none:
                    return "None";
                case Fields.index:
                    return "Entry order";
                case Fields.vesselId:
                    return "Vessel ID";
                case Fields.vesselName:
                    return "Vessel";
                case Fields.universalTime:
                    return "Time";
                case Fields.utcTime:
                    return "UTC";
                case Fields.missionTime:
                    return "Mission Time";
                case Fields.vesselSituation:
                    return "Situation";
                case Fields.controlLevel:
                    return "Control Level";
                case Fields.mainBody:
                    return "Body";
                case Fields.altitude:
                    return "Altitude";
                case Fields.speed:
                    return "Speed";
                case Fields.eventType:
                    return "Event";
                case Fields.notes:
                    return "Notes";
                case Fields.thumbnail:
                    return "Thumbnail";
                case Fields.screenshot:
                    return "Screenshot";
                case Fields.tag:
                    return "Tag";
                case Fields.lastItem:
                    return "none";
            }
            return f.ToString();
        }

        public string displayEventString()
        {
            return displayEventString(eventType);
        }
        public static string displayEventString(Events evt)
        {
            switch (evt)
            {
                case Events.FlightLogRecorded: return "Flight log recorded";
                case Events.ScreenMsgRecord: return "Screen message recorded";
                case Events.Revert: return "Revert";
                case Events.PartDied: return "Part destroyed";
                case Events.Launch: return "Launch";
                case Events.StageSeparation: return "Stage seperation";
                case Events.PartCouple: return "Part couple (docked)";
                case Events.VesselModified: return "Vessel modified";
                case Events.StageActivate: return "Stage activate";
                case Events.OrbitClosed: return "Orbit closed";
                case Events.OrbitEscaped: return "Orbit escaped";
                case Events.VesselRecovered: return "Vessel recovered";
                case Events.Splashdown: return "Splashdown";
                case Events.CrashOrSplashdown: return "Crash";
                case Events.Landed: return "Landed";
                case Events.CrewModified: return "Crew modified";
                case Events.ProgressRecord: return "Progress Record";
                case Events.ManualEntry: return "Manual entry";
                case Events.FinalFrontier: return "Final Frontier";
                case Events.MiscExternal: return "Misc external";
                case Events.CrewKilled: return "Crew killed";
                case Events.CrewOnEVA: return "Crew on EVA";
                case Events.CrewTransferred: return "Crew transferred";
                case Events.DominantBodyChange: return "SOI change";
                case Events.FlagPlant: return "Flag plant";
                case Events.KerbalPassedOutFromGeeForce: return "Kerbal passed out from Gee force";
                case Events.OnScienceChanged: return "Science changed";
                case Events.OnScienceReceived: return "Science received at R&D";
                case Events.OnOrbitalSurveyCompleted: return "Orbital survey completed";
                case Events.OnReputationChanged: return "Reputation changed";
                case Events.OnTechnologyResearched: return "Technology researched";
                case Events.OnTriggeredDataTransmission: return "Data transmission triggered";
                case Events.OnVesselRollout: return "Vessel rollout";
                case Events.OnPartUpgradePurchased: return "Part upgrade purchased";
                case Events.OnPartPurchased: return "Part purchased";
                case Events.OnFundsChanged: return "Funds changed";
            }
            return evt.ToString();
        }

        public string displayVesselSituation()
        {
            return displayVesselSituation(vesselSituation);
        }
        public static string displayVesselSituation(Vessel.Situations s)
        {
            switch (s)
            {
                case Vessel.Situations.DOCKED:
                    return "Docked";
                case Vessel.Situations.ESCAPING:
                    return "Escaping Orbit";
                case Vessel.Situations.FLYING:
                    return "Flying";
                case Vessel.Situations.LANDED:
                    return "Landed";
                case Vessel.Situations.ORBITING:
                    return "In orbit";
                case Vessel.Situations.PRELAUNCH:
                    return "PreLaunch";
                case Vessel.Situations.SPLASHED:
                    return "Splashed Down";
                case Vessel.Situations.SUB_ORBITAL:
                    return "Sub-orbital";
            }
            return s.ToString();
        }

        public string displayControlLevel()
        {
            return displayControlLevel(controlLevel);
        }

        public static string displayControlLevel(Vessel.ControlLevel vcl)
        {
            switch (vcl)
            {
                case Vessel.ControlLevel.FULL:
                    return "Full";
                case Vessel.ControlLevel.NONE:
                    return "None";
                case Vessel.ControlLevel.PARTIAL_MANNED:
                    return "Partial/Manned";
                case Vessel.ControlLevel.PARTIAL_UNMANNED:
                    return "Partial/Unmanned";
            }
            return vcl.ToString();
        }

        public static Fields sortField = Fields.index;
        public static bool sortReverse = false;

        public int CompareTo(LogEntry y)
        {
            int rc = 0;
            switch (sortField)
            {
                case Fields.index:
                    rc = index - y.index;
                    break;

                case Fields.vesselId:
                    rc = String.Compare(vesselId, y.vesselId);
                    break;
                case Fields.vesselName:
                    rc = String.Compare(vesselName, y.vesselName);
                    break;
                case Fields.universalTime:
                    if (universalTime < y.universalTime) rc = -1;
                    else
                        if (universalTime > y.universalTime) rc = 1;
                    break;
                case Fields.utcTime:
                    if (utcTime < y.utcTime) rc = -1;
                    else
                       if (utcTime > y.utcTime) rc = 1;
                    break;
                case Fields.missionTime:
                    if (missionTime < y.missionTime) rc = -1;
                    else
                      if (missionTime > y.missionTime) rc = 1;
                    break;
                case Fields.vesselSituation:
                    if (vesselSituation < y.vesselSituation) rc = -1;
                    else
                     if (vesselSituation > y.vesselSituation) rc = 1;
                    break;
                case Fields.controlLevel:
                    if (controlLevel < y.controlLevel) rc = -1;
                    else
                     if (missionTime > y.missionTime) rc = 1;
                    break;
                case Fields.mainBody:
                    rc = String.Compare(vesselMainBody, y.vesselMainBody);
                    break;
                case Fields.altitude:
                    if (altitude < y.altitude) rc = -1;
                    else
                     if (altitude > y.altitude) rc = 1;
                    break;
                case Fields.speed:
                    if (speed < y.speed) rc = -1;
                    else
                     if (speed > y.speed) rc = 1;
                    break;
                case Fields.eventType:
                    if (eventType < y.eventType) rc = -1;
                    else
                     if (eventType > y.eventType) rc = 1;
                    break;
                case Fields.tag:
                    rc = String.Compare(tag, y.tag);
                    break;
            }
            if (rc == 0)
            {
                if (sortField != Fields.universalTime)
                {
                    if (universalTime < y.universalTime) rc = -1;
                    else
                            if (universalTime > y.universalTime) rc = 1;
                }
                else
                {
                    rc = String.Compare(vesselId, y.vesselId);
                }
            }
            if (sortReverse)
                return -rc;
            return rc;
        }

        public ScreenshotOptions eventScreenshot
        {
            get
            {
                ScreenshotOptions doScreenshot = ScreenshotOptions.No_Screenshot;
                switch (eventType)
                {
                    case Events.FlightLogRecorded: break;
                    case Events.ScreenMsgRecord: break;
                    case Events.Revert: break;
                    case Events.PartDied:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnPartDie;
                        break;
                    case Events.OnVesselRollout:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselRollout;
                        break;
                    case Events.Launch:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnLaunch;
                        Log.Info("Events.Launch, doScreenshot: " + doScreenshot.ToString());
                        break;
                    case Events.StageSeparation:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnStageSeparation;
                        break;
                    case Events.PartCouple:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnPartCouple;
                        break;
                    case Events.VesselModified:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselWasModified;
                        break;
                    case Events.StageActivate:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnStageActivate;
                        break;
                    case Events.OrbitClosed:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselOrbitClosed;
                        break;
                    case Events.OrbitEscaped:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselOrbitEscaped;
                        break;
                    case Events.VesselRecovered:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselRecovered;
                        break;
                    case Events.Landed:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnLanded;
                        break;
                    case Events.Splashdown:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnSplashdown;
                        break;
                    case Events.CrashOrSplashdown:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnCrashSplashdown;
                        break;
                    case Events.CrewModified:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnVesselCrewWasModified;
                        break;
                    case Events.ProgressRecord:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnProgressAchieve;
                        break;
                    case Events.ManualEntry:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnManualEntry;
                        break;
                    case Events.FinalFrontier: doScreenshot = ScreenshotOptions.No_Screenshot; break;
                    case Events.MiscExternal: doScreenshot = ScreenshotOptions.No_Screenshot; break;
                    case Events.CrewKilled:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnCrewKilled;
                        break;
                    case Events.CrewOnEVA:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnCrewOnEVA;
                        break;
                    case Events.CrewTransferred:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnCrewTransferred;
                        break;
                    case Events.DominantBodyChange:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnDominantBodyChange;
                        break;
                    case Events.FlagPlant:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnFlagPlant;
                        break;
                    case Events.KerbalPassedOutFromGeeForce:
                        doScreenshot = HighLogic.CurrentGame.Parameters.CustomParams<KL_23>().screenshotOnKerbalPassedOutFromGeeForce;
                        break;
                }
                return doScreenshot;
            }
        }
    }
}

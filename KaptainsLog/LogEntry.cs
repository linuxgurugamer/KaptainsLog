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
        FlightLogRecorded, ScreenMsgRecord, PartDied, Launch, StageSeparation, PartCouple, VesselModified,
        StageActivate, OrbitClosed, OrbitEscaped, VesselRecovered, Landed, CrewModified, ProgressRecord,
        KerbalPassedOutFromGeeForce, Revert,
        CrewKilled, CrewTransferred, DominantBodyChange, FlagPlant, CrewOnEVA,
        OnScienceChanged, OnScienceReceived, OnOrbitalSurveyCompleted,

        OnReputationChanged, OnTechnologyResearched, OnTriggeredDataTransmission, OnVesselRollout, OnPartUpgradePurchased, OnPartPurchased,
        OnFundsChanged,

        FinalFrontier, MiscExternal,
        ManualEntry
    };

    public enum Fields { none, vesselName, universalTime, utcTime, missionTime,
        vesselSituation, controlLevel, mainBody, altitude, speed,
        eventType, notes, thumbnail, screenshot, lastItem };

    public enum PrintType { noPrint, print, screenshotPrint};
    public class LogEntry
    {
        public string vesselName = "";
        public string id = "";
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
    }
}

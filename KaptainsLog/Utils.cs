using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using ProgressParser;
using KSPAchievements;
using KSP.Localization;

namespace KaptainsLogNamespace
{
    public class Utils
    {
        public Utils instance;
        KaptainsLog klw;

        public bool recoveryRequested = false;

        public Utils(KaptainsLog k)
        {
            instance = this;
            klw = k;
        }

        public void initializeEvents(bool init = true)
        {
            Log.Info("initializeEvents, init: " + init.ToString());
            if (init)
            {
                GameEvents.onLevelWasLoaded.Add(onLevelWasLoaded);

                GameEvents.onCrashSplashdown.Add(onCrashSplashdown);
                GameEvents.onVesselRecovered.Add(onVesselRecovered);
                GameEvents.onLaunch.Add(onLaunch);
                GameEvents.onStageSeparation.Add(onStageSeperation);
                GameEvents.onStageActivate.Add(onStageActivate);
                GameEvents.onPartDie.Add(onPartDie);
                GameEvents.onPartCouple.Add(onPartCouple);
                GameEvents.onVesselWasModified.Add(onVesselWasModified);
                GameEvents.onVesselCrewWasModified.Add(onVesselCrewWasModified);
                GameEvents.onVesselOrbitClosed.Add(onVesselOrbitClosed);
                GameEvents.onVesselOrbitEscaped.Add(onVesselOrbitEscaped);

                GameEvents.OnVesselRecoveryRequested.Add(onVesselRecoveryRequested);

                GameEvents.onCrewKilled.Add(onCrewKilled);
                GameEvents.onCrewTransferred.Add(onCrewTransferred);
                GameEvents.onDominantBodyChange.Add(onDominantBodyChange);
                GameEvents.onFlagPlant.Add(onFlagPlant);

                GameEvents.onKerbalPassedOutFromGeeForce.Add(onKerbalPassedOutFromGeeForce);
                GameEvents.onKerbalStatusChange.Add(onKerbalStatusChange);
                GameEvents.onKerbalInactiveChange.Add(onKerbalInactiveChange);

                GameEvents.onPartUndock.Add(onPartUndock);
                GameEvents.onSameVesselUndock.Add(onSameVesselUndock);
                GameEvents.onUndock.Add(onUndock);
                GameEvents.onCommandSeatInteractionEnter.Add(onCommandSeatInteractionEnter);
                GameEvents.onCommandSeatInteractionEnter.Add(onCommandSeatInteraction);

                GameEvents.onCrewOnEva.Add(onCrewOnEva);
                GameEvents.onCrewBoardVessel.Add(onCrewBoardVessel);
                GameEvents.OnFlightLogRecorded.Add(onFlightLogRecorded);
                GameEvents.OnProgressAchieved.Add(onProgressAchieve);
                GameEvents.OnProgressComplete.Add(onProgressComplete);
                GameEvents.onShowUI.Add(onShowUI);
                GameEvents.onHideUI.Add(onHideUI);

                GameEvents.onGameStatePostLoad.Add(onGameStatePostLoad);
            }
            else
            {
                GameEvents.onLevelWasLoaded.Remove(onLevelWasLoaded);

                GameEvents.onCrashSplashdown.Remove(onCrashSplashdown);
                GameEvents.onVesselRecovered.Remove(onVesselRecovered);
                GameEvents.onLaunch.Remove(onLaunch);
                GameEvents.onStageSeparation.Remove(onStageSeperation);
                GameEvents.onStageActivate.Remove(onStageActivate);
                GameEvents.onPartDie.Remove(onPartDie);
                GameEvents.onPartCouple.Remove(onPartCouple);
                GameEvents.onVesselWasModified.Remove(onVesselWasModified);
                GameEvents.onVesselCrewWasModified.Remove(onVesselCrewWasModified);
                GameEvents.onVesselOrbitClosed.Remove(onVesselOrbitClosed);
                GameEvents.onVesselOrbitEscaped.Remove(onVesselOrbitEscaped);
                GameEvents.OnFlightLogRecorded.Remove(onFlightLogRecorded);
                GameEvents.OnProgressAchieved.Remove(onProgressAchieve);
                GameEvents.OnProgressComplete.Remove(onProgressComplete);

                GameEvents.OnVesselRecoveryRequested.Remove(onVesselRecoveryRequested);
                GameEvents.onKerbalPassedOutFromGeeForce.Remove(onKerbalPassedOutFromGeeForce);
                GameEvents.onKerbalStatusChange.Remove(onKerbalStatusChange);
                GameEvents.onKerbalInactiveChange.Remove(onKerbalInactiveChange);

                GameEvents.onCrewKilled.Remove(onCrewKilled);
                GameEvents.onCrewOnEva.Remove(onCrewOnEva);
                GameEvents.onCrewBoardVessel.Remove(onCrewBoardVessel);
                GameEvents.onCommandSeatInteractionEnter.Remove(onCommandSeatInteractionEnter);
                GameEvents.onCommandSeatInteractionEnter.Remove(onCommandSeatInteraction);

                GameEvents.onCrewTransferred.Remove(onCrewTransferred);
                GameEvents.onDominantBodyChange.Remove(onDominantBodyChange);
                GameEvents.onFlagPlant.Remove(onFlagPlant);

                GameEvents.onPartUndock.Remove(onPartUndock);
                GameEvents.onSameVesselUndock.Remove(onSameVesselUndock);
                GameEvents.onUndock.Remove(onUndock);

                GameEvents.onShowUI.Remove(onShowUI);
                GameEvents.onHideUI.Remove(onHideUI);
                GameEvents.onGameStatePostLoad.Remove(onGameStatePostLoad);

            }
        }

        void onGameStatePostLoad(ConfigNode node)
        {
            Log.Info("onGameStatePostLoad");
            leQ.Clear();
            KaptainsLog.Instance.InitOnLoad();
            CreateLogEntry(Events.Revert, false, "", "");

        }
        void onKerbalInactiveChange(ProtoCrewMember pcm1, bool b1, bool b2)
        {
            Log.Info("onKerbalInactiveChange");
        }

        void onKerbalStatusChange(ProtoCrewMember pcm1, ProtoCrewMember.RosterStatus rs1, ProtoCrewMember.RosterStatus rs2)
        {
            Log.Info("onKerbalStatusChange");
        }

        void onCrewBoardVessel(GameEvents.FromToAction<Part, Part> b)
        {
            Log.Info("onCrewBoardVessel");
            
            Log.Info("onCrewBoardVessel, from: " + b.from.partInfo.name + "    to: " + b.to.partInfo.name);
        }
        void onUndock(EventReport evt)
        {
            Log.Info("onUndock");
            Log.Info("onUndock, evt.origin: " + evt.origin.name + ",   " + evt.origin.partInfo.name);
        }
        void onSameVesselUndock(GameEvents.FromToAction<ModuleDockingNode, ModuleDockingNode> fta)
        {
            Log.Info("onSameVesselUndock");

        }
        void onPartUndock(Part p)
        {
            Log.Info("onPartUndock");

            Log.Info("onPartUndock, vessel: " + p.vessel.vesselName + "   part: " + p.name + ",   " + p.partInfo.name);
            if (p.Modules.Contains<KerbalEVA>())
            {
                Log.Info("onPartUndock, setting kerbalGoingEVA");
                kerbalGoingEVA = true;
            }

         }
        void onCommandSeatInteractionEnter(KerbalEVA k, bool entering)
        {
            Log.Info("onCommandSeatInteractionEnter, entering: " + entering.ToString());
            
        }
        void onCommandSeatInteraction(KerbalEVA k, bool entering)
        {
            Log.Info("onCommandSeatInteraction, entering: " + entering.ToString());

        }
        void onKerbalPassedOutFromGeeForce(ProtoCrewMember crewMember)
        {
            Log.Info("onKerbalPassedOutFromGeeForce");
            CreateLogEntry(Events.KerbalPassedOutFromGeeForce, false, crewMember.name, crewMember.name);
        }

        void onCrewOnEva(GameEvents.FromToAction<Part, Part> b)
        {
            Log.Info("onCrewOnEva");
            Log.Info("from: " + b.from.partInfo.name + "    to: " + b.to.partInfo.name);
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrewOnEVA)
            //    klw.activatePause();
            CreateLogEntry(Events.CrewOnEVA, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrewOnEVA, b.to.vessel.vesselName );

        }
        //bool firstLoad = true;
        private void onLevelWasLoaded(GameScenes scene)
        {
            if (!Shelter.logsLoaded && (scene == GameScenes.SPACECENTER || scene == GameScenes.TRACKSTATION || scene == GameScenes.EDITOR || scene == GameScenes.FLIGHT))
            {
               // LoadLogs();
               // Shelter.logsLoaded = true;
            }

            if (scene == GameScenes.MAINMENU)
            {
                Shelter.logsLoaded = false;
            }
        }

        void onCrashSplashdown(EventReport evt)
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            Log.Info("onCrashSplashdown");
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrashSplashdown)
            //    klw.activatePause();
            CreateLogEntry(Events.Landed, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrashSplashdown);
        }

        void onVesselRecoveryRequested(Vessel v)
        {
            Log.Info("onVesselRecoveryRequested");
            recoveryRequested = true;
        }
        void onVesselRecovered(ProtoVessel pv, bool b)
        {
            if (pv == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            Log.Info("onVesselRecovered");

            recoveryRequested = false;
            CreateLogEntry(Events.VesselRecovered, false, pv.vesselName, pv.vesselName);
        }

        public string getCurrentCrew(Vessel v = null)
        {
            string crew = "";
            if (v == null)
                v = FlightGlobals.ActiveVessel;
            foreach (ProtoCrewMember kerbal in v.GetVesselCrew())
            {
                if (crew != "")
                    crew += ", ";
                crew += kerbal.name;
            }
            return crew;
        }

        void onLaunch(EventReport evt)
        {
            Log.Info("onLaunch 0, LoadedScene: " + HighLogic.LoadedScene.ToString());
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            Log.Info("onLaunch");

            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnLaunch)
            //    klw.activatePause();
            CreateLogEntry(Events.Launch, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnLaunch, getCurrentCrew());
        }

        void onStageSeperation(EventReport evt)
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            Log.Info("onStageSeperation");
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnStageSeparation)
            //    klw.activatePause();
            CreateLogEntry(Events.StageSeparation, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnStageSeparation);
        }
        void onStageActivate(int stage)
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            Log.Info("onStageActivate");
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnStageActivate)
            //    klw.activatePause();
            CreateLogEntry(Events.StageActivate, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnStageActivate);
        }

        void onPartDie(Part p)
        {
            if (p == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            Log.Info("onPartDie");

            string s;
            if (p.vessel.rootPart != p)
                s = p.vessel.name + ", " + p.partInfo.name + " was destroyed";
            else
                s = p.partInfo.name + " was destroyed";
            if (p.vessel == FlightGlobals.ActiveVessel)
                CreateLogEntry(Events.PartDied, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnPartDie, s);
            else
                CreateLogEntry(Events.PartDied, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnDisconnectedPartDie, s);
        }
        void onPartCouple(GameEvents.FromToAction<Part, Part> evt)
        {
            if (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            if (evt.from.vessel.rootPart.Modules.Contains<KerbalEVA>())
                return;
            Log.Info("onPartCouple");
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnPartCouple)
            //    klw.activatePause();

            CreateLogEntry(Events.PartCouple, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnPartCouple);
        }

        void onVesselWasModified(Vessel v)
        {
            if (v == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            //foreach (var p in v.Parts)
            //    if (p.Modules.Contains("KerbalEVA"))
            //        return;
            if (v.rootPart.Modules.Contains<KerbalEVA>())
                return;
            if (kerbalGoingEVA)
            {
                Log.Info("onVesselWasModified, not throwing because kerbalGoingEVA is true, vessel: " + v.vesselName);
                kerbalGoingEVA = false;
                return;
            }
            Log.Info("onVesselWasModified, v.isEVA: " + v.isEVA.ToString());
            Log.Info("onVesselWasModified, v.vesselType: " + v.vesselType.ToString());
            //Log.Info("onVesselWasModified, v.GetComponent<KerbalEVA>(): " + v.GetComponent<KerbalEVA>().ToString());


            if (v.vesselType == VesselType.Flag)
                return;
            Log.Info("onVesselWasModified");

            var s = v.name + " was modified";
            if (v == FlightGlobals.ActiveVessel)
                CreateLogEntry(Events.VesselModified, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselWasModified, s);
else
            CreateLogEntry(Events.VesselModified, false, s);
        }

        void onVesselCrewWasModified(Vessel v)
        {
            if (v == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            Log.Info("onVesselCrewWasModified, kerbalGoingEVA: " + kerbalGoingEVA.ToString() + ",    kerbalTransfered: " + kerbalTransferred.ToString());
            if (kerbalGoingEVA)
                return;
            if (kerbalTransferred > 0)
            {
                Log.Info("onVesselCrewWasModified, not throwing because kerbalTransfered is true, vessel: " + v.vesselName);
                kerbalTransferred--;
                return;
            }
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselCrewWasModified)
            //    klw.activatePause();
            CreateLogEntry(Events.CrewModified, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselCrewWasModified);
        }

        void onVesselOrbitClosed(Vessel v)
        {
            if (v == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            Log.Info("onVesselOrbitClosed");
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselOrbitClosed)
            //    klw.activatePause();
            CreateLogEntry(Events.OrbitClosed, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselOrbitClosed);
        }
        void onVesselOrbitEscaped(Vessel v)
        {

            if (v == null || (HighLogic.LoadedScene != GameScenes.SPACECENTER && HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION))
                return;
            Log.Info("onVesselOrbitEscaped, vessel: " + v.name);
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselOrbitEscaped)
            //    klw.activatePause();
            CreateLogEntry(Events.OrbitEscaped, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnVesselOrbitEscaped);
        }

        void onFlightLogRecorded(Vessel v)
        {
            if (HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.TRACKSTATION)
                return;
            //if (snapshotTaken > 0)
                //return;
            Log.Info("onFlightLogRecorded");
            //CreateLogEntry(Events.FlightLogRecorded);

            FlightLog.Entry fe = VesselTripLog.FromVessel(v).Log.Last();
            if (fe != null)
            {
                Log.Info("Flight Log type: " + fe.type);
                CreateLogEntry(Events.FlightLogRecorded,false,  "At " + GetKerbinTime(fe.flight).ToString() + ", "  + fe.type + " at " + fe.target );
            }
        }       

        void onCrewKilled(EventReport report)
        {
            Log.Info("onCrewKilled");
            // report.sender
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrewKilled)
            //    klw.activatePause();
            CreateLogEntry(Events.CrewKilled, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrewKilled, report.sender);
        }

        bool kerbalGoingEVA = false;
        int kerbalTransferred = 0;
        void onCrewTransferred(GameEvents.HostedFromToAction<ProtoCrewMember, Part> data)
        {
            Log.Info("onCrewTransferred");

            //Log.Info("onCrewTransferred, from: " + data.from.name.ToString() + ", " + data.from.partInfo.name);
            //Log.Info("onCrewTransferred, to: " + data.to.name.ToString() + ", " + data.to.partInfo.name);
            //Log.Info("onCrewTransferred, host: " + data.host.name.ToString() + ", " + data.host.nameWithGender);
            Log.Info("onCrewTransferred, from: " + data.from.name + ", " + data.from.partInfo.name + "   to: " + data.to.name + ", " + data.to.partInfo.name);
            if (data.to.Modules.Contains<KerbalEVA>())
            {
                Log.Info("Kerbal going EVA");
                kerbalGoingEVA = true;
                return;
            }
            kerbalTransferred = 2;

           // if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnCrewTransferred)
           //     klw.activatePause();
            //CreateLogEntry(Events.CrewTransferred, data.)
        }

        void onDominantBodyChange(GameEvents.FromToAction<CelestialBody, CelestialBody> data)
        {
            Log.Info("onDominantBodyChange");

            // This happens when, for example, a kerbal goes EVA on a planet
            if (data.from.bodyName == data.to.bodyName)
                return;
            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnDominantBodyChange)
            //    klw.activatePause();
            CreateLogEntry(Events.DominantBodyChange, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnDominantBodyChange, data.from.bodyName + " to " + data.to.bodyName);
        }

        void onFlagPlant(Vessel v)
        {
            Log.Info("onFlagPlant");

            //if (!klw.pauseActivated && Planetarium.GetUniversalTime() > klw.lastNoteTime && HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnFlagPlant)
            //    klw.activatePause();
            
            CreateLogEntry(Events.FlagPlant, HighLogic.CurrentGame.Parameters.CustomParams<KL_2>().pauseOnFlagPlant,  v.vesselName + " planted flag on " + v.mainBody.name);
        }

        #region AdaptedFromProgressParser
        //
        // The following two methods are adapted from the ProgressParser mod, written by @DMagic
        //
        void onProgressAchieve(ProgressNode node)
        {
            if (node == null)
                return;
            Log.Info("onProgressAchieve");
            // Need to get info from the progressnode and add it to the notes
            if (progressParser .isIntervalType(node))
            {
                string descr = "";
                progressInterval i = progressParser.getIntervalNode(node.Id);

                if (i != null)
                {
                    double nodeRecord = progressParser .getIntervalRecord(node, ref descr);
                    Log.Info("Reached: " + descr + "   isReached: " + node.IsReached.ToString() + "   getRecord: " + i.getRecord(i.Interval).ToString() + ",  reached: " + nodeRecord.ToString());
                    if (i.getRecord(i.Interval) >= nodeRecord)
                        return;

                    if (node.IsReached)
                    {
                        CreateLogEntry(Events.ProgressRecord, false, descr + " record reached: " + i.getRecord(i.Interval).ToString());
                    }
                }
                else
                    Log.Info("No interval node found");
            }
        }

        void onProgressComplete(ProgressNode node)
        {
            if (node == null)
                return;
            if (!node.IsComplete)
                return;
            Log.Info("onProgressComplete");

            Log.Info("onProgressComplete, node type: " + node.GetType().ToString());
            // Need to get info from the progressnode and add it to the notes
            if (progressParser .isIntervalType(node))
            {
                Log.Info("isIntervalType");
                string descr = "";
                progressInterval i = progressParser.getIntervalNode(node.Id);

                if (i != null)
                {
                    if (node.IsReached)
                    {
                        Log.Info("IsReached");
                        double nodeRecord = progressParser .getIntervalRecord(node, ref descr);
                        CreateLogEntry(Events.ProgressRecord, false, Localizer.Format(descr) + " record completed: " + nodeRecord.ToString());
                    }
                }
            }
            else
            {
                Log.Info("isStandardType");
                string logtext = "";
                double t;
                if (progressParser .isPOI(node))
                {
                    progressStandard s = progressParser.getPOINode(node.Id);
                    
                    if (s == null)
                    {
                        Log.Info("POI Progress Node Not Found");
                    }
                    else
                    {
                        logtext = progressParser.vesselNameFromNode(node);

                        try
                        {
                            t = (double)node.GetType().GetField("AchieveDate", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(node);
                            logtext += " " + GetKerbinTime(t);
                        }
                        catch (Exception e)
                        {
                            Log.Warning("Error In Detecting Progress Node Achievement Date\n" + e);
                        }
                    }
                }
                else
                {
                    progressStandard s = progressParser.getStandardNode(node.Id);

                    if (s != null)
                    {
                        Log.Info("standard node found");
                        logtext = Localizer.Format(s.Descriptor); 
                        string note = progressParser.crewNameFromNode(node);

                        if (string.IsNullOrEmpty(note))
                            note = progressParser.vesselNameFromNode(node);

                        logtext += note;

                        try
                        {
                            t = (double)node.GetType().GetField("AchieveDate", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(node);
                            logtext += " " + GetKerbinTime(t);
                        }
                        catch (Exception e)
                        {
                            Log.Warning("Error In Detecting Progress Node Achievement Date\n" + e);
                        }
                    }
                    else
                    {
                        Log.Info("standard node not found");
                        CelestialBody body = progressParser.getBodyFromType(node);

                        if (body == null)
                        {
                            Log.Info("Body From Progress Node Null...");
                        }
                        else
                        {
                            progressBodyCollection b = progressParser.getProgressBody(body);

                            if (b != null)
                            {
                                progressStandard sb = b.getNode(node.Id);

                                if (sb == null)
                                {
                                    Log.Info("Body Sub Progress Node Not Found");
                                }
                                else
                                {
                                    string note = Localizer.Format(progressParser.crewNameFromNode(node));

                                    if (string.IsNullOrEmpty(note))
                                        note = Localizer.Format(progressParser.vesselNameFromNode(node));

                                    logtext = note;

                                    try
                                    {
                                        t = (double)node.GetType().GetField("AchieveDate", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(node);
                                        logtext += " " + GetKerbinTime(t);
                                    }
                                    catch (Exception e)
                                    {
                                        Log.Warning("Error In Detecting Progress Node Achievement Date\n" + e);
                                    }
                                }
                            }
                        }
                    }
                }
                if (logtext != "")
                    CreateLogEntry(Events.ProgressRecord, false, logtext);
            }
        }
        //
        // End of methods adapted from ProgressParser mod
        //
#endregion

        public bool uiVisible = true;
        private void onShowUI()
        {
            Log.Info("UICLASS onShowUI");
            uiVisible = true;
        }

        private void onHideUI()
        {
            Log.Info("UICLASS onHideUI");
            uiVisible = false;
        }

        public LogEntry le = null;

        public Queue<LogEntry> leQ = new Queue<LogEntry>();

        public void CreateLogEntry(Events evt, bool manualEntryRequired, string notes = "", string noActiveVessel = null)
        {
            if (FlightGlobals.ActiveVessel == null && noActiveVessel == null)
            {
                Log.Info("CreateLogEntry, returning due to null ActiveVessel");
                return;
            }
            Log.Info("CreateLogEntry");
            Shelter.dirtyColSel = true;
            LogEntry leLocal = new LogEntry();
            if (noActiveVessel == null)
            {
                leLocal.vesselName = FlightGlobals.ActiveVessel.vesselName;
                leLocal.id = FlightGlobals.ActiveVessel.id.ToString();

                leLocal.universalTime = Planetarium.GetUniversalTime();
                leLocal.missionTime = FlightGlobals.ActiveVessel.missionTime;

                leLocal.utcTime = DateTime.UtcNow;

                leLocal.vesselSituation = FlightGlobals.ActiveVessel.situation;
                leLocal.vesselMainBody = FlightGlobals.ActiveVessel.mainBody.name;
                leLocal.controlLevel = FlightGlobals.ActiveVessel.CurrentControlLevel;
                leLocal.vesselFlagURL = FlightGlobals.ActiveVessel.rootPart.flagURL;

                leLocal.altitude = FlightGlobals.ActiveVessel.altitude;
                leLocal.speed = FlightGlobals.ActiveVessel.speed;
                
                foreach (ProtoCrewMember kerbal in FlightGlobals.ActiveVessel.GetVesselCrew())
                {
                    CrewMember cm = new CrewMember(kerbal.name, kerbal.type, kerbal.experienceLevel);
                    leLocal.crewList.Add(cm);
                }
            } 
            else
            {
                leLocal.vesselName = noActiveVessel;
            }
            leLocal.manualEntryRequired = manualEntryRequired;
            leLocal.eventType = evt;
            leLocal.notes = notes;
            klw.notesText = notes;

            if (manualEntryRequired)
            {
                Log.Info("queueScreenshot 2");
                queueScreenshot(KaptainsLog.kaptainsLogList.Count + 1, ref leLocal);
                klw.notesEntryComplete = false; 
                leQ.Enqueue(leLocal);
                Log.Info("leQ size: " + leQ.Count.ToString());
                le = leQ.Peek();
            } else
            {
                Log.Info("Adding log, current logcount: " + KaptainsLog.kaptainsLogList.Count.ToString());
                Log.Info("vesselName: " + leLocal.vesselName + ", situation: " + leLocal.displayVesselSituation() + ", eventType: " + leLocal.displayEventString() + ", notes: " + leLocal.notes);
                //klw.notesEntryComplete = true; 
                //KaptainsLog.kaptainsLogList.Add(leLocal);
                //Shelter.dirtyFilter = true;
                //SaveLogs();

                queueScreenshot(KaptainsLog.kaptainsLogList.Count + 1, ref leLocal);
                klw.notesEntryComplete = false;
                leQ.Enqueue(leLocal);
                Log.Info("leQ size: " + leQ.Count.ToString());
                le = leQ.Peek();

            }            
        }
        
        static public string SafeLoad(string value, string oldvalue)
        {
            if (value == null)
                return oldvalue;
            return value;
        }

        static public string SafeLoad(string value, double oldvalue)
        {
            if (value == null)
                return oldvalue.ToString();
            return value;
        }
        static public string SafeLoad(string value, int oldvalue)
        {
            if (value == null)
                return oldvalue.ToString();
            return value;
        }
        static public string SafeLoad(string value, bool oldvalue)
        {
            if (value == null)
                return oldvalue.ToString();
            return value;
        }
        static public string SafeLoad(string value, DateTime oldvalue)
        {
            if (value == null)
                return oldvalue.ToString();
            return value;
        }

        public void LoadLogs()
        {
            if (HighLogic.SaveFolder == "DestructiblesTest" || HighLogic.SaveFolder == "")
                return;
            ConfigNode logFile;
            ConfigNode log;
            klw.SAVE_PATH = KaptainsLog.ROOT_PATH + "saves/" + HighLogic.SaveFolder;
            klw.SAVE_PATH = new Uri(klw.SAVE_PATH).LocalPath;
            klw.InitOnLoad();

            //klw.SAVE_PATH = KaptainsLog.ROOT_PATH + "saves/" + HighLogic.SaveFolder;

            Log.Info("Loading log file: " + klw.SAVE_PATH + "/" + klw.logFileName);
            logFile = ConfigNode.Load(klw.SAVE_PATH + "/" + klw.logFileName);

            if (logFile != null)
            {
                log = logFile.GetNode(klw.NODENAME);
                if (log == null)
                    return;
                ConfigNode[] entries = log.GetNodes();
                foreach (var entry in entries)
                {
                    le = new LogEntry();
                    le.vesselName = entry.GetValue("vesselName");
                    le.id = entry.GetValue("id");

                    le.universalTime = Double.Parse(SafeLoad(entry.GetValue("universalTime"), 0));
                    le.missionTime = Double.Parse(SafeLoad(entry.GetValue("missionTime"), 0));
                    le.utcTime = DateTime.Parse(SafeLoad(entry.GetValue("utcTime"), DateTime.UtcNow));

                    le.vesselSituation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), SafeLoad(entry.GetValue("vesselSituation"), "LANDED"));
                    le.controlLevel = (Vessel.ControlLevel)Enum.Parse(typeof(Vessel.ControlLevel), SafeLoad(entry.GetValue("controlLevel"), "FULL"));
                    le.vesselMainBody = SafeLoad(entry.GetValue("vesselMainBody"), "Kerbin");
                    le.altitude = Double.Parse(SafeLoad(entry.GetValue("altitude"), 0));
                    le.speed = Double.Parse(SafeLoad(entry.GetValue("speed"), 0));
                    le.eventType = (Events)Enum.Parse(typeof(Events), SafeLoad(entry.GetValue("eventType"), "ManualEntry"));

                    int notesLines = int.Parse(SafeLoad(entry.GetValue("notesLines"), 0));
                    le.notes = "";
                    for (int i = 0; i < notesLines; i++)
                    {
                        string str = entry.GetValue("notes-" + i.ToString());
                        if (i > 0)
                            le.notes += "\n";
                        le.notes += str;
                    }
                    
                    le.vesselFlagURL = SafeLoad(entry.GetValue("vesselFlagURL"), "");
                    le.screenshotName = SafeLoad(entry.GetValue("screenshotName"), "");
                    le.pngThumbnailName = SafeLoad(entry.GetValue("pngThumbnailName"), "");
                    le.jpgThumbnailName = SafeLoad(entry.GetValue("jpgThumbnailName"), "");

                    ConfigNode[] crewEntries = entry.GetNodes(klw.CREWNODE);
                    foreach (var crew in crewEntries)
                    {
                        CrewMember cm = new CrewMember();
                        cm.name = SafeLoad(crew.GetValue("name"), "none");
                        cm.experienceLevel = int.Parse(SafeLoad(crew.GetValue("experienceLevel"), 0));
                        cm.type = (ProtoCrewMember.KerbalType)Enum.Parse(typeof(ProtoCrewMember.KerbalType), SafeLoad(entry.GetValue("type"), "Crew"));
                        le.crewList.Add(cm);
                    }

                    Log.Info("Loaded log: " +
                        "vesselName: " + le.vesselName +
                        ", id: " + le.id +
                        ", universalTime: " + le.universalTime.ToString() +
                        ", utcTime: " + le.utcTime.ToString() +
                        ", vesselSituation: " + le.vesselSituation.ToString() +
                        ", controlLevel: " + le.controlLevel.ToString() +
                        ", altitude: " + le.altitude.ToString() +
                        ", eventType: " + le.eventType.ToString() +
                        ", notes: " + le.notes


                        );
                    KaptainsLog.kaptainsLogList.Add(le);
                    Shelter.dirtyFilter = true;
                }
            }
        }

        public void SaveLogs()
        {
            Log.Info("SaveLogs");
            if (HighLogic.SaveFolder == "DestructiblesTest" || HighLogic.SaveFolder == "")
                return;

            ConfigNode logFile = new ConfigNode();
            ConfigNode log = new ConfigNode();
            logFile.SetNode(klw.NODENAME, log, true);

            for (int i = 0; i < KaptainsLog.kaptainsLogList.Count; i++)
            {
                le = KaptainsLog.kaptainsLogList[i];
                var entry = new ConfigNode();
                entry.AddValue("vesselName", le.vesselName);
                entry.AddValue("id", le.id);
                entry.AddValue("universalTime", le.universalTime);
                entry.AddValue("missionTime", le.missionTime);
                entry.AddValue("utcTime", le.utcTime);

                entry.AddValue("vesselSituation", le.vesselSituation);
                entry.AddValue("controlLevel", le.controlLevel);
                entry.AddValue("vesselMainBody", le.vesselMainBody);
                entry.AddValue("altitude", le.altitude);
                entry.AddValue("speed", le.speed);
                entry.AddValue("eventType", le.eventType);

                entry.AddValue("notes", le.notes);
                string[] lines = le.notes.Split('\n');
                entry.AddValue("notesLines", lines.Length.ToString());
                for (int cnt = 0; cnt < lines.Length; cnt++)
                {
                    entry.AddValue("notes-" + cnt.ToString(), lines[cnt]);
                }
                foreach (var cm in le.crewList)
                {
                    ConfigNode crewNode = new ConfigNode();
                    crewNode.AddValue("name", cm.name);
                    crewNode.AddValue("experienceLevel", cm.experienceLevel);
                    crewNode.AddValue("type", cm.type);
                    entry.AddNode(klw.CREWNODE, crewNode);
                }

                entry.AddValue("vesselFlagURL", le.vesselFlagURL);
                entry.AddValue("screenshotName", le.screenshotName);
                entry.AddValue("pngThumbnailName", le.pngThumbnailName);
                entry.AddValue("jpgThumbnailName", le.jpgThumbnailName);

                log.AddNode(i.ToString("D5"), entry);
            }
           
            //klw.SAVE_PATH = KaptainsLog.ROOT_PATH + "saves/" + HighLogic.SaveFolder;
            // following line for debugging only
            //logFile.Save(KSP_DIR + "/" + LOG_DIR + "/" + logFileName);
            logFile.Save(klw.SAVE_PATH + "/" + klw.logFileName);
            ScreenMessages.PostScreenMessage("Log Entry Saved");
        }
        
        private static string GetTime(double time, double secondsPerMinute, double minutesPerHour,
              double hoursPerDay, double monthsPerYear, double daysPerYear, double daysPerMonth,
              bool showMonth = true, bool elapsedTime = false)
        {
            long seconds = (long)(time);

            long minutes = (long)(seconds / secondsPerMinute);
            seconds -= (long)(minutes * secondsPerMinute);

            long hours = (long)(minutes / minutesPerHour);
            minutes -= (long)(hours * minutesPerHour);

            long days = (long)(hours / hoursPerDay);
            hours -= (long)(days * hoursPerDay);

            long months = 0;
            long years = 0;
            if (showMonth)
            {
                months = (long)(days / daysPerMonth);
                days -= (long)(months * daysPerMonth);

                years = (long)(months / monthsPerYear);
                months -= (long)(years * monthsPerYear);
            }
            else
            {
                years = (long)(days / daysPerYear);
                days -= (long)(years * daysPerYear);
            }

            if (!elapsedTime)
            {
                // The game starts on Year 1, Day 1
                years += 1;
                months += 1;
                days += 1;
            }
            string str = "";
            if (!elapsedTime || years > 0)
                str = years.ToString() + "y, ";
            if (!elapsedTime || (showMonth && months > 0))
                str += months.ToString() + "m, ";
            if (!elapsedTime || days > 0)
                str += days.ToString() + "d ";
            str += hours.ToString("00") + ":"
                    + minutes.ToString("00") + ":"
                    + seconds.ToString("00");
            return str;
#if false
            if (showMonth)
            {
                return years.ToString() + "y, "
                     + months.ToString() + "m, "
                    + days.ToString() + "d "
                    + hours.ToString("00") + ":"
                    + minutes.ToString("00") + ":"
                    + seconds.ToString("00");
            }
            else
            {
                return years.ToString("00") + "y, "
                     + days.ToString("00") + "d "
                    + hours.ToString("00") + ":"
                    + minutes.ToString("00") + ":"
                    + seconds.ToString("00");
            }
#endif
        }
        public string GetKerbinTime(double ut, bool elapsedTime = false)
        {
            double kerbinSecondsPerEarthSecond = (klw.settings.kerbinSecondsPerMinute * klw.settings.kerbinMinutesPerHour * klw.settings.kerbinHoursPerDay) / klw.settings.earthSecondsPerKerbinDay;
            double scaledUt = (ut + klw.settings.initialOffsetInEarthSeconds) * kerbinSecondsPerEarthSecond;

            return GetTime(scaledUt, klw.settings.kerbinSecondsPerMinute, klw.settings.kerbinMinutesPerHour,
                klw.settings.kerbinHoursPerDay, klw.settings.kerbinMonthsPerYear, klw.settings.kerbinDaysPerYear,
                klw.settings.kerbinDaysPerMonth, true, elapsedTime);
        }

        
        public string getDisplayString(LogEntry le, Fields field, bool fullLength = false)
        {
            string f = "";
            switch (field)
            {
                case Fields.vesselName:
                    f = le.vesselName;
                    break;
                case Fields.universalTime:
                    //f = le.universalTime.ToString("N0");
                    f = GetKerbinTime(le.universalTime);
                    break;
                case Fields.utcTime:
                    DateTime dispDt = le.utcTime.ToLocalTime();
                    //string datePatt = "mm/DD/YY";
                    //f = dispDt.ToString(datePatt);
                    f = dispDt.ToShortDateString();
                    break;
                case Fields.missionTime:
                    //f = le.missionTime.ToString("N1");
                    f = GetKerbinTime(le.missionTime, true);
                    break;
                case Fields.vesselSituation:
                    f = le.displayVesselSituation();
                    break;
                case Fields.controlLevel:
                    return le.displayControlLevel();
                case Fields.mainBody:
                    f = le.vesselMainBody;
                    break;
                case Fields.altitude:
                    f = le.altitude.ToString("N0");
                    break;
                case Fields.speed:
                    f = le.speed.ToString("N0");
                    break;
                case Fields.eventType:
                    f = le.displayEventString();
                    break;
                case Fields.notes:
                    if (le.notes.Length < 40 || fullLength)
                        f = le.notes;
                    else
                        f = le.notes.Substring(0, 37) + "...";
                    break;
                case Fields.screenshot:
                    return le.screenshotName;
                case Fields.thumbnail:
                    return le.jpgThumbnailName;

                case Fields.none:
                    return null;
            }
            return f;
        }



        public void ConvertToJPG(string originalFile, string newFile, int quality = 75)
        {
            Texture2D png = new Texture2D(1, 1);

            byte[] pngData = System.IO.File.ReadAllBytes(originalFile);
            png.LoadImage(pngData);
            byte[] jpgData = png.EncodeToJPG(quality);
            var file = System.IO.File.Open(newFile, System.IO.FileMode.Create);
            var binary = new System.IO.BinaryWriter(file);
            binary.Write(jpgData);
            file.Close();
            //Destroy(png);
            //Resources.UnloadAsset(png);
        }
        
        public bool snapshotInProgress = false;
        public int snapshotTaken = 0;

        public bool wasUIVisible;
       
       // public string thumbnailName;

        public void queueScreenshot(int cnt, ref LogEntry le)
        {
            cnt = KLScenario.GetNextCnt;
            Log.Info("queueScreenshot cnt: " + cnt.ToString());
            string s;
            string screenshotPath = KaptainsLog.ROOT_PATH + "Screenshots/";
            if (HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().saveScreenshotsInSaveFolder)
            {
                screenshotPath = KaptainsLog.ROOT_PATH + "saves/" + HighLogic.SaveFolder + "/KaptainsLogScreenshots/";
                if (!System.IO.Directory.Exists(screenshotPath))
                    System.IO.Directory.CreateDirectory(screenshotPath);
                s = "KL_" + cnt.ToString("000");
            }
            else
            {
                s = KaptainsLog.screenshotPrefix + HighLogic.SaveFolder + "_" + cnt.ToString("000");
            }

            le.pngName = System.IO.Path.GetFullPath(screenshotPath) + s + ".png";
            le.pngThumbnailName = System.IO.Path.GetFullPath(screenshotPath) + s + "_thumb.png";
            le.jpgThumbnailName = System.IO.Path.GetFullPath(screenshotPath) + s + "_thumb.jpg";
            le.jpgName = System.IO.Path.GetFullPath(screenshotPath) + s + ".jpg";

            Log.Info("pngName: " + le.pngName);
            Log.Info("jpgName: " + le.jpgName);
            Log.Info("pngThumbnailName: " + le.pngThumbnailName);
            Log.Info("jpgThumbnailName: " + le.jpgThumbnailName);

            if (!HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().saveAsJPEG)
                le.screenshotName = le.pngName;
            else
                le.screenshotName = le.jpgName;
            if (!snapshotInProgress)
            {
                wasUIVisible = uiVisible;
                snapshotInProgress = true;

                klw.screenshotAfter = Planetarium.GetUniversalTime() + HighLogic.CurrentGame.Parameters.CustomParams<KL_1>().delayBeforePause;

                snapshotTaken = 1;
            }
        }

    }
}

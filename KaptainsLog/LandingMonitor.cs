using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.AllGameScenes, true)]
    class LandingMonitor : MonoBehaviour
    {
        const float ChecksPerSecond = 5;

        public class VesselStatus
        {
            Guid id;
            public bool Landed = false;
            public bool Splashed = false;
            public bool Flying = false;
            public double universalTime;

            public bool landedEvent = false;
            public bool splashedEvent = false;
            public bool flyingEvent = false;



            public VesselStatus(Vessel v)
            {
                id = v.id;
                Landed = v.Landed;
                Splashed = v.Splashed;
                universalTime = Planetarium.GetUniversalTime();
            }
            public void SetLandedTime(bool b)
            {
                Log.Info("LandingMonitor: SetLandedTime, b: " + b.ToString());
                Landed = b;
                Flying = false;
                flyingEvent = false;
                splashedEvent = false;
                if (b)
                    universalTime = Planetarium.GetUniversalTime();
                else
                    landedEvent = false;
            }
            public void SetSplashedTime(bool b)
            {

                Log.Info("LandingMonitor: SetSplashedTime, b: " + b.ToString());
                Splashed = b;
                Flying = false;
                flyingEvent = false;
                landedEvent = false;
                if (b)
                    universalTime = Planetarium.GetUniversalTime();
                else
                    splashedEvent = false;
            }
            // Flying is tricky, we need to allow for bumps, hops, etc.
            // So don't reset any flags until all the conditions have been met
            public void SetFlyingTime(bool b)
            {
                Log.Info("LandingMonitor: SetFlyingTime, b: " + b.ToString());
                if (b)
                    universalTime = Planetarium.GetUniversalTime();
                Flying = b;
            }
            public void SetFlyingEvent()
            {
                Log.Info("LandingMonitor: SetFlyingEvent");
                Landed = false;
                Splashed = false;
                landedEvent = false;
                splashedEvent = false;
                flyingEvent = true;
            }
        }

        static Dictionary<Guid, VesselStatus> vesselLandedDict = new Dictionary<Guid, VesselStatus>();



        void Start()
        {
            if (!HighLogic.CurrentGame.Parameters.CustomParams<KL_11>().EnabledForSave)
                return;
            Log.Info("LandingMonitor.Start");
            DontDestroyOnLoad(this);
            InvokeRepeating("WatchForLanding", 2.0f, 1f / ChecksPerSecond);
        }

        void OnDestroy()
        {
            CancelInvoke();
        }
        internal void WatchForLanding()
        {
            if (!Utils.vesselInFlight)
                return;
            Log.Info("LandingMonitor: WatchForLanding, time: " + Planetarium.GetUniversalTime().ToString("n2") + ",  vesselsLoaded.Count: " + FlightGlobals.fetch.vesselsLoaded.Count().ToString());
            for (int x = FlightGlobals.fetch.vesselsLoaded.Count() - 1; x >= 0; x--)
            {
                var v = FlightGlobals.fetch.vesselsLoaded[x];
                VesselStatus vesselStatus = null;
                try
                {
                    vesselStatus = vesselLandedDict[v.id];
                }
                catch (KeyNotFoundException)
                {
                    vesselStatus = new VesselStatus(v);
                    vesselLandedDict.Add(v.id, vesselStatus);
                }
                if (vesselStatus == null)
                {
                    Log.Error("WatchForLanding, vesselStatus is null");
                }
                if (v.Landed != vesselStatus.Landed)
                    vesselStatus.SetLandedTime(v.Landed);
                if (v.Splashed != vesselStatus.Splashed)
                    vesselStatus.SetSplashedTime(v.Splashed);

                /* Flying definition
                   1. Altitude > 50m
                   2.  Not touching the ground for 5 seconds                   
                 */
                if (v.heightFromTerrain >= HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().minFlyingAltitude)
                {
                    Log.Info("LandingMonitor: v.heightFromTerrain: " + v.heightFromTerrain.ToString());
                    if (!vesselStatus.Flying)
                        vesselStatus.SetFlyingTime(true);

                    if (vesselStatus.Flying &&
                       Planetarium.GetUniversalTime() - vesselStatus.universalTime >= HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().minFlyingTime)
                    {
                        vesselStatus.SetFlyingEvent();
                    }
                }

                /* Landing definition
                    1.  Touching the ground
                    2.  speed less than 0.05
                    3.  Stable for 5 seconds
                */
                if (!vesselStatus.landedEvent && v.Landed &&
                    Planetarium.GetUniversalTime() - vesselStatus.universalTime >= HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().landedStabilityTime &&
                    v.speed < 0.05f)
                {
                    Log.Info("LandingMonitor: Triggering landed event");
                    vesselStatus.landedEvent = true;
                    vesselStatus.Flying = false;
                    Utils.instance.onVesselLanded(v);
                }

            }
        }


    }
}

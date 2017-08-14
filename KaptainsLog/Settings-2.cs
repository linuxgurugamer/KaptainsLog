using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;



namespace KaptainsLogNamespace
{
    public class KL_4 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Initial Display Columns"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 1; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomParameterUI("Vessel name")]
        public bool vesselName = true;
        [GameParameters.CustomParameterUI("Universal time")]
        public bool universalTime = true;
        [GameParameters.CustomParameterUI("UTC time")]
        public bool utcTime = false;
        [GameParameters.CustomParameterUI("Mission time")]
        public bool missionTime = true;

        [GameParameters.CustomParameterUI("Vessel Situation")]
        public bool vesselSituation = true;
        [GameParameters.CustomParameterUI("Control Level")]
        public bool controlLevel = false;
        [GameParameters.CustomParameterUI("Main body")]
        public bool mainBody = true;
        [GameParameters.CustomParameterUI("Altitude")]
        public bool altitude = true;
        [GameParameters.CustomParameterUI("Speed")]
        public bool speed = true;

        [GameParameters.CustomParameterUI("Event Type")]
        public bool eventType = true;
        [GameParameters.CustomParameterUI("Notes")]
        public bool notes = false;
        [GameParameters.CustomParameterUI("Thumbnail")]
        public bool thumbnail = true;

        public bool screenshot = false;
        public bool lastItem = false;




        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }
    public class KL_5 : GameParameters.CustomParameterNode
    {
        public override string Title { get { return "Filter Limits"; } }
        public override GameParameters.GameMode GameMode { get { return GameParameters.GameMode.ANY; } }
        public override string Section { get { return "Kaptain's Log 2"; } }
        public override string DisplaySection { get { return "Kaptain's Log 2"; } }
        public override int SectionOrder { get { return 2; } }
        public override bool HasPresets { get { return true; } }

        [GameParameters.CustomFloatParameterUI("Upper limit on altitude filter", minValue = 100.0f, maxValue = 500000.0f)]
        public double altitudeFilterMax = 300000f;

        [GameParameters.CustomFloatParameterUI("Upper limit on speed filter", minValue = 100.0f, maxValue = 5000.0f)]
        public double speedFilterMax = 2500;





        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
        }

        public override bool Enabled(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override bool Interactible(MemberInfo member, GameParameters parameters)
        {
            return true;
        }

        public override IList ValidValues(MemberInfo member)
        {
            return null;
        }
    }

}

using System;
using System.Text.RegularExpressions;

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;



namespace KaptainsLogNamespace
{
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
        private static int screenshotCnt = 0;

        public static int GetNextCnt
        {
            get
            {
                screenshotCnt++;
                return screenshotCnt;
            }
        }
    }
}

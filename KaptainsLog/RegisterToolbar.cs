
using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

using KSP.UI.Screens;
using ToolbarControl_NS;
using ClickThroughFix;

namespace KaptainsLogNamespace
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class RegisterToolbar : MonoBehaviour
    {
        void Start()
        {
            ToolbarControl.RegisterMod(KaptainsLog.MODID, KaptainsLog.MODNAME);
        }
    }
}
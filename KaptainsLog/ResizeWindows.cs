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

namespace KaptainsLogNamespace
{
    public partial class KaptainsLog
    {

        // Resize windows in the Update
        /// <summary>
        /// Defines available cursor types
        /// </summary>
        public enum CursorType
        {
            ResizeNS,
            ResizeEW,
            ResizeNSEW,
            Default
        }
        private Vector2 originalMousePosition;
        private Boolean windowLocked = false;
        internal CursorType resizing = CursorType.Default;
        //private int originalWindowHeight, originalWindowWidth;
        Rect originalWindow;
        private readonly int minimumHeight = 300;
        private readonly int minimumWidth = 300;
        private Dictionary<String, Texture> buttonTextures = new Dictionary<String, Texture>();
        private readonly String pluginDir = "KaptainsLog";
        string activeResizeWindow = "";
        bool updated = false;

        void CheckForResize(string winName, ref Rect windowRect)
        {
            if (updated)
                return;
            // Fix reversed y position in mouse coordinates
            Vector3 mousePos = Input.mousePosition;
            mousePos.y = Screen.height - mousePos.y;
            //mousePos.x = Screen.width - mousePos.x;

            /*** Resizing ***/

            //Boolean cursorInVZone = new Rect(windowRect.x, windowRect.yMax - 5, windowRect.width, 5).Contains(mousePos);
            //Boolean cursorInHZone = new Rect(windowRect.xMax - 5, windowRect.y, 5, windowRect.height).Contains(mousePos);

            Boolean cursorInVZone = false;
            Boolean cursorInHZone = false;
            if (mousePos.x >= windowRect.x && mousePos.x <= windowRect.xMax &&
                mousePos.y >= windowRect.yMax - 5 && mousePos.y <= windowRect.yMax)
                cursorInVZone = true;
            if (mousePos.x >= windowRect.xMax - 5 && mousePos.x <= windowRect.xMax &&
                mousePos.y >= windowRect.y && mousePos.y <= windowRect.yMax)
                cursorInHZone = true;
            if (Input.GetMouseButtonDown(0))
            {
                if (!windowLocked && cursorInHZone && cursorInVZone && resizing == CursorType.Default)
                {
                    activeResizeWindow = winName;
                    resizing = CursorType.ResizeNSEW;
                    originalWindow = windowRect;
                    originalMousePosition = Mouse.screenPos;
                    SetCursor(CursorType.ResizeEW);
                }
                else if (!windowLocked && cursorInVZone && resizing == CursorType.Default)
                {
                    activeResizeWindow = winName;
                    resizing = CursorType.ResizeNS;
                    originalWindow = windowRect;
                    originalMousePosition = Mouse.screenPos;
                    SetCursor(CursorType.ResizeNS);
                }
                else if (!windowLocked && cursorInHZone && resizing == CursorType.Default)
                {
                    activeResizeWindow = winName;
                    resizing = CursorType.ResizeEW;
                    originalWindow = windowRect;
                    originalMousePosition = Mouse.screenPos;
                    SetCursor(CursorType.ResizeEW);
                }
            }
            else if (Input.GetMouseButtonUp(0) && resizing != CursorType.Default)
            {
                activeResizeWindow = "";
                resizing = CursorType.Default;
                SetCursor(CursorType.Default);
            }

            if (activeResizeWindow == "")
            {
                if (cursorInHZone && cursorInVZone && !windowLocked) // Set cursor to ResizeNS if we're hovering over the bottom edge of the window
                    SetCursor(CursorType.ResizeNSEW);
                else if (cursorInVZone && !cursorInHZone && !windowLocked) // Set cursor to ResizeNS if we're hovering over the bottom edge of the window
                    SetCursor(CursorType.ResizeNS);
                else if (cursorInHZone && !cursorInVZone && !windowLocked) // Set cursor to ResizeNS if we're hovering over the bottom edge of the window
                    SetCursor(CursorType.ResizeEW);

                else if (!cursorInVZone && !cursorInHZone && resizing == CursorType.Default)
                    SetCursor(CursorType.Default);
            }
            if (activeResizeWindow == winName)
            {

                if ((resizing == CursorType.ResizeNS || resizing == CursorType.ResizeNSEW) && windowRect.height >= minimumHeight)
                    windowRect.height = originalWindow.height - (originalMousePosition.y - Mouse.screenPos.y);

                if (windowRect.height < minimumHeight)
                    windowRect.height = minimumHeight;

                if ((resizing == CursorType.ResizeEW || resizing == CursorType.ResizeNSEW) && windowRect.width >= minimumWidth)
                    windowRect.width = originalWindow.width - (originalMousePosition.x - Mouse.screenPos.x);

                if (windowRect.width < minimumWidth)
                    windowRect.width = minimumWidth;
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(HighLogic.CurrentGame.Parameters.CustomParams<KL_13>().ManualEntryKeycode))
            {
                onManualEntry();
            }
         
            // Following is the Window resize code
            if (buttonTextures.Count() == 0)
            {
                buttonTextures.Add("CursorResizeNS", GameDatabase.Instance.GetTexture(pluginDir + "/Icons/CursorResizeNS", false));
                buttonTextures.Add("CursorResizeEW", GameDatabase.Instance.GetTexture(pluginDir + "/Icons/CursorResizeEW", false));
                buttonTextures.Add("CursorResizeNSEW", GameDatabase.Instance.GetTexture(pluginDir + "/Icons/CursorResizeNSEW", false));
            }

            updated = false;
            if (visibleByToolbar)
                CheckForResize("mainWindow", ref mainWindow);

            //if (displayScreenshot)
            //    CheckForResize("imageWindow", ref iv.imageWindow);

            if (ScreenMessagesLog.Instance != null && ScreenMessagesLog.Instance.visible)
                CheckForResize("ScrnMsgsWindow", ref ScreenMessagesLog.Instance.ScrnMsgsWindow);
        }
        /// <summary>
        /// Sets the cursor texture
        /// </summary>
        /// <param name="type"></param>
        public void SetCursor(CursorType type)
        {
            if (type != CursorType.Default)
                updated = true;
            if (type == CursorType.ResizeNS)
                Cursor.SetCursor((Texture2D)buttonTextures["CursorResizeNS"], new Vector2(11, 11), CursorMode.ForceSoftware);
            else
            if (type == CursorType.ResizeEW)
                Cursor.SetCursor((Texture2D)buttonTextures["CursorResizeEW"], new Vector2(11, 11), CursorMode.ForceSoftware);
            else
            if (type == CursorType.ResizeNSEW)
                Cursor.SetCursor((Texture2D)buttonTextures["CursorResizeNSEW"], new Vector2(11, 11), CursorMode.ForceSoftware);
            else if (type == CursorType.Default)
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}

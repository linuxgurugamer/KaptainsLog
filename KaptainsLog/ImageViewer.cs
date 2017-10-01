
using KSP.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using File = KSP.IO.File;

namespace KaptainsLogNamespace
{
    public class ImageViewer
    {
        private static Texture2D _image;
        private static bool visible = false;
        static bool updateSize = false;
        private static string _imagefile;

        public Rect imageWindow;
        bool resetSize = false;

        public int winId = 0;

        bool everVisible = false; // Not static on purpose

        ~ImageViewer()
        {
            if (_image != null)
                Object.Destroy(_image);
        }

        public ImageViewer(string fname)
        {
            LoadImage(fname);
            
        }

        public static bool IsVisible { get { return visible; } }

        public static Texture2D LoadImage(string fname, int width = 0, int height = 0)
        {
            if (System.IO.File.Exists(fname))
            {
                _imagefile = "file://" + fname;
                
                ImageOrig(width, height);
                visible = true;
                updateSize = false;
                return _image;
            }
            else
            {
                Log.Info("File does not exist");
                return null;
            }
  
        }

        private static void ImageOrig(int width = 0, int height = 0)
        {
            var _imagetex = new WWW(_imagefile);
            _image = _imagetex.texture;
            _imagetex.Dispose();

            if (width == 0)
                width = Screen.width / 2;
            if (height == 0)
                height = Screen.height / 2;

            if (_image.width > Screen.width/2 || _image.height > Screen.height/2)
            {

                float finalRatio = Mathf.Min((float)(Screen.width/2) / (float)_image.width, (float)(Screen.height/2) / (float)_image.height);

                float finalWidth = (float)_image.width * finalRatio;
                float finalHeight = (float)_image.height * finalRatio;

                TextureScale.Bilinear(_image, (int)finalWidth, (int)finalHeight);

            }
            updateSize = true;
        }
        private void ImageZm()
        {
            TextureScale.Bilinear(_image, _image.width - ((_image.width * 10) / 100), _image.height - ((_image.height * 10) / 100));
            updateSize = true;
        }
        private void ImageZp()
        {
            TextureScale.Bilinear(_image, _image.width + ((_image.width * 10) / 100), _image.height + ((_image.height * 10) / 100));
            updateSize = true;
            
        }

        public void OnGUI()
        {
            // Saves the current Gui.skin for later restore
            GUISkin _defGuiSkin = GUI.skin;
            
            {
                if (resetSize)
                {
                    ImageOrig();
                    resetSize = false;
                }
                //GUI.skin = _useKSPskin ? HighLogic.Skin : _defGuiSkin;
                //_windowRect = GUI.Window(GUIUtility.GetControlID(0, FocusType.Passive), _windowRect, IvWindow,
                //    "Image viewer");
                if (updateSize)
                {
                    float x, y;

                    x = imageWindow.x + imageWindow.width / 2;
                    y = imageWindow.y + imageWindow.height / 2;
                    imageWindow = new Rect((Screen.width - _image.width) / 2f, (Screen.height - _image.height) / 2f, _image.width, _image.height + 30);

                    if (!everVisible)
                    {
                        everVisible = true;

                        imageWindow.x = (Screen.width - _image.width) / 2f;
                        imageWindow.y = (Screen.height - _image.height) / 2f;
                    }
                    else
                    {
                        imageWindow.x = x - imageWindow.width / 2;
                        imageWindow.y = y - imageWindow.height / 2;
                    }
                    updateSize = false;
                }
                imageWindow = GUILayout.Window(GUIUtility.GetControlID(0, FocusType.Passive), imageWindow, IvWindow,
                        "Image viewer");
            }

            //Restore the skin
            GUI.skin = _defGuiSkin;

        }
        public void IvWindow(int windowID)
        {
            winId = windowID;
            if (_image != null)
            {
                //_windowRect = new Rect(_windowRect.xMin, _windowRect.yMin, _image.width, _image.height + 20f);
                //GUI.DrawTexture(new Rect(0f, 20f, _image.width, _image.height), _image, ScaleMode.ScaleToFit, true, 0f);
                GUILayout.BeginHorizontal();
                GUILayout.Box(_image);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("-10% size"))
                {
                    ImageZm();
                }
                if (GUILayout.Button("Original size"))
                {
                    ImageOrig();
                }
                if (GUILayout.Button("+10% size"))
                {
                    ImageZp();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Quick Export"))
                {
                    KaptainsLog.Instance.EnableExportWindow(true);
                }
#if false
                if (GUILayout.Button("Export"))
                {
                    KaptainsLog.Instance.EnableExportWindow(true);
                }
#endif
                if (GUILayout.Button("Close"))
                    Toggle();
                GUILayout.EndHorizontal();
                //_windowRect = new Rect(_windowRect.xMin, _windowRect.yMin, _image.width, _image.height + 20f);
                //GUI.DrawTexture(new Rect(0f, 20f, _image.width, _image.height), _image, ScaleMode.ScaleToFit, true, 0f);
            }
            else
            {
                imageWindow = new Rect(Screen.width / 2f, Screen.height / 2f, 100f, 100f);
            }
            if (GUI.Button(new Rect(2f, 2f, 13f, 13f), "X"))
            {
                Toggle();
                
            }
            GUI.DragWindow();
        }

        void Toggle()
        {
            KaptainsLog.Instance.displayScreenshot = false;
            Object.Destroy(_image);
            _image = null;
            visible = false;
        }
    }
}

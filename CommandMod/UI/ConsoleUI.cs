using ModLoader.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SFS.Input.KeybindingsPC;

namespace CommandMod.UI
{
    public class ConsoleUI : MonoBehaviour
    {

        // show in what position is the scroll of console
        private Vector2 _scrollConsole = Vector2.zero;

        // show if the console is visible for user
        private bool _isVisible;
        public bool IsVisible
        {
            set
            {
                this.IsVisible = value;
            }
            get
            {
                return this._isVisible;
            }
        }

        // store all records in the game
        private GUIContent _messages;
        public String Messages
        {
            set
            {
                this._messages.text = value;
                this._scrollConsole.y = this._containerStyle.CalcHeight(this._messages, Screen.width * 0.6f);
            }
        }

        // console styles like background and text color
        private GUIStyle _consoleStyle;
        private GUIStyle _inputStyle;
        private GUIStyle _containerStyle;

        private GUILayoutOption _inputHeigth;

        private string _inputString;

        private void Awake()
        {
            this._isVisible = false;
            this._consoleStyle = new GUIStyle();
            this._inputStyle = new GUIStyle();
            this._containerStyle = new GUIStyle();
            this._messages = new GUIContent();
            this._inputHeigth = GUILayout.Height(30);

            SceneHelper.OnBuildSceneLoaded += this.onBuilder;
            SceneHelper.OnWorldSceneLoaded += this.onWorld;
        }

        private void onBuilder(Scene scene)
        {
            KeySettings.AddOnKeyDown_Build(KeySettings.Main.Toggle_Console, this.toggleConsole);
        }

        private void onWorld(Scene scene)
        {
            KeySettings.AddOnKeyDown_World(KeySettings.Main.Toggle_Console, this.toggleConsole);
        }

        private void toggleConsole()
        {
            this._isVisible = !this._isVisible;
        }

        private void Start()
        {
            this._containerStyle.normal.background = this.makeTexture(new Color32(0, 0, 0, 150));
            this._containerStyle.margin.bottom = 5;

            this._consoleStyle.normal.textColor = Color.white;
            this._consoleStyle.fontSize = 16;
            this._consoleStyle.padding.left = 10;



            this._inputStyle.normal.textColor = Color.white;
            this._inputStyle.fontSize = 16;
            this._inputStyle.padding = new RectOffset(5, 5, 5, 5);
            this._inputStyle.normal.background = this.makeTexture(new Color32(55, 55, 55, 180));

        }

        /// <sumary>
        /// Create solid color texture
        ///</sumary>
        private Texture2D makeTexture(Color col)
        {
            Color[] pix = new Color[1];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(1, 1);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void OnGUI()
        {
            if (this._isVisible)
            {
                float consoleHeigth = Screen.height * 0.4f;

                GUI.BeginGroup(new Rect(0, Screen.height - consoleHeigth - 40, Screen.width * 0.6f, consoleHeigth + 35));
                this._scrollConsole = GUILayout.BeginScrollView(this._scrollConsole, this._containerStyle, GUILayout.Width(Screen.width * 0.6f), GUILayout.Height(consoleHeigth));
                GUILayout.Box(this._messages, this._consoleStyle);
                GUILayout.EndScrollView();
                GUI.SetNextControlName("command");
                this._inputString = GUILayout.TextField(this._inputString, this._inputStyle, this._inputHeigth);
                GUI.FocusControl("command");
                GUI.EndGroup();

                // process return and escape event
                Event e = Event.current;
                if (e.isKey)
                {
                    if (e.keyCode == KeyCode.Return && this._inputString.Length > 0)
                    {
                        CommandMod.Main.processInput(this._inputString);
                        this._inputString = "";
                        return;
                    }

                    if(e.keyCode == KeyCode.Escape)
                    {
                        this._isVisible = false;
                    }
                }
            }
        }

   

    }
}

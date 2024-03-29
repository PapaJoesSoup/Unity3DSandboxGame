using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
  public class KeyMap : MonoBehaviour
  {
    public static KeyMap Instance;
    public static Dictionary<string, KeyCode> KeycodeCache = new Dictionary<string, KeyCode>();

    internal List<KeyCode> KbdModCodes;
    internal List<KeyCode> KbdCodes;
    internal List<KeyCode> MouseCodes;
    internal List<KeyCode> JoyStickCodes;
    internal EditState EditingState = EditState.None;
    internal bool IsEditing = false;

    public Dictionary<string, KeyInput> KeyList = new();

    internal string Fire = "Fire";
    internal string Weapon1 = "Weapon1";
    internal string Weapon2 = "Weapon2";
    internal string Weapon3 = "Weapon3";
    internal string Weapon4 = "Weapon4";
    internal string Weapon5 = "Weapon5";
    internal string Jump = "Jump";
    internal string Forward = "Forward";
    internal string Backward = "Backward";
    internal string Left = "Left";
    internal string Right = "Right";
    internal string Up = "Up";
    internal string Down = "Down";
    internal string Crouch = "Crouch";
    internal string ModeNormal = "ModeNormal";
    internal string ModeBounce = "ModeBounce";
    internal string ModeFlying = "ModeFlying";
    internal string CameraUp = "CameraUp";
    internal string CameraDown = "CameraDown";
    internal string CameraLeft = "CameraLeft";
    internal string CameraRight = "CameraRight";
    internal string CameraCenter = "CameraCenter";
    internal string OverheadCamera = "OverheadCamera";
    internal string SwitchCamera = "SwitchCamera";
    internal string CameraRotate = "CameraRotate";
    internal string CameraPan = "CameraPan";
    internal string BoxSummon = "BoxSummon";
    internal string BoxAutoSummon = "BoxAutoSummon";
    internal string BoxAutoAlign = "BoxAutoAlign";
    internal string BoxReset = "BoxReset";
    internal string KeyMapUI = "KeyMapUI";

    void Awake()
    {
      if (Instance != null) GameObject.Destroy(this);
      else
      {
        Instance = this;
        FillLists();
        InitializeKeyInputs();
        FillKeyCodeLookup();
      }
    }

    private void Update()
    {
    }

    private void FillLists()
    {
      KbdModCodes = new List<KeyCode>
      {
        KeyCode.None,
        KeyCode.LeftShift,
        KeyCode.RightShift,
        KeyCode.LeftControl,
        KeyCode.RightControl,
        KeyCode.LeftAlt,
        KeyCode.RightAlt,
        KeyCode.LeftCommand,
        KeyCode.RightCommand,
        KeyCode.LeftApple,
        KeyCode.RightApple
      };

      KbdCodes = new List<KeyCode>
      {
        KeyCode.None,
        KeyCode.Backspace,
        KeyCode.Delete,
        KeyCode.Tab,
        KeyCode.Clear,
        KeyCode.Return,
        KeyCode.Pause,
        KeyCode.Escape,
        KeyCode.Space,
        KeyCode.Keypad0,
        KeyCode.Keypad1,
        KeyCode.Keypad2,
        KeyCode.Keypad3,
        KeyCode.Keypad4,
        KeyCode.Keypad5,
        KeyCode.Keypad6,
        KeyCode.Keypad7,
        KeyCode.Keypad8,
        KeyCode.Keypad9,
        KeyCode.KeypadPeriod,
        KeyCode.KeypadDivide,
        KeyCode.KeypadMultiply,
        KeyCode.KeypadMinus,
        KeyCode.KeypadPlus,
        KeyCode.KeypadEnter,
        KeyCode.KeypadEquals,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.Insert,
        KeyCode.Home,
        KeyCode.End,
        KeyCode.PageUp,
        KeyCode.PageDown,
        KeyCode.F1,
        KeyCode.F2,
        KeyCode.F3,
        KeyCode.F4,
        KeyCode.F5,
        KeyCode.F6,
        KeyCode.F7,
        KeyCode.F8,
        KeyCode.F9,
        KeyCode.F10,
        KeyCode.F11,
        KeyCode.F12,
        KeyCode.F13,
        KeyCode.F14,
        KeyCode.F15,
        KeyCode.Alpha0,
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9,
        KeyCode.Exclaim,
        KeyCode.DoubleQuote,
        KeyCode.Hash,
        KeyCode.Dollar,
        KeyCode.Percent,
        KeyCode.Ampersand,
        KeyCode.Quote,
        KeyCode.LeftParen,
        KeyCode.RightParen,
        KeyCode.Asterisk,
        KeyCode.Plus,
        KeyCode.Comma,
        KeyCode.Minus,
        KeyCode.Period,
        KeyCode.Slash,
        KeyCode.Colon,
        KeyCode.Semicolon,
        KeyCode.Less,
        KeyCode.Equals,
        KeyCode.Greater,
        KeyCode.Question,
        KeyCode.At,
        KeyCode.LeftBracket,
        KeyCode.Backslash,
        KeyCode.RightBracket,
        KeyCode.Caret,
        KeyCode.Underscore,
        KeyCode.BackQuote,
        KeyCode.A,
        KeyCode.B,
        KeyCode.C,
        KeyCode.D,
        KeyCode.E,
        KeyCode.F,
        KeyCode.G,
        KeyCode.H,
        KeyCode.I,
        KeyCode.J,
        KeyCode.K,
        KeyCode.L,
        KeyCode.M,
        KeyCode.N,
        KeyCode.O,
        KeyCode.P,
        KeyCode.Q,
        KeyCode.R,
        KeyCode.S,
        KeyCode.T,
        KeyCode.U,
        KeyCode.V,
        KeyCode.W,
        KeyCode.X,
        KeyCode.Y,
        KeyCode.Z,
        KeyCode.LeftCurlyBracket,
        KeyCode.Pipe,
        KeyCode.RightCurlyBracket,
        KeyCode.Tilde,
        KeyCode.Numlock,
        KeyCode.CapsLock,
        KeyCode.ScrollLock,
        KeyCode.LeftWindows,
        KeyCode.RightWindows,
        KeyCode.AltGr,
        KeyCode.Help,
        KeyCode.Print,
        KeyCode.SysReq,
        KeyCode.Break,
        KeyCode.Menu,
      };

      MouseCodes = new List<KeyCode>
      {
        KeyCode.None,
        KeyCode.Mouse0,
        KeyCode.Mouse1,
        KeyCode.Mouse2,
        KeyCode.Mouse3,
        KeyCode.Mouse4,
        KeyCode.Mouse5,
        KeyCode.Mouse6
      };

      JoyStickCodes = new List<KeyCode>
      {
        KeyCode.None,
        KeyCode.JoystickButton0,
        KeyCode.JoystickButton1,
        KeyCode.JoystickButton2,
        KeyCode.JoystickButton3,
        KeyCode.JoystickButton4,
        KeyCode.JoystickButton5,
        KeyCode.JoystickButton6,
        KeyCode.JoystickButton7,
        KeyCode.JoystickButton8,
        KeyCode.JoystickButton9,
        KeyCode.JoystickButton10,
        KeyCode.JoystickButton11,
        KeyCode.JoystickButton12,
        KeyCode.JoystickButton13,
        KeyCode.JoystickButton14,
        KeyCode.JoystickButton15,
        KeyCode.JoystickButton16,
        KeyCode.JoystickButton17,
        KeyCode.JoystickButton18,
        KeyCode.JoystickButton19,
        KeyCode.Joystick1Button0,
        KeyCode.Joystick1Button1,
        KeyCode.Joystick1Button2,
        KeyCode.Joystick1Button3,
        KeyCode.Joystick1Button4,
        KeyCode.Joystick1Button5,
        KeyCode.Joystick1Button6,
        KeyCode.Joystick1Button7,
        KeyCode.Joystick1Button8,
        KeyCode.Joystick1Button9,
        KeyCode.Joystick1Button10,
        KeyCode.Joystick1Button11,
        KeyCode.Joystick1Button12,
        KeyCode.Joystick1Button13,
        KeyCode.Joystick1Button14,
        KeyCode.Joystick1Button15,
        KeyCode.Joystick1Button16,
        KeyCode.Joystick1Button17,
        KeyCode.Joystick1Button18,
        KeyCode.Joystick1Button19,
        KeyCode.Joystick2Button0,
        KeyCode.Joystick2Button1,
        KeyCode.Joystick2Button2,
        KeyCode.Joystick2Button3,
        KeyCode.Joystick2Button4,
        KeyCode.Joystick2Button5,
        KeyCode.Joystick2Button6,
        KeyCode.Joystick2Button7,
        KeyCode.Joystick2Button8,
        KeyCode.Joystick2Button9,
        KeyCode.Joystick2Button10,
        KeyCode.Joystick2Button11,
        KeyCode.Joystick2Button12,
        KeyCode.Joystick2Button13,
        KeyCode.Joystick2Button14,
        KeyCode.Joystick2Button15,
        KeyCode.Joystick2Button16,
        KeyCode.Joystick2Button17,
        KeyCode.Joystick2Button18,
        KeyCode.Joystick2Button19,
        KeyCode.Joystick3Button0,
        KeyCode.Joystick3Button1,
        KeyCode.Joystick3Button2,
        KeyCode.Joystick3Button3,
        KeyCode.Joystick3Button4,
        KeyCode.Joystick3Button5,
        KeyCode.Joystick3Button6,
        KeyCode.Joystick3Button7,
        KeyCode.Joystick3Button8,
        KeyCode.Joystick3Button9,
        KeyCode.Joystick3Button10,
        KeyCode.Joystick3Button11,
        KeyCode.Joystick3Button12,
        KeyCode.Joystick3Button13,
        KeyCode.Joystick3Button14,
        KeyCode.Joystick3Button15,
        KeyCode.Joystick3Button16,
        KeyCode.Joystick3Button17,
        KeyCode.Joystick3Button18,
        KeyCode.Joystick3Button19,
        KeyCode.Joystick4Button0,
        KeyCode.Joystick4Button1,
        KeyCode.Joystick4Button2,
        KeyCode.Joystick4Button3,
        KeyCode.Joystick4Button4,
        KeyCode.Joystick4Button5,
        KeyCode.Joystick4Button6,
        KeyCode.Joystick4Button7,
        KeyCode.Joystick4Button8,
        KeyCode.Joystick4Button9,
        KeyCode.Joystick4Button10,
        KeyCode.Joystick4Button11,
        KeyCode.Joystick4Button12,
        KeyCode.Joystick4Button13,
        KeyCode.Joystick4Button14,
        KeyCode.Joystick4Button15,
        KeyCode.Joystick4Button16,
        KeyCode.Joystick4Button17,
        KeyCode.Joystick4Button18,
        KeyCode.Joystick4Button19,
        KeyCode.Joystick5Button0,
        KeyCode.Joystick5Button1,
        KeyCode.Joystick5Button2,
        KeyCode.Joystick5Button3,
        KeyCode.Joystick5Button4,
        KeyCode.Joystick5Button5,
        KeyCode.Joystick5Button6,
        KeyCode.Joystick5Button7,
        KeyCode.Joystick5Button8,
        KeyCode.Joystick5Button9,
        KeyCode.Joystick5Button10,
        KeyCode.Joystick5Button11,
        KeyCode.Joystick5Button12,
        KeyCode.Joystick5Button13,
        KeyCode.Joystick5Button14,
        KeyCode.Joystick5Button15,
        KeyCode.Joystick5Button16,
        KeyCode.Joystick5Button17,
        KeyCode.Joystick5Button18,
        KeyCode.Joystick5Button19,
        KeyCode.Joystick6Button0,
        KeyCode.Joystick6Button1,
        KeyCode.Joystick6Button2,
        KeyCode.Joystick6Button3,
        KeyCode.Joystick6Button4,
        KeyCode.Joystick6Button5,
        KeyCode.Joystick6Button6,
        KeyCode.Joystick6Button7,
        KeyCode.Joystick6Button8,
        KeyCode.Joystick6Button9,
        KeyCode.Joystick6Button10,
        KeyCode.Joystick6Button11,
        KeyCode.Joystick6Button12,
        KeyCode.Joystick6Button13,
        KeyCode.Joystick6Button14,
        KeyCode.Joystick6Button15,
        KeyCode.Joystick6Button16,
        KeyCode.Joystick6Button17,
        KeyCode.Joystick6Button18,
        KeyCode.Joystick6Button19,
        KeyCode.Joystick7Button0,
        KeyCode.Joystick7Button1,
        KeyCode.Joystick7Button2,
        KeyCode.Joystick7Button3,
        KeyCode.Joystick7Button4,
        KeyCode.Joystick7Button5,
        KeyCode.Joystick7Button6,
        KeyCode.Joystick7Button7,
        KeyCode.Joystick7Button8,
        KeyCode.Joystick7Button9,
        KeyCode.Joystick7Button10,
        KeyCode.Joystick7Button11,
        KeyCode.Joystick7Button12,
        KeyCode.Joystick7Button13,
        KeyCode.Joystick7Button14,
        KeyCode.Joystick7Button15,
        KeyCode.Joystick7Button16,
        KeyCode.Joystick7Button17,
        KeyCode.Joystick7Button18,
        KeyCode.Joystick7Button19,
        KeyCode.Joystick8Button0,
        KeyCode.Joystick8Button1,
        KeyCode.Joystick8Button2,
        KeyCode.Joystick8Button3,
        KeyCode.Joystick8Button4,
        KeyCode.Joystick8Button5,
        KeyCode.Joystick8Button6,
        KeyCode.Joystick8Button7,
        KeyCode.Joystick8Button8,
        KeyCode.Joystick8Button9,
        KeyCode.Joystick8Button10,
        KeyCode.Joystick8Button11,
        KeyCode.Joystick8Button12,
        KeyCode.Joystick8Button13,
        KeyCode.Joystick8Button14,
        KeyCode.Joystick8Button15,
        KeyCode.Joystick8Button16,
        KeyCode.Joystick8Button17,
        KeyCode.Joystick8Button18,
        KeyCode.Joystick8Button19
      };
    }

    private void InitializeKeyInputs()
    {

    KeyList.Add(Fire, new KeyInput(Fire, KeyCode.None, KeyCode.Mouse0));
    KeyList.Add(Weapon1, new KeyInput(Weapon1, KeyCode.None, KeyCode.Alpha1));
    KeyList.Add(Weapon2, new KeyInput(Weapon2, KeyCode.None, KeyCode.Alpha2));
    KeyList.Add(Weapon3, new KeyInput(Weapon3, KeyCode.None, KeyCode.Alpha3));
    KeyList.Add(Weapon4, new KeyInput(Weapon4, KeyCode.None, KeyCode.Alpha4));
    KeyList.Add(Weapon5, new KeyInput(Weapon5, KeyCode.None, KeyCode.Alpha5));


    KeyList.Add(Jump, new KeyInput(Jump, KeyCode.None, KeyCode.Space));
    KeyList.Add(Forward, new KeyInput(Forward, KeyCode.None, KeyCode.W));
    KeyList.Add(Backward, new KeyInput(Backward, KeyCode.None, KeyCode.S));
    KeyList.Add(Left, new KeyInput(Left, KeyCode.None, KeyCode.A));
    KeyList.Add(Right, new KeyInput(Right, KeyCode.None, KeyCode.D));
    KeyList.Add(Up, new KeyInput(Up, KeyCode.None, KeyCode.LeftShift));
    KeyList.Add(Down, new KeyInput(Down, KeyCode.None, KeyCode.LeftControl));
    KeyList.Add(Crouch, new KeyInput(Crouch, KeyCode.None, KeyCode.C));

    KeyList.Add(ModeNormal, new KeyInput(ModeNormal, KeyCode.None, KeyCode.F1));
    KeyList.Add(ModeBounce, new KeyInput(ModeBounce, KeyCode.None, KeyCode.F2));
    KeyList.Add(ModeFlying, new KeyInput(ModeFlying, KeyCode.None, KeyCode.F3));

    KeyList.Add(CameraUp, new KeyInput(CameraUp, KeyCode.None, KeyCode.Keypad8));
    KeyList.Add(CameraDown, new KeyInput(CameraDown, KeyCode.None, KeyCode.Keypad7));
    KeyList.Add(CameraLeft, new KeyInput(CameraLeft, KeyCode.None, KeyCode.Keypad4));
    KeyList.Add(CameraRight, new KeyInput(CameraRight, KeyCode.None, KeyCode.Keypad6));
    KeyList.Add(CameraCenter, new KeyInput(CameraCenter, KeyCode.None, KeyCode.Keypad5));
    KeyList.Add(OverheadCamera, new KeyInput(OverheadCamera, KeyCode.None, KeyCode.O));
    KeyList.Add(SwitchCamera, new KeyInput(SwitchCamera, KeyCode.None, KeyCode.Z));
    KeyList.Add(CameraRotate, new KeyInput(CameraRotate, KeyCode.None, KeyCode.Mouse1));
    KeyList.Add(CameraPan, new KeyInput(CameraPan, KeyCode.None, KeyCode.Mouse2));

    KeyList.Add(BoxSummon, new KeyInput(BoxSummon, KeyCode.None, KeyCode.B));
    KeyList.Add(BoxAutoSummon, new KeyInput(BoxAutoSummon, KeyCode.None, KeyCode.F4));
    KeyList.Add(BoxAutoAlign, new KeyInput(BoxAutoAlign, KeyCode.None, KeyCode.F5));
    KeyList.Add(BoxReset, new KeyInput(BoxReset, KeyCode.None, KeyCode.Backslash));
    KeyList.Add(KeyMapUI, new KeyInput(KeyMapUI, KeyCode.LeftShift, KeyCode.K));
  }

    internal static KeyCode GetKeyCode(string codeName)
    {
      // Get from cache to prevent unnecessary enum parse
      return KeycodeCache[codeName];
    }

    private void FillKeyCodeLookup()
    {
      //KeycodeCache.Clear();
      foreach (KeyCode code in Enum.GetValues(typeof(KeyCode)))
      {
        try
        {
          KeycodeCache.Add(code.ToString(), code);
        }
        catch { } // Ignore.  there are duplicate string names with different int values in KeyCodes...
      }
    }

  internal enum EditState
    {
      None,
      Key1,
      Key2
    }
  }

  [Serializable]
  public class KeyInput
  {
    // This class assumes Use in Update or OnGui
    public string Action;
    public KeyCode Modifier1;
    public KeyCode KeyPress1;
    public KeyCode Modifier2;
    public KeyCode KeyPress2;

    public KeyInput(string action, KeyCode modifier1 = KeyCode.None, KeyCode keyPress1 = KeyCode.None, KeyCode modifier2 = KeyCode.None, KeyCode keyPress2 = KeyCode.None)
    {
      Action = action;
      Modifier1 = modifier1; KeyPress1 = keyPress1; 
      Modifier2 = modifier2; KeyPress2 = keyPress2;
    }

    /// <summary>
    /// IsKeyDown returns true if the key combination press is detected.  (Same as GetKeyDown)
    /// </summary>
    /// <returns></returns>
    public bool IsKeyDown()
    {
      if (!Input.GetKeyDown(KeyPress1) && !Input.GetKeyDown(KeyPress2)) return false;
      if (Modifier1 == KeyCode.None && Modifier2 == KeyCode.None) return true;
      return (Input.GetKey(Modifier1) && Input.GetKeyDown(KeyPress1)) || (Input.GetKey(Modifier2) && Input.GetKeyDown(KeyPress2));
    }

    /// <summary>
    /// IsKey returns true if the key combination is currently being pressed. (Same as GetKey)
    /// </summary>
    /// <returns></returns>
    public bool IsKey()
    {
      if (!Input.GetKey(KeyPress1) && !Input.GetKey(KeyPress2)) return false;
      if (Modifier1 == KeyCode.None && Modifier2 == KeyCode.None) return true;
      return (Input.GetKey(Modifier1) && Input.GetKey(KeyPress1)) || (Input.GetKey(Modifier2) && Input.GetKey(KeyPress2));
    }

  }
}

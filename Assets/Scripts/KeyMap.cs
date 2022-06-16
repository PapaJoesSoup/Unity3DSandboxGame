using UnityEngine;

namespace Assets.Scripts
{
  public class KeyMap : MonoBehaviour
  {
    public static KeyMap Instance;

    public static KeyCode Fire = KeyCode.Mouse0;
    public static KeyCode Weapon1 = KeyCode.Alpha1;
    public static KeyCode Weapon2 = KeyCode.Alpha2;
    public static KeyCode Weapon3 = KeyCode.Alpha3;
    public static KeyCode Weapon4 = KeyCode.Alpha4;
    public static KeyCode Weapon5 = KeyCode.Alpha5;

    public static KeyCode Jump = KeyCode.Space;
    public static KeyCode Forward = KeyCode.W;
    public static KeyCode Backward = KeyCode.S;
    public static KeyCode Up = KeyCode.LeftShift;
    public static KeyCode Down = KeyCode.LeftControl;
    public static KeyCode Left = KeyCode.A;
    public static KeyCode Right = KeyCode.D;

    public static KeyCode Crouch = KeyCode.C;

    public static KeyCode CameraUp = KeyCode.Keypad8;
    public static KeyCode CameraDown = KeyCode.Keypad7;
    public static KeyCode CameraLeft = KeyCode.Keypad4;
    public static KeyCode CameraRight = KeyCode.Keypad6;
    public static KeyCode CameraCenter = KeyCode.Keypad5;
    public static KeyCode SwitchCamera = KeyCode.Z;

    void Awake()
    {
      if (Instance != null) return;
      Instance = this;
    }
  }
}

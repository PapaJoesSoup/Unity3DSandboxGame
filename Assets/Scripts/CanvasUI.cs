using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
  public class CanvasUI : MonoBehaviour
  {
    internal static bool UiActive = false;
    private GameObject _keyMapper;
    private KeyMap _map;

    private void Awake()
    {
      _keyMapper = GameObject.Find("KeyMapper");
    }
    
    // Start is called before the first frame update
    private void Start()
    {
      _map = KeyMap.Instance;
      _keyMapper.SetActive(false);
      UiActive = false;
      SetCursorLock(true);
    }

    // Update is called once per frame
    private void Update()
    {
      if (!_map.KeyList[_map.KeyMapUI].IsKeyDown() || _keyMapper.activeInHierarchy) return;
      _keyMapper.SetActive(true);
      UiActive = true;
      SetCursorLock(false);
    }

    internal void OnCloseClick()
    {
      SetCursorLock(true);
      _keyMapper.SetActive(false);
    }

    internal static void SetCursorLock(bool enable)
    {
      if (enable)
      {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      }
      else
      {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

      }
    }
  }
}

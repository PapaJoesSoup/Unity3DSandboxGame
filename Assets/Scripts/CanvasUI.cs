using UnityEngine;

namespace Assets.Scripts
{
  public class CanvasUI : MonoBehaviour
  {
    internal static bool UiActive = false;
    private GameObject keyMapper;
    private KeyMap map;

    private void Awake()
    {
      keyMapper = GameObject.Find("KeyMapper");
    }
    
    // Start is called before the first frame update
    private void Start()
    {
      map = KeyMap.Instance;
      keyMapper.SetActive(false);
      UiActive = false;
      SetCursorLock(true);
    }

    // Update is called once per frame
    private void Update()
    {
      if (!map.KeyList[map.KeyMapUI].IsKeyDown() || keyMapper.activeInHierarchy) return;
      keyMapper.SetActive(true);
      UiActive = true;
      SetCursorLock(false);
    }

    internal void OnCloseClick()
    {
      SetCursorLock(true);
      keyMapper.SetActive(false);
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

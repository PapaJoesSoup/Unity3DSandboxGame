using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class KeyMapperUI : MonoBehaviour
  {
    private ScrollRect _scrollR;
    private Button _closeButton;

    private void Awake()
    {
      _closeButton = transform.Find("CloseButton").GetComponent<Button>();
      _scrollR = GetComponent<ScrollRect>();
    }
    // Start is called before the first frame update
    void Start()
    {
      _scrollR.verticalNormalizedPosition = 1f;
    }

    // Update is called once per frame
    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape)) OnClose();
    }

    private void OnClose()
    {
      CanvasUI.UiActive = false;
      CanvasUI.SetCursorLock(true);
      gameObject.SetActive(false);
    }

    private void OnEnable()
    {
      _closeButton.onClick.AddListener(OnClose);
    }

    private void OnDisable()
    {
      _closeButton.onClick.RemoveListener(OnClose);
    }
  }
}

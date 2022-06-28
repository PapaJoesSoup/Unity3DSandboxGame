using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class InputKeyItem : MonoBehaviour
  {
    internal KeyInput InputKey;

    private TMP_Text _actionLabel;
    private TMP_Dropdown _mod1Select;
    private TMP_Dropdown _key1Select;
    private TMP_Dropdown _mod2Select;
    private TMP_Dropdown _key2Select;
    private Button _editButton;
    private Button _saveButton;
    private Button _cancelButton;

    private void Awake()
    {
      _actionLabel = transform.Find("ActionLabel").gameObject.GetComponent<TMP_Text>();
      _mod1Select = transform.Find("Mod1Select").gameObject.GetComponent<TMP_Dropdown>();
      _key1Select = transform.Find("Key1Select").gameObject.GetComponent<TMP_Dropdown>();
      _mod2Select = transform.Find("Mod2Select").gameObject.GetComponent<TMP_Dropdown>();
      _key2Select = transform.Find("Key2Select").gameObject.GetComponent<TMP_Dropdown>();
      _editButton = transform.Find("EditButton").gameObject.GetComponent<Button>();
      _saveButton = transform.Find("SaveButton").gameObject.GetComponent<Button>();
      _cancelButton = transform.Find("CancelButton").gameObject.GetComponent<Button>();
    }

    // Start is called before the first frame update
    private void Start()
    {
      SetDropDownOptions();
      SetItemValues();
      SetItemEditActive(false, false);
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void SetDropDownOptions()
    {
      //Modifier 1 and Modifier 2
      _mod1Select.options.Clear();
      _mod2Select.options.Clear();
      foreach (KeyCode key in KeyMap.Instance.KbdModCodes)
      {
        TMP_Dropdown.OptionData keyOption = new()
        {
          text = key.ToString()
        };
        _mod1Select.options.Add(keyOption);
        _mod2Select.options.Add(keyOption);
      }

      //Key 1 and Key 2
      _key1Select.options.Clear();
      _key2Select.options.Clear();
      foreach (KeyCode key in KeyMap.Instance.KbdCodes)
      {
        TMP_Dropdown.OptionData keyOption = new()
        {
          text = key.ToString()
        };
        _key1Select.options.Add(keyOption);
        _key2Select.options.Add(keyOption);
      }
      
      // Mouse codes
      foreach (KeyCode key in KeyMap.Instance.MouseCodes)
      {
        TMP_Dropdown.OptionData keyOption = new()
        {
          text = key.ToString()
        };
        _key1Select.options.Add(keyOption);
        _key2Select.options.Add(keyOption);
      }
    }

    private void SetItemValues()
    {
      _actionLabel.GetComponent<TMP_Text>().text = InputKey.Action;
      _mod1Select.value = _mod1Select.options.FindIndex(option => option.text == InputKey.Modifier1.ToString());
      _mod2Select.value = _mod2Select.options.FindIndex(option => option.text == InputKey.Modifier2.ToString());
      _key1Select.value = _key1Select.options.FindIndex(option => option.text == InputKey.KeyPress1.ToString());
      _key2Select.value = _key2Select.options.FindIndex(option => option.text == InputKey.KeyPress2.ToString());

      _mod1Select.RefreshShownValue();
      _mod2Select.RefreshShownValue();
      _key1Select.RefreshShownValue();
      _key2Select.RefreshShownValue();

    }

    private void OnEditButtonClick()
    {
      SetItemEditActive(_editButton.gameObject.activeInHierarchy);
    }

    private void OnCancelButtonClick()
    {
      SetItemEditActive(_editButton.gameObject.activeInHierarchy, false);
    }

    private void SetItemEditActive(bool enable, bool save = true)
    {
      if (!enable && save) SaveItemChanges();
      _mod1Select.enabled = _mod2Select.enabled = _key1Select.enabled = _key2Select.enabled = enable;
      _editButton.gameObject.SetActive(!enable);
      _saveButton.gameObject.SetActive(enable);
      _cancelButton.gameObject.SetActive(enable);
    }

    private void SaveItemChanges()
    {
      if (InputKey.Modifier1.ToString() != _mod1Select.options[_mod1Select.value].text)
        InputKey.Modifier1 = KeyMap.GetKeyCode(_mod1Select.options[_mod1Select.value].text);
      if (InputKey.Modifier2.ToString() != _mod2Select.options[_mod2Select.value].text)
        InputKey.Modifier2 = KeyMap.GetKeyCode(_mod2Select.options[_mod2Select.value].text);
      if (InputKey.KeyPress1.ToString() != _key1Select.options[_key1Select.value].text)
        InputKey.KeyPress1 = KeyMap.GetKeyCode(_key1Select.options[_key1Select.value].text);
      if (InputKey.KeyPress2.ToString() != _key2Select.options[_key2Select.value].text)
        InputKey.KeyPress2 = KeyMap.GetKeyCode(_key2Select.options[_key2Select.value].text);

      if (!InputKey.Equals(KeyMap.Instance.KeyList[InputKey.Action]))
      {
        KeyMap.Instance.KeyList[InputKey.Action] = InputKey;
      }
    }

    private void OnEnable()
    {
      _editButton.onClick.AddListener(OnEditButtonClick);
      _saveButton.onClick.AddListener(OnEditButtonClick);
      _cancelButton.onClick.AddListener(OnCancelButtonClick);
    }

    private void OnDisable()
    {
      _editButton.onClick.RemoveListener(OnEditButtonClick);
      _saveButton.onClick.RemoveListener(OnEditButtonClick);
      _cancelButton.onClick.RemoveListener(OnCancelButtonClick);
    }
  }
}

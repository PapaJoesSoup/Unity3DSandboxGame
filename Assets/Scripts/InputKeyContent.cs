using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
  public class InputKeyContent : MonoBehaviour
  {
    public GameObject KeyItemPrefab; 
    private Dictionary<string,KeyInput> m_Keys;
    private GameObject _content;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
      m_Keys = KeyMap.Instance.KeyList;
      foreach (KeyValuePair<string, KeyInput> key in m_Keys)
      {
        GameObject keyItem = Instantiate(KeyItemPrefab, this.transform);
        keyItem.GetComponent<InputKeyItem>().InputKey = key.Value;
      }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  }
}

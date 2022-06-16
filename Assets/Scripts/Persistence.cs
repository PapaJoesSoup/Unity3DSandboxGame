using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
  public class Persistence : MonoBehaviour
  {

    private static StringBuilder sb = new StringBuilder();

    // Start is called before the first frame update
    void Start()
    {
      GetSettings();
    }

    public static void Save()
    {
      BuildSettingsFile();
      // serialize JSON directly to a file
      using StreamWriter file = File.CreateText(Application.persistentDataPath + "PlayerSettings.txt");
      file.Write(sb.ToString());
    }

    public static void Load()
    {
      JObject settingsFile = JObject.Parse(File.ReadAllText(Application.persistentDataPath + "PlayerSettings.txt"));
      if (settingsFile.Count == 0) return;
      // pull data from settings file using jsonConvert
      //...

    }

    private void GetSettings()
    {

      PlayerSettings.Downgrades = new List<string>();
      PlayerSettings.Downgrades = new List<string>();
      PlayerSettings.Downgrades = new List<string>();
      PlayerSettings.Downgrades = new List<string>();

    }

    private static void BuildSettingsFile()
    {
      sb.Clear();
      sb.AppendLine(JsonConvert.SerializeObject(PlayerSettings.Inventory));
      sb.AppendLine(JsonConvert.SerializeObject(PlayerSettings.Quests));
      sb.AppendLine(JsonConvert.SerializeObject(PlayerSettings.Upgrades));
      sb.AppendLine(JsonConvert.SerializeObject(PlayerSettings.Downgrades));
      sb.AppendLine(JsonConvert.SerializeObject(KeyMap.Instance));
      print(sb.ToString());
    }
  }

  public class PlayerSettings
  {
    public static List<string> Inventory = new List<string>();
    public static List<string> Quests = new List<string>();
    public static List<string> Upgrades = new List<string>();
    public static List<string> Downgrades = new List<string>();
  }
}
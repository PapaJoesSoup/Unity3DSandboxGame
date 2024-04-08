using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
  /// <summary>
  /// Legacy Object pool class.  Unity now includes an ObjectPool scheme of its own.
  /// </summary>
  public class ObjectPool : MonoBehaviour
  {

    #region Properties
    // this makes the class a singleton
    public static ObjectPool Instance;

    [Header("Object Pool Settings")]
    // Unity cannot show dictionaries in the Inspector, so we will create a PrefabItem list
    // to populate the dictionary.
    public List<PrefabItem> PrefabItemList = new();

    // This is where we store all of our pooled prefab objects.
    // This is essentially a list of object lists organized by PoolType 
    internal Dictionary<PoolType, List<GameObject>> Pools = new();
    #endregion Properties

    // Awake is called when game loads
    private void Awake()
    {
      // Load prefab object data from the inspector (PrefabItemList) and populate the Pools dictionary.
      LoadObjectPool();

      // load the singleton instance.
      if (Instance == null) Instance = this;
    }

    private void LoadObjectPool()
    {
      // Fill the Object Pools
      foreach (PrefabItem prefab in PrefabItemList)
      {
        PoolType poolType = prefab.Type;
        GameObject prefabObject = prefab.PrefabObject;
        List<GameObject> pool = new List<GameObject>();
        GameObject parent = new GameObject();
        parent.name = prefab.Type.ToString();
        parent.transform.parent = gameObject.transform;
        for (int i = 0; i < prefab.Quantity; i++)
        {
          GameObject obj = Instantiate(prefabObject, parent.transform);
          obj.SetActive(false);
          pool.Add(obj);
        }
        Pools.Add(poolType, pool);
      }
    }

    // Method to retrieve an available object from the pool
    public GameObject GetPooledObject(PoolType type)
    {
      for (int i = 0; i < Pools[type].Count; i++)
      {
        if (Pools[type][i].activeInHierarchy) continue;
        return Pools[type][i];
      }
      // If no available object is found, return null
      return null;
    }

    public GameObject GetPooledObject(PoolType type, bool addNew)
    {
      GameObject result = GetPooledObject(type);
      if (result != null || !addNew) return result;
      // if the result is null and addNew is true, add a new object to the pool and update the dictionary
      PrefabItem prefabitem = GetPrefabItem(type);
      List<GameObject> pool = Pools[type];
      GameObject parent = GameObject.Find(type.ToString());
      result = Instantiate(prefabitem.PrefabObject, parent.transform);
      result.SetActive(false);
      pool.Add(result);
      Pools[type] = pool;

      return result;
    }

    public PrefabItem GetPrefabItem(PoolType type)
    {
      foreach(PrefabItem item in PrefabItemList)
      {
        if (item.Type == type) return item;
      }
      return null;
    }

    [Serializable]
    public enum PoolType
    {
      Laser,
      Bolt,
      Bullet,
      Splash,
      Enemy,
      BulletHole,
      BoltBurn,
      Box,
      Blast1,
      Blast2,
      Asteroid1,
      Asteroid2,
      Asteroid3,
      Asteroid4
    }

    // KeyValuePair class to allow exposing configuration lists to the Inspector.
    [Serializable]
    public class PrefabItem
    {
      public PoolType Type;
      public GameObject PrefabObject;
      public int Quantity;
    }
  }
}

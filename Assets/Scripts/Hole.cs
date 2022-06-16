using Assets.Scripts;
using UnityEngine;

public class Hole : MonoBehaviour
{
  public float LifeSpan = 10f;
  private float _transpired;

    // Update is called once per frame
    void Update()
    {
      _transpired += Time.deltaTime;
      if (_transpired < LifeSpan) return;
      _transpired = 0;
      Disable();
    }

    private void Disable()
    {
      transform.parent = ObjectPool.Instance.transform.Find(ObjectPool.PoolType.BulletHole.ToString());
      gameObject.SetActive(false);
    }

}

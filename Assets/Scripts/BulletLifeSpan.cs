using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLifeSpan : MonoBehaviour
{
  public float LifeSpan;
  float Transpired;

  void FixedUpdate()
  {
    Transpired += Time.fixedDeltaTime;
    if (Transpired < LifeSpan) return;
    Destroy(gameObject);
  }
}
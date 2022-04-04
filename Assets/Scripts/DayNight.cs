using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour
{
  float time = 50;
  [SerializeField] float multiplier = 0;

  // Update is called once per frame
  void Update()
  {
    time += Time.deltaTime * multiplier;
    if(time > 360)
    {
      time = 0;
    }
    transform.rotation = Quaternion.Euler(time, -30, 0);
  }
}
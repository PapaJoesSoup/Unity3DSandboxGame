using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxColor : MonoBehaviour
{
  // Turn on or off this feature.
  [Tooltip("Turn on or off color change feature.")]
  public bool enableColor;

  // We can affect all boxes sharing a material or just affect one box.
  [Tooltip("Change color of all boxes sharing this material")]
  public bool AllSharedBoxes;

  // delay in seconds to change color.
  [Tooltip("Number of seconds to wait for color to change.")]
  public float delayInSeconds = 3;
  float _elapsedTime;


  // Update is called once per frame
  void Update()
  {
       SetBoxColor();
  }

  private void SetBoxColor()
  {
    if (!enableColor) return;
    if (_elapsedTime > delayInSeconds)
    {
      // Get a random set of RGB values between 0 and 1
      float r = Random.Range(0, 100) / 100f;
      float g = Random.Range(0, 100) / 100f;
      float b = Random.Range(0, 100) / 100f;

      if(AllSharedBoxes)
        GetComponent<Renderer>().sharedMaterial.color = new Color(r, g, b, 1); 
      else
        GetComponent<Renderer>().material.color = new Color(r, g, b, 1);

      _elapsedTime = 0;
    }
    _elapsedTime += Time.deltaTime;
  }

}
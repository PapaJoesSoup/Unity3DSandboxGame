using UnityEngine;

namespace Assets.Scripts
{
  public class DayNight : MonoBehaviour
  {
    private float _time = 50;
    [SerializeField] private float _multiplier = 0;

    // Update is called once per frame
    private void Update()
    {
      _time += Time.deltaTime * _multiplier;
      if(_time > 360)
      {
        _time = 0;
      }
      transform.rotation = Quaternion.Euler(_time, -30, 0);
    }
  }
}
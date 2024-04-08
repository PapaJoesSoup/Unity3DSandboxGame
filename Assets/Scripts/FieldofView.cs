using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
  public class FieldOfView : MonoBehaviour
  {
    public float Radius;
    [Range(0,360)]
    public float FovAngle;

    public GameObject PlayerRef;

    public LayerMask TargetMask;
    public LayerMask ObstructionMask;

    public bool CanSeePlayer;

    private void Start()
    {
      PlayerRef = GameObject.FindGameObjectWithTag("Player");
      StartCoroutine(FovRoutine());
    }

    private IEnumerator FovRoutine()
    {
      WaitForSeconds wait = new(0.2f);

      while (true)
      {
        yield return wait;
        FieldOfViewCheck();
      }
    }

    private void FieldOfViewCheck()
    {
      Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, TargetMask);

      if (rangeChecks.Length != 0)
      {
        Transform target = rangeChecks[0].transform;
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        if (Vector3.Angle(transform.forward, directionToTarget) < FovAngle / 2)
        {
          float distanceToTarget = Vector3.Distance(transform.position, target.position);
          CanSeePlayer = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask);
        }
        else
          CanSeePlayer = false;
      }
      else if (CanSeePlayer)
        CanSeePlayer = false;
    }
  }
}
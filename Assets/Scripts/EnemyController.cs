using UnityEngine;

namespace Assets.Scripts
{
  public class EnemyController : MonoBehaviour
  {

    [Header("Health Settings")]
    [SerializeField] internal float _health = 100f;
    [SerializeField] internal float _maxHealth = 100f;




    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    //from http://forum.unity3d.com/threads/raycasting-a-cone-instead-of-single-ray.39426/
    private bool CanSeePlayer(GameObject target)
    {
      float heightOfPlayer = 1.5f;
 
      Vector3 startVec = transform.position;
      startVec.y += heightOfPlayer;
      Vector3 startVecFwd = transform.forward;
      startVecFwd.y += heightOfPlayer;

      Vector3 rayDirection = target.transform.position - startVec;
 
      // If the ObjectToSee is close to this object and is in front of it, then return true
      if ((Vector3.Angle(rayDirection, startVecFwd)) < 110 &&
          (Vector3.Distance(startVec, target.transform.position) <= 20f))
      {
        //Debug.Log("close");
        return true;
      }
      if ((Vector3.Angle(rayDirection, startVecFwd)) < 90 &&
          Physics.Raycast(startVec, rayDirection, out RaycastHit hit, 100f))
      {
        // Detect if player is within the field of view
        return hit.collider.gameObject == target;
      }
      return false;
    }
    
  }
}

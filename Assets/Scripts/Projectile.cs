using UnityEngine;

namespace Assets.Scripts
{
  public class Projectile : MonoBehaviour
  {
    public float Velocity = 100f;
    public float LifeSpan = 3f;
    private float _transpired;
    private Rigidbody _rigidBody;
    public bool ParticlesEnabled = true;

    private void Start()
    {
      _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
      _transpired += Time.fixedDeltaTime;
      if (_transpired < LifeSpan) return;
      // Old method
      //Destroy(gameObject);

      //New method with object pooling
      _transpired = 0;
      gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
      // If we hit something lose the bolt.
      // We can add a particle effect on the contact point if we wish...

      // Old method
      //Destroy(gameObject);

      //New method with object pooling and particle effects
       if (!ParticlesEnabled) return;
     _transpired = 0;
      ContactPoint contact = collision.GetContact(0);

      GameObject splashPrefab = ObjectPool.Instance.GetPooledObject(ObjectPool.PoolType.Splash);
      if (splashPrefab)
      {
        splashPrefab.transform.position = contact.point;
        splashPrefab.transform.forward = contact.normal;
        splashPrefab.SetActive(true);
      }

      GameObject holePrefab = ObjectPool.Instance.GetPooledObject(ObjectPool.PoolType.BulletHole);
      if (holePrefab)
      {
        holePrefab.transform.position = contact.point + new Vector3(0f, 0f, -0.02f);
        holePrefab.transform.forward = -contact.normal;
        holePrefab.transform.parent = contact.otherCollider.transform;
        holePrefab.SetActive(true);
      }
      
      Disable();
    }

    private void Disable()
    {
      _rigidBody.velocity = Vector3.zero;
      gameObject.SetActive(false);
    }

  }
}
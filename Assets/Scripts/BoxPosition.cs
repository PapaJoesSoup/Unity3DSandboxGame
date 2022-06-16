using UnityEngine;

namespace Assets.Scripts
{
  public class BoxPosition : MonoBehaviour
  {
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;
    private KeyCode _resetBox;

    // Start is called before the first frame update
    private void Start()
    {
      PlayerController controller = FindObjectOfType<PlayerController>();
      _resetBox = controller.BoxReset;

      _initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
      _initialRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,
        transform.rotation.w);
    }

    // Update is called once per frame
    private void Update()
    {
      if (!Input.GetKeyDown(_resetBox)) return;
      ResetTransform();
    }

    private void ResetTransform()
    {
      GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
      GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
      transform.SetPositionAndRotation(_initialPosition, _initialRotation);
    }
  }
}

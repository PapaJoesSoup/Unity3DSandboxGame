using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxPosition : MonoBehaviour
{
  private Vector3 initialPosition;
  private Quaternion initialRotation;
  private KeyCode resetBox;

    // Start is called before the first frame update
    void Start()
    {
      PlayerController controller = FindObjectOfType<PlayerController>();
      resetBox = controller.BoxReset;

      initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
      initialRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z,
        transform.rotation.w);
    }

    // Update is called once per frame
    void Update()
    {
      if (!Input.GetKeyDown(resetBox)) return;
      ResetTransform();
    }

    void ResetTransform()
    {
      GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
      GetComponent<Rigidbody>().angularVelocity = new Vector3(0,0,0);
      transform.position = initialPosition;
      transform.rotation = initialRotation;
    }
}

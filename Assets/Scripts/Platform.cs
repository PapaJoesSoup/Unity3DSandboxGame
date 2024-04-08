using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
  public class Platform : MonoBehaviour
  {
    [Header("Movement Settings")] 
    public bool MoveEnable;
    public bool MoveReturn = false;
    public bool MoveToStart = false;
    public bool MoveRepeat = false;
    [Header("")]
    public float MoveSpeed = 0;
    [Header("")]
    public float ForwardXDistance = 0;
    public float ForwardYDistance = 0;
    public float ForwardZDistance = 0;
    [Header("")]
    public float BackXDistance = 0;
    public float BackYDistance = 0;
    public float BackZDistance = 0;

    [Header("Rotation Settings")] 
    public bool RotateEnable;
    public bool RotateReturn = false;
    public bool RotateToStart = false;
    public bool RotateRepeat = false;
    [Header("")]
    public float RotateSpeed = 0;
    [Header("")]
    public float XRotation = 0;
    public float YRotation = 0;
    public float ZRotation = 0;

    [Header("Other Settings")] 
    public bool PlayerTrigger;
    public bool RemoteTrigger = false;
    public Transform TriggerObject;
    public bool FallEnable = false;
    public float FallDelay = 0;

    private Vector3 startPosition;
    private Vector3 forwardPosition;
    private Vector3 backwardPosition;
    private Vector3 startRotation;
    private Vector3 endRotation;
    private bool moveForward = true;
    private bool moveDone;
    private bool rotateForward = true;
    private bool rotateDone;
    private bool falling;
    private bool fallen;
    private float fallTime = 0;


    // Start is called before the first frame update
    void Start()
    {
      startPosition = transform.position;
      forwardPosition = new Vector3(startPosition.x + ForwardXDistance, startPosition.y + ForwardYDistance,
        startPosition.z + ForwardZDistance);
      backwardPosition = new Vector3(startPosition.x + BackXDistance, startPosition.y + BackYDistance,
        startPosition.z + BackZDistance);
      startRotation = transform.eulerAngles;
      endRotation = new Vector3(startRotation.x + XRotation, startRotation.y + YRotation, startRotation.z + ZRotation);
    }

    // Update is called once per frame
    void Update()
    {
      MoveForward();
      MoveBackward();
      RotateForward();
      RotateBackward();
      Fall();
    }

    void MoveForward()
    {
      if (!MoveEnable || !moveForward || moveDone) return;
      transform.position = Vector3.MoveTowards(transform.position, forwardPosition, MoveSpeed * Time.deltaTime);
      if (transform.position == forwardPosition) moveForward = false;
      if (!moveForward && !MoveReturn) moveDone = true;
    }

    void MoveBackward()
    {
      if (!MoveEnable || moveForward || !MoveReturn || moveDone) return;
      transform.position = Vector3.MoveTowards(transform.position, MoveToStart ? startPosition : backwardPosition,
        MoveSpeed * Time.deltaTime);
      if (transform.position == (MoveToStart ? startPosition : backwardPosition)) moveForward = true;
      if (moveForward && !MoveRepeat) moveDone = true;
    }

    void RotateForward()
    {
      if (!RotateEnable || !rotateForward || rotateDone) return;
      transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(endRotation),
        RotateSpeed * Time.deltaTime);
      if (transform.rotation == Quaternion.Euler(endRotation)) rotateForward = false;
      if (!rotateForward && !RotateReturn) rotateDone = true;
    }

    void RotateBackward()
    {
      if (!RotateEnable || rotateForward || !RotateReturn || rotateDone) return;
      transform.rotation = Quaternion.RotateTowards(transform.rotation,
        Quaternion.Euler(RotateToStart ? startRotation : -endRotation),
        RotateSpeed * Time.deltaTime);
      if (transform.rotation == Quaternion.Euler(RotateToStart ? startRotation : -endRotation)) rotateForward = true;
      if (rotateForward && !RotateRepeat) rotateDone = true;
    }

    void Fall()
    {
      if (!FallEnable || fallen) return;
      if (fallTime < FallDelay)
      {
        fallTime += Time.deltaTime;
      }
      else
      {
        MoveEnable = false;
        RotateEnable = false;
        GetComponent<Rigidbody>().isKinematic = false;
        fallen = true;
      }
    }

    void OnTriggerEnter(Collider other)
    {
      if (!RemoteTrigger || TriggerObject == null || other.gameObject != TriggerObject.gameObject) return;
      MoveEnable = true;
    }

    void OnTriggerExit(Collider other)
    {
      if (!RemoteTrigger) return;
    }

    void OnCollisionEnter(Collision other)
    {
      if (!PlayerTrigger) return;
      if (other.gameObject.tag == "Player") MoveEnable = true;
    }

    void OnCollisionExit(Collision other)
    {
      if (!PlayerTrigger) return;
      if (other.gameObject.tag == "Player") MoveEnable = false;
    }

    void OnCollisionStay(Collision other)
    {
      if (!PlayerTrigger) return;
      if (other.gameObject.tag == "Player") MoveEnable = true;
    }

    void OnTriggerStay(Collider other)
    {

    }

  }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
  [SerializeField] private float sensX;
  [SerializeField] private float sensY;
  [SerializeField] private float multiplier = 0.02f;

  public Camera MainCam;
  public GameObject FirePoint;
  public Transform Player;

  float mouseX;
  float mouseY;

  float xRotation;
  float yRotation;

  private bool FirstPersonView = true;

  public float maxZoom = 5;
  public float minZoom = 40;
  public float sensitivity=1;
  public float zoomSpeed = 30;
  public float targetZoom = 45;
  
  private void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
  }

  private void Update()
  {
    MouseInput();

    MouseLook();

    CameraZoom();

    SwitchCameraView();
  }

  private void MouseLook()
  {
    // We want the camera to look up and down, but we want the player (not the cam) to rotate left and right

    //Needed for cam look up and down.
    MainCam.transform.localRotation =
      Quaternion.Euler(xRotation, 0, 0); //put x and y.  

    // Needed for syncing fire point to cam look up and down
    FirePoint.transform.localRotation =
      Quaternion.Euler(xRotation, 0, 0); //put x and y.  .

    // Needed for look left and right.  cam does not swivel in this method.
    Player.rotation = Quaternion.Euler(0, yRotation, 0); //put y.  
}

  private void SwitchCameraView()
  {
    if (!Input.GetKeyDown("z")) return;
    FirstPersonView = !FirstPersonView;

    GetComponent<Camera>().transform.localPosition = FirstPersonView ? 
      new Vector3(0, 0.6f, 0.4f) : 
      new Vector3(0, 1.5f, -6.5f);
  }

  void MouseInput()
  {
    mouseX = Input.GetAxisRaw("Mouse X");
    mouseY = Input.GetAxisRaw("Mouse Y");

    yRotation += mouseX * sensX * multiplier;
    xRotation -= mouseY * sensY * multiplier;

    xRotation = Mathf.Clamp(xRotation, -90f, 90f);
  }

  public void CameraZoom()
  {
    targetZoom -= Input.mouseScrollDelta.y * sensitivity;
    targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
    GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(GetComponent<Camera>().fieldOfView, targetZoom, zoomSpeed * Time.deltaTime);
  }

}

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class PlayerLook : MonoBehaviour
  {
    public Camera MainCam;
    public GameObject FirePoint;
    public Transform Player;
    // layers we want to detect for camera clipping
    public LayerMask Mask;

    // individual controls to adjust x and y feel for mouse looking
    [SerializeField] private float sensX = 100f;
    [SerializeField] private float sensY = 100f;

    // Common setting to adjust both axes together.
    [SerializeField] private float multiplier = 0.02f;

    private float maxDistance;
    private float zOffset;

    private bool firstPersonView = true;
    private readonly Vector3 firstPersonPosition = new Vector3(0, 0.0f, 0.4f);
    private readonly Vector3 thirdPersonPosition = new Vector3(0, 0.75f, -6.5f);

    private float mouseX;
    private float mouseY;

    private float xRotation;
    private float yRotation;


    private void Start()
    {
      maxDistance = thirdPersonPosition.z;
      zOffset = 0.5f;
    }

    private void Update()
    {
      if (CanvasUI.UiActive) return;
      MouseInput();
      MouseLook();
      SwitchCameraView();
      CameraClip();
    }

    private void MouseInput()
    {
      mouseX = Input.GetAxisRaw("Mouse X");
      mouseY = Input.GetAxisRaw("Mouse Y");

      yRotation += mouseX * sensX * multiplier;
      xRotation -= mouseY * sensY * multiplier;

      xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    }

    private void MouseLook()
    {
      // We don't want to run this when the user is in third person and presses the right mouse button.
      if (!firstPersonView && Input.GetKey(KeyCode.Mouse1)) return;

      // We want the camera to look up and down, but we want the player (not the cam) to rotate left and right
      
      //Needed for cam look up and down.Create a new quaternion and set the x rotation to the xRotation variable.
      MainCam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);  

      // Needed for look left and right.  cam does not swivel in this method.
      Player.rotation = Quaternion.Euler(0, yRotation, 0);  

      // Needed for syncing fire point to cam look up and down
      Vector3 direction = new Vector3(xRotation, 0, 0);

      FirePoint.transform.localRotation = Quaternion.Euler(direction); //put x and y.  .

      // if we are using a reticle (screen CenterPoint) then rotate to the direction to camera center.
      Ray ray = MainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f,0));
      Debug.DrawRay (ray.origin, ray.direction * 1000, Color.green);
      if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
      {
        FirePoint.transform.LookAt(hit.point);
      }
      Debug.DrawRay (FirePoint.transform.position, FirePoint.transform.forward * 1000, Color.yellow);
    }

    private void CameraClip()
    {
      if (firstPersonView) return;
      //the following code is for moving the camera if near a wall in third person
      Camera.main.GetComponent<Camera>().nearClipPlane = 0.35f;
      Vector3 direction = (Camera.main.transform.position - transform.position).normalized;
      Ray ray = new Ray(transform.position, direction);
      Camera.main.transform.localPosition = Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance + zOffset, Mask)
        ? new Vector3(0, 0, -(hitInfo.distance - zOffset))
        : new Vector3(0, 0, -maxDistance);
    }

    private void SwitchCameraView()
    {
      if (!Input.GetKeyDown(KeyCode.Z)) return;
      firstPersonView = !firstPersonView;

      GetComponent<Camera>().transform.localPosition = firstPersonView ? 
        firstPersonPosition : 
        thirdPersonPosition;
    }
  }
}

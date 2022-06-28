using UnityEngine;

namespace Assets.Scripts
{
  public class PlayerLook : MonoBehaviour
  {
    [SerializeField] private float _sensX = 100f;
    [SerializeField] private float _sensY = 100f;
    [SerializeField] private float _multiplier = 0.02f;

    public Camera MainCam;
    public GameObject FirePoint;
    public Transform Player;

    private float _mouseX;
    private float _mouseY;

    private float _xRotation;
    private float _yRotation;

    private bool _firstPersonView = true;

    private void Start()
    {
    }

    private void Update()
    {
      if (CanvasUI.UiActive) return;
      MouseInput();
      MouseLook();
      SwitchCameraView();
    }

    private void MouseLook()
    {
      // We don't want to run this when the user is in third person and presses the right mouse button.
      if (!_firstPersonView && Input.GetKey(KeyCode.Mouse1)) return;

      // We want the camera to look up and down, but we want the player (not the cam) to rotate left and right
      
      //Needed for cam look up and down.
      MainCam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);  

      // Needed for look left and right.  cam does not swivel in this method.
      Player.rotation = Quaternion.Euler(0, _yRotation, 0);  

      // Needed for syncing fire point to cam look up and down
      Vector3 direction = new Vector3(_xRotation, 0, 0);

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

    private void MouseInput()
    {
      _mouseX = Input.GetAxisRaw("Mouse X");
      _mouseY = Input.GetAxisRaw("Mouse Y");

      _yRotation += _mouseX * _sensX * _multiplier;
      _xRotation -= _mouseY * _sensY * _multiplier;

      _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
    }

    private void SwitchCameraView()
    {
      if (!Input.GetKeyDown(KeyCode.Z)) return;
      _firstPersonView = !_firstPersonView;

      GetComponent<Camera>().transform.localPosition = _firstPersonView ? 
        new Vector3(0, 0.6f, 0.4f) : 
        new Vector3(0, 1.5f, -6.5f);
    }
  }
}

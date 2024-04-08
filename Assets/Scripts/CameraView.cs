using Assets.Scripts;
using UnityEngine;


// This script is attached to all cameras.  It is "aware" of the 
// gameObject to which it is attached.  Different behaviors are 
// present if the camera is a child of a Player or an Overhead Cam.
public class CameraView : MonoBehaviour
{
  // is set when in first person view. player child cam.
  private bool firstPersonView = true;
  // This is set in start when the gameObject.name = "OverheadCamera".
  private bool overheadView = false;

  public float MaxZoom = 5;
  public float MinZoom = 60;
  public float ZoomSensitivity = 1;
  public float ZoomSpeed = 30;
  public float TargetZoom = 45;

  public float RotateSensitivityX = 100;
  public float RotateSensitivityY = 10;
  public float TranslateSensitivityX = 10f;
  public float TranslateSensitivityY = 10f;


  private Vector3 mouseMvmt = Vector3.zero;

  // Start is called before the first frame update
  private void Start()
  {
    overheadView = gameObject.name == "OverheadCamera";
  }

  // Update is called once per frame
  private void Update()
  {
    if (CanvasUI.UiActive) return;
    PlayerInput();
    ManipulateCameras();
    CameraZoom();
    SwitchCameraView();
  }

  private void PlayerInput()
  {
    mouseMvmt.x = Input.GetAxis("Mouse X");
    mouseMvmt.y = Input.GetAxis("Mouse Y");
  }

  private void SwitchCameraView()
  {
    if (!KeyMap.Instance.KeyList[KeyMap.Instance.SwitchCamera].IsKeyDown() || overheadView) return;
    firstPersonView = !firstPersonView;
    SetPersonViewCameraPosition();
  }

  public void CameraZoom()
  {
    TargetZoom -= Input.mouseScrollDelta.y * ZoomSensitivity;
    TargetZoom = Mathf.Clamp(TargetZoom, MaxZoom, MinZoom);
    GetComponent<Camera>().fieldOfView = Mathf.MoveTowards(GetComponent<Camera>().fieldOfView, TargetZoom, ZoomSpeed * Time.deltaTime);
  }

  public void ManipulateCameras()
  {
    if (overheadView)
    {
      // OverheadCam...
      //Move Camera position up/down, left/right
      if (Input.GetKey(KeyCode.Mouse2) && (mouseMvmt.x != 0 || mouseMvmt.y != 0))
        TranslateCamera();

      if (!Input.GetKey(KeyCode.Mouse1) || (mouseMvmt.x == 0 && mouseMvmt.y == 0)) return;
      if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) return;
      RotateAroundPoint(hit.point);
    }
    else
    {
      //Player Follow Main Cam...
      if (!firstPersonView && Input.GetKeyUp(KeyCode.Mouse1)) 
        SetPersonViewCameraPosition();

      //Move Camera up/down, left/right when in third person view...
      if (!firstPersonView && Input.GetKey(KeyCode.Mouse2) && (mouseMvmt.x != 0 || mouseMvmt.y != 0))
        TranslateCamera();

      // do not perform rotate around unless we are in third person view and correct conditions apply.
      if (firstPersonView || !Input.GetKey(KeyCode.Mouse1) || (mouseMvmt.x == 0 && mouseMvmt.y == 0)) return;
      Vector3 point = transform.parent.position;
      RotateAroundPoint(point);
    }
  }

  private void SetPersonViewCameraPosition()
  {
    GetComponent<Camera>().transform.localPosition = firstPersonView ?
      new(0, 0.6f, 0.4f) :
      new Vector3(0, 1.5f, -6.5f);
  }

  private void TranslateCamera()
  {
    transform.Translate(Vector3.right * mouseMvmt.x * Time.deltaTime * TranslateSensitivityX);
    transform.Translate(Vector3.up * mouseMvmt.y * Time.deltaTime * TranslateSensitivityY);
  }

  private void RotateAroundPoint(Vector3 point)
  {
    // get angle...
    float angleX = RotateSensitivityX * mouseMvmt.x;
    float angleY = RotateSensitivityY * mouseMvmt.y;

    // Rotate camera up and down
    Vector3 rotation = transform.eulerAngles;
    rotation.x -= angleY * Time.deltaTime * RotateSensitivityY;
    transform.eulerAngles = rotation;

    // rotate camera around the player.  We are rotating the camera, not the player...
    transform.RotateAround(point, Vector3.up, angleX * Time.deltaTime * RotateSensitivityX);
  }

}

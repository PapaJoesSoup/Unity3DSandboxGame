using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  [Header("Linked GameObjects")]
    public Camera MainCamera;
    public GameObject FirePoint;
    public GameObject Bullet;
    public PhysicMaterial NormalPlayer;
    public PhysicMaterial PlayerBounce;
    public PhysicMaterial Flying;
    public CapsuleCollider playerModelCollider;
    public Material BoxMaterial;

    [Header("Movement Settings")]
    [SerializeField] LayerMask groundMask;
    public float moveSpeed = 6f;
    public float PDrag = 6f;
    public float airDrag = 2f;
    public float jumpForce = 5f;
    public float upForce = 500f;
    public float downForce = -1.5f;
    public float MaxSpeedMultiplier;
    public float BulletSpeed;
    Vector3 Position;
    float horizontalMovement;
    float verticalMovement;
    float movementMultiplier = 10f;
    [SerializeField] float airSpeedMultiplier = 0.4f;
    Vector3 moveDirection;

    Rigidbody Rigbod;
    bool isGrounded = false;
    bool isFlying = false;
    bool isBoxSummonAuto = false;
    bool isBoxAutoAlign = false;
    Vector3 maxFlyingSpeed; 
    float maxSpeedDifferentialY;
    public int MaxFlyingSpeedY = 0;
    public int MaxFlyingSpeedXZ = 0;

    [Header("Player Settings")]
    [SerializeField] KeyCode KeyCodePlayerModeNormal;
    [SerializeField] KeyCode KeyCodePlayerModeBounce;
    [SerializeField] KeyCode KeyCodePlayerModeFlying;
    [SerializeField] KeyCode jumpKey;
    [SerializeField] KeyCode upKey;
    [SerializeField] KeyCode downKey;
    [SerializeField] KeyCode BoxSummon;
    [SerializeField] KeyCode BoxSummonauto;
    [SerializeField] KeyCode BoxAutoAlign;
    public KeyCode BoxReset;

    public float playerHeight = 2f;

    private void Start()
    {
      maxFlyingSpeed.y = MaxFlyingSpeedY;
        maxFlyingSpeed.x = MaxFlyingSpeedXZ;
        maxFlyingSpeed.z = MaxFlyingSpeedXZ;
        playerModelCollider.material = NormalPlayer;
        Rigbod = GetComponent<Rigidbody>();
        Rigbod.freezeRotation = true;
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight / 2 + .1f, groundMask);

        MyInput();
        ControlDrag();
        Jump();
        PlayerMoveModes();
        Shoot();
        SummonBoxes();
        SetSpeeds();
    }

    private void PlayerMoveModes()
    {
      if (Input.GetKeyDown(KeyCodePlayerModeNormal))
      {
        playerModelCollider.material = NormalPlayer;
        Rigbod.useGravity = true;
        airSpeedMultiplier = 0.06f;
        airDrag = 0.2f;
        print("Normal");
        isFlying = false;

      }
      if (Input.GetKeyDown(KeyCodePlayerModeBounce))
      {
        playerModelCollider.material = PlayerBounce;
        Rigbod.useGravity = true;
        airSpeedMultiplier = 0.06f;
        airDrag = 0.2f;
        print("Bounce ");
        isFlying = false;
      }
      if (Input.GetKeyDown(KeyCodePlayerModeFlying))
      {
        playerModelCollider.material = Flying;
        Rigbod.useGravity = true;
        airSpeedMultiplier = 0.5f;
        airDrag = 2f;
        print("Flying");
        isFlying = true;

      }

    }

    private void SetSpeeds()
    {
      //Max speed stuff
      if (Rigbod.velocity.y > maxFlyingSpeed.y)//Up max
      {
        maxSpeedDifferentialY = (maxFlyingSpeed.y - Rigbod.velocity.y) * MaxSpeedMultiplier;
        Rigbod.AddForce(transform.up * maxSpeedDifferentialY * Time.deltaTime);
      }
      if (Rigbod.velocity.y < -maxFlyingSpeed.y * 2)//Down max
      {

            
        maxSpeedDifferentialY = (-Rigbod.velocity.y - maxFlyingSpeed.y) * MaxSpeedMultiplier;
        Rigbod.AddForce(transform.up * maxSpeedDifferentialY * Time.deltaTime);
            
      }
      //Max speed stuff

    }
    
    private void SummonBoxes()
    {
      if (Input.GetKeyDown(BoxSummon))
      {
        Position = transform.position;
        CreateNewBox(Position);
      }
      if (Input.GetKeyDown(BoxSummonauto))
      {
        isBoxSummonAuto = !isBoxSummonAuto;
      }
      if (isBoxSummonAuto == true)
      {
        Position = transform.position;
        CreateNewBox(Position);
      }

      if (!Input.GetKeyDown(BoxAutoAlign)) return;
      isBoxAutoAlign = isBoxAutoAlign != true;

    }
    
    private void Jump()
    {
      if (Input.GetKeyDown(jumpKey) && isGrounded)
        Rigbod.AddForce((transform.up) * jumpForce, ForceMode.Impulse);
    }
    
    private void Up() 
    {
        if (Rigbod.velocity.y < maxFlyingSpeed.y)
        {
            Rigbod.AddForce(transform.up * upForce * Time.deltaTime);
        }
        else 
        {
        }
    }
    
    private void Down()
    {
        Rigbod.AddForce(transform.up  * downForce * Time.deltaTime);
    }
    
    private void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        // this will set the move direction to the direction the camera is pointing.
        moveDirection = MainCamera.transform.forward * verticalMovement + MainCamera.transform.right * horizontalMovement;
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
        if (Input.GetKey(upKey) && isFlying == true)
        {
            Up();
        }
        if (Input.GetKey(downKey) && isFlying == true)
        {
            Down();
        }
    }
    
    private void ControlDrag()
    {
        if (isGrounded)
        {
            Rigbod.drag = PDrag;
        }
        else
        {
            Rigbod.drag = airDrag;
        }
    }
    
    private void MovePlayer()
    {
        if (isGrounded)
        {
            // lets point the player in the direction of the camera before we move.
            transform.rotation = new Quaternion(transform.rotation.x, MainCamera.transform.localRotation.y, transform.rotation.z, transform.rotation.w);
            Rigbod.AddForce(moveDirection.normalized * movementMultiplier * moveSpeed, ForceMode.Acceleration);
        }
        else
        {
            Rigbod.AddForce(moveDirection.normalized * movementMultiplier * moveSpeed * airSpeedMultiplier, ForceMode.Acceleration);
        }
    }
    
    private GameObject CreateNewBox(Vector3 Position)
    {
        if (isBoxAutoAlign)
        {
            Position.y = Mathf.Round(Position.y);
            Position.x = Mathf.Round(Position.x);
            Position.z = Mathf.Round(Position.z);
        }
        GameObject NewGameobject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        Position.y = Position.y - 1.5f;
        NewGameobject.transform.position = (Position);
        NewGameobject.transform.localScale = new Vector3(1, 1, 1);
        NewGameobject.layer = 3;
        return NewGameobject;

    }
    
    private void Shoot()
    {
      if (!Input.GetButtonDown("Fire1")) return;
        Vector3 InitialPoint = new Vector3();
        InitialPoint = FirePoint.transform.position;
        GameObject bullet = Instantiate(Bullet, InitialPoint, FirePoint.transform.rotation);
        Rigidbody pbody = bullet.GetComponent<Rigidbody>();
        pbody.velocity = FirePoint.transform.forward * BulletSpeed;

        //print("Shot Fired!");
    }

}

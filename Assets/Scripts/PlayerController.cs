using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
  public class PlayerController : MonoBehaviour
  {
    #region Properties
    [Header("Linked GameObjects")]
    public Camera OverheadCam;
    public Camera MainCamera;
    public Canvas Hud;
    public GameObject FirePoint;
    private PhysicMaterial normalPlayer;
    public PhysicMaterial PlayerBounce;
    public PhysicMaterial Flying;
    public CapsuleCollider PlayerModelCollider;
    public Material BoxMaterial;

    // UI components
    private GameObject playerHealthBarObj;
    private Slider playerHealthBar;
    private Text playerHealthBarText;

    private GameObject targetHealthBarObj;
    private Slider targetHealthBar;
    private Text targetHealthBarText;

    [Header("Movement Settings")]
    [SerializeField]
    private LayerMask groundMask;
    public float MoveSpeed = 6f;
    public float PDrag = 6f;
    public float AirDrag = 2f;
    public float JumpForce = 5f;
    public float UpForce = 500f;
    public float DownForce = -1.5f;
    public float MaxSpeedMultiplier;
    public Weapons Weapon = Weapons.Blaster;

    private Vector3 position;
    private float horizontalMovement;
    private float verticalMovement;
    private float movementMultiplier = 10f;
    [SerializeField] private float airSpeedMultiplier = 0.4f;
    private Vector3 moveDirection;

    private Rigidbody rigbod;
    private bool isCrouching;
    private bool isGrounded = false;
    private bool isFlying = false;
    private bool isBoxSummonAuto = false;
    private bool isBoxAutoAlign = false;
    private Vector3 maxFlyingSpeed;
    private float maxSpeedDifferentialY;
    public int MaxFlyingSpeedY = 0;
    public int MaxFlyingSpeedXz = 0;

    [Header("Player Stats Settings")]
    public float PlayerHeight = 2f;

    [Header("Player Health Settings")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float playerPercentFactor;
    private float targetPercentFactor;
    #endregion Properties

    #region Event Handlers

    private void Start()
    {
      maxFlyingSpeed.y = MaxFlyingSpeedY;
      maxFlyingSpeed.x = MaxFlyingSpeedXz;
      maxFlyingSpeed.z = MaxFlyingSpeedXz;
      PlayerModelCollider.material = normalPlayer;
      rigbod = GetComponent<Rigidbody>();
      rigbod.freezeRotation = true;
      OverheadCam.enabled = false;
      SetupHealthBars();
    }

    private void Update()
    {
      if (CanvasUI.UiActive) return;
      isGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight / 2 + .01f, groundMask);

      MyInput();
      FireWeapon();
      MovePlayer();
      ControlDrag();
      Jump();
      PlayerMoveModes();
      SummonBoxes();
      SetSpeeds();
      SwitchCameras();
    }

    private void FixedUpdate()
    {
      if (CanvasUI.UiActive) return;
      if (KeyMap.Instance.KeyList[KeyMap.Instance.Up].IsKey() && isFlying == true) Up();
      if (KeyMap.Instance.KeyList[KeyMap.Instance.Down].IsKey() && isFlying == true) Down();
    }

    #endregion Event Handlers

    #region Movement

    private void PlayerMoveModes()
    {
      isFlying = false;
      rigbod.useGravity = true;
      airSpeedMultiplier = 0.06f;
      AirDrag = 0.2f;
      if (KeyMap.Instance.KeyList[KeyMap.Instance.ModeNormal].IsKeyDown())
      {
        PlayerModelCollider.material = normalPlayer;
        //print("Normal");

      }
      if (KeyMap.Instance.KeyList[KeyMap.Instance.ModeBounce].IsKeyDown())
      {
        PlayerModelCollider.material = PlayerBounce;
        //print("Bounce ");

      }

      if (!KeyMap.Instance.KeyList[KeyMap.Instance.ModeFlying].IsKeyDown()) return;
      PlayerModelCollider.material = Flying;
      //print("Flying");

      isFlying = true;
      airSpeedMultiplier = 0.5f;
      AirDrag = 2f;
    }

    private void MyInput()
    {
      horizontalMovement = Input.GetAxisRaw("Horizontal");
      verticalMovement = Input.GetAxisRaw("Vertical");

      // this will set the move direction to the direction the camera is pointing.
      moveDirection = MainCamera.transform.forward * verticalMovement + MainCamera.transform.right * horizontalMovement;
    }
    
    private void MovePlayer()
    {
      rigbod.AddForce(movementMultiplier * MoveSpeed * (isGrounded ? 1 : airSpeedMultiplier) * moveDirection.normalized, ForceMode.Acceleration);
    }

    private void Jump()
    {
      if (KeyMap.Instance.KeyList[KeyMap.Instance.Jump].IsKeyDown() && isGrounded)
        rigbod.AddForce((transform.up) * JumpForce, ForceMode.Impulse);
    }
    
    private void Up() 
    {
      if (rigbod.velocity.y < maxFlyingSpeed.y)
        rigbod.AddForce(UpForce * Time.deltaTime * transform.up);
    }
    
    private void Down()
    {
      rigbod.AddForce(DownForce * Time.deltaTime * transform.up);
    }

    private void Crouch()
    {
      if (KeyMap.Instance.KeyList[KeyMap.Instance.Crouch].IsKey())
      {
        if (isCrouching) return;
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y - 1, transform.localScale.z), 0.1f);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 0.05f);
        isCrouching = true;
      }
      else if (isCrouching)
      {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, transform.localScale.y + 1, transform.localScale.z), 0.1f);
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), 0.05f);
        isCrouching = false;
      }
    }
    private void SetSpeeds()
    {
      //Max speed stuff
      if (rigbod.velocity.y > maxFlyingSpeed.y)//Up max
      {
        maxSpeedDifferentialY = (maxFlyingSpeed.y - rigbod.velocity.y) * MaxSpeedMultiplier;
        rigbod.AddForce(maxSpeedDifferentialY * Time.deltaTime * transform.up);
      }

      if (!(rigbod.velocity.y < -maxFlyingSpeed.y * 2)) return;
      maxSpeedDifferentialY = (-rigbod.velocity.y - maxFlyingSpeed.y) * MaxSpeedMultiplier;
      rigbod.AddForce(maxSpeedDifferentialY * Time.deltaTime * transform.up);

    }
    
    private void ControlDrag()
    {
      rigbod.drag = isGrounded ? PDrag : AirDrag;
    }
    
    #endregion Movement

    #region Cameras
    private void SwitchCameras()
    {
      if (KeyMap.Instance.KeyList[KeyMap.Instance.OverheadCamera].IsKeyDown())
      {
        OverheadCam.enabled = !OverheadCam.enabled;
      }
    }
    #endregion Cameras

    #region Boxes
    private void SummonBoxes()
    {
      if (KeyMap.Instance.KeyList[KeyMap.Instance.BoxSummon].IsKeyDown())
      {
        position = transform.position;
        CreateNewBox(position);
      }
      if (KeyMap.Instance.KeyList[KeyMap.Instance.BoxAutoSummon].IsKeyDown())
      {
        isBoxSummonAuto = !isBoxSummonAuto;
      }
      if (isBoxSummonAuto == true)
      {
        position = transform.position;
        CreateNewBox(position);
      }

      if (!KeyMap.Instance.KeyList[KeyMap.Instance.BoxAutoAlign].IsKeyDown()) return;
      isBoxAutoAlign = isBoxAutoAlign != true;

    }
    
    private GameObject CreateNewBox(Vector3 position)
    {
      Vector3 boxPosition = position;
      if (isBoxAutoAlign)
      {
        boxPosition.y = Mathf.Round(position.y);
        boxPosition.x = Mathf.Round(position.x);
        boxPosition.z = Mathf.Round(position.z);
      }
      GameObject newGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
      boxPosition.y -= 1.5f;
      newGameObject.transform.position = (boxPosition);
      newGameObject.transform.localScale = new Vector3(1, 1, 1);
      newGameObject.layer = 3;
      return newGameObject;

    }
    #endregion Boxes

    #region Weapons
    private void FireWeapon()
    {
      switch (Weapon)
      {
        case Weapons.Blaster:
          FireProjectile(ObjectPool.PoolType.Bolt);
          break;
        case Weapons.Gun:
        default:
          FireProjectile(ObjectPool.PoolType.Bullet);
          break;
      }
    }

    private void FireProjectile(ObjectPool.PoolType type)
    {
      if (!KeyMap.Instance.KeyList[KeyMap.Instance.Fire].IsKeyDown()) return;
      GameObject projectile = ObjectPool.Instance.GetPooledObject(type);
      if (projectile == null) return;
      projectile.SetActive(true);
      Quaternion rotation = new(
        FirePoint.transform.rotation.x,
        transform.rotation.y, 
        transform.rotation.z, 
        transform.rotation.w);
      float velocity = projectile.GetComponent<Projectile>().Velocity;

      projectile.transform.position = FirePoint.transform.position;
      projectile.transform.localRotation = rotation;
      projectile.GetComponent<Rigidbody>().velocity = FirePoint.transform.forward * velocity;
    }
    
    public enum Weapons
    {
      Blaster,
      Gun
    }
    #endregion Weapons

    #region Health
    public void ApplyDamage(float damage)
    {
      health -= damage;
      if(health < 0) health = 0;
      playerHealthBar.value = health * playerPercentFactor;
      playerHealthBarObj.GetComponentInChildren<Text>().text = playerHealthBar.value + "%";
      if (health == 0) PlayerDeath();
    }

    private void SetupHealthBars()
    {
      playerHealthBarObj = GameObject.Find("PlayerHealthBar");
      playerHealthBar = playerHealthBarObj.GetComponent<Slider>();
      playerHealthBarText = playerHealthBarObj.GetComponentInChildren<Text>();
      playerHealthBarObj.SetActive(true);

      targetHealthBarObj = GameObject.Find("TargetHealthBar");
      targetHealthBar = targetHealthBarObj.GetComponent<Slider>();
      targetHealthBarText = targetHealthBar.GetComponentInChildren<Text>();
      targetHealthBarObj.SetActive(false);

      playerPercentFactor = playerHealthBar.maxValue / maxHealth;
      playerHealthBar.value = health * playerPercentFactor;
      playerHealthBarText.text = playerHealthBar.value + "%";
    }

    private void SetupTargetHealthBar(GameObject target)
    {
      EnemyController enemy = target.GetComponent<EnemyController>();
      if (enemy != null)
      {
        targetPercentFactor = targetHealthBar.maxValue / enemy._maxHealth;
        targetHealthBar.value = enemy._health * targetPercentFactor;
        targetHealthBarText.text = targetHealthBar.value + "%";
      }

      targetPercentFactor = targetHealthBar.maxValue / maxHealth;
      playerHealthBar.value = health * targetPercentFactor;
      playerHealthBarText.text = playerHealthBar.value + "%";
    }

    private void PlayerDeath()
    {
      gameObject.SetActive(false);
    }
    #endregion Health
  }
}

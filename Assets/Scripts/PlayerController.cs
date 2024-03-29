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
    private PhysicMaterial _normalPlayer;
    public PhysicMaterial PlayerBounce;
    public PhysicMaterial Flying;
    public CapsuleCollider PlayerModelCollider;
    public Material BoxMaterial;

    // UI components
    private GameObject _playerHealthBarObj;
    private Slider _playerHealthBar;
    private Text _playerHealthBarText;

    private GameObject _targetHealthBarObj;
    private Slider _targetHealthBar;
    private Text _targetHealthBarText;


    [Header("Movement Settings")]
    [SerializeField]
    private LayerMask _groundMask;
    public float MoveSpeed = 6f;
    public float PDrag = 6f;
    public float AirDrag = 2f;
    public float JumpForce = 5f;
    public float UpForce = 500f;
    public float DownForce = -1.5f;
    public float MaxSpeedMultiplier;
    public Weapons Weapon = Weapons.Blaster;

    private Vector3 _position;
    private float _horizontalMovement;
    private float _verticalMovement;
    private float _movementMultiplier = 10f;
    [SerializeField] private float _airSpeedMultiplier = 0.4f;
    private Vector3 _moveDirection;

    private Rigidbody _rigbod;
    private bool _isGrounded = false;
    private bool _isFlying = false;
    private bool _isBoxSummonAuto = false;
    private bool _isBoxAutoAlign = false;
    private Vector3 _maxFlyingSpeed;
    private float _maxSpeedDifferentialY;
    public int MaxFlyingSpeedY = 0;
    public int MaxFlyingSpeedXz = 0;

    [Header("Player KeyCode Settings")]
    private KeyMap _map;

    [Header("Player Stats Settings")]
    public float PlayerHeight = 2f;

    [Header("Player Health Settings")]
    [SerializeField] private float _health = 100f;
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _playerPercentFactor;
    private float _targetPercentFactor;
    #endregion Properties

    #region Event Handlers

    private void Start()
    {
      _maxFlyingSpeed.y = MaxFlyingSpeedY;
      _maxFlyingSpeed.x = MaxFlyingSpeedXz;
      _maxFlyingSpeed.z = MaxFlyingSpeedXz;
      PlayerModelCollider.material = _normalPlayer;
      _rigbod = GetComponent<Rigidbody>();
      _rigbod.freezeRotation = true;
      OverheadCam.enabled = false;
      SetupHealthBars();
      _map = KeyMap.Instance;
    }

    private void Update()
    {
      if (CanvasUI.UiActive) return;
      _isGrounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight / 2 + .1f, _groundMask);

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
      if (_map.KeyList[_map.Up].IsKey() && _isFlying == true) Up();
      if (_map.KeyList[_map.Down].IsKey() && _isFlying == true) Down();
    }

    #endregion Event Handlers

    #region Movement

    private void PlayerMoveModes()
    {
      _isFlying = false;
      _rigbod.useGravity = true;
      _airSpeedMultiplier = 0.06f;
      AirDrag = 0.2f;
      if (_map.KeyList[_map.ModeNormal].IsKeyDown())
      {
        PlayerModelCollider.material = _normalPlayer;
        //print("Normal");

      }
      if (_map.KeyList[_map.ModeBounce].IsKeyDown())
      {
        PlayerModelCollider.material = PlayerBounce;
        //print("Bounce ");

      }

      if (!_map.KeyList[_map.ModeFlying].IsKeyDown()) return;
      PlayerModelCollider.material = Flying;
      //print("Flying");

      _isFlying = true;
      _airSpeedMultiplier = 0.5f;
      AirDrag = 2f;
    }

    private void MyInput()
    {
      _horizontalMovement = Input.GetAxisRaw("Horizontal");
      _verticalMovement = Input.GetAxisRaw("Vertical");

      // this will set the move direction to the direction the camera is pointing.
      _moveDirection = MainCamera.transform.forward * _verticalMovement + MainCamera.transform.right * _horizontalMovement;
    }
    
    private void MovePlayer()
    {
      _rigbod.AddForce(_movementMultiplier * MoveSpeed * (_isGrounded ? 1 : _airSpeedMultiplier) * _moveDirection.normalized, ForceMode.Acceleration);
    }

    private void Jump()
    {
      if (_map.KeyList[_map.Jump].IsKeyDown() && _isGrounded)
        _rigbod.AddForce((transform.up) * JumpForce, ForceMode.Impulse);
    }
    
    private void Up() 
    {
      if (_rigbod.velocity.y < _maxFlyingSpeed.y)
        _rigbod.AddForce(UpForce * Time.deltaTime * transform.up);
    }
    
    private void Down()
    {
      _rigbod.AddForce(DownForce * Time.deltaTime * transform.up);
    }

    private void SetSpeeds()
    {
      //Max speed stuff
      if (_rigbod.velocity.y > _maxFlyingSpeed.y)//Up max
      {
        _maxSpeedDifferentialY = (_maxFlyingSpeed.y - _rigbod.velocity.y) * MaxSpeedMultiplier;
        _rigbod.AddForce(_maxSpeedDifferentialY * Time.deltaTime * transform.up);
      }

      if (!(_rigbod.velocity.y < -_maxFlyingSpeed.y * 2)) return;
      _maxSpeedDifferentialY = (-_rigbod.velocity.y - _maxFlyingSpeed.y) * MaxSpeedMultiplier;
      _rigbod.AddForce(_maxSpeedDifferentialY * Time.deltaTime * transform.up);

    }
    
    private void ControlDrag()
    {
      _rigbod.drag = _isGrounded ? PDrag : AirDrag;
    }
    
    #endregion Movement

    #region Cameras
    private void SwitchCameras()
    {
      if (Input.GetKeyDown(KeyCode.C))
      {
        OverheadCam.enabled = !OverheadCam.enabled;
      }
    }
    #endregion Cameras

    #region Boxes
    private void SummonBoxes()
    {
      if (_map.KeyList[_map.BoxSummon].IsKeyDown())
      {
        _position = transform.position;
        CreateNewBox(_position);
      }
      if (_map.KeyList[_map.BoxAutoSummon].IsKeyDown())
      {
        _isBoxSummonAuto = !_isBoxSummonAuto;
      }
      if (_isBoxSummonAuto == true)
      {
        _position = transform.position;
        CreateNewBox(_position);
      }

      if (!_map.KeyList[_map.BoxAutoAlign].IsKeyDown()) return;
      _isBoxAutoAlign = _isBoxAutoAlign != true;

    }
    
    private GameObject CreateNewBox(Vector3 position)
    {
      if (_isBoxAutoAlign)
      {
        position.y = Mathf.Round(position.y);
        position.x = Mathf.Round(position.x);
        position.z = Mathf.Round(position.z);
      }
      GameObject newGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
      position.y -= 1.5f;
      newGameObject.transform.position = (position);
      newGameObject.transform.localScale = new(1, 1, 1);
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
      if (!_map.KeyList[_map.Fire].IsKeyDown()) return;
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
      _health -= damage;
      if(_health < 0) _health = 0;
      _playerHealthBar.value = _health * _playerPercentFactor;
      _playerHealthBarObj.GetComponentInChildren<Text>().text = _playerHealthBar.value + "%";
      if (_health == 0) PlayerDeath();
    }

    private void SetupHealthBars()
    {
      _playerHealthBarObj = GameObject.Find("PlayerHealthBar");
      _playerHealthBar = _playerHealthBarObj.GetComponent<Slider>();
      _playerHealthBarText = _playerHealthBarObj.GetComponentInChildren<Text>();
      _playerHealthBarObj.SetActive(true);

      _targetHealthBarObj = GameObject.Find("TargetHealthBar");
      _targetHealthBar = _targetHealthBarObj.GetComponent<Slider>();
      _targetHealthBarText = _targetHealthBar.GetComponentInChildren<Text>();
      _targetHealthBarObj.SetActive(false);

      _playerPercentFactor = _playerHealthBar.maxValue / _maxHealth;
      _playerHealthBar.value = _health * _playerPercentFactor;
      _playerHealthBarText.text = _playerHealthBar.value + "%";
    }

    private void SetupTargetHealthBar(GameObject target)
    {
      EnemyController enemy = target.GetComponent<EnemyController>();
      if (enemy != null)
      {
        _targetPercentFactor = _targetHealthBar.maxValue / enemy._maxHealth;
        _targetHealthBar.value = enemy._health * _targetPercentFactor;
        _targetHealthBarText.text = _targetHealthBar.value + "%";
      }

      _targetPercentFactor = _targetHealthBar.maxValue / _maxHealth;
      _playerHealthBar.value = _health * _targetPercentFactor;
      _playerHealthBarText.text = _playerHealthBar.value + "%";
    }

    private void PlayerDeath()
    {
      gameObject.SetActive(false);
    }
    #endregion Health
  }
}

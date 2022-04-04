using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Components
    Rigidbody2D _rb;
    BoxCollider2D _collider;
    Stats _stats;
    Animator _anim;
    SpriteRenderer _renderer;

    //Input
    bool _shouldJump;
    float _verticalAxisInput;

    [Header("Collision Check")]
    [SerializeField] Vector2 groundBoxPos;
    [SerializeField] Vector2 groundBoxSize;
    [SerializeField] Vector2 groundBoxPosOffset;
    [SerializeField] Vector2 groundBoxSizeOffset;
    [SerializeField] LayerMask groundLayerMask;
    public bool IsGrounded { get; private set; }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _stats = GetComponent<Stats>();
        _anim = GetComponent<Animator>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        groundBoxSize = new Vector2(_collider.bounds.size.x * .99f, .1f) + groundBoxSizeOffset;
        groundBoxPos = new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - (_collider.bounds.size.y / 2));
    }

#if UNITY_EDITOR
    public bool IsGroundedDebug;

    void Update()
    {
        IsGroundedDebug = IsGrounded;

        groundBoxPos = new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - (_collider.bounds.size.y / 2));
        _verticalAxisInput = Input.GetAxisRaw("Horizontal");

        if (_verticalAxisInput != 0)
            _stats.LookDir = (int)_verticalAxisInput;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            _shouldJump = true;

        _anim.SetFloat("horizontalSpeed", Mathf.Abs(_rb.velocity.x));
        _renderer.flipX = _stats.LookDir == -1 ? true : false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(groundBoxPos, groundBoxSize);
    }

#else
    void Update()
    {
        groundBoxPos = new Vector2(_collider.bounds.center.x, _collider.bounds.center.y - (_collider.bounds.size.y / 2));
        _verticalAxisInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded)
            _shouldJump = true;
    }
#endif

    void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapBox(groundBoxPos, groundBoxSize, 0f, groundLayerMask);

        _rb.velocity = new Vector2(_verticalAxisInput * (_stats.MovementSpeed + _stats.MovementSpeedModifiers), _rb.velocity.y);

        if (_shouldJump)
        {
            _rb.AddForce(new Vector2(0, _stats.JumpForce + _stats.JumpForceModifiers), ForceMode2D.Impulse);
            _shouldJump = false;
        }
    }
}


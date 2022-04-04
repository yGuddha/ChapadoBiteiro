using UnityEngine;
using Pathfinding;

public class EnemyIA : MonoBehaviour
{
    [Header("Pathfinder")]
    [SerializeField] Transform target;
    [SerializeField] float activateDistance = 50;
    [SerializeField] float pathUpdateSeconds = .5f;
    [SerializeField] float nextWaypointDistance = 3;

    [Header("Abilities")]
    [SerializeField] bool canJump;
    [SerializeField] bool canFollow;
    [SerializeField] bool canFly;
    [SerializeField] bool canShoot;

    [Header("Shoot")]
    [SerializeField] GameObject projectile;
    [SerializeField] float shootRange;

    //Components
    Path _path;
    Seeker _seeker;
    Rigidbody2D _rb;
    Stats _stats;
    Animator _anim;
    [SerializeField] Vector3 gunPositionOffset;
    [SerializeField] GameObject soul;


    bool _targetWithinDistance => Vector2.Distance(transform.position, target.transform.position) <= activateDistance;
    bool _targetWithinShootDistance => Vector2.Distance(transform.position, target.transform.position) <= shootRange;


    int _currentWaypoint = 0;
    float _pathUpdateTimer;

    bool _shootAnimationIsReady = true;

    void Awake()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();
        _stats = GetComponent<Stats>();
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        if (canFly)
            _rb.gravityScale = 0;
    }

    void Update()
    {
        _anim.SetFloat("velocity", Mathf.Abs(_rb.velocity.x));

        if (_stats.Life <= 0)
        {
            Instantiate(soul, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        if (canShoot)
        {
            if (_targetWithinShootDistance && _shootAnimationIsReady)
            {
                _anim.SetTrigger("shoot"); 
                _shootAnimationIsReady = false;
            }
        }

        if (canFollow)
        {
            _pathUpdateTimer -= Time.deltaTime;
            if (_pathUpdateTimer > 0) return;

            if (_targetWithinDistance && _seeker.IsDone())
            {
                _seeker.StartPath(_rb.position, target.position, OnPathComplete);
                _pathUpdateTimer = pathUpdateSeconds;
            }
        }
    }

    void FixedUpdate()
    {
        if (!canFollow) return;

        FollowPath();
    }

    private void FollowPath()
    {
        if (_path == null) return;
        if (_currentWaypoint >= _path.vectorPath.Count) return;

        Vector2 dir = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
        Vector2 force = dir * (_stats.MovementSpeed * Time.deltaTime);
        _rb.AddForce(force, ForceMode2D.Impulse);

        float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);

        if (distance < nextWaypointDistance)
            _currentWaypoint++;

        if (_rb.velocity.x >= .01f)
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            _stats.LookDir = 1;
        }
        else if (_rb.velocity.x <= -.01f)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
            _stats.LookDir = -1;
        }
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    private void Shoot()
    {
        var angleBetweenSelfAndTarget = Mathf.Atan2(target.transform.position.y - this.transform.position.y,target.transform.position.x - this.transform.position.x);
        var vector = (Vector3)RadianToVector2(angleBetweenSelfAndTarget);
        var proj = Instantiate  (projectile,
                                this.transform.position + vector,
                                Quaternion.Euler(0, 0, angleBetweenSelfAndTarget * Mathf.Rad2Deg));

        proj.GetComponent<Projectile>().Shoot(vector, _stats);

        _shootAnimationIsReady = true;
    }

    private Vector2 RadianToVector2(float radian) =>
        new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));

}

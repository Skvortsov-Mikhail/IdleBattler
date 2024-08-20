using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed;

    [SerializeField] private float m_LifeTime;

    private Timer _lifeCycleTimer;
    private bool _isHit;

    // private Rigidbody2D _rb;

    private Player _player;
    private ProjectilesPool _projectilesPool;
    [Inject]
    public void Construct(Player player, ProjectilesPool projectilesPool)
    {
        _player = player;
        _projectilesPool = projectilesPool;
    }

    /*
     * Движение через физику вызывает неочевидный баг с OnTriggerStay2D() в Enemy.cs
     * UPD: Наличие компонента Rigidbody2D на префабе Projectile выдает неправильное поведение метода OnTriggerStay2D() в Enemy.cs
     * 
     * private void Awake()
     * {
     *     _rb = GetComponent<Rigidbody2D>();
     * }
    */
    private void Start()
    {
        _lifeCycleTimer = new Timer(m_LifeTime);
    }

    private void Update()
    {
        MoveProjectile();

        UpdateTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<Enemy>();

        if (enemy != null & _isHit == false)
        {
            enemy.ApplyDamage(_player.ProjectileDamage);

            _isHit = true;

            _projectilesPool.Pool.Release(this);
        }
    }

    public void SetNewValues(Vector2 startPos, Vector2 targetPos)
    {
        transform.position = startPos;

        // _rb.velocity = Vector3.zero;
        // _rb.velocity = targetPos.normalized * m_Speed;

        var direction = targetPos - startPos;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_lifeCycleTimer != null)
        {
            _lifeCycleTimer.RestartTimer();
        }

        _isHit = false;
    }

    private void MoveProjectile()
    {
        transform.Translate(Vector2.right * Time.deltaTime * m_Speed);
    }

    private void UpdateTimer()
    {
        _lifeCycleTimer.RemoveTime(Time.deltaTime);

        if (_lifeCycleTimer.IsFinished)
        {
            _projectilesPool.Pool.Release(this);
        }
    }
}
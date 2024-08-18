using System;
using UnityEngine;
using Zenject;

public class Enemy : Destructible
{
    public event Action<float> EnemyDamaged;
    public event Action EnemyDied;

    private float _attackTime;
    private float _movementSpeed;

    private Timer _attackTimer;
    private bool _isMoving;

    private Player _player;
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }

    private void Start()
    {
        RotateToPlayer();
    }

    private void Update()
    {
        MoveToPlayer();
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            _isMoving = false;

            UpdateTimer();

            if (_attackTimer.IsFinished)
            {
                player.ApplyDamage(m_Damage);

                _attackTimer.RestartTimer();
            }
        }
    }

    public void InitEnemy(EnemyConfiguration config)
    {
        m_MaxHP = config.MaxHP;
        m_Damage = config.Damage;
        _attackTime = config.AttackTime;
        _movementSpeed = config.MovementSpeed;

        _currentHP = m_MaxHP;

        _attackTimer = new Timer(_attackTime);

        GetComponentInChildren<SpriteRenderer>().sprite = config.VisualModel;

        _isMoving = true;
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);

        EnemyDamaged?.Invoke(damage);

        CheckDeath();
    }

    private void CheckDeath()
    {
        if (_currentHP <= 0)
        {
            EnemyDied?.Invoke();

            Destroy(gameObject);
        }
    }

    private void RotateToPlayer()
    {
        var direction = _player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void MoveToPlayer()
    {
        if (_isMoving == true)
        {
            transform.Translate(Vector2.right * Time.deltaTime * _movementSpeed);
        }
    }

    private void UpdateTimer()
    {
        _attackTimer.RemoveTime(Time.deltaTime);
    }
}

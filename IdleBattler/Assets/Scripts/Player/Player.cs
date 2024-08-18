using System;
using UnityEngine;
using Zenject;

public class Player : Destructible
{
    public event Action<float> HPUpdated;
    public event Action PlayerDied;

    [SerializeField] private PlayerConfiguration m_PlayerConfiguration;

    private float _fireRate;
    private float _attackRadius;

    private Timer _fireTimer;

    public float ProjectileDamage => m_Damage;
    public float MaxHP => m_MaxHP;

    private ProjectilesPool _projectilesPool;
    [Inject]
    public void Construct(ProjectilesPool projectilesPool)
    {
        _projectilesPool = projectilesPool;
    }

    private void Start()
    {
        InitPlayer();

        _fireTimer = new Timer(_fireRate);
    }

    private void Update()
    {
        _fireTimer.RemoveTime(Time.deltaTime);

        if (_fireTimer.IsFinished)
        {
            if (DoRegularAttack() == true)
            {
                _fireTimer.RestartTimer();
            }
        }
    }

    public override void ApplyDamage(float damage)
    {
        base.ApplyDamage(damage);

        HPUpdated?.Invoke(_currentHP);

        CheckDeath();
    }

    public bool DoSpecialAttack()
    {
        return false; // TODO заглушка
    }

    private void CheckDeath()
    {
        if (_currentHP <= 0)
            PlayerDied?.Invoke();
    }

    private void InitPlayer()
    {
        m_MaxHP = m_PlayerConfiguration.MaxHP;
        m_Damage = m_PlayerConfiguration.Damage;
        _fireRate = m_PlayerConfiguration.FireRate;
        _attackRadius = m_PlayerConfiguration.AttackRadius;

        _currentHP = m_MaxHP;
    }

    private bool DoRegularAttack()
    {
        var target = GetNearestEnemy(transform.position);

        if (target == null) return false;

        var projectile = _projectilesPool.Pool.Get();
        projectile.SetNewValues(transform.position, target.transform.position);

        return true;
    }

    private Enemy GetNearestEnemy(Vector2 positionFrom)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(positionFrom, _attackRadius);

        if (targets.Length == 0) return null;

        Enemy nearestEnemy = null;

        float minDistance = float.MaxValue;

        for (int i = 0; i < targets.Length; i++)
        {
            var enemy = targets[i].transform.root.GetComponent<Enemy>();

            if (enemy == null) continue;

            var distance = Vector2.Distance(targets[i].transform.position, positionFrom);

            if (minDistance > distance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
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
    private AudioSource _audio;

    public float ProjectileDamage => m_Damage;
    public float MaxHP => m_MaxHP;

    private ProjectilesPool _projectilesPool;
    private EnemiesContainer _enemiesContainer;
    [Inject]
    public void Construct(ProjectilesPool projectilesPool, EnemiesContainer enemiesContainer)
    {
        _projectilesPool = projectilesPool;
        _enemiesContainer = enemiesContainer;
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

    public bool DoSpecialAttack(AbilityConfiguration abilityConfiguration)
    {
        if (abilityConfiguration.EnemiesAttackedAmount > 1)
        {
            StartCoroutine(DoMassAttack(abilityConfiguration));
            return true;
        }

        /*
         * 
         * Возможно расширение по мере добавления способностей
         * 
         */

        return true;
    }

    private IEnumerator DoMassAttack(AbilityConfiguration abilityConfiguration)
    {
        Vector2 searchingPosFrom = transform.position;

        List<Enemy> excludedEnemies = new List<Enemy>();

        float damage = abilityConfiguration.Damage;

        for (int i = 0; i < abilityConfiguration.EnemiesAttackedAmount; i++)
        {
            var target = _enemiesContainer.GetNearestEnemy(searchingPosFrom, abilityConfiguration.AttackRadius, false, excludedEnemies);

            if (target == null) break;

            target.ApplyDamage(damage, true);

            damage -= damage * abilityConfiguration.DamageReductionMultiplierForEnemy;
            damage = (float)Math.Round(damage, 1);

            excludedEnemies.Add(target);
            searchingPosFrom = target.transform.position;

            yield return new WaitForSeconds(abilityConfiguration.CooldownToNextEnemy);
        }

        StopCoroutine(DoMassAttack(abilityConfiguration));
    }

    private void CheckDeath()
    {
        if (_currentHP <= 0)
        {
            PlayerDied?.Invoke();
        }
    }

    private void InitPlayer()
    {
        m_MaxHP = m_PlayerConfiguration.MaxHP;
        m_Damage = m_PlayerConfiguration.Damage;
        _fireRate = m_PlayerConfiguration.FireRate;
        _attackRadius = m_PlayerConfiguration.AttackRadius;

        _currentHP = m_MaxHP;

        _audio = GetComponent<AudioSource>();
    }

    private bool DoRegularAttack()
    {
        var target = _enemiesContainer.GetNearestEnemy(transform.position, _attackRadius);

        if (target == null) return false;

        var projectile = _projectilesPool.Pool.Get();
        projectile.SetNewValues(transform.position, target.transform.position);

        _audio.Play();

        return true;
    }
}
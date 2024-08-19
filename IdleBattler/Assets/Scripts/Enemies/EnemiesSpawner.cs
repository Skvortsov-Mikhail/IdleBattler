using UnityEngine;
using Zenject;

public class EnemiesSpawner : MonoBehaviour
{
    [SerializeField] private Enemy m_EnemyPrefab;

    [Space(10)]
    [SerializeField] private EnemyConfiguration m_RegularEnemyConfiguration;
    [SerializeField] private EnemyConfiguration m_BossEnemyConfiguration;

    [Space(10)]
    [SerializeField] private float m_SpawnRadius;

    private Timer _regEnemyTimer;
    private Timer _bossTimer;

    private EnemiesContainer _enemiesContainer;
    private DiContainer _diContainer;
    [Inject]
    public void Construct(EnemiesContainer enemiesContainer, DiContainer container)
    {
        _enemiesContainer = enemiesContainer;
        _diContainer = container;
    }

    private void Start()
    {
        InitTimers();
    }

    private void Update()
    {
        UpdateTimers();

        TrySpawnEnemy();
    }

    private void InitTimers()
    {
        _regEnemyTimer = new Timer(m_RegularEnemyConfiguration.RespawnTime);
        _bossTimer = new Timer(m_BossEnemyConfiguration.RespawnTime);
    }

    private void UpdateTimers()
    {
        _regEnemyTimer.RemoveTime(Time.deltaTime);
        _bossTimer.RemoveTime(Time.deltaTime);
    }

    public void TrySpawnEnemy()
    {
        if (_regEnemyTimer.IsFinished)
        {
            var enemy = _diContainer.InstantiatePrefab(m_EnemyPrefab, _enemiesContainer.transform);
            enemy.GetComponent<Enemy>().InitEnemy(m_RegularEnemyConfiguration);

            enemy.transform.position = Random.insideUnitCircle.normalized * m_SpawnRadius;

            _regEnemyTimer.RestartTimer();
        }

        if (_bossTimer.IsFinished)
        {
            var boss = _diContainer.InstantiatePrefab(m_EnemyPrefab);
            boss.GetComponent<Enemy>().InitEnemy(m_BossEnemyConfiguration);

            boss.transform.position = Random.insideUnitCircle.normalized * m_SpawnRadius;

            _bossTimer.RestartTimer();
        }
    }
}
using UnityEngine;
using Zenject;

public class EnemiesSpawnerInstaller : MonoInstaller
{
    [SerializeField] private EnemiesSpawner m_EnemiesSpawner;

    public override void InstallBindings()
    {
        BindEnemiesSpawner();
    }

    private void BindEnemiesSpawner()
    {
        Container.Bind<EnemiesSpawner>().FromInstance(m_EnemiesSpawner).AsSingle();
    }
}
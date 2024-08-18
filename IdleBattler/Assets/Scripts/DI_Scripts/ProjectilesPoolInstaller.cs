using UnityEngine;
using Zenject;

public class ProjectilesPoolInstaller : MonoInstaller
{
    [SerializeField] private ProjectilesPool m_ProjectilesPool;

    public override void InstallBindings()
    {
        BindProjectilesPool();
    }

    private void BindProjectilesPool()
    {
        Container.Bind<ProjectilesPool>().FromInstance(m_ProjectilesPool).AsSingle();
    }
}

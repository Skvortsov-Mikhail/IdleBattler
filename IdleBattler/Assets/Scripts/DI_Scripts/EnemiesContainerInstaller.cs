using UnityEngine;
using Zenject;

public class EnemiesContainerInstaller : MonoInstaller
{
    [SerializeField] private EnemiesContainer m_EnemiesContainer;

    public override void InstallBindings()
    {
        BindEnemiesContainer();
    }

    private void BindEnemiesContainer()
    {
        Container.Bind<EnemiesContainer>().FromInstance(m_EnemiesContainer).AsSingle();
    }
}
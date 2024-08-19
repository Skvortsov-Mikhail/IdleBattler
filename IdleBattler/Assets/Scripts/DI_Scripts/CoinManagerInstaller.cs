using UnityEngine;
using Zenject;

public class CoinManagerInstaller : MonoInstaller
{
    [SerializeField] private CoinManager m_CoinManager;

    public override void InstallBindings()
    {
        BindCoinManager();
    }

    private void BindCoinManager()
    {
        Container.Bind<CoinManager>().FromInstance(m_CoinManager).AsSingle();
    }
}
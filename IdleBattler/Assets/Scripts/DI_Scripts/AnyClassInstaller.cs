using UnityEngine;
using Zenject;

public class AnyClassInstaller : MonoInstaller
{
    [SerializeField] private AnyClass m_AnyClassObject;

    public override void InstallBindings()
    {
        BindAnyClass();
    }

    private void BindAnyClass()
    {
        Container.Bind<AnyClass>().FromInstance(m_AnyClassObject).AsSingle();
    }
}
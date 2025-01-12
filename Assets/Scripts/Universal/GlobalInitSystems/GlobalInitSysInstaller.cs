using UnityEngine;
using Zenject;

public class GlobalInitSysInstaller : MonoInstaller
{
    [SerializeField] private GlobalInitSystems initSys;
    public override void InstallBindings()
    {
        Container
        .Bind<GlobalInitSystems>()
            .FromInstance(initSys)
            .AsSingle();
    }
}

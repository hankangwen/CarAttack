using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zenject;

public class GameSystemsInstaller : MonoInstaller
{
    [SerializeField] private WeaponController controller;
    [SerializeField] private EnvironmentGenerator mapGenerator;

    public override void InstallBindings()
    {
        Container.BindInstance(controller);
        Container.BindInstance(mapGenerator);
    }
}

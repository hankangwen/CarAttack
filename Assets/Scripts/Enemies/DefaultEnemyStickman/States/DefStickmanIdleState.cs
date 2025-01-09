using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefStickmanIdleState : State
{
    [SerializeField] private DefStickmanAttackState stickmanAttackState;

    private StickmanEnemy enemy;
    private Car car;
    public override string stateID => BasicStates.Idle.ToString();

    public override void Init(InitArgs args)
    {
        enemy = args.GetArgument<StickmanEnemy>();
        car = args.GetArgument<Car>();
    }

    public override void RunOnStart()
    {
        enemy.animator.SetBool("idle", true);
    }

    public override State RunCurrentState(float deltaTime)
    {
        if((enemy.transform.position - car.transform.position).magnitude < enemy.playerDetectionDist)
        {
            return stickmanAttackState;
        }
        return this;
    }

    public override void RunOnExit()
    {
        enemy.animator.SetBool("idle", false);
    }

}

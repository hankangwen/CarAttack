using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefStickmanAttackState : State
{
    private StickmanEnemy enemy;
    private Car car;
    public override string stateID => BasicStates.Attack.ToString();

    public override void Init(InitArgs args)
    {
        enemy = args.GetArgument<StickmanEnemy>();
        car = args.GetArgument<Car>();
    }

    public override void RunOnStart()
    {
        enemy.animator.SetBool("run", true);
    }

    public override State RunCurrentState(float deltaTime)
    {
        bool reached;
        enemy.movement.MoveAtDestination(car.transform.position, deltaTime, out reached);
        return this;
    }

    public override void RunOnExit()
    {
        enemy.animator.SetBool("run", false);
    }
}

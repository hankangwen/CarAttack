using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefStickmanIdleState : State
{
    [SerializeField] private DefStickmanAttackState stickmanAttackState;

    private bool attack;
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
        attack = false;
        enemy.vitality.onRecieveDamage += HealthChanged;
        enemy.animator.SetBool("idle", true);
    }

    public override State RunCurrentState(float deltaTime)
    {
        if (car.vitality.hp <= 0) return this;
        if((enemy.transform.position - car.transform.position).magnitude < enemy.playerDetectionDist
            || attack)
        {
            return stickmanAttackState;
        }
        return this;
    }

    public override void RunOnExit()
    {
        enemy.vitality.onRecieveDamage -= HealthChanged;
        enemy.animator.SetBool("idle", false);
    }

    public void HealthChanged(float hpChange)
    {
        attack = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StickmanEnemy : MonoBehaviour, IPooledObject
{
    //TODO: move stats to StatsData object
    [Header("stats")]
    [SerializeField] private float _playerDetectionDist;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _rotSpeed;

    [Header("necessary links")]
    [SerializeField] private StateManager _stateMachine;
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;

    public Animator animator => _animator;
    public Movement movement => _movement;
    public float playerDetectionDist => _playerDetectionDist;
    public float runSpeed => _runSpeed;
    public float rotSpeed => _rotSpeed;

    public void Activate()
    {
        gameObject.SetActive(true);
        _stateMachine.Run(BasicStates.Idle.ToString());
    }

    public void ResetAndDeactivate()
    {
        _stateMachine.Stop();
        animator.Rebind();
        animator.Update(0f);
        gameObject.SetActive(false);
    }

    public void Init()
    {
        Car car = FindObjectOfType<Car>();
        InitArgs initArgs = new InitArgs()
                        .AddArgument<StickmanEnemy>(this)
                        .AddArgument<Animator>(_animator)
                        .AddArgument<Car>(car);
        _stateMachine.Init(initArgs);
        _movement.Init(this);
        gameObject.SetActive(false);
    }
}

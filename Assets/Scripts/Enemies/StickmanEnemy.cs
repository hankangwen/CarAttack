using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickmanEnemy : MonoBehaviour, IPooledObject, IDestroyableEntity, ICarObstacle
{
    //TODO: move stats to StatsData object
    [Header("stats")]
    [SerializeField] private float _playerDetectionDist;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _damage;

    [Header("necessary systems")]
    [SerializeField] private Vitality _vitality;

    [Header("necessary links")]
    [SerializeField] private StateManager _stateMachine;
    [SerializeField] private Animator _animator;
    [SerializeField] private Movement _movement;

    [SerializeField] private ParticleSystem deathParticles;

    public Action onDie { get; set; }
    public Animator animator => _animator;
    public Movement movement => _movement;
    public float playerDetectionDist => _playerDetectionDist;
    public float runSpeed => _runSpeed;
    public float rotSpeed => _rotSpeed;
    public float damage => _damage;
    public Vitality vitality => _vitality;



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

    public void Die()
    {
        gameObject.SetActive(false);
        Instantiate(deathParticles,transform.position,Quaternion.identity);
        onDie?.Invoke();
    }

    public void Init()
    {
        Car car = FindObjectOfType<Car>();//to do: optimize
        InitArgs initArgs = new InitArgs()
                        .AddArgument<StickmanEnemy>(this)
                        .AddArgument<Animator>(_animator)
                        .AddArgument<Car>(car);
        _stateMachine.Init(initArgs);
        _movement.Init(this);
        _vitality.Init();
        _vitality.onHealthFinishes += Die;
        gameObject.SetActive(false);
    }

    public void Colide(out float damage)
    {
        Die();
        damage = this.damage;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class Car : MonoBehaviour, IDestroyableEntity
{
    [SerializeField] private Vitality _vitality;
    [SerializeField] private CarCollisionDetection _collisionDetection;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CarMovement _movement;
    [SerializeField] private List<Gun> guns;
    [Header("destroyed effects")]//TODO: move to effects script
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private Color carDestroyedColor;
    [SerializeField] private MeshRenderer carRenderer;

    public Action onDie { get; set; }

    public CarMovement movement => _movement;
    public Vitality vitality => _vitality;


    private void Start()
    {
        _movement.Init(this);
        _vitality.Init();
        _collisionDetection.Init(this);
        foreach (Gun gun in guns)
        {
            gun.Init(this);
        }
        _vitality.onHealthFinishes += CarDestroyed;
    }

    public void Activate()
    {
        foreach (Gun gun in guns)
        {
            gun.Activate();
        }
    }

    public void Deactivate()
    {
        foreach (Gun gun in guns)
        {
            gun.Deactivate();
        }
    }

    public void ResetCar()
    {
        _rigidbody.isKinematic = true;
        transform.rotation = Quaternion.identity;
        carRenderer.material.color = Color.white;
        fireParticles.Stop();
    }

    private void CarDestroyed()
    {
        onDie?.Invoke();
        _rigidbody.isKinematic = false;
        _rigidbody.AddExplosionForce(300, -movement.movementSpeedVector - new Vector3(0,1), 100,2);
        carRenderer.material.color = carDestroyedColor;
        fireParticles.Play();
    }
}

[Serializable]
public class CarMovement
{
    public Vector3 movementSpeedVector { get; private set; }

    private Car car;
    private Transform transform;
    public void Init(Car car)
    {
        this.car = car;
        this.transform = car.transform;
    }

    public void Move(Vector3 newPosition)
    {
        Vector3 movement = newPosition - transform.position;
        movementSpeedVector = movement / Time.deltaTime;
        transform.position = newPosition;
        transform.rotation = Quaternion.LookRotation(movement);
    }
}

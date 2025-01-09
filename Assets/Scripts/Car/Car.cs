using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour, IDestroyableEntity
{
    [SerializeField] private Vitality _vitality;
    [SerializeField] private CarMovement _movement;
    [SerializeField] private List<Gun> guns;

    public Action onDie { get; set; }

    public CarMovement movement => _movement;
    public Vitality vitality => _vitality;


    private void Start()
    {
        _movement.Init(this);
        _vitality.Init();
        foreach (Gun gun in guns)
        {
            gun.Init(this);
        }
        _vitality.onHealthFinishes += CarDestroyed;
    }

    private void CarDestroyed()
    {
        onDie?.Invoke();
    }
}

[System.Serializable]
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

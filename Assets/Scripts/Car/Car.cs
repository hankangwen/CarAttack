using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private CarMovement _movement;
    [SerializeField] private List<Gun> guns;

    public CarMovement movement => _movement;

    private void Start()
    {
        _movement.Init(this);
        foreach (Gun gun in guns)
        {
            gun.Init(this);
        }
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

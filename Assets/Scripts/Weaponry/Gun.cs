using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gun : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform nozzle;
    [SerializeField] private float shootSpeed = 0.7f;

    private float shootTimer;
    private Car car;

    [Inject]
    protected void Construct(WeaponController controller)
    {
        controller.RegisterGun(this);
    }

    public void Init(Car car)
    {
        this.car = car;
    }

    public void LoockAt(Vector3 position, float deltaTime)
    {
        Vector3 targetLoockDir = position - transform.position;
        targetLoockDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetLoockDir);
    }
    
    private void Update()
    {
        if(shootTimer < 0)
        {
            shootTimer = shootSpeed;
            Projectile proj = Instantiate(projectilePrefab);
            proj.transform.position = nozzle.position;
            proj.transform.rotation = transform.rotation;
            proj.Launch(transform.forward, car.movement.movementSpeedVector);
        }
        else shootTimer -= Time.deltaTime;
    }
}

using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gun : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform nozzle;
    [SerializeField] private float shootSpeed = 0.7f;
    [SerializeField] private float damage;

    [SerializeField] private float angleRestrictions = 80;
    [SerializeField] private GameObject laserPointer;

    private float localY;
    private bool active;
    private float shootTimer;
    private Car car;

    private WeaponController controller;
    [Inject]
    protected void Construct(WeaponController controller)
    {
        this.controller = controller;
    }

    public void Init(Car car)
    {
        this.car = car;
    }
    #region activation
    public void Activate()
    {
        if (active) return;
        controller.RegisterGun(this);
        laserPointer.SetActive(true);
        active = true;
    }

    public void Deactivate()
    {
        if (!active) return;
        controller.UnregisterGun(this);
        laserPointer.SetActive(false);
        transform.DORotateQuaternion(Quaternion.identity,0.5f);
        active = false;
    }
    #endregion

    public void LoockAt(Vector3 position, float deltaTime)
    {
        Vector3 targetLoockDir = position - transform.position;
        targetLoockDir.y = 0;
        transform.rotation = Quaternion.LookRotation(targetLoockDir);
    }

    public void Rotate(float x, float y, float z)
    {
        if (localY + y <= -angleRestrictions || localY + y >= angleRestrictions) return;
        localY += y;
        transform.Rotate(0, y, 0);
    }
    
    private void Update()
    {
        if(!active) return;
        if(shootTimer < 0)
        {
            shootTimer = shootSpeed;
            Projectile proj = Instantiate(projectilePrefab);
            proj.transform.position = nozzle.position;
            proj.transform.rotation = transform.rotation;
            proj.Launch(transform.forward, car.movement.movementSpeedVector, damage);
        }
        else shootTimer -= Time.deltaTime;
    }
}

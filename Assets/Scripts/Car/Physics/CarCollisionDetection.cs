using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollisionDetection : MonoBehaviour
{
    private Car car;
    public void Init(Car car)
    {
        this.car = car;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.activeSelf) return;
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeSelf) return;
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject obj)
    {
        ICarObstacle collidable = obj.GetComponent<ICarObstacle>();
        if (collidable != null)
        {
            float damage;
            collidable.Colide(out damage);
            car.vitality.DealDamage(damage);
        }
    }
}

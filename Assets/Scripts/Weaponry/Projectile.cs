using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float flySpeed;
    [SerializeField] private float flyDuration;

    private float damage;
    private Vector3 moveVector;
    public void Launch(Vector3 loockVector, Vector3 startMoveVect, float damage)
    {
        this.damage = damage;
        moveVector = startMoveVect + (loockVector * flySpeed);
        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {
        LayerMask enemyLayer = LayerMask.GetMask("Enemy");
        Vector3 direction = moveVector.normalized;
        do
        {
            float distanceThisFrame = moveVector.magnitude * Time.deltaTime + 0.1f;

            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceThisFrame, enemyLayer))
            {
                HandleHit(hit.collider);
                break;
            }

            transform.position += moveVector * Time.deltaTime;
            flyDuration -= Time.deltaTime;
            if (flyDuration < 0)
            {
                Destroy(gameObject);
                break;
            }
            yield return new WaitForEndOfFrame();
        } while (true);
    }

    private void HandleHit(Collider collider)
    {
        IDestroyableEntity hittedEntity = collider.GetComponent<IDestroyableEntity>();
        if (hittedEntity == null) { Debug.LogError("Hitted enemy is null"); return; }
        hittedEntity.vitality.DealDamage(damage);
        Destroy(gameObject);
    }
}

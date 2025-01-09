using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float flySpeed;
    [SerializeField] private float flyDuration;


    private Vector3 moveVector;
    public void Launch(Vector3 loockVector, Vector3 startMoveVect)
    {
        moveVector = startMoveVect + (loockVector * flySpeed);
        StartCoroutine(FlyRoutine());
    }

    private IEnumerator FlyRoutine()
    {
        do
        {
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
}

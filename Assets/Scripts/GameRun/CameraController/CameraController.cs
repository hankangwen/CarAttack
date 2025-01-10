using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool ignoreTargetXPos;
    private Coroutine followingRoutine;
    private Car car;

    public void Init(Car car)
    {
        this.car = car;
    }


    public void StartFollowingCar()
    {
        if (followingRoutine != null) return;
        followingRoutine = StartCoroutine(carFollowing());
    }

    public void StopFollowingCar()
    {
        if (followingRoutine == null) return;
        StopCoroutine(followingRoutine);
        followingRoutine = null;
    }

    private IEnumerator carFollowing()
    {
        do
        {
            Vector3 targetPos = car.transform.position;
            if (ignoreTargetXPos) targetPos.x = 0;
            transform.position = targetPos + offset;
            yield return new WaitForEndOfFrame();
        } while (true);
    }
}

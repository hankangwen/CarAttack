using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Quaternion carFollowRotation;
    [SerializeField] private bool ignoreTargetXPos;
    [Header("Idle pos")]
    [SerializeField] private Vector3 camIdlePos;
    [SerializeField] private Quaternion camIdleRotation;
    private Coroutine followingRoutine;
    private Car car;

    public void Init(Car car)
    {
        this.car = car;
    }

    #region car following
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

    public IEnumerator moveToCarFollowingPositionAndRotation()
    {
        return moveToOrientation(car.transform.position + offset, carFollowRotation);
    }

    public IEnumerator moveToIdleFollowingPositionAndRotation()
    {
        return moveToOrientation(camIdlePos, camIdleRotation);
    }
    #endregion

    public IEnumerator moveToOrientation(Vector3 position, Quaternion rotation)
    {
        yield return new WaitForEndOfFrame();
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOMove(position, 1))
            .Join(transform.DORotateQuaternion(rotation, 1));
        bool animCompleted = false;
        sequence.OnComplete(() =>
        {
            animCompleted = true;
            transform.position = position;
            transform.rotation = rotation;
        });

        while (!animCompleted)
        {
            yield return new WaitForEndOfFrame();
        }
    }


}

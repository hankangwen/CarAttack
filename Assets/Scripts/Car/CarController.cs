using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class CarController : MonoBehaviour
{
    [SerializeField] private Swing swing;
    [SerializeField] private float accelerationTime;

    private Car car;
    private Coroutine carMoveRoutine;

    void Start()
    {
        car = GetComponent<Car>();
    }

    public void Move(Vector3 newPosition, float deltaTime)
    {
        newPosition += new Vector3(swing.GetSwingX(deltaTime), 0);
        car.movement.Move(newPosition, deltaTime);
    }

    public void MoveToDestination(Vector3 destination, float speed, Action onReachCallback)
    {
        swing.Reset();
        carMoveRoutine = StartCoroutine(MoveRoutine(destination, speed, onReachCallback));
    }

    public void Stop()
    {
        if (carMoveRoutine == null) return;
        StopCoroutine(carMoveRoutine);
    }

    private IEnumerator MoveRoutine(Vector3 destination, float speed, Action onReachCallback)
    {
        Vector3 startPos = car.transform.position;
        Vector3 targetPos = car.transform.position;
        float pathLength = targetPos.z - startPos.z;
        float moveDur = 0.01f;
        do
        {
            float speedCoef = Mathf.Lerp(0,1,moveDur / accelerationTime);
            float deltaTime = Time.deltaTime;
            targetPos.x = swing.GetSwingX(deltaTime * speedCoef);
            targetPos += new Vector3(0, 0,speed * deltaTime * speedCoef);
            car.movement.Move(targetPos, deltaTime);
            moveDur += deltaTime;
            yield return new WaitForEndOfFrame();
        } while (destination.z - car.transform.position.z > 0.1f);
        onReachCallback?.Invoke();
        carMoveRoutine = null;
    }
}

[Serializable]
public class Swing
{
    [SerializeField] private float swingDistance = 0.5f;
    [SerializeField] private float swingDuration = 10;

    private float XPos;
    private float elapsedTime;

    public float GetSwingX(float deltaTime)
    {
        if (swingDuration <= 0)
            throw new ArgumentException("swingDuration должен быть больше нуля.");
        elapsedTime += deltaTime;
        if (elapsedTime == 0) return 0;
        float normalizedTime = (elapsedTime % swingDuration) / swingDuration;
        XPos = swingDistance * Mathf.Sin(normalizedTime * 2 * Mathf.PI);

        return XPos;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }
}
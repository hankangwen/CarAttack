using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Car))]
public class CarController : MonoBehaviour
{
    [SerializeField] private Swing swing;
    private Car car;

    void Start()
    {
        car = GetComponent<Car>();
    }

    public void Move(Vector3 newPosition, float deltaTime)
    {
        
        newPosition += new Vector3(swing.GetSwingX(deltaTime),0);
        car.movement.Move(newPosition);
    }
}

[System.Serializable]
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
        float normalizedTime = (elapsedTime % swingDuration) / swingDuration;
        XPos = swingDistance * Mathf.Sin(normalizedTime * 2 * Mathf.PI);

        return XPos;
    }

    public void Reset()
    {
        elapsedTime = 0;
    }
}
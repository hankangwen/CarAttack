using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool ignoreTargetXPos;
    private Car car;

    public void Init(Car car)
    {
        this.car = car;
    }

    public void Follow(float deltaTime)
    {
        Vector3 targetPos = car.transform.position;
        if (ignoreTargetXPos) targetPos.x = 0;
        transform.position = targetPos + offset;
    }
}

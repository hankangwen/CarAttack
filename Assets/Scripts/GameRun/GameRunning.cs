using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class GameRunning : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float enemyStartSpawnDist;
    [SerializeField] private EnvironmentGenerator mapGenerator;
    [SerializeField] private EnemySpawner enemySpawner;

    private Coroutine gameRunRoutine;

    private void Start()
    {
        CarController carController = FindObjectOfType<CarController>();
        CameraController camControler = FindObjectOfType<CameraController>();
        Car car = carController.GetComponent<Car>();
        camControler.Init(car);
        mapGenerator.Init(car);
        mapGenerator.GenerateMap(transform.position);
        enemySpawner.Init(car);
        StartRun(car, carController, camControler);
    }

    public void StartRun(Car car, CarController carController, CameraController camControler)
    {
        if (gameRunRoutine != null) throw new System.Exception("Map.StartRun failed. Cant start GameRunningRoutine twoce");
        gameRunRoutine = StartCoroutine(GameRunningRoutine(car, carController, camControler));
    }

    private IEnumerator GameRunningRoutine(Car car, CarController carController, CameraController camControler)
    {
        bool stopGame = false;
        Action stopGameAction = () => stopGame = true;
        Vector3 targetCarPosition = carController.transform.position;
        enemySpawner.StartSpawning(transform.position + new Vector3(0,0, enemyStartSpawnDist));
        car.onDie += stopGameAction;
        camControler.StartFollowingCar();
        car.Activate();
        do
        {
            float deltaTime = Time.deltaTime;

            targetCarPosition += new Vector3(0, 0, moveSpeed * deltaTime);
            carController.Move(targetCarPosition, deltaTime);
            mapGenerator.CustomUpdate(deltaTime);
            enemySpawner.CustomUpdate(deltaTime);
            if(stopGame) break;
            yield return new WaitForEndOfFrame();
        } while (true);
        camControler.StopFollowingCar();
        car.Deactivate();
        car.onDie -= stopGameAction;
    }
}

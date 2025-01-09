using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Map : MonoBehaviour
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
        StartRun(carController, camControler);
    }

    public void StartRun(CarController carController, CameraController camControler)
    {
        if (gameRunRoutine != null) throw new System.Exception("Map.StartRun failed. Cant start GameRunningRoutine twoce");
        gameRunRoutine = StartCoroutine(GameRunningRoutine(carController, camControler));
    }

    private IEnumerator GameRunningRoutine(CarController carController, CameraController camControler)
    {
        Vector3 targetCarPosition = carController.transform.position;
        enemySpawner.StartSpawning(transform.position + new Vector3(0,0, enemyStartSpawnDist));
        do
        {
            float deltaTime = Time.deltaTime;

            targetCarPosition += new Vector3(0, 0, moveSpeed * deltaTime);
            carController.Move(targetCarPosition, deltaTime);
            camControler.Follow(deltaTime);
            mapGenerator.CustomUpdate(deltaTime);
            enemySpawner.CustomUpdate(deltaTime);
            yield return new WaitForEndOfFrame();
        } while (true);
    }
}

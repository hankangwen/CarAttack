using System;
using System.Collections;
using UnityEngine;

public class GameRunning : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float enemyStartSpawnDist;
    [SerializeField] private float distFromEndToStopEnemySpawn = 20;
    [SerializeField] private EnvironmentGenerator mapGenerator;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private ScreensSwitcher screenSwitcher;

    private Coroutine gameRunRoutine;

    private Car car;
    private CarController carController;
    private CameraController camControler;

    public Action<float> onGameStarts;
    public Action<float> onRunProgressChange;
    public void Init(Car car, CarController carController, CameraController camControler)
    {
        this.car = car;
        this.carController = carController;
        this.camControler = camControler;
        camControler.Init(car);
        mapGenerator.Init(car);
        mapGenerator.GenerateMap(transform.position);
        enemySpawner.Init(car);
    }

    public void StartRun(float runLength)
    {
        if (gameRunRoutine != null) throw new Exception("Map.StartRun failed. Cant start GameRunningRoutine twoce");
        onGameStarts?.Invoke(runLength);
        gameRunRoutine = StartCoroutine(GameRunningRoutine(runLength));
    }

    private IEnumerator GameRunningRoutine(float runLength)
    {
        bool gameFinished = false;
        Action stopGameAction = () =>
        {
            GameLoosed();
            gameFinished = true;
        };
        Vector3 targetCarPosition = transform.position + new Vector3(0,0, runLength);
        enemySpawner.StartSpawning(transform.position + new Vector3(0,0, enemyStartSpawnDist), runLength - distFromEndToStopEnemySpawn);
        car.onDie += stopGameAction;
        yield return camControler.moveToCarFollowingPositionAndRotation();

        camControler.StartFollowingCar();
        carController.MoveToDestination(targetCarPosition, moveSpeed, () =>
        {
            GameWon();
            gameFinished = true;
        });
        car.Activate();
        do
        {
            float deltaTime = Time.deltaTime;
            mapGenerator.CustomUpdate(deltaTime);
            onRunProgressChange?.Invoke(Mathf.Lerp(0, 1, (car.transform.position.z - transform.position.z)/runLength));
            if (gameFinished)
            {
                car.onDie -= stopGameAction;
                break;
            }
            yield return new WaitForEndOfFrame();
        } while (true);
        gameRunRoutine = null;
    }

    public void GameLoosed()
    {
        carController.Stop();
        camControler.StopFollowingCar();
        car.Deactivate();
        screenSwitcher.SwitchScreen(UIScreeen.LooseWindow);
    }

    public void GameWon()
    {
        enemySpawner.StopSpawningAndClearEnemies();
        camControler.StopFollowingCar();
        car.Deactivate();
        car.ResetCar();
        screenSwitcher.SwitchScreen(UIScreeen.WinWindow);
    }
}

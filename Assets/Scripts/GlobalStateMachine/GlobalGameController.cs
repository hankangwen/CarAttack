using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GlobalGameController : MonoBehaviour, IOrderedInitializable
{
    [SerializeField] private GameRunning gameRunning;
    [SerializeField] private ScreensSwitcher screenSwitcher;

    private CarController carController;
    private CameraController camControler;
    private Car car;

    public GlobalGameState gameState {  get; private set; }

    public string initOrderId => new InitOrderByType(InitOrder.MainModules);

    [Inject]
    private void Construct(GlobalInitSystems initSys)
    {
        initSys.order.AddToInitOrder(this);
    }

    public void PerformInit(InitArgs initializeArgs)
    {
        carController = FindObjectOfType<CarController>();
        camControler = FindObjectOfType<CameraController>();
        car = carController.GetComponent<Car>();
        gameRunning.Init(car, carController, camControler);
        screenSwitcher.SwitchScreen(UIScreeen.MainMenu);
    }

    public void RunGame()
    {
        if (gameState == GlobalGameState.GameRunning) throw new System.Exception("Can't run game twice. Wait for this run to end");
        screenSwitcher.SwitchScreen(UIScreeen.GameRunning);
        gameRunning.StartRun(100);
        gameState = GlobalGameState.GameRunning;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
        //car.ResetCar();
        //car.transform.position = Vector3.zero;
        //screenSwitcher.SwitchScreen(UIScreeen.MainMenu);
        //gameState = GlobalGameState.Menu;
    }

    public void GetReadyForNewRun()
    {
        car.ResetCar();
        Vector3 carOldPos = car.transform.position;
        car.transform.position = new Vector3(0,0, -17.67f);
        camControler.transform.position += car.transform.position - carOldPos;
        StartCoroutine(camControler.moveToIdleFollowingPositionAndRotation());

        screenSwitcher.SwitchScreen(UIScreeen.MainMenu);
        gameState = GlobalGameState.Menu;
    }
}

public enum GlobalGameState
{
    Menu,
    GameRunning
}
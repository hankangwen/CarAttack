using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameRunProgressBar : MonoBehaviour, IOrderedInitializable
{
    [SerializeField] private GameRunning gameRunState;
    [SerializeField] private Slider runProgressBar;
    [SerializeField] private TextMeshProUGUI movedMetersDisp;

    private float runLength;

    public string initOrderId => new InitOrderByType(InitOrder.Other);

    [Inject]
    private void Construct(GlobalInitSystems initSys)
    {
        initSys.order.AddToInitOrder(this);
    }

    public void PerformInit(InitArgs initializeArgs)
    {
        gameRunState.onGameStarts += Init;
        gameRunState.onRunProgressChange += OnProgressChanged;
    }

    public void Init(float runLength)
    {
        this.runLength = runLength;
        runProgressBar.value = 0;
        SetRunMeters(0);
    }

    public void OnProgressChanged(float newProgress)
    {
        runProgressBar.value = newProgress;
        SetRunMeters(newProgress * runLength);
    }

    private void SetRunMeters(float meters)
    {
        int metersConverted = (int)meters;
        movedMetersDisp.text = $"{metersConverted}m";
    }
}

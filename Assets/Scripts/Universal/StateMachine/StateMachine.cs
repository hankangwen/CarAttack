using System.Linq;
using UnityEngine;


public class StateManager : MonoBehaviour
{
    [SerializeField] private State[] allPossibleStates;

    public bool isRunning { get; private set; }

    private State currentState;
    private State _nextState;

    public void Run(string stateID)
    {
        isRunning = true;
        currentState = allPossibleStates.FirstOrDefault(State => State.stateID == stateID);
        currentState?.RunOnStart();
    }

    public void Stop()
    {
        currentState?.RunOnExit();
        isRunning = false;
    }

    public void Init(InitArgs args)
    {
        foreach (State state in allPossibleStates)
        {
            try
            {
                state.Init(args);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"StateManager.Init. Failed initialization of state \"{state.stateID}\" " +
                    $"with exception: {ex.Message}");
            }

        }
    }

    private void Update() { if(isRunning) RunStateMachine(); }

    private void RunStateMachine()
    {
        _nextState = currentState?.RunCurrentState(Time.deltaTime);
        if (_nextState != currentState) SwitchToTheNextState(_nextState);
    }

    private void SwitchToTheNextState(State _nextState)
    {
        if(_nextState == null) return;
        if (currentState != _nextState)
        {
            currentState.RunOnExit();
            _nextState.RunOnStart();
        }
        currentState = _nextState;
    }
}
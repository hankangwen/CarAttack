using UnityEngine;


public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    [SerializeField] private float customUpdateInterval;

    [SerializeField] private State[] allPossibleStates;

    [SerializeField] private bool manualStart;
    private State _nextState;

    public State CurrentState => currentState;

    public void Run()
    {
        //StartCustomUpdate(customUpdateInterval);
        currentState?.RunOnStart();
    }

    public void Init()
    {

    }

    private void Update() { RunStateMachine(); }

    private void RunStateMachine()
    {
        _nextState = currentState?.RunCurrentState();
        if (_nextState != null) SwitchToTheNextState(_nextState);
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
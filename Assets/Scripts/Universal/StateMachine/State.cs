using UnityEngine;
using System;

public abstract class State : MonoBehaviour {

    public abstract string stateID { get; }

    public abstract State RunCurrentState(float deltaTime);

    public abstract void Init(InitArgs args);

    public virtual void RunOnStart() { }

    public virtual void RunOnExit() { }
}

public enum BasicStates { Idle, Attack }
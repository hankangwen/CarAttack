using UnityEngine;
using System;

public abstract class State : MonoBehaviour {

    [SerializeField] private string _stateID;
    public string stateID => _stateID;

    public abstract State RunCurrentState();

    public virtual void Init()
    {

    }

    public virtual State RunCurrentStateCustomUpdate() => this;

    public virtual void RunOnStart() { }

    public virtual void RunOnExit() { }
}

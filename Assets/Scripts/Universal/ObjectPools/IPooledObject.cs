using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
    public void ResetAndDeactivate();
    public void Activate();

    public void Init();
}

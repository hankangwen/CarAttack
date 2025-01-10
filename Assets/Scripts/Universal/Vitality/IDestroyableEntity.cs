using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyableEntity
{
    public Vitality vitality { get; }
    public Action onDie { get; set; }

    public bool isAlive => vitality.hp > 0;
}

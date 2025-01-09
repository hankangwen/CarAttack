using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Vitality
{
    [SerializeField] private float maxHp;
    private float hp;

    /// <summary>
    /// return hp percent (0 - no hp, 1 - full hp)
    /// </summary>
    public Action<float> onHealthChange { get; set; }
    public Action onHealthFinishes { get; set; }

    public void Init()
    {
        hp = maxHp;
    }

    public void DealDamage(float damage)
    {
        if (damage == 0) return;
        hp -= damage;
        if(hp < 0)
        {
            hp = 0;
            onHealthChange?.Invoke(0);
            onHealthFinishes?.Invoke();
        }
        else onHealthChange?.Invoke(hp / maxHp);

    }
}

using System;
using UnityEngine;

public interface IOrderedInitializable
{
    public string initOrderId { get; }
    public void PerformInit(InitArgs initializeArgs);
}


public abstract class InitOrderObj
{
    public abstract string orderId { get; }
}


[Serializable]
public class InitOrderByType : InitOrderObj
{
    [SerializeField] private InitOrder initType;

    public InitOrderByType(InitOrder initType)
    {
        this.initType = initType;
    }

    public static implicit operator string(InitOrderByType obj)
    {
        return obj.orderId.ToString();
    }

    public override string orderId => initType.ToString();//InitOrderValues.getInitNumByTypeString(initType.ToString());
}

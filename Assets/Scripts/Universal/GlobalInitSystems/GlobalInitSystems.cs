using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Initializes objects
/// </summary>
public class GlobalInitSystems : MonoBehaviour
{
    private Order _order = new Order();
    public Order order => _order;

    private void Start()
    {
        InitializeSystems(new InitArgs());
    }
    public void InitializeSystems(InitArgs initializeArgs)
    {
        if (initializeArgs == null)
        {
            Debug.LogError("GlobalInitSystems init failed. initArgs is null");
            return;
        }
        DOTween.Init();//dotween should be initialized before everithing else
        List<IOrderedInitializable>[] toInitStages = _order.getInitializationOrder();
        foreach (List<IOrderedInitializable> initStage in toInitStages)
        {
            foreach (IOrderedInitializable initTarget in initStage)
            {
                try
                {
                    initTarget.PerformInit(initializeArgs);
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("Some of init services failed with exception:{0}\n {1}",ex.Message, ex.StackTrace);
                }
            }
        }
    }

    public class Order 
    {
        private Dictionary<int, List<IOrderedInitializable>> order = new Dictionary<int, List<IOrderedInitializable>>();

        public void AddToInitOrder(IOrderedInitializable element)
        {
            List<IOrderedInitializable> initList;
            int initOrderNum = InitOrderValues.getInitNumByIdString(element.initOrderId);
            if (!order.TryGetValue(initOrderNum, out initList))
            {
                initList = new List<IOrderedInitializable>();
                order.Add(initOrderNum, initList);
            }
            initList.Add(element);
        }

        public List<IOrderedInitializable>[] getInitializationOrder()
        {
            List<List<IOrderedInitializable>> stagesOrder = new List<List<IOrderedInitializable>>();
            int[] stagesOrderNums = order.Keys.ToArray();
            Array.Sort(stagesOrderNums);
            foreach (int num in stagesOrderNums)
            {
                List<IOrderedInitializable> stage;
                if (!order.TryGetValue(num, out stage))
                {
                    Debug.LogError("Impossiburrrrrrr!");
                    continue;
                }
                stagesOrder.Add(stage);
            }
            return stagesOrder.ToArray();
        }
    }
}

public enum GlobalInitInitType { GameScene, MetaScene, Dungeon }

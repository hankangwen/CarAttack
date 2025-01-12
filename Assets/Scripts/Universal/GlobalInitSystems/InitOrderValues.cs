using System.Collections.Generic;
using UnityEngine;

public static class InitOrderValues
{
    //to do: create ScriptableObject config file for order. And create 2 different instances for meta scene and game scene
    public static Dictionary<string, int> initOrderByType = new Dictionary<string, int>()
    {
        { InitOrder.Tutorial.ToString() , 1},
        { InitOrder.MainModules.ToString() , 2},
        { InitOrder.Lights.ToString() , 3},
        { InitOrder.DayNightCycle.ToString() , 4},
        { InitOrder.Buildings.ToString() , 5},
        { InitOrder.ProductionBases.ToString() , 6},
        { InitOrder.BuildActPanelControllers.ToString(), 7},
        { InitOrder.TownExpandModules.ToString() , 8},
        { InitOrder.DefenceLines.ToString() , 9},
        { InitOrder.Units.ToString(), 10},
        { InitOrder.UnitsGlobalManager.ToString(), 11},
        { InitOrder.RecruitmentSystems.ToString(), 12},
        { InitOrder.Other.ToString(), 13},
        { InitOrder.CutsceneControllers.ToString(), 14}
    };

    public static int getInitNumByIdString(string id)
    {
        int initNum;
        if(!initOrderByType.TryGetValue(id, out initNum))
        {
            Debug.LogError($"InitOrderValues does not contain order configuration for id \"{id}\". Returning 0");
            return 0;
        }
        return initNum;
    }
}

public enum InitOrder 
{ 
    Tutorial,
    MainModules,
    Lights,
    DayNightCycle,
    Buildings,
    ProductionBases,
    BuildActPanelControllers,
    TownExpandModules,
    DefenceLines, 
    /// <summary>
    /// for units that adds to level by drag their prefab to it(will be removed. Do not use)
    /// </summary>
    Units, 
    /// <summary>
    /// for system that spawn units by data from storage
    /// </summary>
    UnitsGlobalManager,
    RecruitmentSystems,
    Other,
    CutsceneControllers
}
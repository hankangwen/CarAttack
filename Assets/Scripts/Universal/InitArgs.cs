using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// container for init arguments
/// </summary>
public class InitArgs
{
    private Dictionary<string, Argument> argsDict = new Dictionary<string, Argument>();

    #region adding arguments
    public InitArgs AddArgument<T>(object argValue) where T : class
    {
        Type type = typeof(T);
        return AddArgument(type.ToString(), argValue);
    }

    public InitArgs AddArgument(string key, object argValue)
    {
        if (argsDict.ContainsKey(key))
        {
            Debug.LogError($"Trying to add argument of type \"{key}\" mulitple times. Will be used first version of argument");
            return this;
        }
        argsDict.Add(key, new Argument(argValue));
        return this;
    }

    #endregion

    #region recieving arguments
    public T GetArgumentByKey<T>(string key) where T : class
    {
        Type generiicType = typeof(T);
        if (argsDict.TryGetValue(key, out Argument result))
        {
            if (result == null) return null;
            if (result.type == generiicType)
                return (T)Convert.ChangeType(result.value, generiicType);
        }
        return null;
    }

    public T GetArgument<T>() where T : class
    {
        Type generiicType = typeof(T);
        if (argsDict.TryGetValue(generiicType.ToString(), out Argument result))
        {
            if (result == null) return null;
            if (result.type == generiicType)
                return (T)Convert.ChangeType(result.value, generiicType);
        }
        throw new Exception($"InitArgs does not contain arg \"{generiicType}\"");
    }

    #endregion
    private class Argument
    {
        public Type type { get; private set; }
        public object value { get; private set; }

        public Argument(object argument)
        {
            this.type = argument.GetType();
            this.value = argument;
        }
    }
}
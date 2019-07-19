using System;
using System.Collections.Generic;
using UnityEngine;

public static class Container
{
    public static Dictionary<Type, IController> Controllers = new Dictionary<Type, IController>();

    public static void Add(Type key, IController value, bool initializeInstantly = false)
    {
        if (!Controllers.ContainsKey(key))
            Controllers.Add(key, value);
        else
            Controllers[key] = value;

        if(initializeInstantly)
            value.Initialize();
    }

    public static T Get<T>() where T : class, IController
    {
        var type = typeof(T);
        if (Controllers.ContainsKey(type))
            return Controllers[type] as T;

        Debug.LogError($"Controller {type} doesn't exist in container.");
        return null;
    }

    public static bool Contains(Type key)
    {
        return Controllers.ContainsKey(key);
    }

    public static void Remove(Type key)
    {
        if (Controllers.ContainsKey(key))
            Controllers.Remove(key);
    }
}

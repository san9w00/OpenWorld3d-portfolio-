using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static Dictionary<Type, Delegate> events = new();

    public static void Subscribe<T>(Action<T> callBack)
    {
        var type = typeof(T);

        if (events.ContainsKey(type))
            events[type] = Delegate.Combine(events[type], callBack);
        else
            events[type] = callBack;
    }

    public static void UnSubscribe<T>(Action<T> callBack)
    {
        var type = typeof(T);

        if (!events.ContainsKey(type)) return;

        var current = Delegate.Remove(events[type], callBack);

        if (current == null)
            events.Remove(type);
        else
            events[type] = current;
    }

    public static void Publish<T>(T eventData)
    {
        var type = typeof(T);

        if (events.TryGetValue(type, out var del))
        {
            (del as Action<T>)?.Invoke(eventData);
        }
    }

    public static void Clear()
    {
        events.Clear();
    }
}

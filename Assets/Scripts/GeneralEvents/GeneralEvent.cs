using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralEvent", menuName = "Events/GeneralEvent")]
public class GeneralEvent : ScriptableObject
{
    public List<GeneralEventListener> listeners = new List<GeneralEventListener>();

    public void Raise(EventArgs eventArgs)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised(eventArgs);
        }
    }

    public void Raise()
    {
        Raise(EventArgs.Empty);
    }

    public void RegisterListener(GeneralEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(GeneralEventListener listener)
    {
        listeners.Remove(listener);
    }
}

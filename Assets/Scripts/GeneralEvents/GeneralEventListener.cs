using System;
using UnityEngine;
using UnityEngine.Events;

public class GeneralEventListener : MonoBehaviour
{
    public GeneralEvent Event;
    public UnityEvent<EventArgs> Response;

    private void OnEnable()
    {
        Event.RegisterListener(this);
    }

    private void OnDisable()
    {
        Event.UnregisterListener(this);
    }

    public void OnEventRaised(EventArgs args)
    {
        Response.Invoke(args);
    }
}
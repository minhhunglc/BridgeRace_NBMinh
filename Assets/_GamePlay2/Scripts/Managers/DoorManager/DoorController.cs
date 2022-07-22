using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IDoTweenPackage
{
    [SerializeField] private int id;

    public void DoDoTween(float y, float duration)
    {
        transform.DOMoveY(y, duration);
    }

    void Start()
    {
        GameEventsState.gameEvents.onDoorTrigger += OnDoorOpen;
        GameEventsState.gameEvents.offDoorTrigger += OnDoorClose;

    }

    private void OnDoorOpen(int id)
    {
        if (id == this.id)
        {
            DoDoTween(11.8f, .5f);
        }
    }

    private void OnDoorClose(int id)
    {
        if (id == this.id)
        {
            DoDoTween(6.54f, .5f);
        }
    }

    private void OnDestroy()
    {
        GameEventsState.gameEvents.onDoorTrigger -= OnDoorOpen;
        GameEventsState.gameEvents.offDoorTrigger -= OnDoorClose;

    }
}

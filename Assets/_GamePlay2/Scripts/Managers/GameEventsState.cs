using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsState : MonoBehaviour
{
    public static GameEventsState gameEvents;

    void Awake()
    {
        gameEvents = this;
    }
    public event Action<int> onDoorTrigger;
    public event Action<int> offDoorTrigger;

    public void DoorEnter(int id)
    {
        if (onDoorTrigger != null)
        {
            onDoorTrigger(id);
        }
    }

    public void DoorExit(int id)
    {
        if (offDoorTrigger != null)
        {
            offDoorTrigger(id);
        }
    }
    public event Action<int> onBricksChange;

    public bool is_blue, is_red, is_green = false;

    public void BrickPositionChanged(int id)
    {
        if (onBricksChange != null)
        {
            onBricksChange(id);
        }


    }
    void OnDisable()
    {
        if (!this.gameObject.scene.isLoaded) return;
    }
}
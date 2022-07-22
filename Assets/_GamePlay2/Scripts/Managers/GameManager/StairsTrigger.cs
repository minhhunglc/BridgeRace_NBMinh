using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsTrigger : MonoBehaviour
{
    [SerializeField] private int id;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_PLAYER))
        {
            GameEventsState.gameEvents.is_blue = true;
            GameEventsState.gameEvents.BrickPositionChanged(id);
        }

        else
        {
            GameEventsState.gameEvents.is_red = true;
            GameEventsState.gameEvents.is_green = true;
            GameEventsState.gameEvents.BrickPositionChanged(id);
        }
    }
}

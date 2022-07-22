using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairsGenerateManager : MonoBehaviour
{
    public int id;

    [SerializeField] private float x, y, z;

    [SerializeField] private GameObject Blue_Bricks, Red_Bricks, Green_Bricks;

    void Start()
    {
        GameEventsState.gameEvents.onBricksChange += Change_Bricks;
    }


    private void Change_Bricks(int id)
    {
        if (id == this.id)
        {
            if (GameEventsState.gameEvents.is_blue)
            {
                Blue_Bricks.transform.DOMove(new Vector3(x, y, z), .2f);
            }

            else if (GameEventsState.gameEvents.is_red)
            {
                Red_Bricks.transform.DOMove(new Vector3(x + 2, y, z), .2f);
            }

            else if (GameEventsState.gameEvents.is_green)
            {
                Green_Bricks.transform.DOMove(new Vector3(x + 1, y, z), .2f);
            }
        }
    }

    void OnDestroy()
    {
        GameEventsState.gameEvents.onBricksChange -= Change_Bricks;
    }

}

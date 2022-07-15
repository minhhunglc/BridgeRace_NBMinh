using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectController : MonoBehaviour
{
    public PlayerBrickController brickController;
    public BrickGenerator brickGenerator;
    public string playerColorName;

    private void OnTriggerEnter(Collider other)
    {
        Brick brick = other.transform.GetComponent<Brick>();

        if (brick.colorName == playerColorName)
        {
            brickGenerator.MakeRemoved(brick.brickNumber);
            Destroy(other.gameObject);
            brickController.UpdatePlayerBricks();
        }
    }
}

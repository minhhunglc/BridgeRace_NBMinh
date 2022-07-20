using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectController : Singleton<PlayerCollectController>
{
    public PlayerBrickController brickController;
    public BrickGenerator brickGenerator;
    public string playerColorName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            VirtualJoystick.Ins.isPlaying = false;
            VirtualJoystick.Ins.playerAnimator.SetInteger("Result", 2);
        }

        else
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
}

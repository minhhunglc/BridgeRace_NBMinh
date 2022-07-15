using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    public const string Tag_PLAYER = "Player";
    public const string Name_COLOR = "red";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag_PLAYER) && gameObject.GetComponent<Brick>().colorName == Name_COLOR)
        {
            BrickCollector brickCollector;
            if (other.TryGetComponent(out brickCollector))
            {
                brickCollector.AddNewBrick(this.transform);
            }
        }
    }
}

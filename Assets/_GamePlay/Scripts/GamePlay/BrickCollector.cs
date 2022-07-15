using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BrickCollector : MonoBehaviour
{
    public const string Tag_BRIDGE = "Bridge";

    public Transform BrickHolderTransform;

    int NumOfBricksHolding = 0;

    public Queue<GameObject> bricks = new Queue<GameObject>();

    private void Update()
    {
        Debug.Log(bricks.Count);

    }
    public void AddNewBrick(Transform _brickToAdd)
    {
        _brickToAdd.SetParent(BrickHolderTransform, true);
        _brickToAdd.localPosition = new Vector3(0, 0.038f * NumOfBricksHolding, -0.03f);
        _brickToAdd.localRotation = Quaternion.identity;
        NumOfBricksHolding++;
        bricks.Enqueue(_brickToAdd.gameObject);
    }
    public void SubBrick()
    {
        bricks.Dequeue();
        Destroy(bricks.Peek());
        NumOfBricksHolding--;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tag_BRIDGE))
        {
            if (bricks.Count != 0)
            {
                SubBrick();
                Destroy(other.gameObject);
                return;
            }

        }
    }
}

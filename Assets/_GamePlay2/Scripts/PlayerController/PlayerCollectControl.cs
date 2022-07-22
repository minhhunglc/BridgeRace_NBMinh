using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectControl : MonoBehaviour
{
    [SerializeField] private Transform collectedObjects;
    [SerializeField] private GameObject prevBrick;
    [SerializeField] private List<GameObject> bricks = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.StartsWith(transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
        {
            AddBricks(other);
        }
        if (bricks.Count > 1 && other.gameObject.CompareTag(Constant.TAG_SETR) || bricks.Count > 1 && other.gameObject.CompareTag(Constant.TAG_SET + transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)) == false && other.gameObject.tag.StartsWith(Constant.TAG_SET))
        {
            SubtractBricks(other);
        }
        if (bricks.Count == 1 && other.gameObject.CompareTag(Constant.TAG_SET) || other.gameObject.CompareTag(Constant.TAG_SETR) || other.gameObject.CompareTag(Constant.TAG_SETG))
        {
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
        }

    }

    private void SubtractBricks(Collider other)
    {
        var othergetMeshRenderer = other.GetComponent<MeshRenderer>();
        var transformGetComponent = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;

        GameObject obje = bricks[bricks.Count - 1];
        bricks.RemoveAt(bricks.Count - 1);
        Destroy(obje);

        othergetMeshRenderer.material = transformGetComponent;
        othergetMeshRenderer.enabled = true;

        other.tag = Constant.TAG_SET + transformGetComponent.name.Substring(0, 1);

        prevBrick = bricks[bricks.Count - 1];
    }

    private void AddBricks(Collider other)
    {
        other.transform.SetParent(collectedObjects);
        Vector3 pos = prevBrick.transform.localPosition;
        pos.y += 0.22f;
        pos.z = 0;
        pos.x = 0;
        other.transform.localRotation = new Quaternion(0, 0.7f, 0, 0.7f);
        other.transform.DOLocalMove(pos, 0.2f);
        prevBrick = other.gameObject;
        bricks.Add(other.gameObject);

        other.tag = Constant.TAG_UNTAGGED;

        BricksGenerate.Ins.GenerateCube(1);
    }
}

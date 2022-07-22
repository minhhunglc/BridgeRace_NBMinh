using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AI_BOT
{
    red = 0,
    green = 2
}


public class AIBotCollect : MonoBehaviour
{
    public List<GameObject> targets = new List<GameObject>();
    public List<GameObject> cubes = new List<GameObject>();

    public bool haveTarget = false;

    public int collected_Random_Number = 5;
    //[SerializeField] private int collected_max, collected_min;

    public Transform finalPoint;

    [Header("Collection")]
    [SerializeField] private Transform _collectedObjects;
    [SerializeField] private GameObject _prevObject;


    public AI_BOT aisEnum;

    int multiplier = 0;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_FINAL))
        {
            SetNewPathForAI();
        }

        if (other.gameObject.tag.StartsWith(transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
        {
            AddBrickAI(other);
        }

        else if (other.gameObject.CompareTag(Constant.TAG_SETB) || other.gameObject.CompareTag(Constant.TAG_SET + transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)) == false && other.gameObject.tag.StartsWith(Constant.TAG_SET))
        {
            SubtractBrickAI(other);
        }

    }

    private void SubtractBrickAI(Collider other)
    {
        if (cubes.Count > 1)
        {
            GameObject obje = cubes[cubes.Count - 1];
            cubes.RemoveAt(cubes.Count - 1);
            Destroy(obje);

            other.GetComponent<MeshRenderer>().material = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material;
            other.GetComponent<MeshRenderer>().enabled = true;

            other.tag = Constant.TAG_SET + transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1);
            multiplier = 0;

        }
        else
        {
            _prevObject = cubes[0].gameObject;
            haveTarget = false;
        }
    }

    private void AddBrickAI(Collider other)
    {
        other.transform.SetParent(_collectedObjects);
        Vector3 pos = _prevObject.transform.localPosition;

        multiplier += 1;

        pos.y += 0.22f * multiplier;
        pos.z = 0;
        pos.x = 0;

        other.transform.localRotation = new Quaternion(0, 0.7f, 0, 0.7f);
        other.transform.DOLocalMove(pos, 0.2f);

        cubes.Add(other.gameObject);

        targets.Remove(other.gameObject);
        other.tag = Constant.TAG_UNTAGGED;
        haveTarget = false;

        BricksGenerate.Ins.GenerateCube((int)aisEnum, this);
    }

    private void SetNewPathForAI()
    {
        gameObject.transform.position = new Vector3(-6.9f, 6.58f, 39.68f);
        GameEventsState.gameEvents.is_green = true;
        GameEventsState.gameEvents.is_red = true;
        FindObjectOfType<AIBotController>().agent.SetDestination(finalPoint.transform.position);
    }
}

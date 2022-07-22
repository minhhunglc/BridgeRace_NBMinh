using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBotController : MonoBehaviour
{
    [SerializeField] private GameObject _targetParents;

    public Transform[] ropes;

    private Vector3 targetTransform;
    public Animator animator;

    public NavMeshAgent agent;

    [SerializeField] private float _radius = 2f;

    [SerializeField] private int _random_number;

    public AIBotCollect aiBotCollect;

    void Start()
    {
        for (int i = 0; i < _targetParents.transform.childCount; i++)
        {
            aiBotCollect.targets.Add(_targetParents.transform.GetChild(i).gameObject);
        }
    }

    void Running_Anim()
    {
        if (!animator.GetBool(Constant.PREFERENCE_ANIM_RUN))
        {
            animator.SetBool(Constant.PREFERENCE_ANIM_RUN, true);
        }
    }

    void FixedUpdate()
    {
        if (!aiBotCollect.haveTarget && aiBotCollect.targets.Count > 0)
        {
            ChooseTarget();
        }
    }

    void ChooseTarget()
    {
        int randomNumber = Random.Range(0, _random_number);
        try
        {
            if (randomNumber == 0 && aiBotCollect.cubes.Count >= aiBotCollect.collected_Random_Number)
            {
                int randomRope = Random.Range(0, ropes.Length);
                List<Transform> ropesNonActiveChild = new List<Transform>();
                foreach (Transform bricks_item in ropes[randomRope])
                {
                    if (!bricks_item.GetComponent<MeshRenderer>().enabled || bricks_item.GetComponent<MeshRenderer>().enabled && bricks_item.gameObject.CompareTag("Set" + transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)) == false)
                    {
                        ropesNonActiveChild.Add(bricks_item);
                    }
                }

                targetTransform = aiBotCollect.cubes.Count > ropesNonActiveChild.Count ? ropesNonActiveChild[ropesNonActiveChild.Count - 1].position : ropesNonActiveChild[aiBotCollect.cubes.Count].position;
            }

            else
            {
                Collider[] hitCollide = Physics.OverlapSphere(transform.position, _radius);
                List<Vector3> colors = new List<Vector3>();
                CheckAnotherTarget(hitCollide, colors);

            }
        }

        catch
        {
            aiBotCollect.haveTarget = false;
        }


        agent.SetDestination(targetTransform);
        Running_Anim();
        aiBotCollect.haveTarget = true;

    }

    private void CheckAnotherTarget(Collider[] hitCollide, List<Vector3> colors)
    {
        for (int i = 0; i < hitCollide.Length; i++)
        {

            if (hitCollide.Length >= 1)
            {
                if (hitCollide[i].tag.StartsWith(transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.name.Substring(0, 1)))
                {
                    colors.Add(hitCollide[i].transform.position);
                }
            }
        }

        if (colors.Count > 0)
        {
            targetTransform = colors[0];
        }
        else
        {

            int random = Random.Range(0, aiBotCollect.targets.Count);
            targetTransform = aiBotCollect.targets[random].transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

}

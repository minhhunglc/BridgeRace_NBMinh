using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BricksGenerate : Singleton<BricksGenerate>
{

    public GameObject redCube, blueCube, greenCube;

    public Transform redCubeParent, greenCubeParent, blueCubeParent;

    public int minX, maxX, minZ, maxZ;

    public LayerMask layerMask;


    public void GenerateCube(int number, AIBotCollect agentCollect = null)
    {
        if (number == 0)
            Generate(redCube, redCubeParent, agentCollect);
        else if (number == 1)
            Generate(blueCube, blueCubeParent, agentCollect);
        else if (number == 2)
            Generate(greenCube, greenCubeParent, agentCollect);

    }

    private void Generate(GameObject gameObject, Transform parent, AIBotCollect agentCollectio = null)
    {
        GameObject g = Instantiate(gameObject);

        g.transform.parent = parent;

        Vector3 desPos = GiveRandomPos();

        g.SetActive(false);


        Collider[] colliders = Physics.OverlapSphere(desPos, 1, layerMask);
        while (colliders.Length != 0)
        {
            desPos = GiveRandomPos();
            colliders = Physics.OverlapSphere(desPos, 1, layerMask);
        }
        g.SetActive(true);
        g.transform.position = desPos;

        if (agentCollectio != null)
        {
            agentCollectio.targets.Add(g);
        }
    }


    private Vector3 GiveRandomPos()
    {
        return new Vector3(Random.Range(minX, maxX), blueCube.transform.position.y, Random.Range(minZ, maxZ));
    }

}

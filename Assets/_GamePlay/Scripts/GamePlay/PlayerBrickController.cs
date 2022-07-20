using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrickController : MonoBehaviour
{
    public float bricks;
    private float brickCount;

    [SerializeField]
    private Transform dummyBrick;
    [SerializeField]
    private Transform placedBrick;
    [SerializeField]
    private Transform brickArea;
    [SerializeField]
    private Transform brickPlacer;
    [SerializeField]
    private Transform placedBricks;

    private Vector3 bridgePos;

    public Color selectedColor;
    public string selectedColorName;

    public BrickGenerator brickGenerator;
    public const string Tag_BRICK = "Bridge";
    public const string Tag_WALL = "WallBridge";
    public const string Color_BRICKCOLOR = "_Color";

    CachedComponent<Brick> cachedBrick;
    CachedComponent<PlacedBrick> cachedPlacedBrick;
    CachedComponent<WayKeeper> cachedWayKeeper;
    void FixedUpdate()
    {

        CheckBridge();
    }

    private void CheckBridge()
    {
        RaycastHit hit;

        if (Physics.Raycast(brickPlacer.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            cachedPlacedBrick = new CachedComponent<PlacedBrick>(hit);
            cachedWayKeeper = new CachedComponent<WayKeeper>(hit);
            Debug.DrawRay(brickPlacer.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);

            if (cachedPlacedBrick.get() != null && cachedPlacedBrick.get().colorName != selectedColorName && bricks <= 0)
            {
                DontStepToOtherColorBrick();
            }
            else if (cachedPlacedBrick.get() != null && cachedPlacedBrick.get().colorName != selectedColorName && bricks > 0)
            {
                ReplaceOtherColorBrick(hit);
            }

            if (hit.collider.CompareTag(Tag_BRICK))
            {
                if (bricks > 0)
                {
                    PlaceBricks(hit);
                }
                else
                {
                    brickCount = cachedWayKeeper.get().bricksPlaced;
                    BlockFallOff();
                }
            }
            else if (hit.collider.CompareTag(Tag_WALL))
            {
                BlockFallOff();
            }

        }
        else
        {
            Debug.DrawRay(brickPlacer.position, transform.TransformDirection(Vector3.down) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    private void PlaceBricks(RaycastHit hit)
    {
        cachedWayKeeper = new CachedComponent<WayKeeper>(hit);
        brickCount = cachedWayKeeper.get().bricksPlaced;
        bridgePos = hit.collider.transform.position;

        Vector3 brickPlacement = new Vector3(bridgePos.x, bridgePos.y + (brickCount * 0.25f), bridgePos.z + (brickCount * 0.65f));

        Transform brick = Instantiate(placedBrick, brickPlacement, placedBrick.transform.rotation, placedBricks);

        brick.GetComponent<Renderer>().material.SetColor(Color_BRICKCOLOR, selectedColor);
        brick.GetComponent<PlacedBrick>().colorName = selectedColorName;

        cachedWayKeeper.get().bricksPlaced += 1;
        RemoveDummyBrick();
        brickGenerator.GenerateRemovedBrick();

    }

    private void BlockFallOff()
    {
        Vector3 playerPosition = transform.position;

        if (transform.position.z > bridgePos.z + (brickCount * 0.550f)) //0.40f
        {
            playerPosition.z = bridgePos.z + (brickCount * 0.550f);
        }

        transform.position = playerPosition;
    }

    private void RemoveDummyBrick()
    {
        GameObject lastChild = brickArea.GetChild(brickArea.childCount - 1).gameObject;
        Destroy(lastChild);
        bricks--;
    }

    public void UpdatePlayerBricks()
    {
        Vector3 brickPosition = new Vector3(brickArea.position.x, brickArea.position.y + (bricks * 0.2f), brickArea.position.z);
        Transform brick = Instantiate(dummyBrick, brickPosition, dummyBrick.transform.rotation, brickArea);
        brick.GetComponent<Renderer>().material.SetColor(Color_BRICKCOLOR, selectedColor);
        bricks++;
    }

    private void DontStepToOtherColorBrick()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.5f);
    }

    private void ReplaceOtherColorBrick(RaycastHit hit)
    {
        Vector3 newBrickposition = hit.transform.position;

        Destroy(hit.collider.gameObject);

        Transform newBrick = Instantiate(placedBrick, newBrickposition, placedBrick.transform.rotation);
        newBrick.GetComponent<Renderer>().material.SetColor(Color_BRICKCOLOR, selectedColor);
        newBrick.GetComponent<PlacedBrick>().colorName = selectedColorName;
        RemoveDummyBrick();
        brickGenerator.GenerateRemovedBrick();
    }

    private void Update()
    {
        brickArea.transform.rotation = Quaternion.Euler(0.0f, 0.0f, gameObject.transform.rotation.z * -1.0f);
    }

}

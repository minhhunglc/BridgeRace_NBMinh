using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerBag
{
    [Header("Bag Settings")]
    [SerializeField] private Transform _bagPosition;
    public Color color;
    [SerializeField] private GameObject _blockPrefabInBag;


    [HideInInspector] public int playerId;

    [HideInInspector] public int blockCount = 0;
    private int _maxBlockCount = 30;

    private List<GameObject> _blocks = new List<GameObject>();


    public bool BagIsFull() { return blockCount == _maxBlockCount - 1; }

    public void SpawnBlocks()
    {
        for (int i = 0; i < _maxBlockCount; i++)
        {
            Vector3 pos = _bagPosition.position + new Vector3(0, i * (_blockPrefabInBag.transform.localScale.y + .05f), 0);
            GameObject block = GameObject.Instantiate(_blockPrefabInBag, pos, _bagPosition.rotation);
            block.SetActive(false);
            block.transform.parent = _bagPosition;
            block.GetComponent<MeshRenderer>().material.color = color;
            _blocks.Add(block);
        }

    }

    public void AddBlock()
    {
        _blocks[blockCount].SetActive(true);

        blockCount++;
        if (blockCount > _maxBlockCount - 1)
            blockCount = _maxBlockCount - 1;
    }
    public void RemoveBlock()
    {
        blockCount--;
        _blocks[blockCount].SetActive(false);
        if (blockCount < 0)
            blockCount = 0;
    }

    public void DropAllBlocks(Transform tf)
    {
        if (blockCount == 0) return;

        int c = blockCount;

        ResetBag();

        for (int i = 0; i < c; i++)
        {
            Block b = SimplePool.Spawn(DataManager.Ins.BlockPrefab, new Vector3(tf.localPosition.x + Random.Range(-2, 2), tf.localPosition.y /*- .2f*/, tf.localPosition.z + Random.Range(-2, 2)), Quaternion.Euler(0, Random.Range(0, 180), 0)) as Block;
            b.gameObject.GetComponent<BoxCollider>().isTrigger = false;
            b.Id = -1;
        }

    }


    public void ResetBag()
    {
        blockCount = 0;
        for (int i = 0; i < _blocks.Count; i++)
            _blocks[i].SetActive(false);

    }
}
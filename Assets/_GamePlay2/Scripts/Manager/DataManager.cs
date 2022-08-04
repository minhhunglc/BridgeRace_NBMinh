using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public Block BlockPrefab;
    public Transform blockFalltf;

    private void Awake()
    {
        SimplePool.Preload(BlockPrefab, 30, blockFalltf);
        Debug.Log("OK");
    }
}
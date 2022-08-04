using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : GameUnit
{
    public int Id;
    public MeshRenderer _renderer;
    public Collider _collider;

    private void FixedUpdate()
    {
        if (transform.position.y < -4f)
        {
            transform.position = new Vector3(Random.Range(-4f, 4f), 0f, 0f);
        }
    }
    public void SetBlock(int id, Color color)
    {
        Id = id;
        _renderer.material.color = color;
    }

    public void CollectMe()
    {
        if (this.Id == -1)
        {
            _renderer.enabled = true;
            _collider.enabled = true;
        }
        else
        {
            _renderer.enabled = false;
            _collider.enabled = false;

            StartCoroutine(ShowMe());
        }
    }

    private IEnumerator ShowMe()
    {
        float time = Random.Range(5, 10);
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(true);
        _renderer.enabled = true;
        _collider.enabled = true;
    }
}
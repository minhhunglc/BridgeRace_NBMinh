using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : CharacterBase
{
    private Vector3 _spawnArea;
    private Vector3 _targetPosition;

    private bool _isBusy;
    private Coroutine _rotateCoroutine;

    private float horizontal = 0;
    private float vertical = 1;


    private void Start()
    {
        dropAction += DropBlock;
        collectAction += CollectBlock;
        collisionAction += CollisionAction;
        IsBot = true;

        GetNewStair();
    }

    #region Collision
    private new void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_BLOCK))
        {
            Block b = other.transform.GetComponent<Block>();
            if (b.Id == bag.playerId || b.Id == -1)
                SetTarget(other.transform.localPosition);
        }
        else if (other.CompareTag(Constant.TAG_BLOCKSPAWNAREA))
        {
            _spawnArea = other.transform.localPosition;
        }

        base.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constant.TAG_BLOCKSPAWNAREA))
        {
            SetTarget(other.transform.localPosition);
        }
    }

    private new void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        if (other.transform.CompareTag(Constant.TAG_DOOR))
        {
            transform.position = _spawnArea;
            _isBusy = false;
            GetNewStair();
        }
    }
    #endregion

    private void FixedUpdate()
    {
        if (!GameManager.Ins.IsPlaying()) return;

        Move(horizontal, vertical);

        if (!IsGround)
        {
            ResetVelocity();

            if (_rotateCoroutine == null)
                _rotateCoroutine = StartCoroutine(RotateMe());

        }

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(0f, 3f, -5.63f);
        }

    }
    private IEnumerator RotateMe()
    {
        float dir = .1f;
        dir = _targetPosition.x < transform.localPosition.x ? -1 : 1;

        Vector3 newRot = transform.localEulerAngles;
        newRot.y += dir * 90;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 2;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(newRot), t);
            yield return null;
        }
        _rotateCoroutine = null;
    }

    private void SetTarget(Vector3 position)
    {
        if (_isBusy) return;

        _targetPosition = position;
        Vector3 normalized = (position - transform.localPosition).normalized;

        horizontal = normalized.x;
        vertical = normalized.z;
    }

    private void CollectBlock()
    {
        if (Random.value >= .85f || bag.blockCount >= 7)
        {
            if (myStair != null)
                SetTarget(myStair.transform.localPosition);
            _isBusy = true;
        }
    }

    private void DropBlock()
    {
        if (bag.blockCount == 0)
        {
            _isBusy = false;
        }
        if (myStair.IsFull)
        {
            _isBusy = false;
            score++;
            GetNewStair();
        }
        else
        {
            SetTarget(_spawnArea);
        }
    }
    private void CollisionAction()
    {
        SetTarget(_spawnArea);
        _isBusy = false;
    }
    private void GetNewStair()
    {
        myStair = LevelManager.Ins.currentLevelSettings.GetStair(score);
        if (myStair == null)
            Invoke("DestroyMe", 4f);
    }
    private void DestroyMe() => Destroy(gameObject);
}
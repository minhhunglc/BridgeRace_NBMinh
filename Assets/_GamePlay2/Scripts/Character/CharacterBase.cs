
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class CharacterBase : Singleton<CharacterBase>, IAnimationState
{
    public delegate void Action();

    [Header("Character Collision Raycast")]
    [SerializeField] private LayerMask _layerMaskCollision;

    [Header("Ground Raycast")]
    [SerializeField] private LayerMask _layerMaskBlock;
    [SerializeField] private LayerMask _layerMaskGround;
    [SerializeField] private Transform _rayPointGround;
    [SerializeField] private Transform _rayPointBlock;

    [Header("Speed")]
    [SerializeField] [Range(1, 20)] private float _speed;

    [Header("Player Bag")]
    public PlayerBag bag;
    [HideInInspector]
    public Stair myStair;

    [Header("Variables")]
    protected bool IsBot;
    protected bool IsGround;
    protected bool IsBlock;
    protected bool IsRunning;
    public int score;

    [Header("Components")]
    public Rigidbody _rigidbody;
    public SkinnedMeshRenderer _mesh;

    [Header("Actions")]
    protected Action collectAction;
    protected Action dropAction;
    protected Action collisionAction;

    [Header("Animatior")]
    public Animator playerAnimator;

    public Animator animator { get => playerAnimator; set => animator = playerAnimator; }
    private void Awake()
    {
        _mesh.material.color = bag.color;
        bag.SpawnBlocks();
    }
    #region  Collision
    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constant.TAG_BLOCK))
        {
            if (bag.BagIsFull()) return;

            Block b = other.GetComponent<Block>();
            if (b.Id == bag.playerId || b.Id == -1)
            {
                bag.AddBlock();
                b.CollectMe();

                if (collectAction != null)
                    collectAction.Invoke();

                if (b.Id == -1) SimplePool.Despawn(b);

            }
        }
        else if (other.CompareTag(Constant.TAG_STAIR))
        {
            if (bag.blockCount > 0)
            {
                Stair s = other.GetComponentInParent<Stair>();

                if (IsBot)
                {
                    if (myStair != s)
                        return;
                }
                else
                {
                    myStair = s;
                }

                s.AddStep(bag.color);

                RemoveBlockInBag();
            }
        }
        else if (other.CompareTag(Constant.TAG_FINISH))
        {
            if (IsBot)
            {
                GameManager.Ins.GameOver();
                bag.ResetBag();

            }
            else
            {
                GameManager.Ins.Win();
                bag.ResetBag();
                Win_Anim();
            }
        }
    }

    protected void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag(Constant.TAG_BLOCK))
        {
            if (bag.BagIsFull()) return;

            Block b = other.gameObject.GetComponent<Block>();
            if (b.Id == bag.playerId || b.Id == -1)
            {
                bag.AddBlock();
                b.CollectMe();

                if (collectAction != null)
                    collectAction.Invoke();

                if (b.Id == -1) SimplePool.Despawn(b);
            }
        }
        if (other.transform.CompareTag(Constant.TAG_STEP) && bag.blockCount > 0)
        {
            MeshRenderer renderer = other.transform.GetComponent<MeshRenderer>();
            if (renderer.material.color == bag.color)
                return;

            renderer.material.color = bag.color;

            if (!IsBot)
            {
                Stair s = other.transform.parent.GetComponent<Stair>();
                myStair = s;
            }

            RemoveBlockInBag();
        }
    }
    #endregion

    #region Blocks & Bag
    private void RemoveBlockInBag()
    {
        bag.RemoveBlock();

        if (dropAction != null)
            dropAction.Invoke();
    }
    public void DropAllBlocks()
    {
        if (collisionAction != null)
            collisionAction.Invoke();

        bag.DropAllBlocks(transform);
    }
    private void CollisionDetectionWithCharacter()
    {
        Debug.DrawRay(transform.position, transform.forward * .6f, Color.blue);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, .6f, _layerMaskCollision))
        {
            var hitcol = hit.transform.GetComponent<CharacterBase>();
            if (hit.collider != null)
            {
                if (hitcol.bag.blockCount < this.bag.blockCount)
                    hitcol.DropAllBlocks();
                //hitcol.Fall_Anim();
                //hitcol._rigidbody.isKinematic = true;
                //hitcol._rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                //hitcol.Idle_Anim();
                ////hitcol.IsRunning = false;
                //StartCoroutine(StopIimeToStand());
                else this.DropAllBlocks();
            }
        }
    }
    #endregion

    #region Physics
    private IEnumerator StopIimeToStand()
    {
        float time = 2f;
        yield return new WaitForSeconds(time);

    }

    protected void Move(float horizontal, float vertical)
    {

        Debug.DrawRay(_rayPointGround.position, -_rayPointGround.up, Color.red);
        IsGround = Physics.Raycast(_rayPointGround.position, -_rayPointGround.up, _layerMaskGround);

        if (vertical != 0 || horizontal != 0 || !IsRunning)
        {
            IsRunning = true;
            Quaternion newRot = Quaternion.Euler(0, (Mathf.Atan2(horizontal, vertical) * 180 / Mathf.PI), 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, Time.deltaTime * 2);

            if (IsGround)
            {
                Vector3 velocity = transform.forward * _speed * Time.deltaTime * 10;
                velocity.y = _rigidbody.velocity.y;
                _rigidbody.velocity = velocity;
                Running_Anim();
            }
            else
            {
                ResetVelocity();
                Idle_Anim();
            }
        }
        else
        {
            ResetVelocity();
            Idle_Anim();
        }
        RaycastHit hit;
        if (Physics.Raycast(_rayPointBlock.position, -_rayPointBlock.up, out hit, .6f, _layerMaskBlock))
        {
            if (bag.blockCount == 0 && hit.collider.GetComponent<MeshRenderer>().material.color != _mesh.material.color)
            {
                ResetVelocity();
                Idle_Anim();
            }
        }
        CollisionDetectionWithCharacter();
    }
    protected void ResetVelocity()
    {
        _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
        _rigidbody.angularVelocity *= 0;
    }
    public void Running_Anim()
    {
        if (!animator.GetBool(Constant.PREFERENCE_ANIM_RUN))
        {
            animator.SetBool(Constant.PREFERENCE_ANIM_RUN, true);
            animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
        }
    }
    public void Idle_Anim()
    {
        if (animator.GetBool(Constant.PREFERENCE_ANIM_RUN))
        {

            animator.SetBool(Constant.PREFERENCE_ANIM_RUN, false);
            animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
        }
    }
    public void Fall_Anim()
    {
        if (!animator.GetBool(Constant.PREFERENCE_ANIM_FALL))
        {
            animator.SetBool(Constant.PREFERENCE_ANIM_FALL, true);
            animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
        }
    }
    public void Stand_Anim()
    {
        if (animator.GetBool(Constant.PREFERENCE_ANIM_FALL))
        {
            animator.SetBool(Constant.PREFERENCE_ANIM_FALL, false);
            animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
        }
    }
    public void Win_Anim()
    {
        animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 2);
    }

    public void Lose_Anim()
    {
        animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 1);
    }
    #endregion
}
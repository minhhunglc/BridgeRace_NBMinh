using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick : Singleton<VirtualJoystick>, IDragHandler, IPointerUpHandler, IPointerDownHandler, IAnimationState
{
    public Transform playerTransform;
    public Transform brickArea;
    public Animator playerAnimator;
    public RectTransform pad;
    public float moveSpeed;
    Vector3 move;
    bool isWalking;
    public bool isPlaying;

    public Animator animator { get => playerAnimator; set => animator = playerAnimator; }

    private void Start()
    {
        isPlaying = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaying == true)
        {
            transform.position = eventData.position;
            transform.localPosition = Vector2.ClampMagnitude(eventData.position - (Vector2)pad.position, pad.rect.width * 0.5f);
            move = new Vector3(transform.localPosition.x, 0, transform.localPosition.y).normalized;

            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool(Constant.PREFERENCE_ANIM_RUN, true);
                animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
            }
        }

    }
    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(PlayerMove());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
        move = Vector3.zero;
        StopCoroutine(PlayerMove());
        isWalking = false;
        animator.SetBool(Constant.PREFERENCE_ANIM_RUN, false);
        animator.SetInteger(Constant.PREFERENCE_ANIM_RESULT, 0);
    }
    IEnumerator PlayerMove()
    {
        while (true)
        {
            playerTransform.GetComponent<Rigidbody>().MovePosition(playerTransform.position + move * moveSpeed * Time.deltaTime);
            //playerTransform.Translate(move * moveSpeed * Time.deltaTime, Space.World);

            if (move != Vector3.zero)
            {
                playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, Quaternion.LookRotation(move), 5 * Time.deltaTime);
                brickArea.eulerAngles = new Vector3(0f, 0f, 0f);
            }

            yield return null;
        }
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
        throw new System.NotImplementedException();
    }

    public void Cheer_Anim()
    {
        throw new System.NotImplementedException();
    }
}

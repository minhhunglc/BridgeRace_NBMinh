using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class VirtualJoystick : Singleton<VirtualJoystick>, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    #region Test
    //public RectTransform m_rtBack;
    //public RectTransform m_rtJoystick;

    //// Target
    //public Transform Player;
    //float m_Radius;
    //public float m_Speed = 5.0f;

    //public Vector3 m_VecMove;
    //bool m_bTouch = false;

    //private void Start()
    //{
    //    m_Radius = m_rtBack.rect.width * 0.5f;
    //}

    //private void FixedUpdate()
    //{
    //    if (m_bTouch)
    //    {
    //        Player.position += m_VecMove;
    //    }
    //}

    //private void OnTouch(Vector2 vecTouch)
    //{

    //    Vector2 vec = new Vector2(vecTouch.x - m_rtBack.position.x, vecTouch.y - m_rtBack.position.y);

    //    // Make sure that vec value does not exceed m_Radius
    //    vec = Vector2.ClampMagnitude(vec, m_Radius);
    //    m_rtJoystick.localPosition = vec;


    //    // Move the joystick background to the distance ratio of the joystick
    //    float fSqr = (m_rtBack.position - m_rtJoystick.position).sqrMagnitude / (m_Radius * m_Radius);

    //    // normalize touch position
    //    Vector2 vecNormal = vec.normalized;

    //    m_VecMove = new Vector3(vecNormal.x * m_Speed * Time.deltaTime * fSqr, 0f, vecNormal.y * m_Speed * Time.deltaTime * fSqr);
    //    Player.eulerAngles = new Vector3(0f, Mathf.Atan2(vecNormal.x, vecNormal.y) * Mathf.Rad2Deg, 0f);
    //}
    #endregion

    public Transform playerTransform;
    public Transform brickArea;
    public Animator playerAnimator;
    public RectTransform pad;
    public float moveSpeed;
    Vector3 move;
    bool isWalking;
    public bool isPlaying;
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
                playerAnimator.SetBool("isWalking", true);
                playerAnimator.SetInteger("Result", 0);
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
        playerAnimator.SetBool("isWalking", false);
        playerAnimator.SetInteger("Result", 0);
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
}

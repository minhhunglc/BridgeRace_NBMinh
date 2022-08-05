using UnityEngine;

public class Player : CharacterBase
{
    [Header("Joystick")]
    [SerializeField] private Joystick _joystick;

    private void Start()
    {
        collectAction += CollectAction;
        dropAction += DropAction;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Ins.IsPlaying()) return;

        float vertical = _joystick.Vertical;
        float horizontal = _joystick.Horizontal;
        Move(horizontal, vertical);

        if (transform.position.y < -5f)
        {
            transform.position = new Vector3(0f, 3f, -5.63f);
        }
    }

    private void DropAction()
    {
        if (myStair.IsFull)
            score++;
    }

    private void CollectAction()
    {
        if (LevelManager.Ins.currentLevelSettings.CheckStairs(score) == false)

        {
            GameManager.Ins.GameOver();
            base.Lose_Anim();
        }
    }

    public void ResetPlayer()
    {
        bag.ResetBag();
        score = 0;
    }
}
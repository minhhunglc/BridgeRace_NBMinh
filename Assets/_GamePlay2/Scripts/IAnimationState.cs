using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimationState
{
    public Animator animator { get; set; }

    void Running_Anim();
    void Idle_Anim();
    void Fall_Anim();
    void Win_Anim();
    void Lose_Anim();
}

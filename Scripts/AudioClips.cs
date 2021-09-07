using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{
    public AudioClip jumpUp, jumpDown, slide, death;
    public AudioSource JumpUpAudS, JumpDownAudS, SlideAudS, DeathAudS;

    void JumpUp()
    {
        JumpUpAudS.PlayOneShot(jumpUp);
    }

    void JumpDown()
    {
        JumpDownAudS.PlayOneShot(jumpDown);
    }

    void Slide()
    {
        SlideAudS.PlayOneShot(slide);
    }

    void Death()
    {
        DeathAudS.PlayOneShot(death);
    }
}

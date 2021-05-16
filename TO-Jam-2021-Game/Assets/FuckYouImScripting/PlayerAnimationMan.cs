using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationMan : MonoBehaviour
{
    private PlayerController pc;
    private BalloonAOE baoe;

    private Animator anim;

    private float prevDirection=1;

    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
        baoe = GetComponentInParent<BalloonAOE>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        anim.SetBool("Grounded", pc.IsGrounded());
        anim.SetFloat("HorSpeed", Mathf.Abs(pc.velocity.x));
        if ((pc.velocity.x < 0 && prevDirection>0) || (pc.velocity.x > 0 && prevDirection < 0))
        {
            this.GetComponent<SpriteRenderer>().flipX = !this.GetComponent<SpriteRenderer>().flipX;
            prevDirection *= -1f;
        }

        anim.SetFloat("VerSpeed", pc.velocity.y);
    }
}

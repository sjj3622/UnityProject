using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMPPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;

    [Header("Animation Names")]
    public string runUPAni = "HMP_IdleB";
    public string runDOWNAni = "HMP_IdleF";
    public string runLEFTAni = "HMP_IdleL";
    public string runRIGHTAni = "HMP_IdleR";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 이동
        Vector2 moveDir = new Vector2(h, v).normalized;
        rb.velocity = moveDir * speed;

        // 애니메이션 방향
        if (moveDir != Vector2.zero)
        {
            if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
            {
                if (moveDir.x > 0)
                    animator.Play(runRIGHTAni);
                else
                    animator.Play(runLEFTAni);
            }
            else
            {
                if (moveDir.y > 0)
                    animator.Play(runUPAni);
                else
                    animator.Play(runDOWNAni);
            }
        }
    }
}

using UnityEngine;

public class Player : PhysicsBase
{
    [Header("Tuning")]
    public float moveSpeed = 7f;
    public float jumpForce = 14f;

    [Header("Rendering & Anim")]
    public SpriteRenderer sr;     
    public Animator anim;         

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        desiredx = x * moveSpeed;

        if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;
            if (anim) anim.SetTrigger("jump");   
        }

        if (sr)
        {
            if (x > 0.01f)  sr.flipX = false;
            else if (x < -0.01f) sr.flipX = true;
        }

        if (anim)
        {
            anim.SetBool("run", Mathf.Abs(x) > 0.01f);
            anim.SetBool("grounded", grounded);    
            anim.SetFloat("vy", velocity.y);         
        }
    }
}
using UnityEngine;

//player inherits from PhysicsBase, so it already has movement physics (velocity, grounded, jump handling,...)
public class Player : PhysicsBase
{
    [Header("Tuning")]
    public float moveSpeed = 7f;     // horizontal movement speed
    public float jumpForce = 14f;    // jump force (vertical boost)

    [Header("Rendering & Anim")]
    public SpriteRenderer sr;        //reference to the player’s sprite (for flipping left/right)
    public Animator anim;            // reference to Animator (to trigger idle, run, jump animations)

    void Update()
    {
        //get player input for left/right movement (raw = -1, 0, or +1)
        float x = Input.GetAxisRaw("Horizontal");

        //store the desired horizontal velocity (PhysicsBase uses desiredx each frame)
        desiredx = x * moveSpeed;

        //handle jump input
        if (grounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = jumpForce;          // apply jump force to vertical velocity
            if (anim) anim.SetTrigger("jump");   // play jump animation (triggered once)
        }

        //handle sprite flipping so the player faces the direction they’re moving
        if (sr)
        {
            if (x > 0.01f)        sr.flipX = false;  // Facing right
            else if (x < -0.01f)  sr.flipX = true;   // Facing left
        }

        //handle animator parameters (continuous animation states)
        if (anim)
        {
            anim.SetBool("run", Mathf.Abs(x) > 0.01f);  //running if moving horizontally
            anim.SetBool("grounded", grounded);         //tells animator whether player is on ground
            anim.SetFloat("vy", velocity.y);            //pass vertical velocity (falling vs jumping)
        }
    }
}
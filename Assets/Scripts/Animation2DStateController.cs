using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation2DStateController : MonoBehaviour
{
    public Animator animator;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
     float health = 100f;

    float velocityX = 0.0f;
    float velocityZ = 0.0f;

    float acceleration = 2.0f;
    float deceleration = 2.0f;

    float maxWalkVelocity = 0.5f;
    float maxRunVelocity = 2.0f;

    float speed = 2f;
    float gravity = -19.1f;
    float jumpHeight = 2f;
    float groundDistance = 0.4f;

    Vector3 velocity;
    bool isGrounded;
    int velocityZHash;
    int velocityXHash;
    string verticalAxis, horizontalAxis;
    KeyCode shift, jump;


    void Start()
    {
        velocityZHash = Animator.StringToHash("velocityZ");
        velocityXHash = Animator.StringToHash("velocityX");
        if (gameObject.name.Equals("fps1"))
        {
            verticalAxis = "Vertical"; horizontalAxis = "Horizontal"; shift = KeyCode.RightShift; jump = KeyCode.Space;
        }
        else { verticalAxis = "Vertical2"; horizontalAxis = "Horizontal2"; shift = KeyCode.LeftShift; jump = KeyCode.Tab; }
    }

    void changeVelocity(float vertical, float horizontal, bool rShiftPressed, float currentMaxVelocity)
    {
        if (vertical > 0 && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        if (vertical < 0 && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        if (horizontal < 0 && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        if (horizontal > 0 && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        if (vertical <= 0 && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        if (vertical >= 0 && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        if (horizontal >= 0 && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        if (horizontal <= 0 && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }

    public void lockOrResetVelocity(float vertical, float horizontal, bool rShiftPressed, float currentMaxVelocity)
    {
        if (vertical == 0 && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }

        if (horizontal == 0 && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        if (vertical > 0 && rShiftPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        else if (vertical > 0 && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.5f))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        else if (vertical > 0 && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.5f))
        {
            velocityZ = currentMaxVelocity;
        }

        if (vertical < 0 && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.5f))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        else if (vertical < 0 && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.5f))
        {
            velocityZ = -currentMaxVelocity;
        }

        if (horizontal > 0 && rShiftPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (horizontal > 0 && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.5f))
            {
                velocityX = currentMaxVelocity;
            }
        }
        else if (horizontal > 0 && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.5f))
        {
            velocityX = currentMaxVelocity;
        }

        if (horizontal < 0 && rShiftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;

        }
        else if (horizontal < 0 && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.5f))
            {
                velocityX = -currentMaxVelocity;
            }
        }
        else if (horizontal < 0 && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity + 0.5f))
        {
            velocityX = -currentMaxVelocity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool rShiftPressed = Input.GetKey(shift);
        float h = Input.GetAxis(horizontalAxis);
        float v = Input.GetAxis(verticalAxis);

        float currentMaxVelocity = rShiftPressed ? maxRunVelocity : maxWalkVelocity;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (rShiftPressed) speed = 4f;
        else speed = 2f;

        changeVelocity(v, h, rShiftPressed, currentMaxVelocity);
        lockOrResetVelocity(v, h, rShiftPressed, currentMaxVelocity);

        animator.SetFloat(velocityZHash, velocityZ);
        animator.SetFloat(velocityXHash, velocityX);
        Vector3 pos = transform.right * h + transform.forward * v;

        controller.Move(pos * speed * Time.deltaTime);

        if (Input.GetKeyDown(jump) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            animator.SetBool("isDead", true);
        }
    }
}

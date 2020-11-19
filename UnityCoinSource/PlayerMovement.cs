using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    private const int BLACK_FALLABLE_PLATFORM_LAYER = 12;
    private const int WHITE_FALLABLE_PLATFORM_LAYER = 13;

    public CharacterController2D controller;
    public float runSpeed = 40f;
    public Animator animator;
    public Joystick joystick;

    [HideInInspector]
    public bool CanClimb = false;
    [HideInInspector]
    public bool IsClimbing = false;
    [HideInInspector]
    public bool IsFallingThroughPlatform = false;
    [HideInInspector]
    public bool CanFallThroughPlatform = false;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    public bool IsJumping = false;
    private GameManager gameManager;
    private Rigidbody2D rb;
    private float fixedGravityScale;
    private bool IsFalling;
    [SerializeField] private GameObject m_HeadCheck;
    [SerializeField] private LayerMask m_WhatIsFallable;
    [SerializeField] private float radius;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        joystick = FindObjectOfType<Joystick>();
        gameManager = FindObjectOfType<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        fixedGravityScale = rb.gravityScale;
    }

    void Update()
    {
        // Prevent PlayerMovement from running when the game isn't running
        if (gameManager == null)
        {
            return;
        }
        
        // 
        if (gameManager.isLoading)
        {
            horizontalMove = 0;
            animator.SetFloat("Speed", 0f);
            return;
        }

        // 
        if (joystick.Horizontal >= .5f)
        {
            horizontalMove = runSpeed;
        }
        else if (joystick.Horizontal < .5f && joystick.Horizontal > 0.2f)
        {
            horizontalMove = runSpeed / 2;
        }
        else if(joystick.Horizontal < -0.2f && joystick.Horizontal >= -0.5f)
        {
            horizontalMove = -runSpeed / 2;
        }
        else if (joystick.Horizontal <= -0.5f)
        {
            horizontalMove = -runSpeed;
        }
        else
        {
            horizontalMove = 0;
        }
        
        if (ShouldFall(name))
        {
            if (name == "PlayerB")
            {
                Physics2D.IgnoreLayerCollision(8, BLACK_FALLABLE_PLATFORM_LAYER, true);
            } else
            {
                Physics2D.IgnoreLayerCollision(8, WHITE_FALLABLE_PLATFORM_LAYER, true);
            }
            IsFalling = true;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_HeadCheck.transform.position, radius, m_WhatIsFallable);
        if (IsFalling)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    Debug.Log("Made it inside here :0");
                    CanFallThroughPlatform = false;
                    IsFalling = false;
                    if (name == "PlayerW") Physics2D.IgnoreLayerCollision(8, WHITE_FALLABLE_PLATFORM_LAYER, false);
                    if (name == "PlayerB") Physics2D.IgnoreLayerCollision(8, BLACK_FALLABLE_PLATFORM_LAYER, false);
                }
            }
        }

        if (joystick.Vertical <= -0.8f && CanClimb)
        {
            verticalMove = -5f;
            IsClimbing = true;
            rb.gravityScale = 0;
        } else if (joystick.Vertical >= 0.8f & CanClimb)
        {
            verticalMove = 5f;
            IsClimbing = true;
            rb.gravityScale = 0;
        }
        else if (CanClimb && IsClimbing)
        {
            verticalMove = 0f;
            rb.gravityScale = 0;
        } else
        {
            rb.gravityScale = fixedGravityScale;
        }
        
        if (IsClimbing)
        {
            if (name == "PlayerW")
            {
                Physics2D.IgnoreLayerCollision(8, WHITE_FALLABLE_PLATFORM_LAYER, true);
            } else
            {
                Physics2D.IgnoreLayerCollision(8, BLACK_FALLABLE_PLATFORM_LAYER, true);
            }
        }

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        
    }
    
    private bool ShouldFall(string name)
    {
        if (name == "PlayerW")
        {
            return joystick.Vertical >= 0.8f && CanFallThroughPlatform;
        } else
        {
            return joystick.Vertical <= -0.8f && CanFallThroughPlatform;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        IsClimbing = false;
        IsJumping = false;
        IsFalling = false;
        if (name == "PlayerW") Physics2D.IgnoreLayerCollision(8, WHITE_FALLABLE_PLATFORM_LAYER, false);
        if (name == "PlayerB") Physics2D.IgnoreLayerCollision(8, BLACK_FALLABLE_PLATFORM_LAYER, false);
    }


    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove, IsJumping);
    }

    public void Jump()
    {
        if (name == "PlayerW") Debug.Log(IsClimbing);
        if (name == "PlayerW") Debug.Log(IsFalling);
        if (!IsClimbing && !IsFalling)
        {
            animator.SetBool("IsJumping", true);
            IsJumping = true;
            Physics2D.IgnoreLayerCollision(8, WHITE_FALLABLE_PLATFORM_LAYER, true);
            Physics2D.IgnoreLayerCollision(8, BLACK_FALLABLE_PLATFORM_LAYER, true);
        }
    }
}

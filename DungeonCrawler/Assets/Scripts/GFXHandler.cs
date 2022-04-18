using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GFXHandler : MonoBehaviour
{
    [SerializeField]
    private AIPath aiPath;

    private SpriteRenderer sRenderer;
    private Animator animator;


    private void Awake()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        MovementCheck();
    }

    private void MovementCheck()
    {
        float velocityX = Mathf.Sign(aiPath.velocity.x);

        if (aiPath.desiredVelocity.x > 0f || aiPath.desiredVelocity.x < 0f || aiPath.desiredVelocity.y > 0f || aiPath.desiredVelocity.y < 0f)
        {
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

        if (aiPath.velocity.x != 0f)
        {
            if (velocityX == 1f)
            {
                sRenderer.flipX = false;
            }
            else if (velocityX == -1f)
            {
                sRenderer.flipX = true;
            }
        }
    }
}

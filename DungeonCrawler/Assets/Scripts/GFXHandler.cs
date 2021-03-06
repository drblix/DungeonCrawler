using UnityEngine;
using Pathfinding;

public class GFXHandler : MonoBehaviour
{
    // <summary> Script for handling the sprite renderer direction and animation of enemies </summary>

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

        if (aiPath.desiredVelocity.x > 0f && aiPath.canMove || aiPath.desiredVelocity.x < 0f && aiPath.canMove || aiPath.desiredVelocity.y > 0f && aiPath.canMove || aiPath.desiredVelocity.y < 0f && aiPath.canMove)
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

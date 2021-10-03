using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    public LayerMask collisionMask;

    const float skinWidth = .05f;    
    float horizontalRaySpacing;
    float verticalRaySpacing;
    public int RaycastAmmount = 5;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    float playerScale;

    void Start()
    {
        playerScale = transform.localScale.x;
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
        UpdateRaycastOrigins();
    }

    public void Move(Vector2 moveAmount, Vector2 input)
    {
        UpdateRaycastOrigins();
        collisions.Reset();

        collisions.moveAmountOld = moveAmount;

        if (moveAmount.x != 0)
        {
            collisions.faceDir = (int)Mathf.Sign(moveAmount.x);
            //Rotate player to face direction
            transform.localScale = new Vector3(playerScale * collisions.faceDir, transform.localScale.y,transform.localScale.z);
        }

        HorizontalCollisions(ref moveAmount);
        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }

        transform.Translate(moveAmount);
    }

    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth)
        {
            rayLength = 2 * skinWidth;
        }
        //left
        for (int i = 0; i < RaycastAmmount; i++)
        {            
            Vector2 rayOrigin = raycastOrigins.bottomRight;            
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * rayLength, Color.green);

            if (hit)
            {
                if(hit.distance == 0)
                {
                    //Skip if statement
                    continue;
                }

                //If moving to the right
                if(directionX == 1)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                }
                rayLength = hit.distance;

                collisions.right = true;
                collisions.hitInfoHorizontal = hit;
            }
        }

        //right
        for (int i = 0; i < RaycastAmmount; i++)
        {
            Vector2 rayOrigin = raycastOrigins.bottomLeft;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.left * rayLength, Color.green);

            if (hit)
            {
                if (hit.distance == 0)
                {
                    //Skip if statement
                    continue;
                }

                //If moving to the left adjust velocity into collision
                if (directionX == -1)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                }              

                rayLength = hit.distance;
              
                collisions.left = true;                             
                collisions.hitInfoHorizontal = hit;
            }
        }
    }

    void VerticalCollisions(ref Vector2 moveAmount)
    {
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;

        for (int i = 0; i < RaycastAmmount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;

            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.green);

            if (hit)
            {
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
               
                collisions.hitInfoVertical = hit;            
            }
        }
    }

    //Update raycast position
    void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;

        bounds.Expand(skinWidth * -2);
        raycastOrigins.bottomLeft = new Vector2(bounds.min.x , bounds.min.y );
        raycastOrigins.bottomRight = new Vector2(bounds.max.x , bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    //Set spacing between each raycast
    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRaySpacing = bounds.size.y / (RaycastAmmount - 1);
        verticalRaySpacing = bounds.size.x / (RaycastAmmount - 1);
    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        //Tag of object hit
        public RaycastHit2D hitInfoVertical;
        public RaycastHit2D hitInfoHorizontal;

        public bool above, below;
        public bool right, left;

        public Vector2 moveAmountOld;
        public int faceDir;

        public void Reset()
        {
            above = false;
            below = false;
            right = false;
            left = false;
        }
    }
}

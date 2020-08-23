using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]

public class physicsController : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public float maxStepHeight = .25f; //the maximum step up the player can make
    public LayerMask solidMask;
    // Start is called before the first frame update
    void Start()
    {
        //get a reference to the rigidbody
        rb = GetComponent<Rigidbody2D>();

        //set up the rigidbody
        rb.freezeRotation = true;
        rb.gravityScale = 0;

        //get a reference to the box collider
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        velocity.x = Input.GetAxis("Horizontal") * 5 * Time.deltaTime; // this line is temporary
        vertical();
        horizontal();
        rb.position += velocity;
        velocity = Vector2.zero;
    }

    //do horizontal collision checks

    void vertical()
    {
        Vector2 pointToCheck = rb.position + velocity;
        Vector2 boxSize = box.size;
        Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from
        float bottomY = pointToCheck.y - (boxSize.y / 2);
        float maximumY = bottomY + maxStepHeight;

        //set up the 3 points
        raycastOrigins[0] = new Vector2(pointToCheck.x - (boxSize.x / 2), 15000);
        raycastOrigins[1] = new Vector2(pointToCheck.x, 15000);
        raycastOrigins[2] = new Vector2(pointToCheck.x + (boxSize.x / 2), 15000);

        //get our raycastHits
        RaycastHit2D[] rh = castRays(raycastOrigins, Vector2.down, 30000, solidMask);

        //get the highest point
        float highest = Mathf.Max(rh[0].point.y, rh[1].point.y, rh[2].point.y);

        //now get the distance
        float distance = highest - bottomY;

        if((highest-bottomY) < maxStepHeight)
        {
            velocity.y = distance;
        }
    }

    void horizontal()
    {
        if(Input.GetAxis("Horizontal")!=0)
        {
            Debug.Log(""); //just so I have a break point
        }
        Vector2 pointToCheck = rb.position + new Vector2((Mathf.Sign(velocity.x) > 0 ? box.size.x/2 : -box.size.x/2),0);
        Vector2 boxSize = box.size;
        Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from
        float bottomY = pointToCheck.y - (boxSize.y / 2);
        float maximumY = bottomY + maxStepHeight;

        //set up the 3 points
        raycastOrigins[0] = new Vector2(pointToCheck.x + (Mathf.Sign(velocity.x) / 2), pointToCheck.y + (boxSize.y / 2));
        raycastOrigins[1] = new Vector2(pointToCheck.x + (Mathf.Sign(velocity.x) / 2), pointToCheck.y);
        raycastOrigins[2] = new Vector2(pointToCheck.x + (Mathf.Sign(velocity.x) / 2), (pointToCheck.y - (boxSize.y / 2)) + 0.1f); //I add the 0.1 to avoid the ray hitting the ground were standing on

        //get our raycastHits
        RaycastHit2D[] rh = castRays(raycastOrigins, new Vector2(Mathf.Sign(velocity.x),0) , velocity.x, solidMask);

        if (rh[0] || rh[1] || rh[2])
        {
            //get the highest point
            float contact = (Mathf.Sign(velocity.x) > 0 ? Mathf.Min(rh[0].point.x, rh[1].point.x, rh[2].point.x) : Mathf.Max(rh[0].point.x, rh[1].point.x, rh[2].point.x));

            //now get the distance
            float distance = (Mathf.Sign(velocity.x) > 0 ? contact - pointToCheck.x : pointToCheck.x - contact);

            velocity.x = Mathf.Sign(velocity.x) * distance;
        }
    }
    void horizontalCol()
    {
        if(checkIfStep())
        {
            //continue without doing horizontal checks
        }
        else
        {
            //do the horizontal checks
            Vector2 castDirection = new Vector2(Mathf.Sign(velocity.x), 0);

            //set up our raycasts
            RaycastHit2D[] rh = new RaycastHit2D[3]; // 0 = top, 1 = middle 2 = bottom
            Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from
            Vector2 rbPos = rb.position;
            Vector2 boxSize = box.size;

            //set up the 3 points
            raycastOrigins[0] = new Vector2(rbPos.x, rbPos.y + (boxSize.y/2));
            raycastOrigins[1] = new Vector2(rbPos.x, rbPos.y);
            raycastOrigins[2] = new Vector2(rbPos.x, rbPos.y - (boxSize.y/2));

            //now we can actually cast the 3 rays
            for (int i = 0; i < 3; i++)
            {
                rh[i] = Physics2D.Raycast(raycastOrigins[i], castDirection, velocity.x + (boxSize.x / 2), solidMask);
                if (rh[i])
                {
                    Debug.Log("horizontal collision from ray: " + i + " with " + rh[i].transform.name);
                }
            }

            //if any of these raycasts hit find which one is closest 
            if(rh[0] || rh[1] || rh[2])
            {
                //find the closest
                float closest = (castDirection.x > 0 ? Mathf.Min(rh[0].point.x, rh[1].point.x, rh[2].point.x) : Mathf.Max(rh[0].point.x, rh[1].point.x, rh[2].point.x));

                //now we can get the distance
                float distance = (castDirection.x > 0 ? closest - (rb.position.x - (boxSize.x / 2)) : closest - (rb.position.x + (boxSize.x / 2)));

                //and we set the velocity
                velocity.x = distance;
            }
        }
    }

    RaycastHit2D[] castRays(Vector2[] origins, Vector2 direction, float distance, LayerMask layerMask)
    {
        RaycastHit2D[] rh = new RaycastHit2D[3]; // 0 = left, 1 = middle 2 = right

        //now we can actually cast the 3 rays
        for (int i = 0; i < 3; i++)
        {
            rh[i] = Physics2D.Raycast(origins[i], direction, distance, layerMask);            
        }

        return rh;

    }
    //the first vertical collision check
    bool checkIfStep()
    {
        /*all that this method will do is check if 
         * A.) the actor is below the collision point
         * B.) if that collision point is low eneough that its just a step up
         * 
         * If it is low eneough they can stay at that x and just move up
         */
        Vector2 pointToCheck = rb.position + velocity;
        Vector2 boxSize = box.size;
        float bottomY = pointToCheck.y - (boxSize.y / 2);
        float maximumY = bottomY + maxStepHeight;
        //if the collider finds a collision at this value or lower they can step up otherwise horizontal collision needs to step in
        
        RaycastHit2D[] rh = new RaycastHit2D[3]; // 0 = left, 1 = middle 2 = right
        Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from

        //set up the 3 points
        raycastOrigins[0] = new Vector2(pointToCheck.x - boxSize.x, 15000);
        raycastOrigins[1] = new Vector2(pointToCheck.x , 15000);
        raycastOrigins[2] = new Vector2(pointToCheck.x + boxSize.x, 15000);

        //now we can actually cast the 3 rays
        for(int i = 0; i<3;i++)
        {
            rh[i] = Physics2D.Raycast(raycastOrigins[i], Vector2.down, 30000, solidMask);
            if (rh[i])
            {
                Debug.Log("vertical collision from ray: " + i + " with " + rh[i].transform.name);
            }
        }

        //now for the step to be valid all 3 points need to be within the step rangle
        if (rh[0].point.y <= maximumY && rh[1].point.y <= maximumY && rh[2].point.y <= maximumY)
        {
            //and a second check to make sure its above the players feet
            if (rh[0].point.y > bottomY && rh[1].point.y > bottomY && rh[2].point.y > bottomY)
            {
                //we can safely assume this is a step we can just walk right up
                //now we need to get the distance between the point we are trying to be at
                //and the y value of this step (which we know is higher than the player

                //first though we need the highest point (in case its a slope
                float highest = Mathf.Max(rh[0].point.y, rh[1].point.y, rh[2].point.y);

                //now get the distance
                float distance = highest - bottomY;

                //and finally set the velocity
                velocity.y = distance;

                //and finally return true since its a step
                return true;
            }
            else
            {
                //this is not a step, we will have to adjust the horizontal collision so for now velocity.y = 0
                velocity.y = 0;

                //return false since we will have to do horizontal checks
                return false;
            }
        }
        else
        {
            //the player should be standing on solid ground
            velocity.y = 0;
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(playerMovement))]
public class playerCollisionHandler : MonoBehaviour
{
    public Rigidbody2D rb;
    public playerMovement movement;
    public lockToGround gLock;
    public LayerMask solidMask;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<playerMovement>();
        gLock = movement.lockGround;
    }

    // Update is called once per frame
    float horizontalColCheck(float xOffset)
    {

        float newXOffset = xOffset;
        //if the player is facing right keep xMod posative, otherwise its negative
        int xMod = (gLock.facingRight ? 1 : -1);
        Vector2 castDirection = new Vector2(xMod, 0);
        
        //cast our 3 rays and return the one with the nearest x value
        RaycastHit2D top = Physics2D.Raycast(new Vector2(rb.position.x, rb.position.y + (gLock.box.size.y / 2)), castDirection, 15000, solidMask);
        RaycastHit2D middle = Physics2D.Raycast(new Vector2(rb.position.x, rb.position.y), castDirection, 15000, solidMask);
        RaycastHit2D bottom = Physics2D.Raycast(new Vector2(rb.position.x, rb.position.y - (gLock.box.size.y / 2)), castDirection, 15000, solidMask);

        //get the distances
        float distanceTop = (top ? Mathf.Abs(rb.position.x - top.point.x) : 15000);
        float distanceMiddle = (middle ? Mathf.Abs(rb.position.x - middle.point.x) : 15000);
        float distancebottom = (bottom ? Mathf.Abs(rb.position.x - top.point.x) : 15000);

        //determine which is the closest
        float min = Mathf.Min(distanceTop, distanceMiddle, distancebottom);

        //set the correct raycast data to use
        RaycastHit2D closest=top;
        if(min==distanceTop)
        {
            closest = top;
        }
        else
        {
            if(min==distanceMiddle)
            {
                closest = middle;
            }
            else
            {
                if(min == distancebottom)
                {
                    closest = bottom;
                }
                else
                {
                    Debug.LogWarning("Error: Raycasts out of range in horizontal collision checks");
                }
            }
        }

        Vector2 closestPoint = closest.point;


        return newXOffset;
    }
}

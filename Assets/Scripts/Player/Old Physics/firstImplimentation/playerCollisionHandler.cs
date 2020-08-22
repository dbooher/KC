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
    public Vector2 gizmoPoint;
    public bool drawGizmo = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<playerMovement>();
        gLock = GetComponent<lockToGround>();
    }

    // Update is called once per frame
    public float horizontalColCheck(float xOffset, float xOffsetIncrease)
    {

        float newXOffset = xOffset + xOffsetIncrease;
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

        if(top || middle || bottom)
        {
            drawGizmo = true;
        }
        else
        {
            drawGizmo = false;
        }
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
        gizmoPoint = closestPoint;

        if(!gLock.facingRight)
        {
             if(gLock.basePosition.x - xOffset - xOffsetIncrease - (gLock.box.size.x / 2) < closestPoint.x)
            {
                newXOffset = closestPoint.x + ( gLock.box.size.x / 2);
            }
        }


        return newXOffset;
    }

    private void OnDrawGizmos()
    {
        if(drawGizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(gizmoPoint, .25f);
        }
    }
}

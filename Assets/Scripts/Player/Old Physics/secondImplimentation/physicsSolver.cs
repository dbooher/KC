using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class physicsSolver : MonoBehaviour
{
    public TextMeshProUGUI infoText;   
    public rayCastInfo rc;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public RaycastHit2D leftHit;
    public RaycastHit2D middleHit;
    public RaycastHit2D rightHit;
    public LayerMask groundMasks;
    public float rayCastStartY = 5000;
    public float rayCastLength = 15000;
    public float speedModifier = 1;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
        rc = new rayCastInfo();
        initializeRCInfo();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        doRaycasts();
        float highestPoint = getHighest(); ;
        float velocityGoal = highestPoint - rb.position.y - (box.size.y / 2);
        rb.velocity = new Vector2(0, velocityGoal);
    }
  

    void doRaycasts()
    {
        //actually preform the casts
        Vector2 rayCastOrigin = new Vector2(rb.position.x, rayCastStartY);
        leftHit = Physics2D.Raycast(rayCastOrigin - new Vector2(box.size.x / 2, 0), Vector2.down, rayCastLength, groundMasks);
        middleHit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastLength, groundMasks);
        rightHit = Physics2D.Raycast(rayCastOrigin + new Vector2(box.size.x / 2, 0), Vector2.down, rayCastLength, groundMasks);

        //update the info

        //left
        if (leftHit)
        {
            rc.leftY = leftHit.point.y;
            rc.LTag = leftHit.transform.tag;
            rc.distanceL = rb.position.y - rc.leftY;
        }
        else
        {
            rc.leftY = rb.position.y;
            rc.LTag = "none";
            rc.distanceL = 0;
        }

        //middle
        if (middleHit)
        {
            rc.midY = middleHit.point.y;
            rc.MTag = middleHit.transform.tag;
            rc.distanceM = rb.position.y - rc.midY;
        }
        else
        {
            rc.midY = rb.position.y;
            rc.MTag = "none";
            rc.distanceM = 0;
        }

        //right
        if (rightHit)
        {
            rc.rightY = rightHit.point.y;
            rc.RTag = rightHit.transform.tag;
            rc.distanceR = rb.position.y - rc.rightY;
        }
        else
        {
            rc.rightY = rb.position.y;
            rc.RTag = "none";
            rc.distanceR = 0;
        }
    }

    float getHighest()
    {
        float highest = 0;
        if (rc.leftY > highest)
        {
            highest = rc.leftY;
        }
        if (rc.midY > highest)
        {
            highest = rc.midY;
        }
        if (rc.rightY > highest)
        {
            highest = rc.rightY;
        }
        return highest;
    }

    //draw the raycasts
    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector2 origin = new Vector2(rb.position.x, rayCastStartY);
            Gizmos.color = Color.red;

            //left
            if (leftHit)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(origin - new Vector2(box.size.x / 2, 0), new Vector3(origin.x - box.size.x / 2, rc.leftY, 0));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin - new Vector2(box.size.x / 2, 0), new Vector3(origin.x - box.size.x / 2, origin.y - rayCastLength, 0));
            }

            //middle
            if (middleHit)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(origin, new Vector3(origin.x, rc.midY, 0));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin, new Vector3(origin.x, origin.y - rayCastLength, 0));
            }

            //right
            if (rightHit)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(origin + new Vector2(box.size.x / 2, 0), new Vector3(origin.x + box.size.x / 2, rc.rightY, 0));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin + new Vector2(box.size.x / 2, 0), new Vector3(origin.x + box.size.x / 2, origin.y - rayCastLength, 0));
            }
        }
    }

    private void initializeRCInfo()
    {
        rc.leftY = 0;
        rc.midY = 0;
        rc.rightY = 0;
        rc.velocityY = 0;
        rc.distanceL = 0;
        rc.distanceM = 0;
        rc.distanceR = 0;
        rc.LTag = "none";
        rc.MTag = "none";
        rc.RTag = "none";
    }

}


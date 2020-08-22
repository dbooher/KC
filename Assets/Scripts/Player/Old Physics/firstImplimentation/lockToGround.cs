using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class lockToGround : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public Vector2 basePosition;
    public Vector2 offset = new Vector2(0,0);
    private float velocityY = 0; //used only for falling
    public float maxStepHeight = 0.2f;
    public float offsetGoal = 0;
    public float offsetLerpSpeed = 1f;
    public float lerpAcceleration = 1f;
    public float lerpIncrease = 1.25f;
    public rayCastInfo rc;
    public float gravity = 1;
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public RaycastHit2D leftHit;
    public RaycastHit2D middleHit;
    public RaycastHit2D rightHit;
    public LayerMask groundMasks;
    public float rayCastStartY = 5000;
    public float rayCastLength = 15000;
    public float newY = 0;
    public float speedModifier = 1;
    public bool facingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        infoText = GameObject.Find("Info").GetComponent<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
        rc = new rayCastInfo();
        initializeRCInfo();
        basePosition = rb.position;
        offset.y = basePosition.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTextMesh();//for debugging
        updateFacing();

        doRaycasts(); //get all the Data for snapping to the ground

        //determine the players new YPos
        float snapPoint = getHighest() + (box.size.y / 2);
        if(offset.y != snapPoint)
        {
            offsetGoal = snapPoint;
        }

        //check direction here
        if (offsetGoal > offset.y)
        {
            //check if the step is too steep
            if (offsetGoal - offset.y > maxStepHeight)      //currently this is creating an issue where its almost always returning true
            {
                offsetGoal = offset.y;                      //or is this line the issue, I dont Fucking know
            }
            else
            {
                offset.y = offsetGoal;
            }
        }
        else
        {
            if(offset.y - gravity > offsetGoal)
            {
                offset.y -= velocityY;
                velocityY += gravity * Time.deltaTime;
            }
            else
            {
                //just set the offset to the final position
                offset.y = offsetGoal;
            }
        }        
        //move the player to the new YPos (this will be edited to use Velocity
        newY = basePosition.y + offset.y;

        //update the players rotation based on current slope angle.
        transform.eulerAngles = new Vector3(0,0,getSlopeAngle());

        //update the move speed
        updateMoveSpeed();
    }

    float getSlopeAngle()
    {
        return Mathf.Atan2(rightHit.point.y - leftHit.point.y, rightHit.point.x - leftHit.point.x) * Mathf.Rad2Deg;
    }

    void updateMoveSpeed()
    {
        if(facingRight)
        {
            //get the absolute angle
            float a = getSlopeAngle();
            a = Mathf.Abs(a);

            //determine if going uphill or downHill
            if(rightHit.point.y > leftHit.point.y)
            {
                //the player is going uphill
                speedModifier = Mathf.Lerp(1, 0, a / 180) / 2;
            }
            else
            {
                if (rightHit.point.y < leftHit.point.y)
                {
                    //the player is going downHill
                    speedModifier = Mathf.Lerp(1.25f , 2, a / 180);
                }
                else
                {
                    //the player is on flat ground
                    speedModifier = 1;
                }
            }
        }
        else
        {
            //get the absolute angle
            float a = getSlopeAngle();
            a = Mathf.Abs(a);

            //determine if going uphill or downHill
            if (leftHit.point.y > rightHit.point.y)
            {
                //the player is going uphill
                speedModifier = Mathf.Lerp(1, 0, a / 360) / 2;
            }
            else
            {
                if (leftHit.point.y < rightHit.point.y)
                {
                    //the player is going downHill
                    speedModifier = Mathf.Lerp(1.25f, 2, a / 360);
                }
                else
                {
                    //the player is on flat ground
                    speedModifier = 1;
                }
            }
        }
    }

    void updateFacing()
    {
        if(Input.GetAxis("Horizontal") > 0 && !facingRight)
        {
            //player is facing right
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0 && facingRight)
            {
                //player is facing left
                facingRight = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    void UpdateTextMesh()
    {       
        //update the info text so i can test everything
        infoText.text = "Off: " + offset.y + " | Base: " + basePosition.x + " , " + basePosition.y + " (L: " + rc.leftY + " | M: " + rc.midY + " | R: " + rc.rightY + ") V:  " + rc.velocityY + " \n" +
            "DL: " + rc.distanceL + " | DM: " + rc.distanceM + " | DR: " + rc.distanceR + "\n" +
            rc.LTag + " | " + rc.MTag + " | " + rc.RTag + "";
    }

    void doRaycasts()
    {
        //actually preform the casts
        Vector2 rayCastOrigin = new Vector2(rb.position.x, rayCastStartY);
        leftHit = Physics2D.Raycast(rayCastOrigin - new Vector2(box.size.x / 2 , 0), Vector2.down, rayCastLength, groundMasks);
        middleHit = Physics2D.Raycast(rayCastOrigin, Vector2.down, rayCastLength, groundMasks);
        rightHit = Physics2D.Raycast(rayCastOrigin + new Vector2(box.size.x / 2 , 0), Vector2.down, rayCastLength, groundMasks);

        //update the info

        //left
        if (leftHit)
        {
            rc.leftY = leftHit.point.y;
            rc.LTag = leftHit.transform.tag;
            rc.distanceL = basePosition.y - rc.leftY;
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
            rc.distanceM = basePosition.y - rc.midY;
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
            rc.distanceR = basePosition.y - rc.rightY;
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
        if(rc.leftY > highest)
        {
            highest = rc.leftY;
        }
        if(rc.midY > highest)
        {
            highest = rc.midY;
        }
        if(rc.rightY > highest)
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
            if(leftHit)
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
                Gizmos.DrawLine(origin , new Vector3(origin.x, rc.midY, 0));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin , new Vector3(origin.x , origin.y - rayCastLength, 0));
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

public struct rayCastInfo
{
    public float leftY;
    public float midY;
    public float rightY;
    public float velocityY;
    public float distanceL;
    public float distanceM;
    public float distanceR;
    public string LTag ;
    public string MTag ;
    public string RTag ;
}

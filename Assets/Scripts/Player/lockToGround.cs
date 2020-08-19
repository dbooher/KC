using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(BoxCollider2D))]
public class lockToGround : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public float offset = 0;
    public float offsetGoal = 0;
    public float offsetLerpSpeed = 1f;
    public float lerpAcceleration = 1f;
    public float lerpIncrease = 1.25f;
    public float baseY = 0;
    public float leftY = 0;
    public float midY = 0;
    public float rightY = 0;
    public float velocityY = 0;
    public float distanceL = 0;
    public float distanceM = 0;
    public float distanceR = 0;
    public string LTag = "none";
    public string MTag = "none";
    public string RTag = "none";
    public Rigidbody2D rb;
    public BoxCollider2D box;
    public RaycastHit2D leftHit;
    public RaycastHit2D middleHit;
    public RaycastHit2D rightHit;
    public LayerMask groundMasks;
    public float rayCastStartY = 5000;
    public float rayCastLength = 15000;
    public float newY = 0;
    // Start is called before the first frame update
    void Start()
    {
        infoText = GameObject.Find("Info").GetComponent<TextMeshProUGUI>();
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateTextMesh();//for debugging

        doRaycasts(); //get all the Data for snapping to the ground

        //determine the players new YPos
        float snapPoint = getHighest() + (box.size.y / 2);
        if(offset!=snapPoint)
        {
            offsetGoal = snapPoint;
        }
        offset = Mathf.Lerp(offset,offsetGoal,Time.deltaTime * (offsetLerpSpeed * lerpAcceleration));
        if(offsetGoal < offset && (offset - offsetGoal)<.1 ) //meaning the player is falling
        {
            lerpAcceleration *= lerpIncrease;
        }
        else
        {
            lerpAcceleration = 1;
        }


        //move the player to the new YPos (this will be edited to use Velocity
        newY = baseY + offset;
    }

    void UpdateTextMesh()
    {       
        //update the info text so i can test everything
        infoText.text = "Off: " + offset + " | Base: " + baseY + " (L: " + leftY + " | M: " + midY + " | R: " + rightY + ") V:  " + velocityY + " \n" +
            "DL: " + distanceL + " | DM: " + distanceM + " | DR: " + distanceR + "\n" +
            LTag + " | " + MTag + " | " + RTag + "";
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
            leftY = leftHit.point.y;
            LTag = leftHit.transform.tag;
            distanceL = baseY - leftY;
        }
        else
        {
            leftY = rb.position.y;
            LTag = "none";
            distanceL = 0;
        }

        //middle
        if (middleHit)
        {
            midY = middleHit.point.y;
            MTag = middleHit.transform.tag;
            distanceM = baseY - midY;
        }
        else
        {
            midY = rb.position.y;
            MTag = "none";
            distanceM = 0;
        }

        //right
        if (rightHit)
        {
            rightY = rightHit.point.y;
            RTag = rightHit.transform.tag;
            distanceR = baseY - rightY;
        }
        else
        {
            rightY = rb.position.y;
            RTag = "none";
            distanceR = 0;
        }
    }

    float getHighest()
    {
        float highest = 0;
        if(leftY > highest)
        {
            highest = leftY;
        }
        if(midY > highest)
        {
            highest = midY;
        }
        if(rightY > highest)
        {
            highest = rightY;
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
                Gizmos.DrawLine(origin - new Vector2(box.size.x / 2, 0), new Vector3(origin.x - box.size.x / 2, leftY, 0));
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
                Gizmos.DrawLine(origin , new Vector3(origin.x, midY, 0));
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
                Gizmos.DrawLine(origin + new Vector2(box.size.x / 2, 0), new Vector3(origin.x + box.size.x / 2, rightY, 0));
            }
            else
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(origin + new Vector2(box.size.x / 2, 0), new Vector3(origin.x + box.size.x / 2, origin.y - rayCastLength, 0));
            }
        }
    }
}

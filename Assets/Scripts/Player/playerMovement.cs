using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(lockToGround))]
public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    lockToGround lockGround;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lockGround= GetComponent<lockToGround>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float f = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        if (f!=0)
        {
            Debug.Log("moving @ " + f + " to : " + (rb.position.x + f) + " , " + rb.position.y);
            rb.MovePosition(new Vector2(rb.position.x + f, lockGround.newY));
        }
    }
}

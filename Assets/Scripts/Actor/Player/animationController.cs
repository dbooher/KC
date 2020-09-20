using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationController : MonoBehaviour
{
    public Transform art;
    public Rigidbody2D rb;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = art.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            //moving right
            anim.SetBool("walking", true);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            if(Input.GetAxis("Horizontal") < 0)
            {
                //moving left
                anim.SetBool("walking", true);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                //not moving
                anim.SetBool("walking", false);
            }
        }
    }
}

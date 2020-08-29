using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepToggle : MonoBehaviour
{
    public physicsController player;
    public bool playerOnStairs = false;
    public BoxCollider2D box;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<physicsController>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player==null)
        {
            player = GameObject.Find("Player").GetComponent<physicsController>();
            box = GetComponent<BoxCollider2D>();
        }       

        if(Input.GetKey(KeyCode.W))
        {
            gameObject.layer = 9;
        }
        else
        {
            if(!player.onStairs)
            {
                gameObject.layer = 11;
            }
        }
    }
}

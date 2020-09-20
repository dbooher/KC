using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "teleportZone")
        {
            if (collision.GetComponent<teleportZone>().requireInput == false)
            {
                collision.SendMessage("teleport");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "teleportZone")
        {
            if (collision.GetComponent<teleportZone>().requireInput == true && Input.GetKey(KeyCode.UpArrow))
            {
                collision.SendMessage("teleport");
            }
        }
    }
}

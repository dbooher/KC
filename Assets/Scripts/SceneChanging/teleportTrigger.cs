using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportTrigger : MonoBehaviour
{
    public bool onTrigger = false;
    public teleportZone zone;
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
            teleportZone z = collision.GetComponent<teleportZone>();
            if(!z.requireInput)
            {
                z.teleport();
            }
            else
            {
                zone = z;
                onTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<teleportZone>().requireInput)
        {
            onTrigger = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(onTrigger && Input.GetKeyUp(KeyCode.W))
        {
            zone.teleport();
        }
    }
}

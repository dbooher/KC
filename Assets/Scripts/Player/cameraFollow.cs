using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Rigidbody2D objToFollow;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objToFollow==null)
        {
            objToFollow = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        }
        transform.position = new Vector3(objToFollow.position.x,objToFollow.position.y,-10) + offset;
    }
}

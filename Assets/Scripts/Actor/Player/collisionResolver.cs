using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(BoxCollider2D))]
[RequireComponent (typeof(Rigidbody2D))]
public class collisionResolver : MonoBehaviour
{
    public bool useLayerMask = false;
    public LayerMask layerMask;
    private Rigidbody2D rb;
    public BoxCollider2D box;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

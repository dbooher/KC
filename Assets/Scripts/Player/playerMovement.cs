using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(lockToGround))]
[RequireComponent (typeof(playerCollisionHandler))]

public class playerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Rigidbody2D rb;
    public lockToGround lockGround;
    public playerCollisionHandler plrCol;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lockGround= GetComponent<lockToGround>();
        plrCol = GetComponent<playerCollisionHandler>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //lockGround.offset.x = plrCol.horizontalColCheck(lockGround.offset.x, Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed * lockGround.speedModifier);
        lockGround.offset.x += Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed * lockGround.speedModifier;
        rb.position = new Vector2(lockGround.offset.x + lockGround.basePosition.x , lockGround.offset.y);
    }
}

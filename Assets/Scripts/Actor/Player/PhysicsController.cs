//Script Written By Dustin Booher ~ Firebomb Games

/* File Description
    /*Responsible for controlling actor physics
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    [RequireComponent (typeof(Rigidbody2D))]
    [RequireComponent (typeof(BoxCollider2D))]

    public class PhysicsController : MonoBehaviour
    {
        #region public fields
            public LayerMask solidMask;
            public LayerMask inactiveStairs; 
            public bool _onStairs
            {
                get
                {
                    return _onStairs;
                }
                set
                {
                    Debug.LogWarning("Someone is trying to set " + name + "'s on stairs value, this is not allowed");
                }
            }
            [Header ("change this value to move the player")]
            public float moveValue = 0;
        #endregion
        
        #region private fields
            private Vector2 _velocity = Vector2.zero;
            private Rigidbody2D _rb; 
            private BoxCollider2D _box;
           
        #endregion
        
        #region serialized fields
            [SerializeField]
            private float maxStepHeight = .25f; //the maximum step up the player can make
        #endregion

        #region private methods
        #endregion
        
        // Start is called before the first frame update
        private void Start()
        {
            //get a reference to the rigidbody
            _rb = GetComponent<Rigidbody2D>();
    
            //set up the rigidbody
            _rb.freezeRotation = true;
            _rb.gravityScale = 0;
    
            //get a reference to the box collider
           _box = GetComponent<BoxCollider2D>();
        }
    
        // Update is called once per frame
        private void FixedUpdate()
        {
            moveValue = Input.GetAxis("Horizontal") * 5 * Time.deltaTime; //ITS TIME TO GET RID OF THIS LINE
           _velocity.x = moveValue;
           moveValue=0;
            Vertical();
            Horizontal();
            _rb.position += _velocity;
           _velocity = Vector2.zero;
            StairsCheck();
        }

        #region directionChecking
            private void Horizontal()
            {
                Vector2 pointToCheck = _rb.position + new Vector2((Mathf.Sign(_velocity.x) > 0 ? _box.size.x/2 : -_box.size.x/2),0);
                Vector2 boxSize = _box.size;
                Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from
                float bottomY = pointToCheck.y - (boxSize.y / 2);
                float maximumY = bottomY + maxStepHeight;
        
                //set up out raycast origins
                raycastOrigins = setUpOrigins(pointToCheck, boxSize, false);
                
                //get our raycastHits
                RaycastHit2D[] rh = castRays(raycastOrigins, new Vector2(Mathf.Sign(_velocity.x),0) , _velocity.x, solidMask);
        
                if (rh[0] && rh[1] && rh[2])
                {
                    //get the highest point
                    float contact = (Mathf.Sign(_velocity.x) > 0 ? Mathf.Min(rh[0].point.x, rh[1].point.x, rh[2].point.x) : Mathf.Max(rh[0].point.x, rh[1].point.x, rh[2].point.x));
        
                   _velocity.x = (Mathf.Sign(_velocity.x) > 0 ? -(contact - pointToCheck.x) : (pointToCheck.x - contact));
                }
            }
            
            private void Vertical()
            {
                Vector2 pointToCheck = _rb.position + _velocity;
                Vector2 boxSize = _box.size;
                Vector2[] raycastOrigins = new Vector2[3]; //the points to cast from
                float bottomY = pointToCheck.y - (boxSize.y / 2);
                float maximumY = bottomY + maxStepHeight;

                //set up out raycast origins
                raycastOrigins = setUpOrigins(pointToCheck, boxSize, true);
    
                //get our raycastHits
                RaycastHit2D[] rh = castRays(raycastOrigins, Vector2.down, 30000, solidMask);
    
                //get the highest point
                float highest = Mathf.Max(rh[0].point.y, rh[1].point.y, rh[2].point.y);
    
                //now get the distance
                float distance = highest - bottomY;
    
                if((highest-bottomY) < maxStepHeight)
                {
                    _velocity.y = distance;
                }
            }
        #endregion
        
        #region raycasting

            //set up the 3 origin points
            private Vector2[] setUpOrigins(Vector2 pointToCheck , Vector2 boxSize , bool isVertical)
            {                
                Vector2[] rcOrigins = new Vector2[3];
                
                rcOrigins[0]=new Vector2(pointToCheck.x - (isVertical ? (boxSize.x/2) : 0) , (isVertical ? 15000 : pointToCheck.y + (boxSize.y/2)));
                rcOrigins[1]=new Vector2(pointToCheck.x , (isVertical ? 15000 : pointToCheck.y ));
                rcOrigins[2]=new Vector2(pointToCheck.x + (isVertical ? (boxSize.x/2) : 0) , (isVertical ? 15000 : (pointToCheck.y + (boxSize.y/2)) + 0.1f)+.2f );

                return rcOrigins;
            }
        
            private RaycastHit2D[] castRays(Vector2[] origins, Vector2 direction, float distance, LayerMask layerMask)
            {
                RaycastHit2D[] rh = new RaycastHit2D[3]; // 0 = left, 1 = middle 2 = right
        
                //now we can actually cast the 3 rays
                for (int i = 0; i < 3; i++)
                {
                    rh[i] = Physics2D.Raycast(origins[i], direction, distance, layerMask);            
                }
        
                return rh;
            }  
            
            private void StairsCheck()
            {
                RaycastHit2D rh = Physics2D.Raycast(_rb.position, Vector2.down, 10000, inactiveStairs);
                if(rh == true ? _onStairs = true : _onStairs = false);
            }
        #endregion
    }
    
}

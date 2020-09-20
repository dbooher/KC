using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Actor;

namespace World
{
    public class StepToggle : MonoBehaviour
    {
        private PhysicsController _actor;

        public BoxCollider2D box;

        // Start is called before the first frame update
        private void Start()
        {
            _actor = GameObject.Find("_actor").GetComponent<PhysicsController>();
            box = GetComponent<BoxCollider2D>();
        }


        private void Awake()
        {
            if (_actor == null)
            {
                _actor = GameObject.Find("_actor").GetComponent<PhysicsController>();
            }

            if (box == null)
            {
                box = GetComponent<BoxCollider2D>();
            }
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.layer = 9;
            }
            else
            {
                if (!_actor._onStairs)
                {
                    gameObject.layer = 11;
                }
            }
        }
    }
}

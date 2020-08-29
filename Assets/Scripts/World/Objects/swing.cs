using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swing : MonoBehaviour
{
    public Vector2 minMaxAngle;
    public Vector2 minMaxSpeedMult;
    public float timePerSwing;
    public float currentSpeed;
    public bool positive = true;
    public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timer<timePerSwing)
        {
            timer += Time.deltaTime * currentSpeed;
        }
        else
        {
            timer = 0;
            currentSpeed = Random.Range(minMaxSpeedMult.x, minMaxSpeedMult.y);
            if(positive)
            {
                positive = false;
            }
            else
            {
                positive = true;
            }
        }
        //do the swing
        if(positive)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(minMaxAngle.y, minMaxAngle.x, timer / timePerSwing));
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Lerp(minMaxAngle.x, minMaxAngle.y, timer / timePerSwing));
        }
    }
}

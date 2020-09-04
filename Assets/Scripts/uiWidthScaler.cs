using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class uiWidthScaler : MonoBehaviour
{
    public float screenPercentage = 1;
    public float buffer = 100;
    public float screenSize;
    public RectTransform trans;
    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        screenSize = Screen.width;
        float width = (screenSize - buffer) * screenPercentage;
        trans.sizeDelta = new Vector2(width, trans.sizeDelta.x);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteInEditMode]
public class itemFrameController : MonoBehaviour
{
    public itemFrameController[] frameControllers;
    public GameObject itemImage;
    public TextMeshPro quantity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        frameControllers = FindObjectsOfType<itemFrameController>();
        int frameCount = frameControllers.Length;
        GameObject parent = transform.parent.gameObject;
        RectTransform parentRect = parent.GetComponent<RectTransform>();
        Vector2 sizeDelta = new Vector2(0, 0);
        sizeDelta.y = parentRect.sizeDelta.y - 10;
        sizeDelta.x = sizeDelta.y;
        RectTransform frameRect = GetComponent<RectTransform>();
        frameRect.sizeDelta = sizeDelta;     
    }
}
